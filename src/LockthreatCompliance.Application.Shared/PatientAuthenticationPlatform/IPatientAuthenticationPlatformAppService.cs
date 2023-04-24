using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Domains.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.PatientAuthenticationPlatform.Dtos;

namespace LockthreatCompliance.PatientAuthenticationPlatform
{
    public interface IPatientAuthenticationPlatformAppService : IApplicationService
    {
        Task<bool>CreateOrEditPAPGlobalAttachment(CreateorEditPatientAuthenticationPlatformGlobalAttachmentDto input);

        Task<bool> CreatePAPwithSkip(CreateOrEditPatientAuthenticationPlatformDto input, int flag);
        Task<List<BusinessEntitiListDto>> CreateOrEdit(CreateOrEditPatientAuthenticationPlatformDto input, int flag);
        Task<List<PatientAuthenticationPlatformGlobalAttachmentDto>> GetAllPAPGlobalAttachemnt();
        Task<PagedResultDto<PatientAuthenticationPlatformListDto>> GetAll(GetAllPatientAuthenticationPlatformsInput input);
       // Task CreateOrEdit(CreateOrEditPatientAuthenticationPlatformDto input, int flag);
        Task Delete(int input);
        Task DeletePAPContactInformation(int input);
        Task<PatientAuthenticationPlatformDto> GetPatientAuthenticationPlatformById(int input);
        Task<List<BusinessEntitiListDto>> BusinessEntitiesByUserId(int input, int papId);
        Task<string> GetGroupName(BusinessEntitiListDto[] input);
    }
}