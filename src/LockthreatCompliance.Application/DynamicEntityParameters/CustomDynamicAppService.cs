using Abp.Domain.Repositories;
using Abp.DynamicEntityParameters;
using Abp.UI;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.DynamicEntityParameters
{
    public class CustomDynamicAppService : LockthreatComplianceAppServiceBase, ICustomDynamicAppService
    {
        private readonly IRepository<DynamicParameterValue> _dynamicParameterValueRepository;
        private readonly IRepository<DynamicParameter> _dynamicParameterManager;

        public CustomDynamicAppService(IRepository<DynamicParameterValue> dynamicParameterValueRepository, IRepository<DynamicParameter> dynamicParameterManager)
        {
            _dynamicParameterValueRepository = dynamicParameterValueRepository;
            _dynamicParameterManager = dynamicParameterManager;
        }

        public async Task<List<DynamicNameValueDto>> GetDynamicEntityDatabyName(string dynamicEntityName)
        {
            var getDynamicValues = new List<DynamicNameValueDto>();
            try
            {
                var getcheckId = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == dynamicEntityName.Trim().ToLower());
                if (getcheckId != null)
                {

                    var getother = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getcheckId.Id)
                        .Select(x => new DynamicNameValueDto()
                        {
                            Id = x.Id,
                            Name = x.Value,
                        }).ToListAsync();
                    if (getother.Count() != 0)
                    {
                        getDynamicValues = ObjectMapper.Map<List<DynamicNameValueDto>>(getother);
                    }
                    return getDynamicValues;
                }
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return getDynamicValues;
        }

        public async Task<List<DynamicNameValueDto>> GetAuditStatus(string dynamicEntityName)
        {
            var getDynamicValues = new List<DynamicNameValueDto>();
            var getother = new List<DynamicNameValueDto>();
            try
            {

                var getcheckId = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == dynamicEntityName.Trim().ToLower());
                if (getcheckId != null)
                {
                    var getAuditProjectStatus = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getcheckId.Id).ToListAsync();
                    getAuditProjectStatus.ForEach(obj =>
                    {
                        var items = new DynamicNameValueDto();
                        items.Name = obj.Value.Split('.').Skip(1).FirstOrDefault();
                        items.Id = obj.Id;
                        getother.Add(items);

                    });                 
                    if (getother.Count() != 0)
                    {
                        getDynamicValues = ObjectMapper.Map<List<DynamicNameValueDto>>(getother);
                    }
                    return getDynamicValues;
                }
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return getDynamicValues;
        }

    }
}
