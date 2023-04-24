using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LockthreatCompliance.ContactTypes.Exporting;
using LockthreatCompliance.ContactTypes.Dtos;
using LockthreatCompliance.Dto;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;

namespace LockthreatCompliance.ContactTypes
{
    [AbpAuthorize]
    public class ContactTypesAppService : LockthreatComplianceAppServiceBase, IContactTypesAppService
    {
        private readonly IRepository<ContactType> _contactTypeRepository;
        private readonly IContactTypesExcelExporter _contactTypesExcelExporter;


        public ContactTypesAppService(IRepository<ContactType> contactTypeRepository, IContactTypesExcelExporter contactTypesExcelExporter)
        {
            _contactTypeRepository = contactTypeRepository;
            _contactTypesExcelExporter = contactTypesExcelExporter;

        }

        [AbpAllowAnonymous]
        public async Task<IReadOnlyList<ContactTypeDto>> GetAllForLookUp()
        {
            var res = await _contactTypeRepository.GetAll()
                .Select(e => new ContactTypeDto
                {
                    Id = e.Id,
                    Name = e.Name
                }).ToListAsync();
            return res.AsReadOnly();
        }

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_ContactTypes)]

        public async Task<PagedResultDto<GetContactTypeForViewDto>> GetAll(GetAllContactTypesInput input)
        {

            var filteredContactTypes = _contactTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter.Trim().ToLower()))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Trim().ToLower() == input.NameFilter.Trim().ToLower());

            var pagedAndFilteredContactTypes = filteredContactTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var contactTypes = from o in pagedAndFilteredContactTypes
                               select new GetContactTypeForViewDto()
                               {
                                   ContactType = new ContactTypeDto
                                   {
                                       Name = o.Name,
                                       Id = o.Id
                                   }
                               };

            var totalCount = await filteredContactTypes.CountAsync();

            return new PagedResultDto<GetContactTypeForViewDto>(
                totalCount,
                await contactTypes.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_ContactTypes)]
        public async Task<GetContactTypeForViewDto> GetContactTypeForView(int id)
        {
            var contactType = await _contactTypeRepository.GetAsync(id);

            var output = new GetContactTypeForViewDto { ContactType = ObjectMapper.Map<ContactTypeDto>(contactType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_ContactTypes_Edit)]
        public async Task<GetContactTypeForEditOutput> GetContactTypeForEdit(EntityDto input)
        {
            var contactType = await _contactTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetContactTypeForEditOutput { ContactType = ObjectMapper.Map<CreateOrEditContactTypeDto>(contactType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditContactTypeDto input)
        {
            if (input.Id == null)
            {
                var validate = await _contactTypeRepository.GetAll().Where(x => x.Name.Trim().ToLower() == input.Name.Trim().ToLower()).FirstOrDefaultAsync();
                if (validate == null)
                {
                    await Create(input);
                }
                else
                {
                    throw new UserFriendlyException("Contact Type Already Exist");
                }
            }
            else
            {
                var validate = await _contactTypeRepository.GetAll().Where(x => x.Name.Trim().ToLower() == input.Name.Trim().ToLower() && x.Id != input.Id).FirstOrDefaultAsync();
                if (validate == null)
                {
                    await Update(input);
                }
                else
                {
                    throw new UserFriendlyException("Contact Type Already Exist");
                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_ContactTypes_Create)]
        protected virtual async Task Create(CreateOrEditContactTypeDto input)
        {
            var contactType = ObjectMapper.Map<ContactType>(input);


            if (AbpSession.TenantId != null)
            {
                contactType.TenantId = (int?)AbpSession.TenantId;
            }


            await _contactTypeRepository.InsertAsync(contactType);
        }

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_ContactTypes_Edit)]
        protected virtual async Task Update(CreateOrEditContactTypeDto input)
        {
            var contactType = await _contactTypeRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, contactType);
        }

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_ContactTypes_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _contactTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetContactTypesToExcel(GetAllContactTypesForExcelInput input)
        {

            var filteredContactTypes = _contactTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var query = (from o in filteredContactTypes
                         select new GetContactTypeForViewDto()
                         {
                             ContactType = new ContactTypeDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });


            var contactTypeListDtos = await query.ToListAsync();

            return _contactTypesExcelExporter.ExportToFile(contactTypeListDtos);
        }


    }
}