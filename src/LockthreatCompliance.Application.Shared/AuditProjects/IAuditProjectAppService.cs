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
    public interface IAuditProjectAppService : IApplicationService
    {
        Task<bool> GetCheckStatusAuditProject(long AuditProjectId);
        Task<List<BusinessEntitiesListDto>> GetAuditReportEntity(long AuditProjectId);
        Task CreateExternalAssessmentAuditProject(List<BusinessEnityGroupWiesDto> input, long AuditProjectId);
        Task<PagedResultDto<GetCertificateImport>> GetCertificateImport(GetAllAuditProjectInput input);
        Task<List<GetAllComplianceAuditDto>> GetComplianceAuditSummary(long AuditProjectId);
      
        Task<List<CreateAndUpdateAuditRequestClarification>> GetAllClarificationAuditProject(long AuditProjectId);
        Task RequestAndResponseAuditProjectClarification(List<CreateAndUpdateAuditRequestClarification> input);
       Task SendnotificationForAuditProject(AuditProjectStatusIds input);
        Task AuditQuestResponseAndAuditStatusUpdate(List<AuditQuestResponseDto> input);
        Task<bool> GetcheckAuditQuestionButton(long AuditProjectid);
        Task<bool> GetCheckFileAndQuesgtionGenerated(long AuditProjectid);
        Task<List<ReportFileUploadDto>> GetReports(long AuditProjectId);
        Task GetCheckFinialCAPASubmited();
        Task GetCheckCAPASubmited();
        Task<int> AuditProjectStatusId();
        Task<bool> AuditProjectAccepted(long auditProjectId);
        Task<CountEmailSendDto> SendnotificationtoEntity(List<AuditProjectDto> items);
        Task SetAuditStatusEntityNotify(long auditProjectId);
        Task<bool> GetCheckAuditProjectStatus(long auditProjectId);
        Task<List<AuditQuestResponseDto>> GetAllAuditQuestResponseByAuditProjectId(int input, int groupId);
        Task<List<QuestionGroupListDto>> GetQuestionaryGroupAll(List<int> input);
        Task<AuditProjectGroupDto> GetAuditProjectGroup(long Id);
        Task<PagedResultDto<AuditProjectDto>> GetAuditProjects(GetAllAuditProject input);
        Task AddUpdateAuditProject(AuditProjectDto input);
      //  Task<List<QuestionGroupListDto>> GetQuestionaryGroupAll();
        Task<AuditProjectDto> GetAuditProjectForEdit(long id);
        Task DeleteAuditProject(long id);
        Task<ExternalAssessmentListDto> GetAllExternalEntityByAuditProjectId(int input);
        Task<GetAllFindingForAuditProjectOutputDto> GetAllFindingForAuditProject(GetAllFindingForAuditProjectInputDto input);
        Task<AuditProjectReportOutputDto> GetAuditProjectReportForAuditProject(int input);

        Task CreateOrUpdateAuditQuestResponse(List<AuditQuestResponseDto> input);
        Task<CorrectiveActionPlanWithBusinessEntityDto> GetCorrectiveActionByAuditProjectId(int input);
        Task<AuditProjectPdfDto> AuditProjectPdfById(long input);
        Task<List<AuditProjectWithBusinessEntityFacility>> GetAuditProjectBusinessEntityFacilities();

        Task<List<BusinessEntityUserDto>> GetAuditProjetUsers(int? groupId, long auditProjectId);

         Task<List<ContactDto>> GetContactUsersint(int? groupId, int? businessEntityId);

        Task<FindingReportStageOneDto> FindingReportStageWise(long id, FindingReportCategory type);
        Task<CorrectiveActionReportStageOneDto> CorrectiveActionReportStageWise(long id, FindingReportCategory type);
        Task<CertificationProposalReportDto> CertificationProposalReport(long id);
        Task<List<ReportFileUploadDto>> GetEntityCertificate(long AuditProjectId);
        Task<PagedResultDto<GetCertificateImport>> GetCertificateImportByLicenseNumber(GetCertificateImportByLicenseNumberInput input);
        Task SendAuditProjectCertificate(List<CertificateQRCodeDto> inputdata, int auditProject);
        Task UpdateAccessPermissionField(long auditProjectId, AccessPermission accessPermission);
        //Task<List<long>> ReauditPermissionDateCkekers();
        Task<List<IdAndPermissionDto>> ReauditPermissionDateCkekers();
        Task<List<IdAndPermissionDto>> ReauditPermissionCkeker();
        Task<List<AuditProjectDto>> AuditFilter(int input);

    }
}
