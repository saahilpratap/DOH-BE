

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LockthreatCompliance.Contacts.Exporting;
using LockthreatCompliance.Contacts.Dtos;
using LockthreatCompliance.Dto;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using static LockthreatCompliance.Authorization.Roles.StaticRoleNames.Tenants;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.Sessions;
using LockthreatCompliance.Authorization.Users;
using Abp.UI;
using Abp.DynamicEntityParameters;
using Twilio.TwiML.Messaging;
using LockthreatCompliance.ContactTypes;
using LockthreatCompliance.Common;

namespace LockthreatCompliance.Contacts
{
    [AbpAuthorize(AppPermissions.Pages_Contacts)]
    public class ContactsAppService : LockthreatComplianceAppServiceBase, IContactsAppService
    {
        private readonly ICommonLookupAppService _commonlookupManagerRepository;
        private readonly IRepository<ContactType> _contactTypeRepositry ;
        private readonly IRepository<Contact> _contactRepository;
        private readonly IContactsExcelExporter _contactsExcelExporter;
        private readonly RoleManager _roleManager;
        private readonly ApplicationSession _appSession;


        public ContactsAppService(ApplicationSession appSession, IRepository<ContactType> contactTypeRepositry, ICommonLookupAppService commonlookupManagerRepository,
            IRepository<Contact> contactRepository, IContactsExcelExporter contactsExcelExporter, RoleManager roleManager)
        {
            _contactTypeRepositry = contactTypeRepositry;
            _appSession = appSession;
            _roleManager = roleManager;
            _contactRepository = contactRepository;
            _commonlookupManagerRepository = commonlookupManagerRepository;
            _contactsExcelExporter = contactsExcelExporter;

        }

        public async Task<PagedResultDto<GetContactForViewDto>> GetAll(GetAllContactsInput input)
        {
            var currentUser = await GetCurrentUserAsync();
            var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();

            var filteredContacts = _contactRepository.GetAll().Include(x => x.ContactType).Include(x => x.BusinessEntity)
                              .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                        // .WhereIf(_appSession.UserOriginType == UserOriginType.BusinessEntity || _appSession.UserOriginType == UserOriginType.ExternalAuditor, e => e.BusinessEntityId == GetCurrentUser().BusinessEntityId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BusinessEntity.CompanyName.Contains(input.Filter) || e.FirstName.Contains(input.Filter) || e.LastName.Contains(input.Filter) || e.JobTitle.Contains(input.Filter) || e.Mobile.Contains(input.Filter) || e.DirectPhone.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => ("CON -" + e.Id).Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter), e => e.FirstName == input.FirstNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LastNameFilter), e => e.LastName == input.LastNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobTitleFilter), e => e.JobTitle == input.JobTitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MobileFilter), e => e.Mobile == input.MobileFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DirectPhoneFilter), e => e.DirectPhone == input.DirectPhoneFilter);
                        
            var pagedAndFilteredContacts = filteredContacts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var contacts = from o in pagedAndFilteredContacts
                           select new GetContactForViewDto()
                           {
                               Contact = new ContactDto
                               {
                                   Code = o.Code,
                                   FirstName = o.FirstName,
                                   LastName = o.LastName,
                                   JobTitle = o.JobTitle,
                                   Mobile = o.Mobile,
                                   DirectPhone = o.DirectPhone,
                                   Id = o.Id,
                                   Email = o.Email,
                                   BusinessEntityId = o.BusinessEntityId,
                                   ContactType = o.ContactType.Name
                               }
                           };

            var totalCount = await filteredContacts.CountAsync();

            return new PagedResultDto<GetContactForViewDto>(
                totalCount,
                await contacts.ToListAsync()
            );
        }

        public async Task<GetContactForViewDto> GetContactForView(int id)
        {
            var contact = await _contactRepository.GetAsync(id);

            var output = new GetContactForViewDto { Contact = ObjectMapper.Map<ContactDto>(contact) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Contacts_Edit)]
        public async Task<GetContactForEditOutput> GetContactForEdit(EntityDto input)
        {
            var contact = await _contactRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetContactForEditOutput { Contact = ObjectMapper.Map<CreateOrEditContactDto>(contact) };

            return output;
        }

