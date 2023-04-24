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
using LockthreatCompliance.Contacts.Dtos;
using Abp.Domain.Uow;
using LockthreatCompliance.Url;
using Abp.Localization;
using Abp.Threading;
using System.Threading.Tasks;
using LockthreatCompliance.ContactTypes;
using System.Linq.Dynamic.Core;
using System.Linq;

namespace LockthreatCompliance.Contacts.Importing
{
    public class ImportContactValueToExcelJob : BackgroundJob<ImportUsersFromExcelJobArgs>, ITransientDependency
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

        private readonly IRepository<Contact> _contactRepository;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IRepository<ContactType> _contactTypeRepository;
        private readonly IContactListExcelDataReader _contactListExcelDataReader;     
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ImportContactValueToExcelJob(
             IRepository<PreRegisterBusinessEntity> preRegisterEntityRepository, IRepository<BusinessEntity> businessRepository,
            IAppNotifier appNotifier, IRepository<User, long> userRepository, IBinaryObjectManager binaryObjectManager, IRepository<Country> countriesRepository, ILocalizationManager localizationManager, IObjectMapper objectMapper, IPreEntityListExcelDataReader preEntityListExcelDataReader,
            IPasswordHasher<User> passwordHasher, RoleManager roleManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            OrganizationUnitManager organizationUnitManager, IUserEmailer userEmailer,
            IRepository<UserOriginity> userOriginityRepository, IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<Contact> contactRepository,
            IRepository<BusinessEntity> businessEntityRepository,
            IRepository<ContactType> contactTypeRepository,
            IContactListExcelDataReader contactListExcelDataReader,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _contactRepository = contactRepository;
            _businessEntityRepository = businessEntityRepository;
            _contactTypeRepository = contactTypeRepository;
            _contactListExcelDataReader = contactListExcelDataReader;
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
                var entities = GetContactFromExcelOrNull(args);
                if (entities == null || entities.Count() ==0)
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                CreateContact(args, entities);
            }
        }

        private List<ImportContactDto> GetContactFromExcelOrNull(ImportUsersFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _contactListExcelDataReader.GetContactFromExcel(file.Bytes, args.TenantId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void CreateContact(ImportUsersFromExcelJobArgs args, List<ImportContactDto> entities)
        {
            foreach (var item in entities)
            {
                var checkBusinessEntity = _businessEntityRepository.GetAll().Where(x=>x.Id == item.BusinessEntityId).FirstOrDefault();
                var checkContactType = _contactTypeRepository.GetAll().Where(x => x.Id == item.ContactTypeId).FirstOrDefault();
                var checkEmail = _contactRepository.GetAll().Where(x => x.Email == item.Email).FirstOrDefault();
                if(checkBusinessEntity != null)
                {
                    if(checkContactType !=null)
                    {
                        if(checkEmail == null)
                        {
                            var contact = new Contact {
                                TenantId = args.TenantId,
                                FirstName =item.FirstName,
                                LastName=item.LastName,
                                JobTitle=item.JobTitle,
                                Mobile=item.Mobile,
                                DirectPhone=item.DirectPhone,
                                CompanyName=item.CompanyName,
                                BusinessEntityId=item.BusinessEntityId,
                                ContactTypeId=item.ContactTypeId,
                                ContactOwnerType= (ContactOwnerType)item.ContactOwnerType,
                                Email =item.Email

                            };
                            _contactRepository.Insert(contact);
                        }
                    }
                }

            }
            AsyncHelper.RunSync(() => ProcessImportUsersResultAsync(args));
        }

        private async Task ProcessImportUsersResultAsync(ImportUsersFromExcelJobArgs args)
        {
            if (args.User != null)
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    "All Contact Entities Import Done.", //_localizationSource.GetString("AllUsersSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
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
