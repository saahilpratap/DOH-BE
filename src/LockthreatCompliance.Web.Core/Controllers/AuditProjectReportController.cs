using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Exceptions;
using LockthreatCompliance.Exceptions.Dtos;
using LockthreatCompliance.Incidents;
using Microsoft.AspNetCore.Mvc;
using LockthreatCompliance.Incidents.Dtos;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.AssessmentSchedules;
using LockthreatCompliance.AuditReports;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.BusinessEntities.Dtos;
using Abp.Application.Services.Dto;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.FindingReports;
using Abp.Domain.Repositories;
using Abp.DynamicEntityParameters;
using Microsoft.EntityFrameworkCore;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.Domains;

namespace LockthreatCompliance.Web.Controllers
{

    public class AuditProjectReportController : LockthreatComplianceControllerBase
    {
        private IConverter _converter;
        private readonly IExceptionsAppService _iExceptionsAppServiceRepository;
        private readonly IIncidentsAppService _incidentsAppServiceRepository;
        private readonly IAuditProjectAppService _auditProjectAppService;
        private readonly IExtAssementScheduleAppService _extAssementScheduleAppService;
        private readonly IAuditReportAppService _auditReportAppService;
        private readonly IBusinessEntitiesAppService _businessEntitiesAppService;
        private readonly IRepository<DynamicParameterValue> _dynamicParameterValueRepository;
        private readonly IRepository<AuditProject,long> _auditProjectRepository;
        private readonly IRepository<ExternalAssessment> _externalAssessmentRepository;
        private readonly IRepository<ReviewData> _reviewDataRepository;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IRepository<FindingReport> _findingReportRepository;
        private readonly IRepository<AuditReport> _auditReportRepository;
        private readonly IRepository<AuditReportEntities> _auditReportEntitiesrepository;
        private readonly IRepository<AuditTeamSignature> _auditTeamSignaturerepository;
        private readonly IRepository<Domain> _domainsrepository;
        private readonly IRepository<AuditReports.ComplianceAuditSummary> _ComplianceAuditSummaryrepository;
        private readonly IRepository<AuditReportFacility> _auditReportFacilityRepository;
        public AuditProjectReportController(IConverter converter, IIncidentsAppService incidentsAppServiceRepository,
            IExceptionsAppService iExceptionsAppServiceRepository, IAuditProjectAppService auditProjectAppService,
            IExtAssementScheduleAppService extAssementScheduleAppService, IAuditReportAppService auditReportAppService,
            IBusinessEntitiesAppService businessEntitiesAppService, IRepository<DynamicParameterValue> dynamicParameterValueRepository, IRepository<AuditProject, long> auditProjectRepository
            , IRepository<ExternalAssessment> externalAssessmentRepository, IRepository<ReviewData> reviewDataRepository, IRepository<BusinessEntity> businessEntityRepository, IRepository<FindingReport> findingReportRepository,
            IRepository<AuditReport> auditReportRepository, IRepository<AuditReportEntities> auditReportEntitiesrepository,
            IRepository<AuditTeamSignature> auditTeamSignaturerepository, IRepository<Domain> domainsrepository, IRepository<AuditReports.ComplianceAuditSummary> ComplianceAuditSummaryrepository,
            IRepository<AuditReportFacility> auditReportFacilityRepository)
        {
            _iExceptionsAppServiceRepository = iExceptionsAppServiceRepository;
            _incidentsAppServiceRepository = incidentsAppServiceRepository;
            _auditProjectAppService = auditProjectAppService;
            _extAssementScheduleAppService = extAssementScheduleAppService;
            _auditReportAppService = auditReportAppService;
            _businessEntitiesAppService = businessEntitiesAppService;
            _converter = converter;
            _auditProjectAppService = auditProjectAppService;
            _externalAssessmentRepository = externalAssessmentRepository;
            _reviewDataRepository = reviewDataRepository;
            _businessEntityRepository = businessEntityRepository;
            _auditProjectRepository = auditProjectRepository;
            _dynamicParameterValueRepository = dynamicParameterValueRepository;
            _findingReportRepository = findingReportRepository;
            _auditReportRepository = auditReportRepository;
            _auditReportEntitiesrepository = auditReportEntitiesrepository;
            _auditTeamSignaturerepository = auditTeamSignaturerepository;
            _domainsrepository = domainsrepository;
            _ComplianceAuditSummaryrepository = ComplianceAuditSummaryrepository;
            _auditReportFacilityRepository = auditReportFacilityRepository;
        }

        public async Task<FindingReportStageOneDto> GetAuditProjectFindingStage1(long Id)
        {
            //var report = new CorrectiveActionPlanReport();

            FindingReportStageOneDto result = new FindingReportStageOneDto();
            var obj = await _auditProjectAppService.FindingReportStageWise(Id, FindingReportCategory.Stage1);
            result = obj;
            return result;
        }

        public async Task<FindingReportStageOneDto> GetAuditProjectFindingStage2(long Id)
        {
            FindingReportStageOneDto result = new FindingReportStageOneDto();
            var obj = await _auditProjectAppService.FindingReportStageWise(Id, FindingReportCategory.Stage2);
            result = obj;
            return result;
        }

        public async Task<CertificationProposalReportDto> CertificationProposalReport(long Id)
        {
            CertificationProposalReportDto result = new CertificationProposalReportDto();
            var obj = await _auditProjectAppService.CertificationProposalReport(Id);
            result = obj;
            return result;
        }

        public async Task<CorrectiveActionReportStageOneDto> GetAuditProjectCorrectiveActionStage1(long Id)
        {
            CorrectiveActionReportStageOneDto result = new CorrectiveActionReportStageOneDto();
            var obj = await _auditProjectAppService.CorrectiveActionReportStageWise(Id, FindingReportCategory.Stage1);
            result = obj;
            return result;
        }

        public async Task<CorrectiveActionReportStageOneDto> GetAuditProjectCorrectiveActionStage2(long Id)
        {
            CorrectiveActionReportStageOneDto result = new CorrectiveActionReportStageOneDto();
            var obj = await _auditProjectAppService.CorrectiveActionReportStageWise(Id, FindingReportCategory.Stage2);
            result = obj;
            return result;
        }

        public async Task<AuditProjectReport1Dto> GetAuditProject1(long Id)
        {
            AuditProjectReport1Dto result = new AuditProjectReport1Dto();
            var documentTypes = await _extAssementScheduleAppService.GetAssessmentTypes();
            var oldAuditProject = await _auditProjectAppService.GetAuditProjectForEdit(Id);
            var auditProjectInfo = await _auditProjectAppService.AuditProjectPdfById(Id);
            var auditReport = await _auditReportAppService.GetAuditReportInfoByAuditProjectId(Id);
            var facilityNames = await _auditProjectAppService.GetAuditProjectGroup(Id);

            var facilityCount = facilityNames.BusinessEntity.Count();

            var auditTeamStateInfo = auditReport.AuditReportTeamStageList;
            var stage1Info = new List<string>();
            var stage2Info = auditTeamStateInfo.GroupBy(x => x.DominName).Select(x => new { name = x, list = x.ToList() }).ToList();

            var totalStage2Count = stage2Info.Sum(x => x.list.Count());

            stage2Info.ForEach(x =>
            {
                var templist = x.list.Select(y => y.ControlRequirement).ToList();
                templist.ForEach(y =>
                {
                    stage1Info.Add(y);
                });
            });

            result.OldAuditProject = oldAuditProject;
            result.AuditProjectDto = auditProjectInfo;
            result.DocumentTypes = documentTypes;
            result.FacilityNames = facilityNames;
            result.AuditReport = auditReport;

            return result;
        }