        public async Task<List<ContactDto>> GetAllContact(List<int> BusinessEntityId)
        {
            var query = new List<ContactDto>();
            var getId = new List<int>();
            try
            {
                getId = await _contactTypeRepositry.GetAll().Where(l => l.Name.ToLower().Trim() == ("General Contacts").ToLower().Trim() || (l.Name.ToLower().Trim() == ("Technical Contacts").ToLower().Trim())).Select(x => x.Id).ToListAsync();

              //  getId = await _dynamicParameterValueRepositry.GetAll().Where(x =>x.Value.Equals('General Contacts').ToString() && x.TenantId==AbpSession.TenantId).Select(x => x.Id).ToListAsync();

                query = await _contactRepository.GetAll().Where(x => getId.Contains((int)x.ContactTypeId) && BusinessEntityId.Contains(x.BusinessEntityId)).
                           Select(x => new ContactDto()
                           {
                               FullName = x.FirstName + " " + x.LastName+"-"+x.BusinessEntity.CompanyName,
                               Id = x.Id,
                               BusinessEntityId = x.BusinessEntityId,
                               ContactType=x.ContactType.Name
                           }).ToListAsync();

                return query;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CreateOrEdit(CreateOrEditContactDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Contacts_Create)]
        protected virtual async Task Create(CreateOrEditContactDto input)
        {
            var contact = ObjectMapper.Map<Contact>(input);


            if (AbpSession.TenantId != null)
            {
                contact.TenantId = (int?)AbpSession.TenantId;
            }


            await _contactRepository.InsertAsync(contact);
        }

        [AbpAuthorize(AppPermissions.Pages_Contacts_Edit)]
        protected virtual async Task Update(CreateOrEditContactDto input)
        {
            var contact = await _contactRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, contact);
        }

        [AbpAuthorize(AppPermissions.Pages_Contacts_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _contactRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetContactsToExcel(GetAllContactsForExcelInput input)
        {

            var filteredContacts = _contactRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.FirstName.Contains(input.Filter) || e.LastName.Contains(input.Filter) || e.JobTitle.Contains(input.Filter) || e.Mobile.Contains(input.Filter) || e.DirectPhone.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter), e => e.FirstName == input.FirstNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LastNameFilter), e => e.LastName == input.LastNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobTitleFilter), e => e.JobTitle == input.JobTitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MobileFilter), e => e.Mobile == input.MobileFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DirectPhoneFilter), e => e.DirectPhone == input.DirectPhoneFilter);

            var query = (from o in filteredContacts
                         select new ImportContactDto()
                         {
                            
                                 Code = o.Code,
                                 TenantId =o.TenantId,
                                 FirstName = o.FirstName,
                                 LastName = o.LastName,
                                 JobTitle = o.JobTitle,
                                 Mobile = o.Mobile,
                                 DirectPhone = o.DirectPhone,
                                 CompanyName =o.CompanyName,
                                 BusinessEntityId=o.BusinessEntityId,
                                 ContactTypeId= (int)o.ContactTypeId,
                                 ContactOwnerType= (int)o.ContactOwnerType,
                                 Email=o.Email
                         });


            var contactListDtos = await query.ToListAsync();

            return _contactsExcelExporter.ExportToFile(contactListDtos);
        }


        public async Task<List<ContactDto>> GetContactByBusinessEntity(int Id)
        {
            var getdata = new List<ContactDto>();
            try
            {
                getdata =await _contactRepository.GetAll().Where(x => x.BusinessEntityId == Id).Select(x => new ContactDto()
                             {
                                 FirstName=x.FirstName,
                                 LastName=x.LastName,                                
                                   JobTitle = x.JobTitle,
                                   Mobile = x.Mobile,
                                   DirectPhone = x.DirectPhone,                                 
                                   Email = x.Email
                              }).ToListAsync();

                return getdata;
            }
            catch(Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

    }
}