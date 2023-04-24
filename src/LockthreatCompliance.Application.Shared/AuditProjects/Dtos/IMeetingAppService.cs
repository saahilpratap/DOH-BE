using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.BusinessEntities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.AuditProjects.Dtos
{
    public interface IMeetingAppService : IApplicationService
    {
        Task<List<BusinessEntityDto>> GetAuditProjectByOrganization(long AuditProjectId);
        Task<List<AuditProjectLink>> GetAuditProjectByVendor(int vendorId);
        Task<PagedResultDto<AuditMeetingDto>> GetAuditMeetings(GetAllMeetings input);
        Task AddUpdateAuditMeeting(AuditMeetingDto input);

        Task<AuditMeetingDto> GetAuditMeetingForEdit(long id);
        Task DeleteAuditMeeting(long id);
        Task<string> GetAuditMeetingPdf(long id);
    }
}