        public async Task<AuditProjectReportDto> GetAuditProject2(long Id)
        {
            List<ReviewDataReportDto> result = new List<ReviewDataReportDto>();
            List<ReviewDataReportDto> result2 = new List<ReviewDataReportDto>();
            var query = await _auditProjectRepository.GetAll().Where(a => a.Id == Id).
                                Include(a => a.Country)
                               .Include(a => a.AuthDocuments)
                               .Include(a => a.Actors).
                                Include(a => a.AuditType)
                               .Include(a => a.AuditCoordinator).
                                Include(a => a.AuditStatus)
                               .Include(x => x.AuditProjectQuestionGroup)
                               .Include(a => a.AuditArea).
                               Include(a => a.AuditManager).
                               Include(a => a.LeadAuditor)
                              .Include(a => a.LeadAuditee).FirstOrDefaultAsync();


            AuditProjectReportDto auditProjectReportDto = new AuditProjectReportDto();
            auditProjectReportDto.AuditTitle = query.AuditTitle;
            auditProjectReportDto.AuditType = query.AuditStageId == null ? "" : _dynamicParameterValueRepository.GetAll().Where(x => x.Id == query.AuditStageId).Select(x => x.Value).FirstOrDefault();
            auditProjectReportDto.Stage1StartDate = query.StartDate == null ? "" : Convert.ToDateTime(query.StartDate).ToString("dd/mm/yyyy");
            auditProjectReportDto.Stage1EndDate = query.EndDate == null ? "" : Convert.ToDateTime(query.EndDate).ToString("dd/mm/yyyy");
            auditProjectReportDto.Stage2StartDate = query.StageStartDate == null ? "" : Convert.ToDateTime(query.StageStartDate).ToString("dd/mm/yyyy");
            auditProjectReportDto.Stage2EndDate = query.StageEndDate == null ? "" : Convert.ToDateTime(query.StageEndDate).ToString("dd/mm/yyyy");
            auditProjectReportDto.LeadAuditorName = query.LeadAuditor == null ? "" : query.LeadAuditor.Name;
            auditProjectReportDto.NumberOfAuditor = query.Actors == null ? 0 : query.Actors.Where(x=>x.AuditProjectTeamUserType == AuditProjectTeamUserType.AuditorTeam).Count();
            var getExternalAssessment = _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity).Include(x => x.EntityGroup).Where(x => x.AuditProjectId == query.Id).ToList();
            if (getExternalAssessment.Count() == 1)
            {
                var getBusinessEntity = getExternalAssessment.FirstOrDefault();
                auditProjectReportDto.BusinessEntityName = getBusinessEntity.BusinessEntity == null ? "" : getBusinessEntity.BusinessEntity.LicenseNumber + "-" + getBusinessEntity.BusinessEntity.CompanyName;
               if(getBusinessEntity.BusinessEntity != null)
                {
                    var AdminDetails = new AuditManagementNameDto();
                    AdminDetails.UserName = "" + getBusinessEntity.BusinessEntity.AdminName + "-" + getBusinessEntity.BusinessEntity.AdminSurname;
                    AdminDetails.Position = "" + getBusinessEntity.BusinessEntity.AdminPosition;
                    var backUpContactDetails = new AuditManagementNameDto();
                    backUpContactDetails.UserName = "" + getBusinessEntity.BusinessEntity.BackupContactName;
                    backUpContactDetails.Position = "" + getBusinessEntity.BusinessEntity.BackupDesignation;
                    var primaryContactDetails = new AuditManagementNameDto();
                    primaryContactDetails.UserName = "" + getBusinessEntity.BusinessEntity.PrimaryContactName;
                    primaryContactDetails.Position = "" + getBusinessEntity.BusinessEntity.Designation;
                    auditProjectReportDto.auditManagementNameDto.Add(AdminDetails);
                    auditProjectReportDto.auditManagementNameDto.Add(backUpContactDetails);
                    auditProjectReportDto.auditManagementNameDto.Add(primaryContactDetails);
                }
            }
            else
            {
                var getBusinessEntity = getExternalAssessment.FirstOrDefault();
                auditProjectReportDto.BusinessEntityName = getBusinessEntity.EntityGroup == null ? "" : getBusinessEntity.EntityGroup.Name;
                if(getBusinessEntity.EntityGroup != null)
                {
                    var getbusinessEntity2 = _businessEntityRepository.GetAll().Where(x => x.Id == getBusinessEntity.EntityGroup.PrimaryEntityId).FirstOrDefault();
                    var AdminDetails = new AuditManagementNameDto();
                    AdminDetails.UserName =""+ getbusinessEntity2.AdminName + "-" + getbusinessEntity2.AdminSurname;
                    AdminDetails.Position =""+getbusinessEntity2.AdminPosition;
                    var backUpContactDetails = new AuditManagementNameDto();
                    backUpContactDetails.UserName =""+ getbusinessEntity2.BackupContactName;
                    backUpContactDetails.Position =""+ getbusinessEntity2.BackupDesignation;
                    var primaryContactDetails = new AuditManagementNameDto();
                    primaryContactDetails.UserName =""+ getbusinessEntity2.PrimaryContactName;
                    primaryContactDetails.Position =""+ getbusinessEntity2.Designation;
                    auditProjectReportDto.auditManagementNameDto.Add(AdminDetails);
                    auditProjectReportDto.auditManagementNameDto.Add(backUpContactDetails);
                    auditProjectReportDto.auditManagementNameDto.Add(primaryContactDetails);
                }
                
            }

            foreach (var item in getExternalAssessment)
            {

                var tempList = _reviewDataRepository.GetAll().Include(x => x.ControlRequirement).Where(x => x.ExternalAssessmentId == item.Id)
                     .Select(x => new
                     {
                         DomainName = x.ControlRequirement.DomainName,
                         ResponseType = x.ResponseType,
                         UpdatedResponseType = x.UpdatedResponseType,
                         Comment = x.Comment,
                         UpdatedMarks = (x.UpdatedResponseType == ReviewDataResponseType.FullyCompliant || x.UpdatedResponseType == ReviewDataResponseType.NotApplicable) ? 100 :
                                (x.UpdatedResponseType == ReviewDataResponseType.NotSelected || x.UpdatedResponseType == ReviewDataResponseType.NonCompliant) ? 0 : 50,
                         Marks = (x.ResponseType == ReviewDataResponseType.FullyCompliant || x.ResponseType == ReviewDataResponseType.NotApplicable) ? 100 :
                                (x.ResponseType == ReviewDataResponseType.NotSelected || x.ResponseType == ReviewDataResponseType.NonCompliant) ? 0 : 50
                     }).ToList();

                if (tempList.Count() != 0)
                {
                    //result = tempList.GroupBy(x => x.DomainName).Select(y => new ReviewDataReportDto
                    //{
                    //    DomainName = y.Key.ToString(),
                    //    ResponsePercent = (int)Math.Round(Convert.ToDecimal(y.Sum(x => x.Marks) / y.Count())),
                    //    CapaResponsePercent = (int)Math.Round(Convert.ToDecimal(y.Sum(x => x.UpdatedMarks) / y.Count())),
                    //    Comment = y.FirstOrDefault().Comment
                    //}).ToList();

                    //result2 = tempList.GroupBy(x => x.DomainName).Select(y => new ReviewDataReportDto
                    //{
                    //    DomainName = y.Key.ToString(),
                    //    ResponsePercent = (int)Math.Round(Convert.ToDecimal(y.Sum(x => x.Marks) / y.Count())),
                    //    CapaResponsePercent = (int)Math.Round(Convert.ToDecimal(y.Sum(x => x.UpdatedMarks) / y.Count())),
                    //    Comment = y.FirstOrDefault().Comment
                    //}).Skip(1).OrderBy(x => int.Parse(x.DomainName.Split('-')[0].Split(' ')[1])).ToList();

                    var totalPercentage = 0;

                    //foreach(var item2 in result2)
                    //{
                    //    totalPercentage = totalPercentage + item2.ResponsePercent;
                    //}
                    auditProjectReportDto.consolatedPercentage = ""+(totalPercentage /result2.Count());
                    auditProjectReportDto.reviewDataReportDto = result2;                 
                   auditProjectReportDto.stageOnereviewDataReportDto = result.Where(p => !result2.Any(l => p.DomainName == l.DomainName)).ToList();
                }
            }
            return auditProjectReportDto;

        }


