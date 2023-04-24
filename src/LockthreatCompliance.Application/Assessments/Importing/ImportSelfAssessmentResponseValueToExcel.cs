using Abp.BackgroundJobs;
using Abp.Dependency;
using LockthreatCompliance.Authorization.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;


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
using System.Linq.Dynamic.Core;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LockthreatCompliance.AuthoritativeDocuments;

namespace LockthreatCompliance.Assessments.Importing
{
    public class ImportSelfAssessmentResponseValueToExcel : BackgroundJob<ImportSelfAssesmentResponseFromExcelJobArgs>, ITransientDependency
    {
        private readonly RoleManager _roleManager;
        private readonly IRepository<PreRegisterBusinessEntity> _preRegisterEntityRepository;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;
        private readonly IPreEntityListExcelDataReader _preEntityListExcelDataReader;
        private readonly IRepository<Assessment> _assessmentRepository;
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

        public IAppUrlService AppUrlService { get; set; }

        private readonly ISelfAssessmentResponseListExcelDataReader _selfAssessmentResponseListExcelDataReader;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<ControlRequirement> _controlRequirementsRepository;
        private readonly IRepository<ReviewData> _reviewDataRepository;

        public ImportSelfAssessmentResponseValueToExcel(
            IRepository<PreRegisterBusinessEntity> preRegisterEntityRepository, IRepository<Assessment> assessmentRepository, IRepository<BusinessEntity> businessRepository,
           IAppNotifier appNotifier, IRepository<User, long> userRepository, IBinaryObjectManager binaryObjectManager, IRepository<Country> countriesRepository, ILocalizationManager localizationManager, IObjectMapper objectMapper, IPreEntityListExcelDataReader preEntityListExcelDataReader,
           IPasswordHasher<User> passwordHasher, RoleManager roleManager,
           IRepository<OrganizationUnit, long> organizationUnitRepository,
           OrganizationUnitManager organizationUnitManager, IUserEmailer userEmailer,
           IRepository<UserOriginity> userOriginityRepository, IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
           IRepository<ControlRequirement> controlRequirementsRepository,
           IRepository<ReviewData> reviewDataRepository,
           ISelfAssessmentResponseListExcelDataReader selfAssessmentResponseListExcelDataReader,
           IUnitOfWorkManager unitOfWorkManager)
        {
            _reviewDataRepository = reviewDataRepository;
            _controlRequirementsRepository = controlRequirementsRepository;
            _selfAssessmentResponseListExcelDataReader = selfAssessmentResponseListExcelDataReader;
            _unitOfWorkManager = unitOfWorkManager;
            _roleManager = roleManager;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _userRepository = userRepository;
            _countriesRepository = countriesRepository;
            _assessmentRepository = assessmentRepository;
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
        public override void Execute(ImportSelfAssesmentResponseFromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var entities = GetControlRequirementsFromExcelOrNull(args);
                if (entities == null)
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                CreateSelfAssessmentRespone(args, entities);
            }
        }

