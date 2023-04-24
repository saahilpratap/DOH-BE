using Abp.Auditing;
using Abp.Dependency;
using Abp.Domain.Repositories;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.FindingReports;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using System.Text;
using System.Linq.Dynamic.Core;
using Abp.UI;
using Abp.Linq.Extensions;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.FindingReports.Dtos;
using LockthreatCompliance.DynamicEntityParameters;
using LockthreatCompliance.ExternalAssessments.Dtos;
using Abp.Timing;
using LockthreatCompliance.Assessments;
using LockthreatCompliance.CustomExceptions;
using LockthreatCompliance.Url;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.Questions;
using LockthreatCompliance.EntityGroups;
using LockthreatCompliance.AuditProjectGroups;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.Domains.Dtos;
using Abp.AutoMapper;
using NPOI.HSSF.Record.Chart;
using LockthreatCompliance.QuestionGroups.Dtos;
using LockthreatCompliance.Questions.Dtos;
using LockthreatCompliance.AuditQuestResponses;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.Enums;
using NPOI.SS.Formula.Functions;
using LockthreatCompliance.Common;
using LockthreatCompliance.Dto;
using LockthreatCompliance.FacilityTypes;
using LockthreatCompliance.AuditProjects.Exporting;
using LockthreatCompliance.WrokFlows;
using Abp.Notifications;
using System.Text.RegularExpressions;
using System.Web;
using Abp.Runtime.Security;
using LockthreatCompliance.Contacts;
using LockthreatCompliance.Contacts.Dtos;
using LockthreatCompliance.EntityFrameworkCore.RepositoryExtensions;
using LockthreatCompliance.AuditReports;
using AutoMapper;
using System.IO;
using LockthreatCompliance.Domains;
using Abp.Domain.Uow;

namespace LockthreatCompliance.Web.DXServices.DataHandler
{

    public class ReportDataHandlerBase : LockthreatComplianceAppServiceBase
    {
        private static IRepository<AuditLog, long> _auditLogRepository;
        private static IRepository<BusinessEntity> _businessEntityRepository;
        private static IRepository<AuditProject, long> _auditProjectRepository;
        private static IRepository<ExternalAssessment> _externalAssessmentRepository;
        private static IRepository<DynamicParameterValue> _dynamicParameterValueRepository;
        private static IRepository<FindingReport> _findingReportRepository;
        private static IAuditProjectAppService _auditProjectAppService;
        private static IRepository<EntityApplicationSetting> _entityApplicationSettingRepository;
        private static IRepository<AuditDocumentPath, long> _auditDocumentPathRepository;
        private static IRepository<ReviewData> _reviewDataRepository;
        private static IRepository<AuditReports.AuditReportEntities> _auditReportEntitiesRepository;
        private static IRepository<AuditReports.AuditReport> _auditReportRepository;
        private static IRepository<AuditReportEntities> _auditReportEntitiesrepository;
        private static IRepository<AuditTeamSignature> _auditTeamSignaturerepository;
        private static IRepository<Domain> _domainsrepository;
        private static IRepository<AuditReports.ComplianceAuditSummary> _ComplianceAuditSummaryrepository;
        private static IRepository<AuditReportFacility> _auditReportFacilityRepository;
        private static IUnitOfWorkManager _unitOfWorkManager;
        private static IRepository<EntityGroup> _entityGroupRepository;
        static ReportDataHandlerBase()
        {
            _entityGroupRepository = IocManager.Instance.Resolve<IRepository<EntityGroup>>();
            _unitOfWorkManager = IocManager.Instance.Resolve<IUnitOfWorkManager>(); 
            _auditLogRepository = IocManager.Instance.Resolve<IRepository<AuditLog, long>>();
            _businessEntityRepository = IocManager.Instance.Resolve<IRepository<BusinessEntity>>();
            _auditProjectRepository = IocManager.Instance.Resolve<IRepository<AuditProject, long>>();
            _externalAssessmentRepository = IocManager.Instance.Resolve<IRepository<ExternalAssessment>>();
            _findingReportRepository = IocManager.Instance.Resolve<IRepository<FindingReport>>();
            _entityApplicationSettingRepository = IocManager.Instance.Resolve<IRepository<EntityApplicationSetting>>();
            _auditDocumentPathRepository = IocManager.Instance.Resolve<IRepository<AuditDocumentPath, long>>();
            _dynamicParameterValueRepository = IocManager.Instance.Resolve<IRepository<DynamicParameterValue>>();
            _reviewDataRepository = IocManager.Instance.Resolve<IRepository<ReviewData>>();
            _auditReportEntitiesRepository = IocManager.Instance.Resolve<IRepository<AuditReports.AuditReportEntities>>();
            _auditReportRepository = IocManager.Instance.Resolve<IRepository<AuditReports.AuditReport>>();
            _auditReportEntitiesrepository = IocManager.Instance.Resolve<IRepository<AuditReportEntities>>();
            _auditTeamSignaturerepository = IocManager.Instance.Resolve<IRepository<AuditTeamSignature>>();
            _domainsrepository = IocManager.Instance.Resolve<IRepository<Domain>>();
            _ComplianceAuditSummaryrepository = IocManager.Instance.Resolve<IRepository<AuditReports.ComplianceAuditSummary>>();
            _auditReportFacilityRepository = IocManager.Instance.Resolve<IRepository<AuditReportFacility>>();

        }

