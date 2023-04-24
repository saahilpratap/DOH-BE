

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LockthreatCompliance.FacilityTypes.Exporting;
using LockthreatCompliance.FacilityTypes.Dtos;
using LockthreatCompliance.Dto;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Uow;
using LockthreatCompliance.AuthoritativeDocuments;
using Abp.UI;
using LockthreatCompliance.BusinessEntities;

namespace LockthreatCompliance.FacilityTypes
{
    [AbpAuthorize]
    public class FacilityTypesAppService : LockthreatComplianceAppServiceBase, IFacilityTypesAppService
    {
        private readonly IRepository<FacilityType> _facilityTypeRepository;
        private readonly IFacilityTypesExcelExporter _facilityTypesExcelExporter;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<FacilitySubType> _facilitySubTypeRepository;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;

        public FacilityTypesAppService(IUnitOfWorkManager unitOfWorkManager, IRepository<BusinessEntity> businessEntityRepository, IRepository<FacilitySubType> facilitySubTypeRepository, IRepository<FacilityType> facilityTypeRepository, IFacilityTypesExcelExporter facilityTypesExcelExporter)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _facilityTypeRepository = facilityTypeRepository;
            _facilityTypesExcelExporter = facilityTypesExcelExporter;
            _facilitySubTypeRepository = facilitySubTypeRepository;
            _businessEntityRepository = businessEntityRepository;
        }


        [AbpAllowAnonymous]

        public async Task<PagedResultDto<GetFacilityTypeForViewDto>> GetAll(GetAllFacilityTypesInput input)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var filteredFacilityTypes = _facilityTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

                var pagedAndFilteredFacilityTypes = filteredFacilityTypes
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var facilityTypes = from o in pagedAndFilteredFacilityTypes
                                    select new GetFacilityTypeForViewDto()
                                    {
                                        FacilityType = new FacilityTypeDto
                                        {
                                           ControlType=o.ControlType.ToString(),
                                            Name = o.Name,
                                            Id = o.Id
                                        }
                                    };

                var totalCount = await filteredFacilityTypes.CountAsync();

                return new PagedResultDto<GetFacilityTypeForViewDto>(
                    totalCount,
                    await facilityTypes.ToListAsync()
                );
            }
        }

        [AbpAllowAnonymous]
        public async Task<List<GetFacilityTypeForViewDto>> GetAllFacilityType()
        {


            var facilityTypes = await (from o in _facilityTypeRepository.GetAll()
                                       select new GetFacilityTypeForViewDto()
                                       {
                                           FacilityType = new FacilityTypeDto
                                           {
                                               Name = o.Name,
                                               Id = o.Id
                                           }
                                       }).ToListAsync();

            return facilityTypes;

        }

        [AbpAllowAnonymous]
        public async Task<List<FacilityTypeDto>> GetAllFacilityTypes()
        { 
            var facilityTypes = await (from o in _facilityTypeRepository.GetAll()
                                       select new FacilityTypeDto()
                                       {
                                           Name = o.Name,
                                           Id = o.Id
                                       }).ToListAsync();

            return facilityTypes;

        }

        public async Task<GetFacilityTypeForViewDto> GetFacilityTypeForView(int id)
        {
            var facilityType = await _facilityTypeRepository.GetAsync(id);

            var output = new GetFacilityTypeForViewDto { FacilityType = ObjectMapper.Map<FacilityTypeDto>(facilityType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_FacilityTypes_Edit)]
        public async Task<GetFacilityTypeForEditOutput> GetFacilityTypeForEdit(EntityDto input)
        {
            var facilityType = await _facilityTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetFacilityTypeForEditOutput { FacilityType = ObjectMapper.Map<CreateOrEditFacilityTypeDto>(facilityType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditFacilityTypeDto input)
        {
            if (input.Id == null)
            {
                var validate = await  _facilityTypeRepository.GetAll().Where(x => x.Name.Trim().ToLower() == input.Name.Trim().ToLower()).FirstOrDefaultAsync();
                if (validate == null)
                {
                    await Create(input);
                }
                else
                {
                   throw new UserFriendlyException("Facility Name Already Exist");
                }
            }
            else
            {
                var validate = await  _facilityTypeRepository.GetAll().Where(x => x.Name.Trim().ToLower() == input.Name.Trim().ToLower() && x.Id != input.Id).FirstOrDefaultAsync();
                if (validate == null)
                {
                    await Update(input);
                }
                else
                {
                    throw new UserFriendlyException("Facility Name Already Exist");
                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_FacilityTypes_Create)]
        protected virtual async Task Create(CreateOrEditFacilityTypeDto input)
        {
            var facilityType = ObjectMapper.Map<FacilityType>(input);
            if (AbpSession.TenantId != null)
            {
                facilityType.TenantId = (int?)AbpSession.TenantId;
            }
            await _facilityTypeRepository.InsertAsync(facilityType);
        }

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_FacilityTypes_Edit)]
        protected virtual async Task Update(CreateOrEditFacilityTypeDto input)
        {
            var facilityType = await _facilityTypeRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, facilityType);
        }

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_FacilityTypes_Delete)]
        public async Task Delete(EntityDto input)
        {
            try
            {

                var check = await _facilitySubTypeRepository.GetAll().Where(x => x.FacilityTypeId == input.Id).FirstOrDefaultAsync();
                var checkbusinessentity = await _businessEntityRepository.GetAll().Where(x => x.FacilitySubTypeId == input.Id).FirstOrDefaultAsync();
                if (check == null && checkbusinessentity==null)
                {
                    await _facilityTypeRepository.DeleteAsync(input.Id);
                }
                else
                {
                    throw new UserFriendlyException("You can not delete this record");
                }
            }
            catch(Exception ex)
            {
                throw new UserFriendlyException("You can not delete this record");
            }
        }

        public async Task<FileDto> GetFacilityTypesToExcel(GetAllFacilityTypesForExcelInput input)
        {

            var filteredFacilityTypes = _facilityTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var query = (from o in filteredFacilityTypes
                         select new ImportFacilityTypes()
                         {
                                 ControlType = o.ControlType.ToString(),
                                 Name = o.Name,
                                 Id = o.Id,
                                 TenantId=o.TenantId
                             
                         });


            var facilityTypeListDtos = await query.ToListAsync();

            return _facilityTypesExcelExporter.ExportToFile(facilityTypeListDtos);
        }


    }
}