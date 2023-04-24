using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.AuditProjectGroups;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.AuditQuestResponses;
using LockthreatCompliance.AuditReports.Dto;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.CertificateQRCode.Dto;
using LockthreatCompliance.Contacts.Dtos;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using LockthreatCompliance.Enums;
using LockthreatCompliance.FindingReports;
using LockthreatCompliance.FindingReports.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.AuditProjects
{
    public interface IAuditProjectCloneAppService : IApplicationService
    {

        Task<CreateCloneAuditProjectDto> GetCloneForAuditProject(CloneAuditProjectInputDto input);
        Task<CreateCloneAuditProjectDto> GetCloneAuditProject(CreateCloneAuditProjectDto input);        
        Task<List<ReviewGroupDto>> CalculateReviewGroup(CreateCloneAuditProjectDto input);
        Task CreateAuditProjectClone(CreateCloneAuditProjectDto input);
        Task<GetRestrictedEntitiesOutputDto> GetRestrictedEntities(long input);

    }
}