        public static List<AuditLog> getauditLogMaster()
        {
            var auditLog = _auditLogRepository.GetAll().ToList();
            return auditLog;
        }

        public static FindingReportStageOneDto FindingReportStageWise(long id, FindingReportCategory type)
        {
            var result = new FindingReportStageOneDto();
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
            var newList = new List<StageOneFinding>();
            var findingList = _findingReportRepository.GetAll().Where(x => x.Category == type).Where(x => externalAssessmentIds.Contains((int)x.AssessmentId))
                .Include(x => x.BusinessEntity).Include(x => x.ControlRequirement).Include(x => x.Assessment).ThenInclude(x => x.Reviews)
                .Include(x => x.ControlRequirement).ThenInclude(x => x.ControlStandard)
                .Include(x => x.ControlRequirement).ThenInclude(x => x.ControlStandard).ThenInclude(x => x.Domain)
                                     .Select(x => new StageOneFinding
                                     {
                                         No = "FND-" + x.Id,
                                         Section = "" + x.ControlRequirement.ControlStandard.DomainId,
                                         ControlRef = x.ControlRequirement.OriginalId,
                                         AuditQuestionSubject = x.Title,
                                         EntityComplaiance = (x.Assessment.Reviews.Where(y => y.ControlRequirementId == x.ControlRequirementId).FirstOrDefault() != null)
                                         ? "" + ((ReviewDataResponseType)x.Assessment.Reviews.Where(y => y.ControlRequirementId == x.ControlRequirementId).FirstOrDefault().ResponseType).ToString() : "",
                                         FindingDescription = "" + x.OtherCategoryName,
                                         FindingReference = "" + x.Reference,
                                         DomainName = x.ControlRequirement.DomainName,
                                         SrNo = x.FindingCAPAStatus.ToString()
                                     }).ToList();
            foreach (var item in findingList)
            {

                if (item.EntityComplaiance == "NotSelected")
                {
                    item.EntityComplaiance = "Not Selected";
                }
                else if (item.EntityComplaiance == "NotApplicable")
                {
                    item.EntityComplaiance = "Not Applicable";
                }
                else if (item.EntityComplaiance == "NonCompliant")
                {
                    item.EntityComplaiance = "Non Compliant";
                }
                else if (item.EntityComplaiance == "PartiallyCompliant")
                {
                    item.EntityComplaiance = "Partially Compliant";
                }
                else if (item.EntityComplaiance == "FullyCompliant")
                {
                    item.EntityComplaiance = "Fully Compliant";
                }
                if (item.SrNo == "CapaOpen")
                {
                    item.SrNo = "Open";
                }
                else if (item.SrNo == "CapaClosed")
                {
                    item.SrNo = "Close";
                }
                else
                {
                    item.SrNo = "Open";
                }
                newList.Add(item);
            }
            var listA = newList.Where(x => !x.DomainName.ToLower().Contains("Domain ".ToLower())).ToList();
            var ListB = newList.Where(x => x.DomainName.ToLower().Contains("Domain ".ToLower())).OrderBy(x => int.Parse(x.DomainName.Split('-')[0].Split(' ')[1])).ToList();
            var listAstageOne = new List<StageOneFinding>();
            var listBstageOne = new List<StageOneFinding>();
            var list3stageOne = new List<StageOneFinding>();
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
            //int srno = 1;
            foreach (var item in list3stageOne)
            {

                result.StageOneFindingInfo.Add(item);
            }
            result.StageOneFindingInfo.OrderBy(x => x.No).ThenBy(x => x.Section).ThenBy(x => x.ControlRef);
            if (auditProject != null)
            {
                var startdate = auditProject.StartDate == null ? "" : Convert.ToDateTime(auditProject.StartDate).ToString("dd/MM/yyyy");
                var enddate = auditProject.EndDate == null ? "" : Convert.ToDateTime(auditProject.EndDate).ToString("dd/MM/yyyy");
                var stage2startdate = auditProject.StageStartDate == null ? "" : Convert.ToDateTime(auditProject.StageStartDate).ToString("dd/MM/yyyy");
                var stage2enddate = auditProject.StageEndDate == null ? "" : Convert.ToDateTime(auditProject.StageEndDate).ToString("dd/MM/yyyy");
                if (type == FindingReportCategory.Stage1)
                {
                    result.StageDate = "Stage 1 Start Date - " + startdate + " -" + " Stage 1 End Date- " + enddate;
                }
                else
                {
                    result.StageDate = "Stage 2 Start Date - " + stage2startdate + " -" + " Stage 2 End Date- " + stage2enddate;
                }
                result.Date = startdate;
                result.LeadAuditor = auditProject.LeadAuditor != null ? auditProject.LeadAuditor.FullName : "";
                result.Audit = auditProject.AuditTitle;

                if (auditProject.EntityGroup != null && auditProject.EntityGroupId != null)
                {
                    result.GroupName = auditProject.EntityGroup == null ? "" : auditProject.EntityGroup.Name;
                }
                else
                {
                    using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
                    {

                        result.GroupName = auditProject.EntityGroupId == null ? "" : _entityGroupRepository.GetAll().Where(x => x.Id == auditProject.EntityGroupId).FirstOrDefault().Name;
                    }
                }



              //  result.GroupName = (auditProject.EntityGroup != null) ? auditProject.EntityGroup.Name : "" + externalAssessmentList.FirstOrDefault().BusinessEntity.CompanyName;
                result.Standard = (auditProject.AuthDocuments != null) ? auditProject.AuthDocuments.FirstOrDefault().AuthoritativeDocument.Name : "";
                result.LicenseNo = (auditProject.EntityGroup != null) ? businessEntityList.Where(x => x.Id == auditProject.EntityGroup.PrimaryEntityId).FirstOrDefault().LicenseNumber :
                    externalAssessmentList.FirstOrDefault().BusinessEntity.LicenseNumber;
            }
            return result;
        }