        private List<ImportSelfAssessmentDto> GetControlRequirementsFromExcelOrNull(ImportSelfAssesmentResponseFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _selfAssessmentResponseListExcelDataReader.GetAssessmentResponseFromExcel(file.Bytes, args.TenantId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateSelfAssessmentRespone(ImportSelfAssesmentResponseFromExcelJobArgs args, List<ImportSelfAssessmentDto> entities)
        {
            var version = "0.1";
            var allSum = 0;
            var finalTotal = 0;

            var basicTotal = 0.0;
            var transitionalTotal = 0.0;
            var advanceTotal = 0.0;
            var basicCount = 0;
            var transitionalCount = 0;
            var advanceCount = 0;

            var businessEntity = _assessmentRepository.FirstOrDefault(x => x.Id == args.AssesmentId);
            var BusinessEntitiesAssessment = _assessmentRepository.GetAll().Where(x => x.BusinessEntityId == businessEntity.BusinessEntityId).OrderBy(x => x.Id).ToList();
            var masterAssessmentId = BusinessEntitiesAssessment.FirstOrDefault().Id;
            for (int i = 0; i < BusinessEntitiesAssessment.Count(); i++)
            {
                if (args.AssesmentId == BusinessEntitiesAssessment[i].Id)
                    version = "0." + (i + 1);
            }

            foreach (var item in entities)
            {
                var getControlRequirement = _controlRequirementsRepository.GetAll().Where(x => x.OriginalId.Trim().ToLower() == item.OriginalID.Trim().ToLower()).FirstOrDefault();
                if (getControlRequirement != null)
                {
                    var entryExist = _reviewDataRepository.GetAll().Include(x => x.ControlRequirement).Where(x => x.ControlRequirementId == getControlRequirement.Id && x.AssessmentId == args.AssesmentId).FirstOrDefault();
                    if (entryExist != null)
                    {
                        entryExist.Comment = item.Comment;
                        if (item.Response.Trim().ToLower() == "Fully Compliant".ToLower())
                        {
                            entryExist.ResponseType = (ReviewDataResponseType)4;
                        }
                        else if (item.Response.Trim().ToLower() == "Partially Compliant".ToLower())
                        {
                            entryExist.ResponseType = (ReviewDataResponseType)3;
                        }
                        else if (item.Response.Trim().ToLower() == "Not Applicable".ToLower())
                        {
                            entryExist.ResponseType = (ReviewDataResponseType)1;
                        }
                        else if (item.Response.Trim().ToLower() == "Non Compliant".ToLower())
                        {
                            entryExist.ResponseType = (ReviewDataResponseType)2;
                        }
                        else
                        {
                            entryExist.ResponseType = (ReviewDataResponseType)0;
                        }

                        if (entryExist.ControlRequirement.ControlType == ControlType.Basic)
                        {
                            if (item.Response.Trim().ToLower() == "Fully Compliant".ToLower())
                            {
                                basicTotal = basicTotal + 100;
                                basicCount++;
                            }
                            else if (item.Response.Trim().ToLower() == "Partially Compliant".ToLower())
                            {
                                basicTotal = basicTotal + 50;
                                basicCount++;
                            }
                            else if (item.Response.Trim().ToLower() == "Non Compliant".ToLower())
                            {
                                basicTotal = basicTotal + 0;
                                basicCount++;
                            }
                        }
                        else if (entryExist.ControlRequirement.ControlType == ControlType.Transitional)
                        {
                            if (item.Response.Trim().ToLower() == "Fully Compliant".ToLower())
                            {
                                transitionalTotal = transitionalTotal + 100;
                                transitionalCount++;
                            }
                            else if (item.Response.Trim().ToLower() == "Partially Compliant".ToLower())
                            {
                                transitionalTotal = transitionalTotal + 50;
                                transitionalCount++;
                            }
                            else if (item.Response.Trim().ToLower() == "Non Compliant".ToLower())
                            {
                                transitionalTotal = transitionalTotal + 0;
                                transitionalCount++;
                            }
                        }
                        else if (entryExist.ControlRequirement.ControlType == ControlType.Advanced)
                        {
                            if (item.Response.Trim().ToLower() == "Fully Compliant".ToLower())
                            {
                                advanceTotal = advanceTotal + 100;
                                advanceCount++;
                            }
                            else if (item.Response.Trim().ToLower() == "Partially Compliant".ToLower())
                            {
                                advanceTotal = advanceTotal + 50;
                                advanceCount++;
                            }
                            else if (item.Response.Trim().ToLower() == "Non Compliant".ToLower())
                            {
                                advanceTotal = advanceTotal + 0;
                                advanceCount++;
                            }
                        }

                        _reviewDataRepository.UpdateAsync(entryExist);
                    }
                    else
                    {
                        var masterEntry = _reviewDataRepository.GetAll().Include(x => x.ControlRequirement).Where(x => x.ControlRequirementId == getControlRequirement.Id && x.AssessmentId == masterAssessmentId).FirstOrDefault();



                        if (masterEntry != null)
                        {
                            masterEntry.Comment = item.Comment;
                            if (item.Response.Trim().ToLower() == "Fully Compliant".ToLower())
                            {
                                masterEntry.ResponseType = (ReviewDataResponseType)4;
                            }
                            else if (item.Response.Trim().ToLower() == "Partially Compliant".ToLower())
                            {
                                masterEntry.ResponseType = (ReviewDataResponseType)3;
                            }
                            else if (item.Response.Trim().ToLower() == "Not Applicable".ToLower())
                            {
                                masterEntry.ResponseType = (ReviewDataResponseType)1;
                            }
                            else if (item.Response.Trim().ToLower() == "Non Compliant".ToLower())
                            {
                                masterEntry.ResponseType = (ReviewDataResponseType)2;
                            }
                            else
                            {
                                masterEntry.ResponseType = (ReviewDataResponseType)0;
                            }
                            masterEntry.AssessmentId = args.AssesmentId;
                            masterEntry.Id = 0;
                            masterEntry.Version = version;

                            if (masterEntry.ControlRequirement.ControlType == ControlType.Basic)
                            {
                                if (item.Response.Trim().ToLower() == "Fully Compliant".ToLower())
                                {
                                    basicTotal = basicTotal + 100;
                                    basicCount++;
                                }
                                else if (item.Response.Trim().ToLower() == "Partially Compliant".ToLower())
                                {
                                    basicTotal = basicTotal + 50;
                                    basicCount++;
                                }
                                else if (item.Response.Trim().ToLower() == "Non Compliant".ToLower())
                                {
                                    basicTotal = basicTotal + 0;
                                    basicCount++;
                                }
                            }
                            else if (masterEntry.ControlRequirement.ControlType == ControlType.Transitional)
                            {
                                if (item.Response.Trim().ToLower() == "Fully Compliant".ToLower())
                                {
                                    transitionalTotal = transitionalTotal + 100;
                                    transitionalCount++;
                                }
                                else if (item.Response.Trim().ToLower() == "Partially Compliant".ToLower())
                                {
                                    transitionalTotal = transitionalTotal + 50;
                                    transitionalCount++;
                                }
                                else if (item.Response.Trim().ToLower() == "Non Compliant".ToLower())
                                {
                                    transitionalTotal = transitionalTotal + 0;
                                    transitionalCount++;
                                }
                            }
                            else if (masterEntry.ControlRequirement.ControlType == ControlType.Advanced)
                            {
                                if (item.Response.Trim().ToLower() == "Fully Compliant".ToLower())
                                {
                                    advanceTotal = advanceTotal + 100;
                                    advanceCount++;
                                }
                                else if (item.Response.Trim().ToLower() == "Partially Compliant".ToLower())
                                {
                                    advanceTotal = advanceTotal + 50;
                                    advanceCount++;
                                }
                                else if (item.Response.Trim().ToLower() == "Non Compliant".ToLower())
                                {
                                    advanceTotal = advanceTotal + 0;
                                    advanceCount++;
                                }
                            }

                            //if (item.Response.Trim().ToLower() == "Fully Compliant".ToLower() || item.Response.Trim().ToLower() == "Not Applicable".ToLower())
                            //    allSum = allSum + 100;
                            //else if (item.Response.Trim().ToLower() == "Partially Compliant".ToLower())
                            //    allSum = allSum + 50;
                            //else
                            //    allSum = allSum + 0;
                            //finalTotal++;

                            _reviewDataRepository.InsertAsync(masterEntry);
                        }

                    }
                }
            }

            var tempAssessmentObj = _assessmentRepository.GetAll().Where(x => x.Id == args.AssesmentId).FirstOrDefault();
            var advanceScore = (advanceTotal == 0) ? 0 : Math.Round((advanceTotal * 100) / (advanceCount * 100));
            var basicScore = (basicTotal == 0) ? 0 : Math.Round((basicTotal * 100) / (basicCount * 100));
            var transitionalScore = (transitionalTotal == 0) ? 0 : Math.Round((transitionalTotal * 100) / (transitionalCount * 100));

            if (basicCount > 0)
                finalTotal++;
            if (transitionalCount > 0)
                finalTotal++;
            if (advanceCount > 0)
                finalTotal++;

            var tempScore = ((advanceScore + basicScore + transitionalScore) / finalTotal);
            var reviewScore = Math.Round(tempScore);
            tempAssessmentObj.ReviewScore = Convert.ToString(reviewScore) =="NaN" ? 0: int.Parse(reviewScore.ToString());
            var updatedAssessmentId = _assessmentRepository.InsertOrUpdateAndGetIdAsync(tempAssessmentObj);

            if (tempAssessmentObj.ReviewScore == 0)
            {
                AsyncHelper.RunSync(() => ProcessImportNotApplicapleResultAsync(args));
            }
            else
            {
                AsyncHelper.RunSync(() => ProcessImportUsersResultAsync(args));
            }
        }
        private async Task ProcessImportUsersResultAsync(ImportSelfAssesmentResponseFromExcelJobArgs args)
        {
            if (args.User != null)
            {
                await _appNotifier.SendSelfAssessmentAsync(
                    args.User,
                    "Self Assessment Import Completed.", args.AssesmentId, //_localizationSource.GetString("AllUsersSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private async Task ProcessImportNotApplicapleResultAsync(ImportSelfAssesmentResponseFromExcelJobArgs args)
        {
            if (args.User != null)
            {
                await _appNotifier.SendSelfAssessmentAsync(
                    args.User,
                    "All responses cannot be 'Not Applicable'. Refer ADHICS standard for control applicability.", args.AssesmentId, //_localizationSource.GetString("AllUsersSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportSelfAssesmentResponseFromExcelJobArgs args)
        {
            AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                args.User,
                "Self Assessment Import Failed, Please fix the import file and try again.",//_localizationSource.GetString("FileCantBeConvertedToUserList"),
                Abp.Notifications.NotificationSeverity.Warn));
        }
    }
}
