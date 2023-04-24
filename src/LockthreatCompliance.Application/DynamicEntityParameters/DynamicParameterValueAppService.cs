using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.DynamicEntityParameters;
using Microsoft.AspNetCore.Authorization;
using LockthreatCompliance.Authorization;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using Abp.Domain.Repositories;
using LockthreatCompliance.Dto;
using LockthreatCompliance.DynamicEntityParameters.Exporting;
using Abp.Collections.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace LockthreatCompliance.DynamicEntityParameters
{
    [Authorize(AppPermissions.Pages_Administration_DynamicParameterValue)]
    public class DynamicParameterValueAppService : LockthreatComplianceAppServiceBase, IDynamicParameterValueAppService
    {
        private readonly IDynamicParameterValueManager _dynamicParameterValueManager;
        private readonly IDynamicParameterValueStore _dynamicParameterValueStore;
        private readonly IDynamicParameterExcelExporter _dynamicParameterExcelExporter;
        private readonly IRepository<DynamicParameterValue> _dynamicParameterValueRepository;
        private readonly IRepository<DynamicParameter> _dynamicParameterRepository;
        public DynamicParameterValueAppService(IDynamicParameterValueManager dynamicParameterValueManager, IDynamicParameterValueStore dynamicParameterValueStore,IDynamicParameterExcelExporter dynamicParameterExcelExporter, IRepository<DynamicParameterValue> dynamicParameterValueRepository, IRepository<DynamicParameter> dynamicParameterRepository)
        {
            _dynamicParameterValueManager = dynamicParameterValueManager;
            _dynamicParameterValueStore = dynamicParameterValueStore;
            _dynamicParameterExcelExporter = dynamicParameterExcelExporter;
            _dynamicParameterValueRepository = dynamicParameterValueRepository;
            _dynamicParameterRepository = dynamicParameterRepository;
        }

        public FileDto GetDynamicParaValueEntitiesToExcel()
        {
            var dynamicParameterValueListDtos = (from p in _dynamicParameterValueRepository.GetAll()
                                                 join e in _dynamicParameterRepository.GetAll()
                                                 on p.DynamicParameterId equals e.Id
                                                 select new ImportDynamicParameterValueDto
                                                 {  
                                                     Id=p.Id,
                                                     TenantId = p.TenantId,
                                                     EntityFullName = p.Value,
                                                     DynamicParameterId = p.DynamicParameterId,
                                                     DynamicParameterName = e.ParameterName
                                                 }).ToList();
            return _dynamicParameterExcelExporter.ExportDynamicParameterValueToFile(ObjectMapper.Map<List<DynamicParameterValueExcelDto>>(dynamicParameterValueListDtos));

        }

        public async Task<DynamicParameterValueDto> Get(int id)
        {
            var entity = await _dynamicParameterValueManager.GetAsync(id);
            return ObjectMapper.Map<DynamicParameterValueDto>(entity);
        }

        public async Task<ListResultDto<DynamicParameterValueDto>> GetAllValuesOfDynamicParameter(EntityDto input)
        {
            var entities = await _dynamicParameterValueStore.GetAllValuesOfDynamicParameterAsync(input.Id);
            return new ListResultDto<DynamicParameterValueDto>(
                ObjectMapper.Map<List<DynamicParameterValueDto>>(entities)
            );
        }

        [Authorize(AppPermissions.Pages_Administration_DynamicParameterValue_Create)]
        public async Task Add(DynamicParameterValueDto dto)
        {
            dto.TenantId = AbpSession.TenantId;
            await _dynamicParameterValueManager.AddAsync(ObjectMapper.Map<DynamicParameterValue>(dto));
        }

        [Authorize(AppPermissions.Pages_Administration_DynamicParameterValue_Edit)]
        public async Task Update(DynamicParameterValueDto dto)
        {
            dto.TenantId = AbpSession.TenantId;
            await _dynamicParameterValueManager.UpdateAsync(ObjectMapper.Map<DynamicParameterValue>(dto));
        }

        [Authorize(AppPermissions.Pages_Administration_DynamicParameterValue_Delete)]
        public async Task Delete(int id)
        {
            await _dynamicParameterValueManager.DeleteAsync(id);
        }
    }
}