        public async Task<FindingReportAllStageDto> FindingReportAllStageWise(long id)
        {
            var result = new FindingReportAllStageDto();
            var businessEntityList = _businessEntityRepository.GetAll().ToList();
            var auditProject = _auditProjectRepository.GetAll().Include(x => x.LeadAuditor).Include(x => x.AuthDocuments).ThenInclude(x => x.AuthoritativeDocument).Include(x => x.EntityGroup).Where(x => x.Id == id).FirstOrDefault();
            var externalAssessmentList = _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == id).Include(x => x.BusinessEntity).ToList();
            var externalAssessmentIds = externalAssessmentList.Select(x => x.Id).ToList();

            externalAssessmentList.ForEach(y =>
            {
                var temp = new BusinessEntityDto();
                temp.Name = y.BusinessEntity == null ? null : y.BusinessEntity.CompanyName;
                temp.Id = y.BusinessEntity == null ? 0 : y.BusinessEntity.Id;
                temp.LicenseNumber = y.BusinessEntity == null ? null : y.BusinessEntity.LicenseNumber;
                result.BusinessEntityList.Add(temp);

            });

            var findingList = _findingReportRepository.GetAll().Where(x => externalAssessmentIds.Contains((int)x.AssessmentId))
                .Include(x => x.BusinessEntity).Include(x => x.ControlRequirement).Include(x => x.Assessment).ThenInclude(x => x.Reviews)
                .Include(x => x.ControlRequirement).ThenInclude(x => x.ControlStandard)
                .Include(x => x.ControlRequirement).ThenInclude(x => x.ControlStandard).ThenInclude(x => x.Domain)
                                     .Select(x => new AllStageFinding
                                     {
                                         No = "" + x.Id,
                                         Section = "" + x.ControlRequirement.ControlStandard.DomainId,
                                         ControlRef = x.ControlRequirement.OriginalId,
                                         AuditQuestionSubject = x.Title,
                                         EntityComplaiance = (x.Assessment.Reviews.Where(y => y.ControlRequirementId == x.ControlRequirementId).FirstOrDefault() != null)
                                         ? "" + ((ReviewDataResponseType)x.Assessment.Reviews.Where(y => y.ControlRequirementId == x.ControlRequirementId).FirstOrDefault().ResponseType).ToString() : "",
                                         FindingDescription = "" + x.OtherCategoryName,
                                         FindingReference = "" + x.Reference,
                                         DomainName = x.ControlRequirement.DomainName,
                                         StageType = x.Category.ToString()
                                     }).ToList();

            var listA = findingList.Where(x => !x.DomainName.ToLower().Contains("Domain ".ToLower())).ToList();
            var ListB = findingList.Where(x => x.DomainName.ToLower().Contains("Domain ".ToLower())).OrderBy(x => int.Parse(x.DomainName.Split('-')[0].Split(' ')[1])).ToList();
            var listAstageOne = new List<AllStageFinding>();
            var listBstageOne = new List<AllStageFinding>();
            var list3stageOne = new List<AllStageFinding>();
            for (int i = 0; i < listA.Count(); i++)
            {
                var temp = listA[i];
                temp.Section = "A";
                if (listA[i].FindingDescription != "" && listA[i].FindingDescription != null)
                {
                    var splitString = listA[i].FindingDescription.Split('`');
                    temp.FindingDescription = splitString[0];
                    temp.FindingReference = listA[i].FindingReference;
                    listAstageOne.Add(temp);
                }
            }

            for (int i = 0; i < ListB.Count(); i++)
            {
                var temp = ListB[i];
                temp.Section = "B";
                if (ListB[i].FindingDescription != "" && ListB[i].FindingDescription != null)
                {
                    var splitString = ListB[i].FindingDescription.Split('`');
                    temp.FindingDescription = splitString[0];
                    temp.FindingReference = ListB[i].FindingReference;
                    listBstageOne.Add(temp);
                }
            }

            list3stageOne.AddRange(listAstageOne);
            list3stageOne.AddRange(listBstageOne);
            int srno = 1;
            foreach (var item in list3stageOne)
            {
                item.SrNo = srno++;
                result.StageAllFindingInfo.Add(item);
            }