        public static FindingReportAllStageDto FindingReportAllStageWise(long id)
        {
            try
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
                var newList = new List<AllStageFinding>();
                var findingList = _findingReportRepository.GetAll().Where(x => externalAssessmentIds.Contains((int)x.AssessmentId))
                    .Include(x => x.BusinessEntity).Include(x => x.ControlRequirement).Include(x => x.Assessment).ThenInclude(x => x.Reviews)
                    .Include(x => x.ControlRequirement).ThenInclude(x => x.ControlStandard)
                    .Include(x => x.ControlRequirement).ThenInclude(x => x.ControlStandard).ThenInclude(x => x.Domain)
                                         .Select(x => new AllStageFinding
                                         {
                                             No = "FND-" + x.Id,
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
                foreach (var item in findingList)
                {

                    if (item.EntityComplaiance == "NotSelected")
                    {
                        item.EntityComplaiance = "Not Selected";
                    }
                    else if (item.EntityComplaiance == "NotApplicable")
                    {
                        item.EntityComplaiance = "Not Applicable";
                    }
                    else if (item.EntityComplaiance == "NonCompliant")
                    {
                        item.EntityComplaiance = "Non Compliant";
                    }
                    else if (item.EntityComplaiance == "PartiallyCompliant")
                    {
                        item.EntityComplaiance = "Partially Compliant";
                    }
                    else if (item.EntityComplaiance == "FullyCompliant")
                    {
                        item.EntityComplaiance = "Fully Compliant";
                    }

                    newList.Add(item);
                }
                var listA = newList.Where(x => !x.DomainName.ToLower().Contains("Domain ".ToLower())).ToList();
                var ListB = newList.Where(x => x.DomainName.ToLower().Contains("Domain ".ToLower())).OrderBy(x => int.Parse(x.DomainName.Split('-')[0].Split(' ')[1])).ToList();
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
                result.StageAllFindingInfo.OrderBy(x => x.No).ThenBy(x => x.Section).ThenBy(x => x.ControlRef);
                if (auditProject != null)
                {
                    var startDate = auditProject.StartDate == null ? "" : Convert.ToDateTime(auditProject.StartDate).ToString("dd/MM/yyyy");
                    var endDate = auditProject.EndDate == null ? "" : Convert.ToDateTime(auditProject.EndDate).ToString("dd/MM/yyyy");
                    var startDate2 = auditProject.StageStartDate == null ? "" : Convert.ToDateTime(auditProject.StageStartDate).ToString("dd/MM/yyyy");
                    var stageEnd2 = auditProject.StageEndDate == null ? "" : Convert.ToDateTime(auditProject.StageEndDate).ToString("dd/MM/yyyy");

                    var tempLicense = (auditProject.EntityGroup != null) ? businessEntityList.Where(x => x.Id == auditProject.EntityGroup.PrimaryEntityId).FirstOrDefault().LicenseNumber :
                    externalAssessmentList.FirstOrDefault().BusinessEntity.LicenseNumber;
                    result.Date = "Stage 1 Start Date : " + startDate + "- Stage 1 End Date : " + endDate + "\nStage 2 Start Date : " + startDate2 + "- Stage 2 End Date : " + stageEnd2;
                    result.LeadAuditor = auditProject.LeadAuditor != null ? auditProject.LeadAuditor.FullName : "";

                    result.Standard = (auditProject.AuthDocuments != null) ? auditProject.AuthDocuments.FirstOrDefault().AuthoritativeDocument.Name : "";
                    if (externalAssessmentList.Count() == 1)
                    {
                        var licenseNo = externalAssessmentList.FirstOrDefault().BusinessEntity.LicenseNumber;
                        result.GroupName = externalAssessmentList.FirstOrDefault().BusinessEntity.CompanyName;
                        result.LicenseNo = licenseNo + " - " + result.GroupName;
                    }
                    else
                    {
                        var licenseNo = "";
                        foreach (var item in externalAssessmentList)
                        {
                            licenseNo += item.BusinessEntity.LicenseNumber + ",";
                        }

                        if (auditProject.EntityGroup != null && auditProject.EntityGroupId != null)
                        {
                           result.GroupName = auditProject.EntityGroup == null ? "" : auditProject.EntityGroup.Name;
                        }
                        else
                        {
                            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
                            {

                                result.GroupName = auditProject.EntityGroupId == null ? "" : _entityGroupRepository.GetAll().Where(x => x.Id == auditProject.EntityGroupId).FirstOrDefault().Name;
                            }
                        }
                        //result.GroupName = (auditProject.EntityGroup != null) ? auditProject.EntityGroup.Name : "" + externalAssessmentList.FirstOrDefault().BusinessEntity.CompanyName;
                        result.LicenseNo = licenseNo.Remove(licenseNo.Length - 1, 1) + " - " + result.GroupName;
                    }

                    result.Audit = auditProject.RemoteDesktopAudit == false ? "Onsite" : "Remote Audit";
                    if (externalAssessmentList.Count() != 1)
                    {
                        var primaryList = new HashSet<string>();
                        foreach (var item in externalAssessmentList)
                        {
                            if (item.BusinessEntity.PrimaryContactName!= null && item.BusinessEntity.PrimaryContactName!= "")
                            {
                                var tempString = item.BusinessEntity.PrimaryContactName.Split(",");
                                foreach (var item2 in tempString)
                                {
                                    primaryList.Add(item2);
                                }
                            }

                        }

                        foreach (var item2 in primaryList)
                        {
                            result.MgntRepresentative += item2 + ",";
                        }
                    }
                    else
                    {
                        result.MgntRepresentative = externalAssessmentList.FirstOrDefault().BusinessEntity.PrimaryContactName;
                    }
                }
                return result;
            }
            catch(Exception ex)
            {
                throw new  UserFriendlyException("Record not Found", ex.Message);
            }
        }

        public static CorrectiveActionReportStageOneDto CorrectiveActionReportStageWise(long id, FindingReportCategory type)
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
            var newList = new List<StageOneCorrectiveAction>();
            var findingList = _findingReportRepository.GetAll().Include(x => x.AssignedToUser).Include(x => x.FindingCoordinator).Include(x => x.FindingManager)
                .Include(x => x.FindingOwner).Where(x => x.Category == type).Where(x => externalAssessmentIds.Contains((int)x.AssessmentId))
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
                                         Status = x.FindingCAPAStatus.ToString(),
                                         DomainName = x.ControlRequirement.DomainName
                                     }).ToList();
            foreach (var item in findingList)
            {
                if (item.Status == "CapaOpen")
                {
                    item.Status = "Open";
                }
                else if (item.Status == "CapaClosed")
                {
                    item.Status = "Close";
                }
                else
                {
                    item.Status = "Open";
                }
                newList.Add(item);
            }

