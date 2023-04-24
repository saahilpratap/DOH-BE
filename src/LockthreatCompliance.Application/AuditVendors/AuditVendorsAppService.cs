using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LockthreatCompliance.AuditVendors.Exporting;
using LockthreatCompliance.AuditVendors.Dtos;
using LockthreatCompliance.Dto;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.Organizations;
using LockthreatCompliance.Common;
using LockthreatCompliance.Enums;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.EntityFrameworkCore.RepositoryExtensions;
using LockthreatCompliance.CustomExceptions;
using LockthreatCompliance.Sessions;
using LockthreatCompliance.Authorization.Users;

namespace LockthreatCompliance.AuditVendors
{
    [AbpAuthorize(AppPermissions.Pages_AuditManagement_AuditVendors)]
    public class AuditVendorsAppService : LockthreatComplianceAppServiceBase, IAuditVendorsAppService
    {
        private readonly IRepository<AuditVendor> _auditVendorRepository;
        private readonly IAuditVendorsExcelExporter _auditVendorsExcelExporter;
        private readonly IEntityUserCreator _entityUserCreator;
        private readonly ApplicationSession _appSession;

        public AuditVendorsAppService(
            IRepository<AuditVendor> auditVendorRepository, 
            IAuditVendorsExcelExporter auditVendorsExcelExporter,
            IEntityUserCreator entityUserCreator,
            ApplicationSession appSession
            )
        {
            _entityUserCreator = entityUserCreator;
            _auditVendorRepository = auditVendorRepository;
            _auditVendorsExcelExporter = auditVendorsExcelExporter;
            _appSession = appSession;
        }

        public async Task<PagedResultDto<GetAuditVendorForViewDto>> GetAll(GetAllAuditVendorsInput input)
        {

            var filteredAuditVendors = _auditVendorRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Phone.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.Website.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.City.Contains(input.Filter) || e.State.Contains(input.Filter) || e.PostalCode.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(input.MinRegistrationDateFilter != null, e => e.RegistrationDate >= input.MinRegistrationDateFilter)
                        .WhereIf(input.MaxRegistrationDateFilter != null, e => e.RegistrationDate <= input.MaxRegistrationDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneFilter), e => e.Phone == input.PhoneFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email == input.EmailFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WebsiteFilter), e => e.Website == input.WebsiteFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address == input.AddressFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City == input.CityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateFilter), e => e.State == input.StateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PostalCodeFilter), e => e.PostalCode == input.PostalCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);
                        //WhereIf(_appSession.UserOriginType == UserOriginType.ExternalAuditor)

            var pagedAndFilteredAuditVendors = filteredAuditVendors
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var auditVendors = from o in pagedAndFilteredAuditVendors
                               select new GetAuditVendorForViewDto()
                               {
                                   AuditVendor = new AuditVendorDto
                                   {
                                       Name = o.Name,
                                       RegistrationDate = o.RegistrationDate,
                                       Phone = o.Phone,
                                       Email = o.Email,
                                       Website = o.Website,
                                       Address = o.Address,
                                       City = o.City,
                                       State = o.State,
                                       PostalCode = o.PostalCode,
                                       Description = o.Description,
                                       Id = o.Id
                                   }
                               };

            var totalCount = await filteredAuditVendors.CountAsync();

            return new PagedResultDto<GetAuditVendorForViewDto>(
                totalCount,
                await auditVendors.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_AuditManagement_AuditVendors_Edit)]
        public async Task<GetAuditVendorForEditOutput> GetAuditVendorForEdit(EntityDto input)
        {
            var auditVendor = await _auditVendorRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAuditVendorForEditOutput { AuditVendor = ObjectMapper.Map<CreateOrEditAuditVendorDto>(auditVendor) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAuditVendorDto input)
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

        [AbpAuthorize(AppPermissions.Pages_AuditManagement_AuditVendors_Create)]
        protected virtual async Task Create(CreateOrEditAuditVendorDto input)
        {
            //var auditVendor = ObjectMapper.Map<AuditVendor>(input);

            //var organizationUser = await _entityUserCreator.CreateAsync
            //    (
            //    auditVendor.Name,
            //    auditVendor.Email
            //    , AbpSession.TenantId,
            //    EntityType.ExternalAudit,
            //    auditVendor.Name,
            //    null,
            //    false
            //    );

            //auditVendor.OrganizationUnit = organizationUser.OrganizationUnit;
            //auditVendor.OrganizationUnitId = organizationUser.OrganizationUnit.Id;
            //if (AbpSession.TenantId != null)
            //{
            //    auditVendor.TenantId = (int?)AbpSession.TenantId;
            //}


            //await _auditVendorRepository.InsertAsync(auditVendor);
        }


        [AbpAuthorize(AppPermissions.Pages_AuditManagement_AuditVendors_Edit)]
        protected virtual async Task Update(CreateOrEditAuditVendorDto input)
        {
            var auditVendor = await _auditVendorRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, auditVendor);
        }


        [AbpAuthorize(AppPermissions.Pages_AuditManagement_AuditVendors_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _auditVendorRepository.DeleteAsync(input.Id);
        }


        public async Task<FileDto> GetAuditVendorsToExcel(GetAllAuditVendorsForExcelInput input)
        {

            var filteredAuditVendors = _auditVendorRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Phone.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.Website.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.City.Contains(input.Filter) || e.State.Contains(input.Filter) || e.PostalCode.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(input.MinRegistrationDateFilter != null, e => e.RegistrationDate >= input.MinRegistrationDateFilter)
                        .WhereIf(input.MaxRegistrationDateFilter != null, e => e.RegistrationDate <= input.MaxRegistrationDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneFilter), e => e.Phone == input.PhoneFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email == input.EmailFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WebsiteFilter), e => e.Website == input.WebsiteFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address == input.AddressFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City == input.CityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateFilter), e => e.State == input.StateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PostalCodeFilter), e => e.PostalCode == input.PostalCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var query = (from o in filteredAuditVendors
                         select new GetAuditVendorForViewDto()
                         {
                             AuditVendor = new AuditVendorDto
                             {
                                 Name = o.Name,
                                 RegistrationDate = o.RegistrationDate,
                                 Phone = o.Phone,
                                 Email = o.Email,
                                 Website = o.Website,
                                 Address = o.Address,
                                 City = o.City,
                                 State = o.State,
                                 PostalCode = o.PostalCode,
                                 Description = o.Description,
                                 Id = o.Id
                             }
                         });


            var auditVendorListDtos = await query.ToListAsync();

            return _auditVendorsExcelExporter.ExportToFile(auditVendorListDtos);
        }

        public async Task Activate(EntityDto input)
        {
            var auditVendor = await _auditVendorRepository.GetIncluding(e => e.Id == input.Id, "OrganizationUnit");
            if (auditVendor == null)
            {
                throw new NotFoundException($"Couldn't find Audit Entity with ID {input.Id}");
            }
            auditVendor.Activate();
        }

        public async Task Deactivate(EntityDto input)
        {
            var auditVendor = await _auditVendorRepository.GetIncluding(e => e.Id == input.Id, "OrganizationUnit");
            if (auditVendor == null)
            {
                throw new NotFoundException($"Couldn't find Audit Entity with ID {input.Id}");
            }
            auditVendor.Deactivate();
        }
    }
}