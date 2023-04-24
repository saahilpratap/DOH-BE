using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using LockthreatCompliance.FindingReports.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.FindingReports
{
    public interface  IFindingReportAppService : IApplicationService
    {
        Task<FindingListDto> FindingCAPAApproved(AllclosedCapaDto input);
        Task<FindingListDto> FindingCAPAAccept(long AuditProjectId, FindingReportCategory category);
        Task<FindingListDto> FinalCAPASubmitted (long AuditProjectId, FindingReportCategory category);
        Task<bool> SetFindingStatus(long findingId, int statusId);
        Task<bool> AllcapaClosed(AllclosedCapaDto input);
        Task DeleteFinding(long FindingId, long AuditProjectId);
        Task<FindingStatusWiesShowBtnDto> GetCheckCAPASubmittedandCapaApprovedForFinding(long AuditProjectId, FindingReportCategory category);
       
      
        //Task<bool> SetCAPASubmited(long AuditProjectId, FindingReportCategory category);
        Task<bool> SetCapaApproved(long AuditProjectId, FindingReportCategory category);
        Task<bool> SetCapaAccept(long AuditProjectId, FindingReportCategory category);

        Task<AuditStausWiesShowButton> GetcheckAuditStatus(long AuditProjectId);
        Task<List<int>> GetCAPASubmited(List<FindingReportDto> input);
        Task<int> GetcheckFinding(int controlRequirementId, int assessmentId);
        Task<PagedResultDto<FindingReportDto>> GetAllFindingReportRelatedAuditProject(GetAllFindingReportsInput input);
        Task CreateOrEdit(FindingInputDto input);
        Task<GetFindingReportDtoForView> GetFindingReportForEdit(EntityDto input);

        Task<PagedResultDto<FindingReportDto>> GetAll(GetAllFindingReportsInput input);

        Task<List<DynamicNameValueDto>> GetDynamicEntityFindingStatus(string dynamicEntityName);
        Task<ExternalAuditorAndAuditeeDto> IsExternalAuditorAndAuditee(long findingId);
        Task<PagedResultDto<FindingReportLogDto>> GetAllFindingLogs(GetAllFindingReportLogInput input);
        Task<List<LatestFindingByEntitIdDto>> GetUpdatedFindingByEntity(int assessmentId);

        Task<FileDto> GetAllFindingByAuditProjects(GetAllFindingByFilterInput input);

        Task<FileDto> GetAllFindingCAPAByAuditProjects(GetAllFindingByFilterInput input);

    }
}
