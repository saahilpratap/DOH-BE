using Abp.BackgroundJobs;
using Abp.Dependency;
using LockthreatCompliance.Authorization.Users.Dto;
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
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Uow;
using LockthreatCompliance.Domains;
using LockthreatCompliance.ControlStandards;
using LockthreatCompliance.ControlRequirements.Dtos;
using System.Collections.Generic;
using Abp.Threading;
using System.Threading.Tasks;
using Abp.Localization;
using System.Linq.Dynamic.Core;
using NPOI.OpenXml4Net.Util;
using System.Linq;
using LockthreatCompliance.AuthoritativeDocuments;

namespace LockthreatCompliance.ControlRequirements.Importing
{
    public class ImportControlRequirementsValueToExcelJob : BackgroundJob<ImportUsersFromExcelJobArgs>, ITransientDependency
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

        public IAppUrlService AppUrlService { get; set; }

        private readonly IRepository<ControlRequirement> _controlRequirementRepository;
        private readonly IRepository<Domain> _domainRepository;
        private readonly IRepository<ControlStandard> _controlStandardRepository;
        private readonly IRepository<AuthoritativeDocument> _authoritativeDocumentRepository;
        private readonly IControlRequirementsListExcelDataReader _controlRequirementsListExcelDataReader;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ImportControlRequirementsValueToExcelJob(
             IRepository<PreRegisterBusinessEntity> preRegisterEntityRepository, IRepository<BusinessEntity> businessRepository,
            IAppNotifier appNotifier, IRepository<User, long> userRepository, IBinaryObjectManager binaryObjectManager, IRepository<Country> countriesRepository, ILocalizationManager localizationManager, IObjectMapper objectMapper, IPreEntityListExcelDataReader preEntityListExcelDataReader,
            IPasswordHasher<User> passwordHasher, RoleManager roleManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            OrganizationUnitManager organizationUnitManager, IUserEmailer userEmailer,
            IRepository<UserOriginity> userOriginityRepository, IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<ControlRequirement> controlRequirementRepository,
            IRepository<Domain> domainRepository,
            IRepository<AuthoritativeDocument> authoritativeDocumentRepository,
            IRepository<ControlStandard> controlStandardRepository,
            IControlRequirementsListExcelDataReader controlRequirementsListExcelDataReader,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _controlRequirementRepository = controlRequirementRepository;
            _domainRepository = domainRepository;
            _controlStandardRepository = controlStandardRepository;
            _authoritativeDocumentRepository = authoritativeDocumentRepository;
            _controlRequirementsListExcelDataReader = controlRequirementsListExcelDataReader;
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
        public override void Execute(ImportUsersFromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var entities = GetControlRequirementsFromExcelOrNull(args);
                if (entities == null || entities.Count() == 0)
                {
                   SendInvalidExcelNotification(args);
                    return;
                }

                CreateControlRequirements(args, entities);
            }
        }