            var listA = newList.Where(x => !x.DomainName.ToLower().Contains("Domain ".ToLower())).ToList();
            var ListB = newList.Where(x => x.DomainName.ToLower().Contains("Domain ".ToLower())).OrderBy(x => int.Parse(x.DomainName.Split('-')[0].Split(' ')[1])).ToList();
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
                result.LeadAuditor = auditProject.LeadAuditor != null ? auditProject.LeadAuditor.FullName : "";


                if (auditProject.EntityGroup != null && auditProject.EntityGroupId != null)
                {
                    result.GroupName = auditProject.EntityGroup == null ? "" : auditProject.EntityGroup.Name;
                }
                else
                {
                    using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
                    {

                        result.GroupName = auditProject.EntityGroupId == null ? "" : _entityGroupRepository.GetAll().Where(x => x.Id == auditProject.EntityGroupId).FirstOrDefault().Name;
                    }
                }

                result.GroupName = (auditProject.EntityGroup != null) ? auditProject.EntityGroup.Name : "" + externalAssessmentList.FirstOrDefault().BusinessEntity.CompanyName;
                result.Standard = (auditProject.AuthDocuments != null) ? auditProject.AuthDocuments.FirstOrDefault().AuthoritativeDocument.Name : "";
                result.LicenseNo = (auditProject.EntityGroup != null) ? businessEntityList.Where(x => x.Id == auditProject.EntityGroup.PrimaryEntityId).FirstOrDefault().LicenseNumber :
                    externalAssessmentList.FirstOrDefault().BusinessEntity.LicenseNumber;
            }
            return result;
        }


        public static CorrectiveActionReportStageOneDto CorrectiveActionReportStageAll(long id)
        {

            try
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
                var newList = new List<StageOneCorrectiveAction>();
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
                                             Status = x.FindingCAPAStatus.ToString(),
                                             DomainName = x.ControlRequirement.DomainName,
                                             StageType = x.Category.ToString()
                                         }).ToList();
                foreach (var item in findingList)
                {
                    if (item.Status == "CapaOpen")
                    {
                        item.Status = "Open";
                    }
                    else if (item.Status == "CapaClosed")
                    {
                        item.Status = "Close";
                    }
                    else
                    {
                        item.Status = "Open";
                    }
                    newList.Add(item);
                }

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
                    result.LeadAuditor = auditProject.LeadAuditor != null ? auditProject.LeadAuditor.FullName : "";

                    if (auditProject.EntityGroup != null && auditProject.EntityGroupId != null)
                    {
                        result.GroupName = auditProject.EntityGroup == null ? "" : auditProject.EntityGroup.Name;
                    }
                    else
                    {
                        using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
                        {
                            result.GroupName = auditProject.EntityGroupId == null ? "" : _entityGroupRepository.GetAll().Where(x => x.Id == auditProject.EntityGroupId).FirstOrDefault().Name;
                        }
                    }
                   // result.GroupName = (auditProject.EntityGroup != null) ? auditProject.EntityGroup.Name : "" + externalAssessmentList.FirstOrDefault().BusinessEntity.CompanyName;
                    result.Standard = (auditProject.AuthDocuments != null) ? auditProject.AuthDocuments.FirstOrDefault().AuthoritativeDocument.Name : "";
                    result.LicenseNo = (auditProject.EntityGroup != null) ? businessEntityList.Where(x => x.Id == auditProject.EntityGroup.PrimaryEntityId).FirstOrDefault().LicenseNumber :
                        externalAssessmentList.FirstOrDefault().BusinessEntity.LicenseNumber;
                }
                return result;
            }
            catch(Exception ex)
            {
                throw new UserFriendlyException("Report Not Found", ex.Message);
            }
        }

        public static ReportFileUploadDto Checkpath(long auditProjectId, string fileName, ReportTypes type)
        {
            try
            {
                string filepaths = null;
                string nameFile = null;
                string webRootPath = _entityApplicationSettingRepository.GetAll().Select(x => x.Attachmentpath).FirstOrDefault();
                nameFile = "AUD-" + auditProjectId;
                var filePath = new ReportFileUploadDto();

                if (webRootPath != null)
                {
                    var uploads = Path.Combine(webRootPath, "AuditReports");

                    var cehckexternalAssessment = _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity).Where(x => x.AuditProjectId == auditProjectId && x.HasQuestionaireGenerated == true).FirstOrDefault();

                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }
                    if (cehckexternalAssessment != null)
                    {
                        filePath.FileName = nameFile + "-EXT-" + cehckexternalAssessment.Id + "-" + cehckexternalAssessment.BusinessEntity.LicenseNumber + "-" + fileName + ".pdf";
                        var checkfileName = _auditDocumentPathRepository.GetAll().Where(x => x.FileName.Trim().ToLower() == filePath.FileName.Trim().ToLower()).FirstOrDefault();
                        if (checkfileName == null)
                        {
                            var documentPath = new AuditDocumentPath()
                            {
                                Id = 0,
                                FileName = filePath.FileName,
                                AuditProjectId = auditProjectId,
                                ReportType = type
                            };
                            _auditDocumentPathRepository.InsertAsync(documentPath);
                        }
                        var filePathss = Path.Combine(uploads, filePath.FileName);
                        filePath.FilePath = filePathss;
                    }
                    else
                    {
                        throw new InvalidOperationException("Please Generate External Assessment Question..");
                        //   throw new UserFriendlyException("Please Generate External Assessment Question..");
                        // throw new Exception("Please Generate External Assessment Question...");
                    }

                }

                return filePath;
            }
            catch(Exception ex)
            {
                throw new UserFriendlyException("Path not Found..");
            }
        }

        public static AuditProjectReportDto GetAuditProject2(long id)
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
            //auditProjectReportDto.ReviewedBySign = ReviewedBy == null ? "" : ReviewedBy.Signature;            
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
                auditProjectReportDto.ReviewedBySign = getBusinessEntity.BusinessEntity.FacilityTypeId == null ? "Type of facilities or entities in the scope of audit" : "Type of facilities or entities in the scope of audit - " + getBusinessEntity.BusinessEntity.FacilityType.Name;
                auditProjectReportDto.FacilityGroup = "Not Applicable";
                auditProjectReportDto.NotinFacilityGroup = "Not Applicable";

            }
            else
            {
                var getBusinessEntity = getExternalAssessment.Where(x => x.HasQuestionaireGenerated == true).FirstOrDefault();
                if (getBusinessEntity.EntityGroup != null && getBusinessEntity.EntityGroupId!=null)
                {
                    auditProjectReportDto.BusinessEntityName = getBusinessEntity.EntityGroup == null ? "" : getBusinessEntity.EntityGroup.Name;
                }
                else
                {
                    using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
                    {

                        auditProjectReportDto.BusinessEntityName = getBusinessEntity.EntityGroupId == null ? "" : _entityGroupRepository.GetAll().Where(x=>x.Id== getBusinessEntity.EntityGroupId).FirstOrDefault().Name;
                    }
                }
                    auditProjectReportDto.ReviewedBySign = getBusinessEntity.BusinessEntity.FacilityTypeId == null ? "Type of facilities or entities in the scope of audit" : "Type of facilities or entities in the scope of audit - " + getBusinessEntity.BusinessEntity.FacilityType.Name;
                if (getBusinessEntity.EntityGroup != null)
                {

                    if (getSampledEntities.Count() <= 10)
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

                var tempList = _reviewDataRepository.GetAll().Include(x => x.ControlRequirement).Where(x => x.ExternalAssessmentId == item.Id && x.ResponseType != ReviewDataResponseType.NotSelected && x.ResponseType != ReviewDataResponseType.NotApplicable)
                     .Select(x => new
                     {
                         DomainName = x.ControlRequirement.DomainName,
                         ResponseType = x.ResponseType,
                         UpdatedResponseType = x.UpdatedResponseType,
                         Comment = x.Comment,

                         UpdatedMarks =
                         (x.UpdatedResponseType==ReviewDataResponseType.NotSelected || x.UpdatedResponseType == ReviewDataResponseType.NotSelected)
                         ? (x.ResponseType == ReviewDataResponseType.FullyCompliant ? "100" : x.ResponseType == ReviewDataResponseType.PartiallyCompliant ? "50" : x.ResponseType == ReviewDataResponseType.NonCompliant ? "0" : "")
                         : (x.UpdatedResponseType == ReviewDataResponseType.FullyCompliant ? "100" : x.UpdatedResponseType == ReviewDataResponseType.PartiallyCompliant ? "50" : x.UpdatedResponseType == ReviewDataResponseType.NonCompliant ? "0" : ""),
                                        
                         Marks = x.ResponseType == ReviewDataResponseType.FullyCompliant ? "100" : x.ResponseType == ReviewDataResponseType.PartiallyCompliant ? "50" : x.ResponseType == ReviewDataResponseType.NonCompliant ? "0" : "",
                     }).ToList();

                if (tempList.Count() != 0)
                {

                    result2 = tempList.GroupBy(x => x.DomainName).Select(y => new ReviewDataReportDto
                    {
                        DomainName = y.Key.ToString(),
                        ResponsePercent = "" + (int)Math.Round(Convert.ToDecimal(y.Where(x => x.Marks != "").Sum(x => Convert.ToInt32(x.Marks)) / y.Count())) + ".00",
                        CapaResponsePercent =
                        (y.Count(yy=>yy.UpdatedResponseType == 0)== y.Count()) ? 
                        "" :
                        ("" + (int)Math.Round(Convert.ToDecimal((y.Where(x => x.UpdatedMarks != "").Sum(x => Convert.ToInt32(x.UpdatedMarks)) == 0 ? -1 : y.Where(x => x.UpdatedMarks != "").Sum(x => Convert.ToInt32(x.UpdatedMarks))) / (y.Where(x => x.UpdatedMarks != "").Count() == 0 ? -1 : y.Where(x => x.UpdatedMarks != "").Count()))) + ".00") == "1.00" ? "" : "" + (int)Math.Round(Convert.ToDecimal((y.Where(x => x.UpdatedMarks != "").Sum(x => Convert.ToInt32(x.UpdatedMarks)) == 0 ? -1 : y.Where(x => x.UpdatedMarks != "").Sum(x => Convert.ToInt32(x.UpdatedMarks))) / (y.Where(x => x.UpdatedMarks != "").Count() == 0 ? -1 : y.Where(x => x.UpdatedMarks != "").Count()))) + ".00",
                        Comment = ""
                    }).Where(x => x.DomainName.ToLower().Contains("Domain ".ToLower())).OrderBy(x => int.Parse(x.DomainName.Split('-')[0].Split(' ')[1])).ToList();



                    var getDomain = _domainsrepository.GetAll().Where(x => x.Name.Trim().ToLower() != "Section A".Trim().ToLower()).ToList();

                    var reviewDataReportDtoList = new List<ReviewDataReportDto>();
                    foreach (var item3 in getDomain)
                    {
                        var reviewDataReportDto = new ReviewDataReportDto();
                        foreach (var item4 in result2)
                        {
                            var getCompliance = _ComplianceAuditSummaryrepository.GetAll().Include(x => x.Domain).Where(x => x.Domain.Name.Trim().ToLower() == item3.Name.Trim().ToLower() && x.AuditProjectId == id).OrderByDescending(x => x.Id).ToList().FirstOrDefault();
                            if (item3.Name == item4.DomainName)
                            {
                                reviewDataReportDto.DomainName = item4.DomainName;
                                reviewDataReportDto.ResponsePercent = item4.ResponsePercent;
                                reviewDataReportDto.CapaResponsePercent = item4.CapaResponsePercent;
                                reviewDataReportDto.Comment = getCompliance == null ? "" : getCompliance.Description;
                            }
                            else
                            {
                                reviewDataReportDto.DomainName = item3.Name;
                                reviewDataReportDto.Comment = getCompliance == null ? "" : getCompliance.Description;
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
                    var getAfterCapaCount = result2.Where(x => x.CapaResponsePercent != "").Select(x => x.CapaResponsePercent).Count();
                    if (getAfterCapaCount != 0 && getAfterCapaCount != null)
                    {
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
                    }
                    var finalCaparesponsePerctage = "";
                    if (totalCapaRespPercentage != 0)
                    {
                        finalCaparesponsePerctage = "" + +decimal.Round((totalCapaRespPercentage / result2.Count()), 2, MidpointRounding.AwayFromZero);
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

        public static CertificationProposalReportDto CertificationProposalReport(long id)
        {
            var result = new CertificationProposalReportDto();
            var businessEntityList = _businessEntityRepository.GetAll().ToList();
            var auditProject = _auditProjectRepository.GetAll().Include(x => x.LeadAuditor).Include(x => x.AuthDocuments).ThenInclude(x => x.AuthoritativeDocument)
                .Include(x => x.EntityGroup).Include(x => x.AuditStage).Where(x => x.Id == id).FirstOrDefault();
            var externalAssessmentList = _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == id).Include(x => x.BusinessEntity).ToList();

            if (auditProject != null)
            {
                result.ProposalDate = auditProject.StartDate != null ? Convert.ToDateTime(auditProject.StartDate).ToString("dd/MM/yyyy") : "";
                result.LeadAuditor = auditProject.LeadAuditor != null ? "" + auditProject.LeadAuditor.FullName : "";
                result.Stage1StartDate = auditProject.StartDate != null ? Convert.ToDateTime(auditProject.StartDate).ToString("dd/MM/yyyy") : "";
                result.Stage1EndDate = auditProject.EndDate != null ? Convert.ToDateTime(auditProject.EndDate).ToString("dd/MM/yyyy") : "";
                result.Stage2StartDate = auditProject.StageStartDate != null ? Convert.ToDateTime(auditProject.StageStartDate).ToString("dd/MM/yyyy") : "";
                result.Stage2EndDate = auditProject.StageEndDate != null ? Convert.ToDateTime(auditProject.StageEndDate).ToString("dd/MM/yyyy") : "";
                result.TypeofAudit = auditProject.AuditStageId != null ? "" + auditProject.AuditStage.Value : "";

                result.Standard = (auditProject.AuthDocuments != null) ? auditProject.AuthDocuments.FirstOrDefault().AuthoritativeDocument.Name : "";
                result.LicenseNo = (auditProject.EntityGroup != null) ? businessEntityList.Where(x => x.Id == auditProject.EntityGroup.PrimaryEntityId).FirstOrDefault().LicenseNumber :
                    externalAssessmentList.FirstOrDefault().BusinessEntity.LicenseNumber;

                var auditReport = (auditProject.EntityGroup != null) ? _auditReportEntitiesRepository.GetAll().Where(x => x.AuditProjectId == auditProject.Id && x.BusinessEntityId == auditProject.EntityGroup.PrimaryEntityId).FirstOrDefault()
                    : _auditReportEntitiesRepository.GetAll().Where(x => x.AuditProjectId == auditProject.Id && x.BusinessEntityId == externalAssessmentList.FirstOrDefault().BusinessEntityId).FirstOrDefault();

                result.TotalMandays = (auditReport != null) ? "" + auditReport.ManDays : "";
            }


            var ExternalAssessmentId = (auditProject.EntityGroup != null) ? externalAssessmentList.Where(x => x.BusinessEntityId == auditProject.EntityGroup.PrimaryEntityId).FirstOrDefault().Id :
                    externalAssessmentList.FirstOrDefault().Id;

            var tempList = _reviewDataRepository.GetAll().Include(x => x.ControlRequirement).Where(x => x.ExternalAssessmentId == ExternalAssessmentId)
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
                result.FullyCompliantCount = tempList.Where(x => x.ResponseType == ReviewDataResponseType.FullyCompliant).Count();
                result.PartiallyCompliantCount = tempList.Where(x => x.ResponseType == ReviewDataResponseType.PartiallyCompliant).Count();

                var listA = tempList.GroupBy(x => x.DomainName).Select(y => new DomainInfo
                {
                    Domain = y.Key.ToString(),
                    Auditor = "",
                    AuditeeRepresentative = "",
                    ActualScore = "" + (int)Math.Round(Convert.ToDecimal(y.Sum(x => x.Marks) / y.Count())),
                    CapaScore = "" + (int)Math.Round(Convert.ToDecimal(y.Sum(x => x.UpdatedMarks) / y.Count())),
                    LevelOfCompliance = (int)Math.Round(Convert.ToDecimal(y.Sum(x => x.Marks) / y.Count())) < 86 ? "Non Compliant" : "Compliant"
                }).Where(x => !x.Domain.ToLower().Contains("Domain ".ToLower())).ToList();

                result.DomainInfos = tempList.GroupBy(x => x.DomainName).Select(y => new DomainInfo
                {
                    Domain = y.Key.ToString(),
                    Auditor = "",
                    AuditeeRepresentative = "",
                    ActualScore = "" + (int)Math.Round(Convert.ToDecimal(y.Sum(x => x.Marks) / y.Count())),
                    CapaScore = "" + (int)Math.Round(Convert.ToDecimal(y.Sum(x => x.UpdatedMarks) / y.Count())),
                    LevelOfCompliance = (int)Math.Round(Convert.ToDecimal(y.Sum(x => x.Marks) / y.Count())) < 86 ? "Non Compliant" : "Compliant"
                }).Where(x => x.Domain.ToLower().Contains("Domain ".ToLower())).OrderBy(x => int.Parse(x.Domain.Split('-')[0].Split(' ')[1])).ToList();

                result.DomainInfos.Insert(0, listA.FirstOrDefault());
            }

            var TempTotal = 0;
            var TempTotal1 = 0;

            foreach (var item in result.DomainInfos)
            {
                TempTotal = TempTotal + int.Parse(item.ActualScore);
                TempTotal1 = TempTotal1 + int.Parse(item.CapaScore);
            }

            if (TempTotal != 0)
            {
                var percentageAverage = TempTotal / result.DomainInfos.Count();
                result.ActualScoreAverage = "" + percentageAverage;
                result.Grade = percentageAverage < 86 ? "B" : "A";
            }

            if (TempTotal1 != 0)
            {
                var percentageAverage = TempTotal1 / result.DomainInfos.Count();
                result.CapaScoreAverage = "" + percentageAverage;
            }

            return result;

        }
    }
}