            if (auditProject != null)
            {
                result.Date = System.DateTime.Now.ToString("dd/MM/yyyy");
                result.LeadAuditor = auditProject.LeadAuditor != null ? auditProject.LeadAuditor.FullName : "";
                result.Audit = auditProject.AuditTitle;
                result.GroupName = (auditProject.EntityGroup != null) ? auditProject.EntityGroup.Name : "" + externalAssessmentList.FirstOrDefault().BusinessEntity.CompanyName;
                result.Standard = (auditProject.AuthDocuments != null) ? auditProject.AuthDocuments.FirstOrDefault().AuthoritativeDocument.Name : "";
                result.LicenseNo = (auditProject.EntityGroup != null) ? businessEntityList.Where(x => x.Id == auditProject.EntityGroup.PrimaryEntityId).FirstOrDefault().LicenseNumber :
                    externalAssessmentList.FirstOrDefault().BusinessEntity.LicenseNumber;
                if (auditProject.EntityGroup != null)
                {
                    var getPrimaryEntity = _businessEntityRepository.GetAll().Where(x => x.Id == auditProject.EntityGroup.PrimaryEntityId).FirstOrDefault();
                    result.MgntRepresentative = getPrimaryEntity != null ? getPrimaryEntity.PrimaryContactName : "";
                }
                else
                {
                    result.MgntRepresentative = externalAssessmentList.FirstOrDefault().BusinessEntity.PrimaryContactName;
                }
            }
            return result;
        }

        public async Task<AuditProjectReportDto> GetAuditProject3(long Id)
        {
            List<ReviewDataReportDto> result = new List<ReviewDataReportDto>();
            List<ReviewDataReportDto> result2 = new List<ReviewDataReportDto>();
            List<ReviewDataReportDto> result3 = new List<ReviewDataReportDto>();
            List<ReviewDataReportDto> result4 = new List<ReviewDataReportDto>();
            var query = _auditProjectRepository.GetAll().Where(a => a.Id == Id).
                               Include(a => a.Country)
                              .Include(a => a.AuthDocuments)
                              .Include(a => a.Actors).
                               Include(a => a.AuditType)
                              .Include(a => a.AuditCoordinator).
                               Include(a => a.AuditStatus)
                              .Include(x => x.AuditProjectQuestionGroup)
                              .Include(a => a.AuditArea).
                              Include(a => a.AuditManager).
                              Include(a => a.LeadAuditor)
                             .Include(a => a.LeadAuditee).FirstOrDefault();
            var getAuditTeam = _auditReportRepository.GetAll().Where(x => x.AuditProjectId == Id).FirstOrDefault();
            var getAuditTeamSign = _auditTeamSignaturerepository.GetAll().Include(x => x.User).Where(x => x.AuditProjectId == Id).ToList();
            AuditProjectReportDto auditProjectReportDto = new AuditProjectReportDto();
            auditProjectReportDto.AuditTitle = query.AuditTitle;
            auditProjectReportDto.AuditType = query.AuditStageId == null ? "" : _dynamicParameterValueRepository.GetAll().Where(x => x.Id == query.AuditStageId).Select(x => x.Value).FirstOrDefault();
            auditProjectReportDto.Stage1StartDate = query.StartDate == null ? "" : Convert.ToDateTime(query.StartDate).ToString("dd/MM/yyyy");
            auditProjectReportDto.Stage1EndDate = query.EndDate == null ? "" : Convert.ToDateTime(query.EndDate).ToString("dd/MM/yyyy");
            auditProjectReportDto.Stage2StartDate = query.StageStartDate == null ? "" : Convert.ToDateTime(query.StageStartDate).ToString("dd/MM/yyyy");
            auditProjectReportDto.Stage2EndDate = query.StageEndDate == null ? "" : Convert.ToDateTime(query.StageEndDate).ToString("dd/MM/yyyy");
            auditProjectReportDto.LeadAuditorName = query.LeadAuditor == null ? "" : query.LeadAuditor.FullName;
            auditProjectReportDto.NumberOfAuditor = query.Actors == null ? 0 : query.Actors.Where(x => x.AuditProjectTeamUserType == AuditProjectTeamUserType.AuditorTeam).Count();
            auditProjectReportDto.NumberOfAuditor = getAuditTeam == null ? 0 : getAuditTeam.NumberofAuditors;
            auditProjectReportDto.AuditConclusion = getAuditTeam == null ? "" : getAuditTeam.AuditConclusions;
            auditProjectReportDto.AuditClosure = getAuditTeam == null ? "" : getAuditTeam.ClosureFinding;
            auditProjectReportDto.AreaOfImprovement = getAuditTeam == null ? "" : getAuditTeam.AreaImprovement;
            auditProjectReportDto.AuditMethodology = query.RemoteDesktopAudit == false ? "Onsite" : "Remote Audit";
            auditProjectReportDto.Performance1 = getAuditTeam == null ? "" : getAuditTeam.Performance1;
            auditProjectReportDto.Performance2 = getAuditTeam == null ? "" : getAuditTeam.Performance2;

            var PreparedBy = getAuditTeamSign == null ? null : getAuditTeamSign.Where(x => x.Type == BusinessEntityWorkflowActorType.Approver && x.Signature != null).FirstOrDefault();
            auditProjectReportDto.PreparedBy = PreparedBy == null ? "" : PreparedBy.Name;
            auditProjectReportDto.PreparedBySign = PreparedBy == null ? "" : PreparedBy.Signature;
            auditProjectReportDto.PreparedByDate = PreparedBy == null ? "" : PreparedBy.CreationTime.ToString("dd/MM/yyyy");

            var ReviewedBy = getAuditTeamSign == null ? null : getAuditTeamSign.Where(x => x.Type == BusinessEntityWorkflowActorType.Reviewer && x.Signature != null).FirstOrDefault();
            auditProjectReportDto.ReviewedBy = ReviewedBy == null ? "" : ReviewedBy.Name;
            auditProjectReportDto.ReviewedBySign = ReviewedBy == null ? "" : ReviewedBy.Signature;
            auditProjectReportDto.ReviewedByDate = ReviewedBy == null ? "" : ReviewedBy.CreationTime.ToString("dd/MM/yyyy");

            var AcknowledgedBy = getAuditTeamSign == null ? null : getAuditTeamSign.Where(x => x.Type == BusinessEntityWorkflowActorType.Authority && x.Signature != null).FirstOrDefault();
            auditProjectReportDto.AcknowledgedBy = AcknowledgedBy == null ? "" : AcknowledgedBy.Name;
            auditProjectReportDto.AcknowledgedBySign = AcknowledgedBy == null ? "" : AcknowledgedBy.Signature;
            auditProjectReportDto.AcknowledgedByDate = AcknowledgedBy == null ? "" : AcknowledgedBy.CreationTime.ToString("dd/MM/yyyy");

            var getExternalAssessment = _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity).Include(x => x.EntityGroup).Include(x => x.Reviews).Where(x => x.AuditProjectId == query.Id).ToList();
            var getSampledEntities = _auditReportEntitiesrepository.GetAll().Include(x => x.BusinessEntity).Where(x => x.AuditProjectId == Id).ToList();
            if (getExternalAssessment.Count() == 1)
            {
                var getBusinessEntity = getExternalAssessment.FirstOrDefault();
                auditProjectReportDto.BusinessEntityName = getBusinessEntity.BusinessEntity == null ? "" : getBusinessEntity.BusinessEntity.LicenseNumber + "-" + getBusinessEntity.BusinessEntity.CompanyName;
                auditProjectReportDto.FacilityGroup = "Not Applicable";
                auditProjectReportDto.NotinFacilityGroup = "Not Applicable";

            }
            else
            {
                var getBusinessEntity = getExternalAssessment.FirstOrDefault();
                auditProjectReportDto.BusinessEntityName = getBusinessEntity.EntityGroup == null ? "" : getBusinessEntity.EntityGroup.Name;
                if (getBusinessEntity.EntityGroup != null)
                {

                    if (getSampledEntities.Count() <= 10)
                    {
                        foreach (var item in getSampledEntities)
                        {
                            if (item.Sampled == true)
                            {
                                auditProjectReportDto.NotinFacilityGroup += item.BusinessEntity.CompanyName + ",";
                            }

                            auditProjectReportDto.FacilityGroup += item.BusinessEntity.CompanyName + ", ";

                        }
                    }
                    else
                    {
                        foreach (var item in getSampledEntities)
                        {
                            if (item.Sampled == true)
                            {
                                auditProjectReportDto.NotinFacilityGroup += item.BusinessEntity.LicenseNumber + ",";
                            }

                            auditProjectReportDto.FacilityGroup += item.BusinessEntity.LicenseNumber + ", ";
                        }
                    }
                }

            }
            auditProjectReportDto.auditManagementNameDto = _auditReportFacilityRepository.GetAll().Where(x => x.AuditProjectId == Id).Select(x => new AuditManagementNameDto()
            {
                Position = x.Position,
                UserName = x.Name,
                FacilityType = x.Facility
            }).ToList();




            foreach (var item in getExternalAssessment.Where(x => x.Reviews.Count() != 0))
            {
                var getSectionAReview = _reviewDataRepository.GetAll().Include(x => x.ControlRequirement).Where(x => x.ResponseType == ReviewDataResponseType.NonCompliant && x.ExternalAssessmentId == item.Id && x.ControlRequirement.DomainName.Trim().ToLower() == "Section A".Trim().ToLower()).Select(x => new
                {
                    assessmentId = x.ExternalAssessmentId,
                    controlRequirementId = x.ControlRequirementId
                }).ToList();
                var sectionACount = 0;
                var sectionBCount = 0;
                var getSectionAfinding = _findingReportRepository.GetAll().Include(x => x.ControlRequirement).Where(x => (x.Status == FindingReportStatus.CapaOpen || x.Status == FindingReportStatus.New) && x.AssessmentId == item.Id && x.ControlRequirement.DomainName.Trim().ToLower() == "Section A".Trim().ToLower()).ToList();
                if (getSectionAReview.Count() != 0 && getSectionAfinding.Count() != 0)
                {
                    sectionACount = getSectionAfinding.Where(x => getSectionAReview.Any(Z => x.ControlRequirementId == Z.controlRequirementId)).Count();
                }
                var getSectionBReview = _reviewDataRepository.GetAll().Include(x => x.ControlRequirement).Where(x => x.ResponseType == ReviewDataResponseType.NonCompliant && x.ExternalAssessmentId == item.Id && x.ControlRequirement.DomainName.Trim().ToLower() != "Section A".Trim().ToLower()).Select(x => new
                {
                    assessmentId = x.ExternalAssessmentId,
                    controlRequirementId = x.ControlRequirementId
                }).ToList();

                var getSectionBfinding = _findingReportRepository.GetAll().Include(x => x.ControlRequirement).Where(x => (x.Status == FindingReportStatus.CapaOpen || x.Status == FindingReportStatus.New) && x.AssessmentId == item.Id && x.ControlRequirement.DomainName.Trim().ToLower() != "Section A".Trim().ToLower()).ToList();
                if (getSectionBReview.Count() != 0 && getSectionBfinding.Count() != 0)
                {
                    sectionBCount = getSectionBfinding.Where(x => getSectionBReview.Any(Z => x.ControlRequirementId == Z.controlRequirementId)).Count();
                }

                var sectionA = new ReviewDataReportDto();
                sectionA.DomainName = "Section A";
                sectionA.ResponsePercent = "" + sectionACount;
                sectionA.Comment = "Reference -Stage 1 Audit Findings Section A";
                auditProjectReportDto.stageOnereviewDataReportDto.Add(sectionA);

                var sectionB = new ReviewDataReportDto();
                sectionB.DomainName = "Section B";
                sectionB.ResponsePercent = "" + sectionBCount;
                sectionB.Comment = "Reference- Stage 1 Audit Findings Section B";
                auditProjectReportDto.stageOnereviewDataReportDto.Add(sectionB);

                var tempList = _reviewDataRepository.GetAll().Include(x => x.ControlRequirement).Where(x => x.ExternalAssessmentId == item.Id && x.ResponseType != ReviewDataResponseType.NotSelected)
                     .Select(x => new
                     {
                         DomainName = x.ControlRequirement.DomainName,
                         ResponseType = x.ResponseType,
                         UpdatedResponseType = x.UpdatedResponseType,
                         Comment = x.Comment,
                         UpdatedMarks = x.UpdatedResponseType == ReviewDataResponseType.FullyCompliant ? "100" : x.UpdatedResponseType == ReviewDataResponseType.PartiallyCompliant ? "50" : x.UpdatedResponseType == ReviewDataResponseType.NonCompliant ? "0" : "",
                         Marks = x.ResponseType == ReviewDataResponseType.FullyCompliant ? "100" : x.ResponseType == ReviewDataResponseType.PartiallyCompliant ? "50" : x.ResponseType == ReviewDataResponseType.NonCompliant ? "0" : "",
                     }).ToList();

                if (tempList.Count() != 0)
                {

                    result2 = tempList.GroupBy(x => x.DomainName).Select(y => new ReviewDataReportDto
                    {
                        DomainName = y.Key.ToString(),
                        ResponsePercent = "" + (int)Math.Round(Convert.ToDecimal(y.Where(x => x.Marks != "").Sum(x => Convert.ToInt32(x.Marks)) / y.Count())) + ".00",
                        CapaResponsePercent = ("" + (int)Math.Round(Convert.ToDecimal((y.Where(x => x.UpdatedMarks != "").Sum(x => Convert.ToInt32(x.UpdatedMarks)) == 0 ? -1 : y.Where(x => x.UpdatedMarks != "").Sum(x => Convert.ToInt32(x.UpdatedMarks))) / (y.Where(x => x.UpdatedMarks != "").Count() == 0 ? -1 : y.Where(x => x.UpdatedMarks != "").Count()))) + ".00") == "1.00" ? "" : "" + (int)Math.Round(Convert.ToDecimal((y.Where(x => x.UpdatedMarks != "").Sum(x => Convert.ToInt32(x.UpdatedMarks)) == 0 ? -1 : y.Where(x => x.UpdatedMarks != "").Sum(x => Convert.ToInt32(x.UpdatedMarks))) / (y.Where(x => x.UpdatedMarks != "").Count() == 0 ? -1 : y.Where(x => x.UpdatedMarks != "").Count()))) + ".00",
                        Comment = ""
                    }).Where(x => x.DomainName.ToLower().Contains("Domain ".ToLower())).OrderBy(x => int.Parse(x.DomainName.Split('-')[0].Split(' ')[1])).ToList();



                    var getDomain = _domainsrepository.GetAll().Where(x => x.Name.Trim().ToLower() != "Section A".Trim().ToLower()).ToList();

                    var reviewDataReportDtoList = new List<ReviewDataReportDto>();
                    foreach (var item3 in getDomain)
                    {
                        var reviewDataReportDto = new ReviewDataReportDto();
                        foreach (var item4 in result2)
                        {

                            if (item3.Name == item4.DomainName)
                            {
                                var getCompliance = _ComplianceAuditSummaryrepository.GetAll().Include(x => x.Domain).Where(x => x.Domain.Name.Trim().ToLower() == item4.DomainName.Trim().ToLower() && x.AuditProjectId == Id).ToList().FirstOrDefault();
                                reviewDataReportDto.DomainName = item4.DomainName;
                                reviewDataReportDto.ResponsePercent = item4.ResponsePercent;
                                reviewDataReportDto.CapaResponsePercent = item4.CapaResponsePercent;
                                reviewDataReportDto.Comment = getCompliance == null ? "" : getCompliance.Description;
                            }
                            else
                            {
                                reviewDataReportDto.DomainName = item3.Name;
                            }
                        }
                        reviewDataReportDtoList.Add(reviewDataReportDto);
                    }

                    decimal totalPercentage = 0;
                    decimal totalCapaRespPercentage = 0;
                    foreach (var item2 in result2)
                    {
                        if (item2.ResponsePercent != "")
                        {
                            totalPercentage = totalPercentage + Convert.ToDecimal(item2.ResponsePercent);
                        }
                    }
                    foreach (var item2 in result2)
                    {
                        if (item2.CapaResponsePercent != "")
                        {
                            totalCapaRespPercentage = totalCapaRespPercentage + Convert.ToDecimal(item2.CapaResponsePercent);
                        }
                    }
                    var finalCaparesponsePerctage = "";
                    if (totalCapaRespPercentage != 0)
                    {
                        finalCaparesponsePerctage = "" + (totalCapaRespPercentage / result2.Where(x => x.CapaResponsePercent != "").Count());
                    }
                    auditProjectReportDto.Recommendation = finalCaparesponsePerctage;
                    auditProjectReportDto.consolatedPercentage = "" + decimal.Round((totalPercentage / result2.Count()), 2, MidpointRounding.AwayFromZero);
                    auditProjectReportDto.ScoreDesc = "Your score assigned after the audit is " + auditProjectReportDto.consolatedPercentage + "%. The passing score is 86%, however the score assigned after evaluation of Corrective action plan is " + finalCaparesponsePerctage + "%, and the technical committee might approve the certificate under condition of implementing the noncompliance findings within 90 days.Implementation of corrective action plan will be verified and evaluated and is subject to revoke of certification in case noncompliance is identified during such evaluation. The score is indicative of your level of compliance and it’s not a public domain information.";
                    auditProjectReportDto.reviewDataReportDto = reviewDataReportDtoList.Where(x => x.DomainName.ToLower().Contains("Domain ".ToLower())).OrderBy(x => int.Parse(x.DomainName.Split('-')[0].Split(' ')[1])).ToList();
                    //auditProjectReportDto.stageOnereviewDataReportDto = result.Where(p => !result2.Any(l => p.DomainName == l.DomainName)).ToList();
                }
            }
            return auditProjectReportDto;
        }

        public async Task<CorrectiveActionReportStageOneDto> CorrectiveActionReportStageAll(long id)
        {
            int[] domainList = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            var result = new CorrectiveActionReportStageOneDto();
            var businessEntityList = _businessEntityRepository.GetAll().ToList();
            var auditProject = _auditProjectRepository.GetAll().Include(x => x.LeadAuditor).Include(x => x.AuthDocuments).ThenInclude(x => x.AuthoritativeDocument).Include(x => x.EntityGroup).Where(x => x.Id == id).FirstOrDefault();
            var externalAssessmentList = _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == id).Include(x => x.BusinessEntity).ToList();
            var externalAssessmentIds = externalAssessmentList.Select(x => x.Id).ToList();

            var extAssessment = externalAssessmentList.FirstOrDefault(e => e.AuditProjectId == auditProject.Id);
            if (extAssessment != null)
            {
                result.AuditVendor = extAssessment.Vendor == null ? "" : extAssessment.Vendor.CompanyName;
            }
            externalAssessmentList.ForEach(y =>
            {

                var temp = new BusinessEntityDto();
                temp.Name = y.BusinessEntity.CompanyName;
                temp.Id = y.BusinessEntity.Id;
                temp.LicenseNumber = y.BusinessEntity.LicenseNumber;
                result.BusinessEntityList.Add(temp);
            });

            var findingList = _findingReportRepository.GetAll().Where(x => x.Category == FindingReportCategory.Stage1 || x.Category == FindingReportCategory.Stage2).Include(x => x.AssignedToUser).Include(x => x.FindingCoordinator).Include(x => x.FindingManager)
                .Include(x => x.FindingOwner)
                 .Where(x => externalAssessmentIds.Contains((int)x.AssessmentId))
                .Include(x => x.BusinessEntity).Include(x => x.Assessment).ThenInclude(x => x.Reviews)
                .Include(x => x.ControlRequirement).ThenInclude(x => x.ControlStandard)
                .Include(x => x.ControlRequirement).ThenInclude(x => x.ControlStandard).ThenInclude(x => x.Domain)
                                     .Select(x => new StageOneCorrectiveAction
                                     {
                                         No = "FND-" + x.Id,
                                         Section = "" + x.ControlRequirement.ControlStandard.DomainId,
                                         ControlRef = x.ControlRequirement.OriginalId,
                                         RootCause = x.OtherCategoryName,
                                         ActualRootCause = "",
                                         Resp = "Assign User : " + (x.AssignedToUser == null ? "-" : x.AssignedToUser.Name + " " + x.AssignedToUser.Surname),
                                         CorrectiveAction = "" + x.Details,
                                         AcceptReject = x.FindingAction == 0 ? "" : ((FindingReportAction)x.FindingAction).ToString(),
                                         ExpectedClosureDate = x.ActionResponseDate == null ? "" : Convert.ToDateTime(x.ActionResponseDate).ToString("dd/MM/yyyy"),
                                         Status = x.Status.ToString(),
                                         DomainName = x.ControlRequirement.DomainName,
                                         StageType = x.Category.ToString()
                                     }).ToList();


            var listA = findingList.Where(x => !x.DomainName.ToLower().Contains("Domain ".ToLower())).ToList();
            var ListB = findingList.Where(x => x.DomainName.ToLower().Contains("Domain ".ToLower())).OrderBy(x => int.Parse(x.DomainName.Split('-')[0].Split(' ')[1])).ToList();
            var listAstageOne = new List<StageOneCorrectiveAction>();
            var listBstageOne = new List<StageOneCorrectiveAction>();
            var list3stageOne = new List<StageOneCorrectiveAction>();
            for (int i = 0; i < listA.Count(); i++)
            {
                var temp = listA[i];
                temp.Section = "A";
                if (listA[i].CorrectiveAction != "" && listA[i].CorrectiveAction != null)
                {
                    var splitString = listA[i].CorrectiveAction.Split('`');
                    temp.CorrectiveAction = splitString[0];
                    temp.ActualRootCause = splitString.Length == 1 ? "" : splitString[1];
                    listAstageOne.Add(temp);
                }
            }
            for (int i = 0; i < ListB.Count(); i++)
            {
                var temp = ListB[i];
                temp.Section = "B";
                if (ListB[i].CorrectiveAction != "" && ListB[i].CorrectiveAction != null)
                {
                    var splitString = ListB[i].CorrectiveAction.Split('`');
                    temp.CorrectiveAction = splitString[0];
                    temp.ActualRootCause = splitString.Length == 1 ? "" : splitString[1];
                    listBstageOne.Add(temp);
                }
            }

            list3stageOne.AddRange(listAstageOne);
            list3stageOne.AddRange(listBstageOne);
            int srno = 1;
            foreach (var item in list3stageOne)
            {
                item.SrNo = srno++;
                result.StageOneCorrectiveActionInfo.Add(item);
            }
            // result.StageOneCorrectiveActionInfo = findingList;
            result.StageOneCorrectiveActionInfo.OrderBy(x => x.No).ThenBy(x => x.Section).ThenBy(x => x.ControlRef);
            if (auditProject != null)
            {
                result.Date = auditProject.StartDate;
                result.LeadAuditor = auditProject.LeadAuditor != null ? auditProject.LeadAuditor.Name : "";
                result.GroupName = (auditProject.EntityGroup != null) ? auditProject.EntityGroup.Name : "" + externalAssessmentList.FirstOrDefault().BusinessEntity.CompanyName;
                result.Standard = (auditProject.AuthDocuments != null) ? auditProject.AuthDocuments.FirstOrDefault().AuthoritativeDocument.Name : "";
                result.LicenseNo = (auditProject.EntityGroup != null) ? businessEntityList.Where(x => x.Id == auditProject.EntityGroup.PrimaryEntityId).FirstOrDefault().LicenseNumber :
                    externalAssessmentList.FirstOrDefault().BusinessEntity.LicenseNumber;
            }
            return result;
        }

        public async Task<AuditProjectReportDto> GetAuditProject5(long id)
        {
            List<ReviewDataReportDto> result = new List<ReviewDataReportDto>();
            List<ReviewDataReportDto> result2 = new List<ReviewDataReportDto>();
            List<ReviewDataReportDto> result3 = new List<ReviewDataReportDto>();
            List<ReviewDataReportDto> result4 = new List<ReviewDataReportDto>();
            var query = _auditProjectRepository.GetAll().Where(a => a.Id == id).
                               Include(a => a.Country)
                              .Include(a => a.AuthDocuments)
                              .Include(a => a.Actors).
                               Include(a => a.AuditType)
                              .Include(a => a.AuditCoordinator).
                               Include(a => a.AuditStatus)
                              .Include(x => x.AuditProjectQuestionGroup)
                              .Include(a => a.AuditArea).
                              Include(a => a.AuditManager).
                              Include(a => a.LeadAuditor)
                             .Include(a => a.LeadAuditee).FirstOrDefault();
            var getAuditTeam = _auditReportRepository.GetAll().Where(x => x.AuditProjectId == id).FirstOrDefault();
            var getAuditTeamSign = _auditTeamSignaturerepository.GetAll().Include(x => x.User).Where(x => x.AuditProjectId == id).ToList();
            AuditProjectReportDto auditProjectReportDto = new AuditProjectReportDto();
            auditProjectReportDto.AuditTitle = query.AuditTitle;
            auditProjectReportDto.AuditType = query.AuditStageId == null ? "" : _dynamicParameterValueRepository.GetAll().Where(x => x.Id == query.AuditStageId).Select(x => x.Value).FirstOrDefault();
            auditProjectReportDto.Stage1StartDate = query.StartDate == null ? "" : Convert.ToDateTime(query.StartDate).ToString("dd/MM/yyyy");
            auditProjectReportDto.Stage1EndDate = query.EndDate == null ? "" : Convert.ToDateTime(query.EndDate).ToString("dd/MM/yyyy");
            auditProjectReportDto.Stage2StartDate = query.StageStartDate == null ? "" : Convert.ToDateTime(query.StageStartDate).ToString("dd/MM/yyyy");
            auditProjectReportDto.Stage2EndDate = query.StageEndDate == null ? "" : Convert.ToDateTime(query.StageEndDate).ToString("dd/MM/yyyy");
            auditProjectReportDto.LeadAuditorName = query.LeadAuditor == null ? "" : query.LeadAuditor.FullName;
            auditProjectReportDto.NumberOfAuditor = query.Actors == null ? 0 : query.Actors.Where(x => x.AuditProjectTeamUserType == AuditProjectTeamUserType.AuditorTeam).Count();
            auditProjectReportDto.NumberOfAuditor = getAuditTeam == null ? 0 : getAuditTeam.NumberofAuditors;
            auditProjectReportDto.AuditConclusion = getAuditTeam == null ? "" : getAuditTeam.AuditConclusions;
            auditProjectReportDto.AuditClosure = getAuditTeam == null ? "" : getAuditTeam.ClosureFinding;
            auditProjectReportDto.AreaOfImprovement = getAuditTeam == null ? "" : getAuditTeam.AreaImprovement;
            auditProjectReportDto.AuditMethodology = query.RemoteDesktopAudit == false ? "Onsite" : "Remote Audit";
            auditProjectReportDto.Performance1 = getAuditTeam == null ? "" : getAuditTeam.Performance1;
            auditProjectReportDto.Performance2 = getAuditTeam == null ? "" : getAuditTeam.Performance2;

            var PreparedBy = getAuditTeamSign == null ? null : getAuditTeamSign.Where(x => x.Type == BusinessEntityWorkflowActorType.Approver && x.Signature != null).FirstOrDefault();
            auditProjectReportDto.PreparedBy = PreparedBy == null ? "" : PreparedBy.Name;
            auditProjectReportDto.PreparedBySign = PreparedBy == null ? "" : PreparedBy.Signature;
            auditProjectReportDto.PreparedByDate = PreparedBy == null ? "" : PreparedBy.CreationTime.ToString("dd/MM/yyyy");

            var ReviewedBy = getAuditTeamSign == null ? null : getAuditTeamSign.Where(x => x.Type == BusinessEntityWorkflowActorType.Reviewer && x.Signature != null).FirstOrDefault();
            auditProjectReportDto.ReviewedBy = ReviewedBy == null ? "" : ReviewedBy.Name;
            auditProjectReportDto.ReviewedBySign = ReviewedBy == null ? "" : ReviewedBy.Signature;
            auditProjectReportDto.ReviewedByDate = ReviewedBy == null ? "" : ReviewedBy.CreationTime.ToString("dd/MM/yyyy");

            var AcknowledgedBy = getAuditTeamSign == null ? null : getAuditTeamSign.Where(x => x.Type == BusinessEntityWorkflowActorType.Authority && x.Signature != null).FirstOrDefault();
            auditProjectReportDto.AcknowledgedBy = AcknowledgedBy == null ? "" : AcknowledgedBy.Name;
            auditProjectReportDto.AcknowledgedBySign = AcknowledgedBy == null ? "" : AcknowledgedBy.Signature;
            auditProjectReportDto.AcknowledgedByDate = AcknowledgedBy == null ? "" : AcknowledgedBy.CreationTime.ToString("dd/MM/yyyy");

            var getExternalAssessment = _externalAssessmentRepository.GetAll().Include(x => x.EntityGroup).Include(x => x.Reviews).Include(x => x.BusinessEntity).ThenInclude(x => x.FacilityType).Where(x => x.AuditProjectId == query.Id).ToList();
            var getSampledEntities = _auditReportEntitiesrepository.GetAll().Include(x => x.BusinessEntity).Where(x => x.AuditProjectId == id).ToList();
            if (getExternalAssessment.Count() == 1)
            {
                var getBusinessEntity = getExternalAssessment.Where(x => x.HasQuestionaireGenerated == true).FirstOrDefault();
                auditProjectReportDto.BusinessEntityName = getBusinessEntity.BusinessEntity == null ? "" : getBusinessEntity.BusinessEntity.LicenseNumber + "-" + getBusinessEntity.BusinessEntity.CompanyName;
                auditProjectReportDto.FacilityTypeName = getBusinessEntity.BusinessEntity.FacilityTypeId == null ? "" : getBusinessEntity.BusinessEntity.FacilityType.Name;
                auditProjectReportDto.FacilityGroup = "Not Applicable";
                auditProjectReportDto.NotinFacilityGroup = "Not Applicable";

            }
            else
            {
                var getBusinessEntity = getExternalAssessment.Where(x => x.HasQuestionaireGenerated == true).FirstOrDefault();
                auditProjectReportDto.BusinessEntityName = getBusinessEntity.EntityGroup == null ? "" : getBusinessEntity.EntityGroup.Name;
                auditProjectReportDto.FacilityTypeName = getBusinessEntity.BusinessEntity.FacilityTypeId == null ? "" : getBusinessEntity.BusinessEntity.FacilityType.Name;
                if (getBusinessEntity.EntityGroup != null)
                {

                    if (getSampledEntities.Count() <= 10)
                    {
                        foreach (var item in getSampledEntities)
                        {
                            if (item.Sampled == true)
                            {
                                auditProjectReportDto.NotinFacilityGroup += item.BusinessEntity.CompanyName + ",";
                            }

                            auditProjectReportDto.FacilityGroup += item.BusinessEntity.CompanyName + ", ";

                        }
                    }
                    else
                    {
                        foreach (var item in getSampledEntities)
                        {
                            if (item.Sampled == true)
                            {
                                auditProjectReportDto.NotinFacilityGroup += item.BusinessEntity.LicenseNumber + ",";
                            }

                            auditProjectReportDto.FacilityGroup += item.BusinessEntity.LicenseNumber + ", ";
                        }
                    }
                }

            }
            auditProjectReportDto.auditManagementNameDto = _auditReportFacilityRepository.GetAll().Where(x => x.AuditProjectId == id).Select(x => new AuditManagementNameDto()
            {
                Position = x.Position,
                UserName = x.Name,
                FacilityType = x.Facility
            }).ToList();




            foreach (var item in getExternalAssessment.Where(x => x.Reviews.Count() != 0))
            {
                var getSectionAReview = _reviewDataRepository.GetAll().Include(x => x.ControlRequirement).Where(x => x.ResponseType == ReviewDataResponseType.NonCompliant && x.ExternalAssessmentId == item.Id && x.ControlRequirement.DomainName.Trim().ToLower() == "Section A".Trim().ToLower()).Select(x => new
                {
                    assessmentId = x.ExternalAssessmentId,
                    controlRequirementId = x.ControlRequirementId
                }).ToList();
                var sectionACount = 0;
                var sectionBCount = 0;
                var getSectionAfinding = _findingReportRepository.GetAll().Include(x => x.ControlRequirement).Where(x => (x.Status == FindingReportStatus.CapaOpen || x.Status == FindingReportStatus.New) && x.AssessmentId == item.Id && x.ControlRequirement.DomainName.Trim().ToLower() == "Section A".Trim().ToLower()).ToList();
                if (getSectionAReview.Count() != 0 && getSectionAfinding.Count() != 0)
                {
                    sectionACount = getSectionAfinding.Where(x => getSectionAReview.Any(Z => x.ControlRequirementId == Z.controlRequirementId)).Count();
                }
                var getSectionBReview = _reviewDataRepository.GetAll().Include(x => x.ControlRequirement).Where(x => x.ResponseType == ReviewDataResponseType.NonCompliant && x.ExternalAssessmentId == item.Id && x.ControlRequirement.DomainName.Trim().ToLower() != "Section A".Trim().ToLower()).Select(x => new
                {
                    assessmentId = x.ExternalAssessmentId,
                    controlRequirementId = x.ControlRequirementId
                }).ToList();

                var getSectionBfinding = _findingReportRepository.GetAll().Include(x => x.ControlRequirement).Where(x => (x.Status == FindingReportStatus.CapaOpen || x.Status == FindingReportStatus.New) && x.AssessmentId == item.Id && x.ControlRequirement.DomainName.Trim().ToLower() != "Section A".Trim().ToLower()).ToList();
                if (getSectionBReview.Count() != 0 && getSectionBfinding.Count() != 0)
                {
                    sectionBCount = getSectionBfinding.Where(x => getSectionBReview.Any(Z => x.ControlRequirementId == Z.controlRequirementId)).Count();
                }

                var sectionA = new ReviewDataReportDto();
                sectionA.DomainName = "Section A";
                sectionA.ResponsePercent = "" + getSectionAReview.Count();
                sectionA.Comment = "Reference - Stage 1 and Stage 2 Audit Findings Section A";
                auditProjectReportDto.stageOnereviewDataReportDto.Add(sectionA);

                var sectionB = new ReviewDataReportDto();
                sectionB.DomainName = "Section B";
                sectionB.ResponsePercent = "" + getSectionBReview.Count();
                sectionB.Comment = "Reference - Stage 1 and Stage 2 Audit Findings Section B";
                auditProjectReportDto.stageOnereviewDataReportDto.Add(sectionB);

                var tempList = _reviewDataRepository.GetAll().Include(x => x.ControlRequirement).Where(x => x.ExternalAssessmentId == item.Id && x.ResponseType != ReviewDataResponseType.NotSelected)
                     .Select(x => new
                     {
                         DomainName = x.ControlRequirement.DomainName,
                         ResponseType = x.ResponseType,
                         UpdatedResponseType = x.UpdatedResponseType,
                         Comment = x.Comment,
                         UpdatedMarks = x.UpdatedResponseType == ReviewDataResponseType.FullyCompliant ? "100" : x.UpdatedResponseType == ReviewDataResponseType.PartiallyCompliant ? "50" : x.UpdatedResponseType == ReviewDataResponseType.NonCompliant ? "0" : "",
                         Marks = x.ResponseType == ReviewDataResponseType.FullyCompliant ? "100" : x.ResponseType == ReviewDataResponseType.PartiallyCompliant ? "50" : x.ResponseType == ReviewDataResponseType.NonCompliant ? "0" : "",
                     }).ToList();

                if (tempList.Count() != 0)
                {

                    result2 = tempList.GroupBy(x => x.DomainName).Select(y => new ReviewDataReportDto
                    {
                        DomainName = y.Key.ToString(),
                        ResponsePercent = "" + (int)Math.Round(Convert.ToDecimal(y.Where(x => x.Marks != "").Sum(x => Convert.ToInt32(x.Marks)) / y.Count())) + ".00",
                        CapaResponsePercent = ("" + (int)Math.Round(Convert.ToDecimal((y.Where(x => x.UpdatedMarks != "").Sum(x => Convert.ToInt32(x.UpdatedMarks)) == 0 ? -1 : y.Where(x => x.UpdatedMarks != "").Sum(x => Convert.ToInt32(x.UpdatedMarks))) / (y.Where(x => x.UpdatedMarks != "").Count() == 0 ? -1 : y.Where(x => x.UpdatedMarks != "").Count()))) + ".00") == "1.00" ? "" : "" + (int)Math.Round(Convert.ToDecimal((y.Where(x => x.UpdatedMarks != "").Sum(x => Convert.ToInt32(x.UpdatedMarks)) == 0 ? -1 : y.Where(x => x.UpdatedMarks != "").Sum(x => Convert.ToInt32(x.UpdatedMarks))) / (y.Where(x => x.UpdatedMarks != "").Count() == 0 ? -1 : y.Where(x => x.UpdatedMarks != "").Count()))) + ".00",
                        Comment = ""
                    }).Where(x => x.DomainName.ToLower().Contains("Domain ".ToLower())).OrderBy(x => int.Parse(x.DomainName.Split('-')[0].Split(' ')[1])).ToList();



                    var getDomain = _domainsrepository.GetAll().Where(x => x.Name.Trim().ToLower() != "Section A".Trim().ToLower()).ToList();

                    var reviewDataReportDtoList = new List<ReviewDataReportDto>();
                    foreach (var item3 in getDomain)
                    {
                        var reviewDataReportDto = new ReviewDataReportDto();
                        foreach (var item4 in result2)
                        {

                            if (item3.Name == item4.DomainName)
                            {
                                var getCompliance = _ComplianceAuditSummaryrepository.GetAll().Include(x => x.Domain).Where(x => x.Domain.Name.Trim().ToLower() == item4.DomainName.Trim().ToLower() && x.AuditProjectId == id).ToList().FirstOrDefault();
                                reviewDataReportDto.DomainName = item4.DomainName;
                                reviewDataReportDto.ResponsePercent = item4.ResponsePercent;
                                reviewDataReportDto.CapaResponsePercent = item4.CapaResponsePercent;
                                reviewDataReportDto.Comment = getCompliance == null ? "" : getCompliance.Description;
                            }
                            else
                            {
                                reviewDataReportDto.DomainName = item3.Name;
                            }
                        }
                        reviewDataReportDtoList.Add(reviewDataReportDto);
                    }

                    decimal totalPercentage = 0;
                    decimal totalCapaRespPercentage = 0;
                    foreach (var item2 in result2)
                    {
                        if (item2.ResponsePercent != "")
                        {
                            totalPercentage = totalPercentage + Convert.ToDecimal(item2.ResponsePercent);
                        }
                    }
                    foreach (var item2 in result2)
                    {
                        if (item2.CapaResponsePercent != "")
                        {
                            totalCapaRespPercentage = totalCapaRespPercentage + Convert.ToDecimal(item2.CapaResponsePercent);
                        }
                        else
                        {
                            totalCapaRespPercentage = totalCapaRespPercentage + Convert.ToDecimal(item2.ResponsePercent);
                        }
                    }
                    var finalCaparesponsePerctage = "";
                    if (totalCapaRespPercentage != 0)
                    {
                        finalCaparesponsePerctage = "" + (totalCapaRespPercentage / result2.Count());
                    }
                    auditProjectReportDto.Recommendation = finalCaparesponsePerctage;
                    auditProjectReportDto.consolatedPercentage = "" + decimal.Round((totalPercentage / result2.Count()), 2, MidpointRounding.AwayFromZero);
                    auditProjectReportDto.ScoreDesc = "Your score assigned after the audit is " + auditProjectReportDto.consolatedPercentage + "%. The passing score is 86%, however the score assigned after evaluation of Corrective action plan is " + finalCaparesponsePerctage + "%, and the technical committee might approve the certificate under condition of implementing the noncompliance findings within 90 days.Implementation of corrective action plan will be verified and evaluated and is subject to revoke of certification in case noncompliance is identified during such evaluation. The score is indicative of your level of compliance and it’s not a public domain information.";
                    auditProjectReportDto.reviewDataReportDto = reviewDataReportDtoList.Where(x => x.DomainName.ToLower().Contains("Domain ".ToLower())).OrderBy(x => int.Parse(x.DomainName.Split('-')[0].Split(' ')[1])).ToList();
                    //auditProjectReportDto.stageOnereviewDataReportDto = result.Where(p => !result2.Any(l => p.DomainName == l.DomainName)).ToList();
                }
            }
            return auditProjectReportDto;
        }

    }


}