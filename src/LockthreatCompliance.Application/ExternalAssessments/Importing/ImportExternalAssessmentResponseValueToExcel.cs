using Abp.BackgroundJobs;
using Abp.Dependency;
using LockthreatCompliance.ExternalAssessments.Dtos;
using System;

using Abp.Domain.Repositories;
using Abp.Localization.Sources;
using Abp.ObjectMapping;
using LockthreatCompliance.Notifications;
using LockthreatCompliance.Storage;
using LockthreatCompliance.Authorization.Users;
using Abp.Authorization.Users;
using Abp.Organizations;
using Microsoft.AspNetCore.Identity;
using LockthreatCompliance.BusinessEntities.Importing;
using LockthreatCompliance.Countries;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.Url;
using Abp.Domain.Uow;
using Abp.Threading;
using LockthreatCompliance.Assessments.Dto;
using Abp.Localization;
using System.Threading.Tasks;
using LockthreatCompliance.ControlRequirements;
using System.Collections.Generic;
using System.Linq;
using LockthreatCompliance.FindingReports.Dtos;
using LockthreatCompliance.ExternalAssessments.Exporting;

namespace LockthreatCompliance.ExternalAssessments.Importing
{
    public class ImportExternalAssessmentResponseValueToExcel : BackgroundJob<ImportExternalAssesmentResponseFromExcelJobArgs>, ITransientDependency
    {

        private readonly RoleManager _roleManager;
        private readonly IRepository<PreRegisterBusinessEntity> _preRegisterEntityRepository;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;
        private readonly IPreEntityListExcelDataReader _preEntityListExcelDataReader;
        private readonly IRepository<BusinessEntity> _businessRepository;
        private readonly IRepository<Country> _countriesRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<UserOriginity> _userOriginityRepository;
        private const string defaultPassword = "123qwe";
        private const char organizationCodeSeparator = '.';
        private readonly IUserEmailer _userEmailer;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;

        private readonly IExternalAssessmentsExcelExporter _externalAssessmentsExcelExporter;

        public IAppUrlService AppUrlService { get; set; }

        private readonly IExternalAssessmentResponseListExcelDataReader _externalAssessmentResponseListExcelDataReader;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<ControlRequirement> _controlRequirementsRepository;
        private readonly IRepository<ReviewData> _reviewDataRepository;

        public ImportExternalAssessmentResponseValueToExcel(IExternalAssessmentsExcelExporter externalAssessmentsExcelExporter,
            IRepository<PreRegisterBusinessEntity> preRegisterEntityRepository, IRepository<BusinessEntity> businessRepository,
           IAppNotifier appNotifier, IRepository<User, long> userRepository, IBinaryObjectManager binaryObjectManager, IRepository<Country> countriesRepository, ILocalizationManager localizationManager, IObjectMapper objectMapper, IPreEntityListExcelDataReader preEntityListExcelDataReader,
           IPasswordHasher<User> passwordHasher, RoleManager roleManager,
           IRepository<OrganizationUnit, long> organizationUnitRepository,
           OrganizationUnitManager organizationUnitManager, IUserEmailer userEmailer,
           IRepository<UserOriginity> userOriginityRepository, IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
           IRepository<ControlRequirement> controlRequirementsRepository,
           IRepository<ReviewData> reviewDataRepository,
           IExternalAssessmentResponseListExcelDataReader externalAssessmentResponseListExcelDataReader,
           IUnitOfWorkManager unitOfWorkManager)
        {
            _externalAssessmentsExcelExporter = externalAssessmentsExcelExporter;
            _reviewDataRepository = reviewDataRepository;
            _controlRequirementsRepository = controlRequirementsRepository;
            _externalAssessmentResponseListExcelDataReader = externalAssessmentResponseListExcelDataReader;
            _unitOfWorkManager = unitOfWorkManager;
            _roleManager = roleManager;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _userRepository = userRepository;
            _countriesRepository = countriesRepository;
            _businessRepository = businessRepository;
            _preRegisterEntityRepository = preRegisterEntityRepository;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _localizationSource = localizationManager.GetSource(LockthreatComplianceConsts.LocalizationSourceName);
            _objectMapper = objectMapper;
            _preEntityListExcelDataReader = preEntityListExcelDataReader;
            _passwordHasher = passwordHasher;
            _organizationUnitRepository = organizationUnitRepository;
            _organizationUnitManager = organizationUnitManager;
            _userOriginityRepository = userOriginityRepository;
            _userEmailer = userEmailer;
            AppUrlService = NullAppUrlService.Instance;
        }