        private List<ImportControlRequirementDto> GetControlRequirementsFromExcelOrNull(ImportUsersFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _controlRequirementsListExcelDataReader.GetControlRequirementFromExcel(file.Bytes, args.TenantId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void CreateControlRequirements(ImportUsersFromExcelJobArgs args, List<ImportControlRequirementDto> entities)
        {
            bool checkValidImport = entities.All(x => x.CanBeImported == true);
            var checkValidHeader = entities.FirstOrDefault();
            if (checkValidHeader.InvalidCount != false)
            { 
            foreach (var item in entities)
            {
                if (item.CanBeImported == true)
                {
                    int domainId = 0;
                    int controlStandardId = 0;
                    var getDomian = _domainRepository.GetAll();
                    var checkDomian = getDomian.Where(x => x.Name.Trim().ToLower() == item.DomainName.Trim().ToLower() && x.AuthoritativeDocumentId == item.AuthoritativeDocumentId).FirstOrDefault();
                    var AuthorizarionName = _authoritativeDocumentRepository.Get(item.AuthoritativeDocumentId);
                    if (AuthorizarionName != null)
                    {
                        if (checkDomian == null)
                        {
                            var domain = new Domain
                            {
                                TenantId = args.TenantId,
                                Name = item.DomainName,
                                AuthoritativeDocumentName = AuthorizarionName.Name,
                                AuthoritativeDocumentId = AuthorizarionName.Id
                            };
                            domainId = _domainRepository.InsertAndGetId(domain);

                            var controlStandards = new ControlStandard
                            {
                                TenantId = args.TenantId,
                                OriginalControlId = item.OriginalId,
                                DomainName = item.DomainName,
                                Name = item.ControlStandardName,
                                AuthoritativeDocumentId = AuthorizarionName.Id,
                                DomainId = domainId
                            };
                            controlStandardId = _controlStandardRepository.InsertAndGetId(controlStandards);

                            var controlRequirement = new ControlRequirement
                            {
                                TenantId = args.TenantId,
                                OriginalId = item.OriginalId,
                                DomainName = item.DomainName,
                                ControlStandardName = item.ControlStandardName,
                                AuthoritativeDocumentId = AuthorizarionName.Id,
                                ControlType = (ControlType)item.ControlType,
                                Description = item.Description,
                                ControlStandardId = controlStandardId,
                                IndustryMandated = item.IndustryMandated,
                                Iscored = item.Iscored

                            };
                            _controlRequirementRepository.Insert(controlRequirement);
                        }
                        else
                        {
                            var getControlStandard = _controlStandardRepository.GetAll();
                            var checkControlStandards = getControlStandard.Where(x => x.Name.Trim().ToLower() == item.ControlStandardName.Trim().ToLower() && x.DomainId == checkDomian.Id && x.AuthoritativeDocumentId == item.AuthoritativeDocumentId).FirstOrDefault();
                            var getcontrolRequirements = _controlRequirementRepository.GetAll();
                            var checkControlRequirements = getcontrolRequirements.Where(x => x.ControlStandardName.Trim().ToLower() == item.ControlStandardName.Trim().ToLower() && x.OriginalId.Trim().ToLower() == item.OriginalId.Trim().ToLower() && x.DomainName == checkDomian.Name && x.AuthoritativeDocumentId == item.AuthoritativeDocumentId).FirstOrDefault();
                            if (checkControlStandards == null)
                            {
                                var controlStandards = new ControlStandard
                                {
                                    TenantId = args.TenantId,
                                    OriginalControlId = item.OriginalId,
                                    DomainName = item.DomainName,
                                    Name = item.ControlStandardName,
                                    AuthoritativeDocumentId = AuthorizarionName.Id,
                                    DomainId = checkDomian.Id
                                };
                                controlStandardId = _controlStandardRepository.InsertAndGetId(controlStandards);

                                var controlRequirement = new ControlRequirement
                                {
                                    TenantId = args.TenantId,
                                    OriginalId = item.OriginalId,
                                    DomainName = item.DomainName,
                                    ControlStandardName = item.ControlStandardName,
                                    AuthoritativeDocumentId = AuthorizarionName.Id,
                                    ControlType = (ControlType)item.ControlType,
                                    Description = item.Description,
                                    ControlStandardId = controlStandardId,
                                    IndustryMandated = item.IndustryMandated,
                                    Iscored = item.Iscored

                                };
                                _controlRequirementRepository.Insert(controlRequirement);
                            }
                            else
                            {
                                if (checkControlRequirements == null)
                                {
                                    var controlRequirement = new ControlRequirement
                                    {
                                        TenantId = args.TenantId,
                                        OriginalId = item.OriginalId,
                                        DomainName = item.DomainName,
                                        ControlStandardName = item.ControlStandardName,
                                        AuthoritativeDocumentId = AuthorizarionName.Id,
                                        ControlType = (ControlType)item.ControlType,
                                        Description = item.Description,
                                        ControlStandardId = checkControlStandards.Id,
                                        IndustryMandated = item.IndustryMandated,
                                        Iscored = item.Iscored
                                    };
                                    _controlRequirementRepository.Insert(controlRequirement);
                                }
                            }
                        }

                    }
                }
            }
            AsyncHelper.RunSync(() => ProcessImportUsersResultAsync(args));
            }
            else
            {
                SendInvalidColumnNotification(args, entities);
                return;
            }

        }

        private void SendInvalidColumnNotification(ImportUsersFromExcelJobArgs args, List<ImportControlRequirementDto> entities)
        {
            var firstEntries = entities.ToList().FirstOrDefault();
            AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                args.User,
                "Import Failed.Column (" + firstEntries.InvalidName + "..) Does Not Match,Please check Column Name or use the import template provided",//_localizationSource.GetString("FileCantBeConvertedToUserList"),
                Abp.Notifications.NotificationSeverity.Warn));
        }
        private async Task ProcessImportUsersResultAsync(ImportUsersFromExcelJobArgs args)
        {
            if (args.User != null)
            {
                await _appNotifier.GlobalMessageAsync(
                    args.User,
                    "All Control Requirement Entities Import Done- " + args.Code,
                    "ControlRequirement-" + args.User.UserId,//_localizationSource.GetString("AllUsersSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelRowNotification(ImportUsersFromExcelJobArgs args, List<ImportControlRequirementDto> entities)
        {
            var getError = entities.Where(x => x.CanBeImported == false).ToList().FirstOrDefault();
            AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                args.User,
                "Entities import process has failed. Please Check Row No " + getError.RowName + " , Cell " + getError.Exception,//_localizationSource.GetString("FileCantBeConvertedToUserList"),
                Abp.Notifications.NotificationSeverity.Warn));
        }

        private void SendInvalidExcelNotification(ImportUsersFromExcelJobArgs args)
        {
            AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                args.User,
                "Entities import process has failed. File is invalid.",//_localizationSource.GetString("FileCantBeConvertedToUserList"),
                Abp.Notifications.NotificationSeverity.Warn));
        }
    }
}
