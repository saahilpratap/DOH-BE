using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using LockthreatCompliance.FacilitySubtypes.Dto;
using LockthreatCompliance.FacilityTypes.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.FacilitySubtypes
{
   public interface IFacilitySubTypeAppService: IApplicationService
    {
        Task<List<FacilitySubTypeDto>> GetAllFacilitysubTypesList(List<int> Input);
        Task<List<FacilitySubTypeDto>> GetFacilitySubtypeAll(int input);
        Task<List<FacilityTypeDto>> GetFacilityAll();
        Task CreateOrUpdateFacilitySubType(CreateorEditFacilitySubTypeDto input);
        Task<CreateorEditFacilitySubTypeDto> GetFacilityTypeForEdit(int input);
        Task DeleteFacilitySubType(int input);
        Task<PagedResultDto<FacilitySubTypeList>> GetFacilitySubTypeList(FacilitySubTypeinputDto input);
    }
}
