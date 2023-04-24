using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using LockthreatCompliance.FacilitySubtypes.Dto;
using LockthreatCompliance.FacilityTypes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;

using LockthreatCompliance.Exceptions;
using LockthreatCompliance.FacilityTypes.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.FacilitySubtypes.Exporting;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.BusinessEntities;
using Abp.UI;

namespace LockthreatCompliance.FacilitySubtypes
{
    public class FacilitySubTypeAppService : LockthreatComplianceAppServiceBase, IFacilitySubTypeAppService
    {
        private readonly IRepository<FacilitySubType> _facilitySubTypeRepository;

        private readonly IRepository<FacilityType> _facilityTypeRepository;

        private readonly IFacilitySubTypesExcelExporter _facilitySubTypesExcelExporter;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        public FacilitySubTypeAppService(IRepository<FacilitySubType> facilitySubTypeRepository, IRepository<BusinessEntity> businessEntityRepository, IRepository<FacilityType> facilityTypeRepository, IFacilitySubTypesExcelExporter facilitySubTypesExcelExporter)
        {
            _facilityTypeRepository = facilityTypeRepository;
            _facilitySubTypeRepository = facilitySubTypeRepository;
            _facilitySubTypesExcelExporter = facilitySubTypesExcelExporter;
            _businessEntityRepository = businessEntityRepository;

        }

        public async Task CreateOrUpdateFacilitySubType(CreateorEditFacilitySubTypeDto input)
        {

            if (input.Id == 0)
            {
                var facilitySubType = ObjectMapper.Map<FacilitySubType>(input);
                if (AbpSession.TenantId != null)
                {
                    facilitySubType.TenantId = (int?)AbpSession.TenantId;
                }
                await _facilitySubTypeRepository.InsertAsync(facilitySubType);

            }
            else
            {
                var facilitySubType = await _facilitySubTypeRepository.FirstOrDefaultAsync((int)input.Id);
                ObjectMapper.Map(input, facilitySubType);
            }

        }

        public async Task<CreateorEditFacilitySubTypeDto> GetFacilityTypeForEdit(int input)
        {
            var output = new CreateorEditFacilitySubTypeDto();
            try
            {
                var facilityType = await _facilitySubTypeRepository.FirstOrDefaultAsync(input);
                output = ObjectMapper.Map<CreateorEditFacilitySubTypeDto>(facilityType);

                return output;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteFacilitySubType(int input)
        {
            try
            {
                
                var checkbusinessentity = await _businessEntityRepository.GetAll().Where(x => x.FacilitySubTypeId == input).FirstOrDefaultAsync();
                if ( checkbusinessentity == null)
                {
                    await _facilitySubTypeRepository.DeleteAsync(input);
                }
                {
                    throw new UserFriendlyException("You can not delete this record");
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException("You can not delete this record");
            }

        }

        public async Task<PagedResultDto<FacilitySubTypeList>> GetFacilitySubTypeList(FacilitySubTypeinputDto input)
        {
            try
            {
                var query = _facilitySubTypeRepository.GetAllIncluding().Include(x => x.FacilityType)
                             .WhereIf(input.Filter != null, x => x.FacilitySubTypeName.Contains(input.Filter.Trim().ToLower()))
                              .WhereIf(input.FacilityTypeId != 0, x => x.FacilityTypeId == input.FacilityTypeId)
                              .WhereIf(input.ControlTypeId != -1, x => x.ControlType == (ControlType)input.ControlTypeId);
                                

                var facilitysubType = await query.CountAsync();

                var facilitysubTypes = await query
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                var facilitysubTypeList = ObjectMapper.Map<List<FacilitySubTypeList>>(facilitysubTypes);

                return new PagedResultDto<FacilitySubTypeList>(
                   facilitysubType,
                   facilitysubTypeList.ToList()
                   );
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public async Task<List<FacilityTypeDto>> GetFacilityAll()
        {
            var facilityTypes = new List<FacilityTypeDto>();

            try
            {
                facilityTypes = await (from o in _facilityTypeRepository.GetAll()
                                       select new FacilityTypeDto()
                                       {
                                           Name = o.Name,
                                           Id = o.Id
                                       }).ToListAsync();
                return facilityTypes;
            }
            catch (System.Exception)
            {
                throw;
            }

        }

        public async Task<List<FacilitySubTypeDto>> GetFacilitySubtypeAll(int input)
        {
            var query = new List<FacilitySubTypeDto>();
            try
            {
                query = await (from o in _facilitySubTypeRepository.GetAll().Where(x => x.FacilityTypeId == input).Include(x => x.FacilityType)
                               select new FacilitySubTypeDto()
                               {
                                   Id = o.Id,
                                   FacilitySubTypeName = o.FacilitySubTypeName,
                                   FacilityTypeId = o.FacilityType.Id

                               }).ToListAsync();
                return query;
            }
            catch (System.Exception)
            {
                throw;
            }

        }

       public async Task<List<FacilitySubTypeDto>> GetAllFacilitysubTypesList(List<int> Input)
        {
            var query = new List<FacilitySubTypeDto>();
            try
            {
                query = await(from o in _facilitySubTypeRepository.GetAll().Where(x => Input.Contains((int)x.FacilityTypeId)).Include(x => x.FacilityType)
                              select new FacilitySubTypeDto()
                              {
                                  Id = o.Id,
                                  FacilitySubTypeName = o.FacilitySubTypeName,
                                  FacilityTypeId = o.FacilityType.Id
                              }).ToListAsync();
                return query;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<FileDto> GetFacilitySubTypesToExcel(FacilitySubTypeinputDto input)
        {

            var facilitySubTypes = _facilitySubTypeRepository.GetAllIncluding().Include(x => x.FacilityType)
                            .WhereIf(input.Filter != null, x => x.FacilitySubTypeName.Contains(input.Filter.Trim().ToLower()))
                             .WhereIf(input.FacilityTypeId != 0, x => x.FacilityTypeId == input.FacilityTypeId)
                             .WhereIf(input.ControlTypeId != -1, x => x.ControlType == (ControlType)input.ControlTypeId);

            var query = (from o in facilitySubTypes
                        select new ImportFacilitySubType()
                        {
                            ControlType = o.ControlType.ToString(),
                            FacilityTypeName = o.FacilityType.Name,
                            FacilitySubTypeName = o.FacilitySubTypeName,
                            Id = o.Id,
                            TenantId = o.TenantId

                        });


            var facilityTypeListDtos = await query.ToListAsync();

            return _facilitySubTypesExcelExporter.ExportToFile(facilityTypeListDtos);
        }

    }
}