        [UnitOfWork]
        public override void Execute(ImportExternalAssesmentResponseFromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var entities = GetControlRequirementsFromExcelOrNull(args);
                if (entities == null)
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                CreateExternalAssessmentRespone(args, entities);
            }
        }

        private List<ImportSelfAssessmentDto> GetControlRequirementsFromExcelOrNull(ImportExternalAssesmentResponseFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _externalAssessmentResponseListExcelDataReader.GetAssessmentResponseFromExcel(file.Bytes, args.TenantId);
            }
            catch (Exception)
            {
                return null;
            } 
        }
        private void CreateExternalAssessmentRespone(ImportExternalAssesmentResponseFromExcelJobArgs args, List<ImportSelfAssessmentDto> entities)
        {
            var validList = entities.Where(x => x.IsvalidResponse == true && x.IsValidUpdateResponse == true && x.IsvalidOriginalId == true).ToList();
            if (validList.Count != 0)
            {
                foreach (var item in validList)
                {
                    var getControlRequirement = _controlRequirementsRepository.GetAll().Where(x => x.OriginalId.Trim().ToLower() == item.OriginalID.Trim().ToLower()).FirstOrDefault();
                    if (getControlRequirement != null)
                    {
                        var entryExist = _reviewDataRepository.GetAll().Where(x => x.ControlRequirementId == getControlRequirement.Id && x.ExternalAssessmentId == args.ExternalAssesmentId).FirstOrDefault();
                        if (entryExist != null)
                        {
                            entryExist.Comment = item.Comment;
                            entryExist.ResponseType = (ReviewDataResponseType)Convert.ToInt32(item.Response);
                            entryExist.UpdatedResponseType = (ReviewDataResponseType)Convert.ToInt32(item.UpdatedResponse);

                            var updatedReviewId = _reviewDataRepository.InsertOrUpdateAndGetIdAsync(entryExist);
                        }
                    }
                }

                AsyncHelper.RunSync(() => ProcessImportUsersResultAsync(args, entities));
            }
            else
            {
                AsyncHelper.RunSync(() => ProcessImportUsersResultAsync(args, entities));
            }
            
        }

        private async Task ProcessImportUsersResultAsync(ImportExternalAssesmentResponseFromExcelJobArgs args, List<ImportSelfAssessmentDto> entities)
        {
            if (args.User != null)
            {
                var InvalidData = entities.Where(x => x.IsvalidResponse == false || x.IsValidUpdateResponse==false || x.IsvalidOriginalId==false).ToList();
                if (InvalidData.Count() != 0)
                {
                    var file = _externalAssessmentsExcelExporter.ExportToFileExternalAssessmentReviews(InvalidData);

                    var message = "Download File to check Invalid External Assessment- " + args.ExternalAssesmentId;

                    await _appNotifier.GlobleCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName, message, "External-" + args.User.UserId);
                }
                else
                { 
                await _appNotifier.SendExternalAssessmentAsync(
                    args.User,
                    "External Assessment Import Completed.",args.ExternalAssesmentId, //_localizationSource.GetString("AllUsersSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
                    }
            }
        }

    

        private void SendInvalidExcelNotification(ImportExternalAssesmentResponseFromExcelJobArgs args)
        {
            AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                args.User,
                "External Assessment Import Failed, Please fix the import file and try again.",//_localizationSource.GetString("FileCantBeConvertedToUserList"),
                Abp.Notifications.NotificationSeverity.Warn));
        }
    }
}
