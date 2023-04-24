using Abp.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.WrokFlows;
using LockthreatCompliance.Authorization.Users;
using Microsoft.EntityFrameworkCore;
using Abp.Collections.Extensions;
using System.Linq;
using LockthreatCompliance.Hangfire.Dto;
using LockthreatCompliance.AuditProjects;
using System;
using LockthreatCompliance.BusinessRisks;
using LockthreatCompliance.WorkFlow;
using LockthreatCompliance.WorkFllows;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.Incidents;
using LockthreatCompliance.FindingReports;
using System.Reflection;
using AutoMapper;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.AuditProjects.Dtos;
using System.Text.RegularExpressions;
using LockthreatCompliance.EntityGroups;
using LockthreatCompliance.Assessments.Dto;
using Abp;

namespace LockthreatCompliance.Hangfire
{
    public class HangfireAuditProjectAppService : LockthreatComplianceAppServiceBase, IHangfireAuditProjectAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<StateAction, long> _stateActionRepository;
        private readonly IRepository<WorkFlowPage, long> _WorkFlowPageRepository;
        private readonly IUserEmailer _userEmailer;
        private readonly ICustomTemplateAppService _customTemplateAppServiceEmailer;
        private readonly IRepository<EntityApplicationSetting> _entityApplicationSettingRepository;
        private readonly IRepository<AuditProject, long> _auditProjectRepository;
        private readonly IRepository<BusinessEntity, int> _businessEntityRepository;
        private readonly IRepository<ExternalAssessment, int> _externalAssessmentRepository;
        private readonly IRepository<AuditProjectStatus, long> _auditProjectStatusRepository;
        private readonly IRepository<AssessmentStatusLog, long> _assessmentStatusLogRepository;
        private readonly IRepository<BusinessRiskStatusLog, long> _businessRiskLogRepository;
        private readonly IRepository<EntityGroupMember> _entityGroupMemberRepository;
      

        public HangfireAuditProjectAppService(IRepository<User, long> userRepository,
            IRepository<StateAction, long> stateActionRepository,
            IUserEmailer userEmailer, ICustomTemplateAppService customTemplateAppServiceEmailer, IRepository<EntityApplicationSetting> entityApplicationSettingRepository,
            IRepository<AuditProject, long> auditProjectRepository,
            IRepository<BusinessEntity, int> businessEntityRepository,
            IRepository<ExternalAssessment, int> externalAssessmentRepository,
            IRepository<AuditProjectStatus, long> auditProjectStatusRepository,
            IRepository<AssessmentStatusLog, long> assessmentStatusLogRepository,
            IRepository<BusinessRiskStatusLog, long> businessRiskLogRepository,
            IRepository<EntityGroupMember> entityGroupMemberRepository
           
           )
        {
            _userRepository = userRepository;
            _stateActionRepository = stateActionRepository;
            _userEmailer = userEmailer;
            _customTemplateAppServiceEmailer = customTemplateAppServiceEmailer;
            _entityApplicationSettingRepository = entityApplicationSettingRepository;
            _auditProjectRepository = auditProjectRepository;
            _businessEntityRepository = businessEntityRepository;
            _externalAssessmentRepository = externalAssessmentRepository;
            _auditProjectStatusRepository = auditProjectStatusRepository;
            _assessmentStatusLogRepository = assessmentStatusLogRepository;
            _businessRiskLogRepository = businessRiskLogRepository;
            _entityGroupMemberRepository = entityGroupMemberRepository;          
           
        }

        public async Task SendMailDailyForAuditProject()
        {
            List<CustomNotificationOutputNewDto> result = new List<CustomNotificationOutputNewDto>();

            var userList = await _userRepository.GetAll().IgnoreQueryFilters().ToListAsync();
            var entityApplicationSetting = await _entityApplicationSettingRepository.GetAll().FirstOrDefaultAsync();

            result = await GetAuditProjectEmailerList(result, userList);
            result = await GetAssessmentStatusLogEmailerList(result, userList);

            //  result = await GetBusinessRiskStatusLogEmailerList(result, userList);
         
            foreach (var emailObj in result)
            {
                emailObj.ToEmailId = emailObj.ToEmailId.Distinct().ToList();
                emailObj.CcEmailId = emailObj.CcEmailId.Distinct().ToList();
                emailObj.BccEmailId = emailObj.BccEmailId.Distinct().ToList();
                await _userEmailer.SendAuditProjectDailyAsync(emailObj.ToEmailId, emailObj.CcEmailId, emailObj.BccEmailId, 1, emailObj.Type + "", emailObj.Body, emailObj.Subject, entityApplicationSetting.Attachmentpath + "`" + emailObj.FileJson);
            }

        }

        private async Task<List<CustomNotificationOutputNewDto>> GetAuditProjectEmailerList(List<CustomNotificationOutputNewDto> result, List<User> userList)
        {
            List<string> replacerList = new List<string>();

            var stateActionList = await _stateActionRepository.GetAll().Include(x => x.State).ThenInclude(x => x.WorkFlowPage).Include(x => x.Template)
                  .Where(x => x.State.WorkFlowPage.PageName == "Audit Project" && x.State.IsStateOpen == false).ToListAsync();

            var allAuditProjectStatusInfo = await _auditProjectStatusRepository.GetAll().Include(x => x.AuditProject).ToListAsync();

            var allExternalAssessmentList = await _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity)
                .Where(x=>x.BusinessEntity.Status == EntityTypeStatus.Active).ToListAsync();

            var classProperties = await _customTemplateAppServiceEmailer.GetClassProperties(2);
            var getBusinessEntitiesProperties = await _customTemplateAppServiceEmailer.GetBusinessEntitiesProperties();
            getBusinessEntitiesProperties.ForEach(x =>
            {
                classProperties.Add(x);
            });

            replacerList = GetReplacerList(replacerList, classProperties);

            foreach (var item in stateActionList)
            {
                if (item.State.IsStateActive)
                {
                    result = await FilterByDate(allAuditProjectStatusInfo, result, item, userList, replacerList, allExternalAssessmentList);
                }
            }

            return result;
        }

        private async Task<List<CustomNotificationOutputNewDto>> FilterByDate(List<AuditProjectStatus> allAuditProjectStatus, List<CustomNotificationOutputNewDto> output, StateAction stateAction, List<User> actors, List<string> replacerLists, List<ExternalAssessment> externalAssessmentList)
        {
            List<CustomNotificationOutputNewDto> result = output;
            List<string> replacerList = replacerLists;
            var dealLineCalculation = new HHMMDDDto();

            var filterAuditProjectList = allAuditProjectStatus.Where(x => x.StatusId == stateAction.State.AuditProjectStatus && x.StatusId == x.AuditProject.AuditStatusId).ToList();

            dealLineCalculation = GetStateDeadlines(stateAction, dealLineCalculation);
            dealLineCalculation = GetDaysDeadlines(stateAction, dealLineCalculation);

            filterAuditProjectList.ForEach(x =>
            {
                DateTime compareString = new DateTime();
                DateTime currentDate = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy HH:mm"));
                DateTime todate = Convert.ToDateTime(DateTime.Now.AddMinutes(10).ToString("dd-MMM-yyyy HH:mm"));
                var approverEmailList = new List<string>();
                var businessEntitiesList = externalAssessmentList.Where(y => y.AuditProjectId == x.AuditProjectId).Select(y => y.BusinessEntity).ToList();

                var auditProjectObj = x.AuditProject;
                var tempObj = x.AuditProject;
                var props = tempObj.GetType().GetProperties();

                if (x.ActionDate != null)
                {
                    var tempDate = ((DateTime)x.ActionDate).AddDays(dealLineCalculation.Day).AddHours(dealLineCalculation.Hr).AddMinutes(dealLineCalculation.Min).ToString("dd-MMM-yyyy HH:mm");
                    compareString = Convert.ToDateTime(tempDate);
                }

                if (currentDate < compareString && compareString <= todate)
                {
                    CustomNotificationOutputNewDto temp = new CustomNotificationOutputNewDto();
                    temp.Subject = stateAction.Template.TemplateSubject;
                    temp.Body = stateAction.Template.TemplateBody;
                    temp.FileJson = stateAction.Template.TemplateDescription;
                    var mystrBody = stateAction.Template.TemplateBody;
                    var mystrSubject = stateAction.Template.TemplateSubject;
                    List<string> templatevariables = new List<string>();

                    templatevariables = GetTemplateVariables(templatevariables, mystrBody);
                    templatevariables = GetTemplateVariables(templatevariables, mystrSubject);

                    temp = GetUpdatedTemplate(props, templatevariables, temp, auditProjectObj, null, null,null);

                    var ToList = ReplaceEmails(stateAction.Template.TemplateTo, businessEntitiesList);
                    var CcList = ReplaceEmails(stateAction.Template.TemplateCc, businessEntitiesList);
                    var BccList = ReplaceEmails(stateAction.Template.TemplateBcc, businessEntitiesList);

                    if (ToList.Count() > 0)
                    {
                        temp.ToEmailId.AddRange(ToList);
                    }

                    if (CcList.Count() > 0)
                    {
                        temp.CcEmailId.AddRange(CcList);
                    }

                    if (BccList.Count() > 0)
                    {
                        temp.BccEmailId.AddRange(BccList);
                    }

                    temp.Type = "" + stateAction.Template.TemplateTitle;
                    result.Add(temp);
                }
            });

            return output;
        }

        #region Common Private Methods

        private List<string> GetTemplateVariables(List<string> templateVariables, string str)
        {
            List<string> result = new List<string>();
            result = templateVariables;
            while (str.Contains("{"))
            {
                result.Add("{" + str.Split('{', '}')[1] + "}");
                str = str.Replace("{" + str.Split('{', '}')[1] + "}", "");
            };
            return result;
        }

        private List<string> GetReplacerList(List<string> replacerList, List<string> classProperties)
        {
            List<string> result = new List<string>();
            result = replacerList;
            classProperties.ForEach(x =>
            {
                result.Add("{" + x + "}");
            });
            return result;
        }

        private List<string> ReplaceEmails(string input, List<BusinessEntity> businessEntities)
        {
            var result = new List<string>();
            input = input == null ? "" : input;
            List<string> emailArray = input.Split(',').ToList();

            emailArray.ForEach(templateVariables =>
            {
                if (templateVariables.Contains("{"))
                {
                    if (templateVariables.Contains("{Business_Entity_Admin_Email}"))
                    {
                        var emailList1 = businessEntities.Where(x => x.AdminEmail != null && x.AdminEmail != "").Select(x => x.AdminEmail).Distinct().ToList();
                        result = GetFinalEmailList(emailList1, result);
                    }
                    else if (templateVariables.Contains("{Audit_Agency_Admin_Email}"))
                    {
                        var emailList2 = businessEntities.Where(x => x.AdminEmail != null && x.AdminEmail != "").Select(x => x.AdminEmail).Distinct().ToList();
                        result = GetFinalEmailList(emailList2, result);
                    }
                    else if (templateVariables.Contains("{Owner_Email}"))
                    {
                        var emailList3 = businessEntities.Where(x => x.Owner_Email != null && x.Owner_Email != "").Select(x => x.Owner_Email).Distinct().ToList();
                        result = GetFinalEmailList(emailList3, result);
                    }
                    else if (templateVariables.Contains("{Director_Incharge_Email}"))
                    {
                        var emailList4 = businessEntities.Where(x => x.Director_Incharge_Email != null && x.Director_Incharge_Email != "").Select(x => x.Director_Incharge_Email).Distinct().ToList();
                        result = GetFinalEmailList(emailList4, result);
                    }
                    else if (templateVariables.Contains("{CISO_Email}"))
                    {
                        var emailList5 = businessEntities.Where(x => x.CISO_Email != null && x.CISO_Email != "").Select(x => x.CISO_Email).Distinct().ToList();
                        result = GetFinalEmailList(emailList5, result);
                    }
                    else if (templateVariables.Contains("{Primary_Contact_Email}"))
                    {
                        var emailList6 = businessEntities.Where(x => x.OfficialEmail != null && x.OfficialEmail != "").Select(x => x.OfficialEmail).Distinct().ToList();
                        result = GetFinalEmailList(emailList6, result);
                    }
                    else if (templateVariables.Contains("{Secondary_Contact_Email}"))
                    {
                        var emailList7 = businessEntities.Where(x => x.BackupOfficialEmail != null && x.BackupOfficialEmail != "").Select(x => x.BackupOfficialEmail).Distinct().ToList();
                        result = GetFinalEmailList(emailList7, result);
                    }
                    else if (templateVariables.Contains("{LeadAuditor_Email}"))
                    {
                        var emailList8 = businessEntities.Where(x => x.AdminEmail != null && x.AdminEmail != "").Select(x => x.AdminEmail).Distinct().ToList();
                        result = GetFinalEmailList(emailList8, result);
                    }
                    else if (templateVariables.Contains("{Group_Admin}"))
                    {
                        var businessEntityList = businessEntities.Select(x => x.Id);
                        var getGroup = _entityGroupMemberRepository.GetAll().Include(x=>x.EntityGroup).Where(x => businessEntityList.Contains(x.BusinessEntityId)).FirstOrDefault();
                        if (getGroup != null)
                        {
                            var getuser = _userRepository.FirstOrDefault(x => x.Id == getGroup.EntityGroup.UserId);
                            var usermailId = new List<string>() { getuser.EmailAddress };
                            result = GetFinalEmailList(usermailId, result);
                        }
                    }

                }
                else
                {
                    string email = templateVariables.Trim();
                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                    if (isEmail == true)
                    {
                        result.Add(email);
                    }
                }
            });
            var list = result;
            
            return result;
        }

        private List<string> ReplaceGroupEmails(string input, List<EntityGroupMember> businessEntities)
        {
            var result = new List<string>();
            input = input == null ? "" : input;
            List<string> emailArray = input.Split(',').ToList();

            emailArray.ForEach(templateVariables =>
            {
                if (templateVariables.Contains("{"))
                {
                    if (templateVariables.Contains("{Business_Entity_Admin_Email}"))
                    {
                        var emailList1 = businessEntities.Where(x => x.BusinessEntity.AdminEmail != null && x.BusinessEntity.AdminEmail != "").Select(x => x.BusinessEntity.AdminEmail).Distinct().ToList();
                        result = GetFinalEmailList(emailList1, result);
                    }
                    else if (templateVariables.Contains("{Audit_Agency_Admin_Email}"))
                    {
                        var emailList2 = businessEntities.Where(x => x.BusinessEntity.AdminEmail != null && x.BusinessEntity.AdminEmail != "").Select(x => x.BusinessEntity.AdminEmail).Distinct().ToList();
                        result = GetFinalEmailList(emailList2, result);
                    }
                    else if (templateVariables.Contains("{Owner_Email}"))
                    {
                        var emailList3 = businessEntities.Where(x => x.BusinessEntity.Owner_Email != null && x.BusinessEntity.Owner_Email != "").Select(x => x.BusinessEntity.Owner_Email).Distinct().ToList();
                        result = GetFinalEmailList(emailList3, result);
                    }
                    else if (templateVariables.Contains("{Director_Incharge_Email}"))
                    {
                        var emailList4 = businessEntities.Where(x => x.BusinessEntity.Director_Incharge_Email != null && x.BusinessEntity.Director_Incharge_Email != "").Select(x => x.BusinessEntity.Director_Incharge_Email).Distinct().ToList();
                        result = GetFinalEmailList(emailList4, result);
                    }
                    else if (templateVariables.Contains("{CISO_Email}"))
                    {
                        var emailList5 = businessEntities.Where(x => x.BusinessEntity.CISO_Email != null && x.BusinessEntity.CISO_Email != "").Select(x => x.BusinessEntity.CISO_Email).Distinct().ToList();
                        result = GetFinalEmailList(emailList5, result);
                    }
                    else if (templateVariables.Contains("{Primary_Contact_Email}"))
                    {
                        var emailList6 = businessEntities.Where(x => x.BusinessEntity.OfficialEmail != null && x.BusinessEntity.OfficialEmail != "").Select(x => x.BusinessEntity.OfficialEmail).Distinct().ToList();
                        result = GetFinalEmailList(emailList6, result);
                    }
                    else if (templateVariables.Contains("{Secondary_Contact_Email}"))
                    {
                        var emailList7 = businessEntities.Where(x => x.BusinessEntity.BackupOfficialEmail != null && x.BusinessEntity.BackupOfficialEmail != "").Select(x => x.BusinessEntity.BackupOfficialEmail).Distinct().ToList();
                        result = GetFinalEmailList(emailList7, result);
                    }
                    else if (templateVariables.Contains("{LeadAuditor_Email}"))
                    {
                        var emailList8 = businessEntities.Where(x => x.BusinessEntity.AdminEmail != null && x.BusinessEntity.AdminEmail != "").Select(x => x.BusinessEntity.AdminEmail).Distinct().ToList();
                        result = GetFinalEmailList(emailList8, result);
                    }

                }
                else
                {
                    string email = templateVariables.Trim();
                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                    if (isEmail == true)
                    {
                        result.Add(email);
                    }
                }
            });

            return result;
        }

        public List<string> GetFinalEmailList(List<string> input, List<string> output)
        {
            var result = output;
            input.ForEach(x =>
            {
                //result.AddRange(x.Split(","));
                var splitEmail = x.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in splitEmail)
                {
                    string email = item.Trim();
                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                    if (isEmail == true)
                    {
                        result.Add(item);
                    }
                }
            });
            return result;
        }

        private HHMMDDDto GetStateDeadlines(StateAction sa, HHMMDDDto output)
        {
            HHMMDDDto result = output;
            switch (sa.State.ActionTimeType)
            {
                case ActionTimeType.Days:
                    result.Day += sa.State.StateDeadline;
                    break;
                case ActionTimeType.Weeks:
                    result.Day += sa.State.StateDeadline * 7;
                    break;
                case ActionTimeType.Months:
                    result.Day += sa.State.StateDeadline * 30;
                    break;
                case ActionTimeType.Hours:
                    result.Hr += sa.State.StateDeadline;
                    break;
                case ActionTimeType.Minutes:
                    result.Min += sa.State.StateDeadline;
                    break;
                default:
                    break;
            }
            return result;
        }

        private HHMMDDDto GetDaysDeadlines(StateAction sa, HHMMDDDto output)
        {
            HHMMDDDto result = output;
            if (sa.ActionTimeType == ActionTimeType.Days)
            {
                switch (sa.ActionCategory)
                {
                    case ActionCategory.Before:
                        result.Day += (0 - sa.ActionTime);
                        break;
                    case ActionCategory.Onday:
                        result.Day += 0;
                        break;
                    case ActionCategory.After:
                        result.Day += sa.ActionTime;
                        break;
                    case ActionCategory.Escalation:
                        result.Day += sa.ActionTime;
                        break;
                    default:
                        break;
                }
            }
            else if (sa.ActionTimeType == ActionTimeType.Weeks || sa.ActionTimeType == ActionTimeType.Months)
            {
                int additionalDays = sa.ActionTimeType == ActionTimeType.Weeks ? 7 : 30;
                switch (sa.ActionCategory)
                {
                    case ActionCategory.Before:
                        result.Day += (0 - (sa.ActionTime * additionalDays));
                        break;
                    case ActionCategory.Onday:
                        result.Day += 0;
                        break;
                    case ActionCategory.After:
                        result.Day += (sa.ActionTime * additionalDays);
                        break;
                    case ActionCategory.Escalation:
                        result.Day += (sa.ActionTime * additionalDays);
                        break;
                    default:
                        break;
                }
            }
            else if (sa.ActionTimeType == ActionTimeType.Hours)
            {
                switch (sa.ActionCategory)
                {
                    case ActionCategory.Before:
                        result.Hr += (0 - sa.ActionTime);
                        break;
                    case ActionCategory.Onday:
                        result.Hr += 0;
                        break;
                    case ActionCategory.After:
                        result.Hr += sa.ActionTime;
                        break;
                    case ActionCategory.Escalation:
                        result.Hr += sa.ActionTime;
                        break;
                    default:
                        break;
                }
            }
            else if (sa.ActionTimeType == ActionTimeType.Minutes)
            {
                switch (sa.ActionCategory)
                {
                    case ActionCategory.Before:
                        result.Min += (0 - sa.ActionTime);
                        break;
                    case ActionCategory.Onday:
                        result.Min += 0;
                        break;
                    case ActionCategory.After:
                        result.Min += sa.ActionTime;
                        break;
                    case ActionCategory.Escalation:
                        result.Min += sa.ActionTime;
                        break;
                    default:
                        break;
                }
            }


            return result;
        }


        #endregion

        private CustomNotificationOutputNewDto GetUpdatedTemplate(PropertyInfo[] ObjectProperties, List<string> templateVariables, CustomNotificationOutputNewDto templateObj, AuditProject auditProjectObj, Assessment assessmentObj, BusinessRisk businessRiskObj,AssessmentTemplateDto assessmentTemplateDto)
        {
            CustomNotificationOutputNewDto result = new CustomNotificationOutputNewDto();
            result = templateObj;

            if (auditProjectObj != null)
            {
                templateVariables.ForEach(x =>
                {
                    foreach (var column in ObjectProperties)
                    {
                        if ("{" + column.Name + "}" == x)
                        {
                            var newValue3 = "" + column.GetValue(auditProjectObj);
                            templateObj.Subject = templateObj.Subject.Replace(x, newValue3);
                            templateObj.Body = templateObj.Body.Replace(x, newValue3);
                        }
                    }
                });
            }
            else if (assessmentTemplateDto != null)
            {
                try
                {
                    foreach (var x in templateVariables)
                    {
                        foreach (var column in ObjectProperties)
                        {
                            if ("{" + column.Name + "}" == x)
                            {
                                var newValue3 = "" + column.GetValue(assessmentTemplateDto);
                                templateObj.Subject = templateObj.Subject.Replace(x, newValue3);
                                templateObj.Body = templateObj.Body.Replace(x, newValue3);
                            }
                        }
                    }
                }
                catch(Exception ex)
                {

                }
            }
            else if (businessRiskObj != null)
            {
                templateVariables.ForEach(x =>
                {
                    foreach (var column in ObjectProperties)
                    {
                        if ("{" + column.Name + "}" == x)
                        {
                            var newValue3 = "" + column.GetValue(businessRiskObj);
                            templateObj.Subject = templateObj.Subject.Replace(x, newValue3);
                            templateObj.Body = templateObj.Body.Replace(x, newValue3);
                        }
                    }
                });
            }



            return result;
        }

        // For Assessment Status Log table Functions

        private async Task<List<CustomNotificationOutputNewDto>> GetAssessmentStatusLogEmailerList(List<CustomNotificationOutputNewDto> result, List<User> userList)
        {
            List<string> replacerList = new List<string>();

            var stateActionList = await _stateActionRepository.GetAll().Include(x => x.State).ThenInclude(x => x.WorkFlowPage).Include(x => x.Template)
                  .Where(x => x.State.WorkFlowPage.PageName == "Self Assessment" && x.State.IsStateOpen == false).ToListAsync();

            var allAssessmentStatusLogInfo = await _assessmentStatusLogRepository.GetAll().Include(x => x.Assessment).ThenInclude(x => x.BusinessEntity)
                .Where(x => x.Status == x.Assessment.Status && x.Assessment.BusinessEntity.Status == EntityTypeStatus.Active).ToListAsync();

            var classProperties = await _customTemplateAppServiceEmailer.GetClassProperties(6);
            var getBusinessEntitiesProperties = await _customTemplateAppServiceEmailer.GetBusinessEntitiesProperties();
            getBusinessEntitiesProperties.ForEach(x =>
            {
                classProperties.Add(x);
            });

            replacerList = GetReplacerList(replacerList, classProperties);

            foreach (var item in stateActionList)
            {
                if (item.State.IsStateActive)
                {
                    result = await FilterByDateAssessmentLog(allAssessmentStatusLogInfo, result, item, userList, replacerList);
                }
            }

            return result;
        }

        private async Task<List<CustomNotificationOutputNewDto>> FilterByDateAssessmentLog(List<AssessmentStatusLog> allAssessmentStatusLog, List<CustomNotificationOutputNewDto> output, StateAction stateAction, List<User> actors, List<string> replacerLists)
        {
            List<CustomNotificationOutputNewDto> result = output;
            List<string> replacerList = replacerLists;
            var filterAssessmentLogList = new List<AssessmentStatusLog>();

            var dealLineCalculation = new HHMMDDDto();

            var query = allAssessmentStatusLog.Where(x => x.Status == (AssessmentStatus)stateAction.State.AuditProjectStatus).ToList();

            filterAssessmentLogList = query.OrderBy(x => x.ActionDate).GroupBy(x => x.AssessmentId)
                .Select(y => y.GroupBy(x => x.Status)
                .Select(x => x.OrderByDescending(y => y.ActionDate).FirstOrDefault()).FirstOrDefault())
                .ToList();
            DateTime currentDate2 = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy HH:mm"));
            DateTime todate2 = Convert.ToDateTime(DateTime.Now.AddMinutes(10).ToString("dd-MMM-yyyy HH:mm"));
            dealLineCalculation = GetStateDeadlines(stateAction, dealLineCalculation);
            dealLineCalculation = GetDaysDeadlines(stateAction, dealLineCalculation);
            var Grp = filterAssessmentLogList.GroupBy(c => new { c.Assessment.EntityGroupId})
                      .Select(group => new
                                  {
                                   EntityGroupId = group.Key.EntityGroupId,
                                  AssessmentLogList = group.Where(x=>Convert.ToDateTime(((DateTime)x.CreationTime).AddDays(dealLineCalculation.Day).AddHours(dealLineCalculation.Hr).AddMinutes(dealLineCalculation.Min).ToString("dd-MMM-yyyy HH:mm")) > currentDate2 && Convert.ToDateTime(((DateTime)x.CreationTime).AddDays(dealLineCalculation.Day).AddHours(dealLineCalculation.Hr).AddMinutes(dealLineCalculation.Min).ToString("dd-MMM-yyyy HH:mm")) <= todate2).ToList(),
                                  BusinessEntity = group.Where(x => Convert.ToDateTime(((DateTime)x.CreationTime).AddDays(dealLineCalculation.Day).AddHours(dealLineCalculation.Hr).AddMinutes(dealLineCalculation.Min).ToString("dd-MMM-yyyy HH:mm")) > currentDate2 && Convert.ToDateTime(((DateTime)x.CreationTime).AddDays(dealLineCalculation.Day).AddHours(dealLineCalculation.Hr).AddMinutes(dealLineCalculation.Min).ToString("dd-MMM-yyyy HH:mm")) <= todate2).Select(x=>x.Assessment.BusinessEntity).ToList()
                      }).ToList();

            var GroupList = Grp.Where(x => x.EntityGroupId != null).ToList();
            var SingleEntity = Grp.Where(x => x.EntityGroupId == null).FirstOrDefault();
            if (GroupList != null)
            {
                foreach (var y in GroupList)
                {
                    CustomNotificationOutputNewDto temp = new CustomNotificationOutputNewDto();
                    if (y.EntityGroupId != null)
                    {
                        var getx = y.AssessmentLogList.LastOrDefault();
                        if (getx != null)
                        {
                            DateTime compareString = new DateTime();
                            DateTime currentDate = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy HH:mm"));
                            DateTime todate = Convert.ToDateTime(DateTime.Now.AddMinutes(10).ToString("dd-MMM-yyyy HH:mm"));
                            var approverEmailList = new List<string>();
                            var businessEntitiesList = new List<BusinessEntity>();
                            //businessEntitiesList = await _entityGroupMemberRepository.GetAll().Where(x => x.EntityGroupId == y.EntityGroupId).Include(X => X.BusinessEntity).ToListAsync();
                            businessEntitiesList = y.BusinessEntity;
                            var assessmentTemplateDto = new AssessmentTemplateDto();
                            if (getx.Assessment != null)
                            {
                               
                                    assessmentTemplateDto.Name = getx.Assessment.Name;
                                    assessmentTemplateDto.EntityGroupId = getx.Assessment.EntityGroupId;
                                    assessmentTemplateDto.ReportingDate = getx.Assessment.ReportingDeadLine;
                                    assessmentTemplateDto.ScheduleDetailId = getx.Assessment.ScheduleDetailId;
                                    assessmentTemplateDto.AssessmentDate = getx.Assessment.Date;
                                    assessmentTemplateDto.AuthoritativeDocumentId = getx.Assessment.AuthoritativeDocumentId;
                                    assessmentTemplateDto.AssessmentTypeId = getx.Assessment.AssessmentTypeId;
                                    assessmentTemplateDto.AssessmentType = getx.Assessment.AssessmentType;
                                    assessmentTemplateDto.Info = getx.Assessment.Info;
                                    assessmentTemplateDto.SendSmsNotification = getx.Assessment.SendSmsNotification;
                                    assessmentTemplateDto.SendEmailNotification = getx.Assessment.SendEmailNotification;
                                    assessmentTemplateDto.Feedback = getx.Assessment.Feedback;
                                    assessmentTemplateDto.AuthoritativeDocumentName = getx.Assessment.AuthoritativeDocumentName;
                                    assessmentTemplateDto.Code = getx.Assessment.Code;
                                    assessmentTemplateDto.Status = getx.Assessment.Status;
                                    assessmentTemplateDto.BusinessEntityName = getx.Assessment.BusinessEntityName;
                                    assessmentTemplateDto.BusinessEntityId = getx.Assessment.BusinessEntityId;
                                    assessmentTemplateDto.ReviewScore = getx.Assessment.ReviewScore;
                                   assessmentTemplateDto.EntityType = getx.Assessment.BusinessEntity.EntityType;
                                
                                var sb = "";
                                if (businessEntitiesList.Count > 0)
                                {

                                    sb = sb + "<div style='float:center !important'>";

                                    sb = sb + "<table style='width:100%; font-size: 12px;border-collapse: collapse'>";
                                    sb = sb + "<tr style='background-color:sandybrown; border:solid 1px black; font-size: 8px !important'>";
                                    sb = sb + "<th style='width:15%; border:solid 1px black;font-family:Arial, Helvetica;text-align: center; font-size: 12px !important'>LICENSE NUMBER</th>";
                                    sb = sb + "<th style='width:55%; border:solid 1px black;font-family:Arial, Helvetica;text-align: center; font-size: 12px !important'>FACILITY NAME</th>";
                                    foreach (var item in businessEntitiesList)
                                    {
                                        sb = sb + "<tr style='border:solid 1px black'>";
                                        sb = sb + "<td style='border: solid 1px black; font-family:Arial, Helvetica, sans-serif;text-align: center;font-size: 8px !important'>" + item.LicenseNumber + "</td>";
                                        sb = sb + "<td style='border:solid 1px black; font-family:Arial, Helvetica, sans-serif;text-align: center;font-size: 8px !important'>" + item.CompanyName + "</td>";                                        
                                        sb = sb + "</tr>";
                                    }
                                    sb = sb + "</table></div>";
                                }
                                assessmentTemplateDto.EntityLists = sb;
                                var getyear = Convert.ToDateTime(assessmentTemplateDto.ReportingDate).Year;
                                if (Convert.ToDateTime(assessmentTemplateDto.ReportingDate).Month == 3)
                                {

                                    assessmentTemplateDto.Quater = "Q1 - " + getyear;
                                }
                                else if (Convert.ToDateTime(assessmentTemplateDto.ReportingDate).Month == 6)
                                {

                                    assessmentTemplateDto.Quater = "Q2 - " + getyear;
                                }
                                else if (Convert.ToDateTime(assessmentTemplateDto.ReportingDate).Month == 9)
                                {

                                    assessmentTemplateDto.Quater = "Q3 - " + getyear;
                                }
                                else if (Convert.ToDateTime(assessmentTemplateDto.ReportingDate).Month == 12)
                                {

                                    assessmentTemplateDto.Quater = "Q4 - " + getyear;
                                }
                            }
                            var assessmentObj =  assessmentTemplateDto;
                            var tempObj = assessmentObj;
                            var props = tempObj.GetType().GetProperties();

                            if (getx.ActionDate != null)
                            {
                                var tempDate = ((DateTime)getx.ActionDate).AddDays(dealLineCalculation.Day).AddHours(dealLineCalculation.Hr).AddMinutes(dealLineCalculation.Min).ToString("dd-MMM-yyyy HH:mm");
                                compareString = Convert.ToDateTime(tempDate);
                            }
                            if (currentDate < compareString && compareString <= todate)
                            {

                                temp.Subject = stateAction.Template.TemplateSubject;
                                temp.Body = stateAction.Template.TemplateBody;

                                temp.FileJson = stateAction.Template.TemplateDescription;
                                var mystrBody = stateAction.Template.TemplateBody;
                                var mystrSubject = stateAction.Template.TemplateSubject;
                                List<string> templatevariables = new List<string>();

                                templatevariables = GetTemplateVariables(templatevariables, mystrBody);
                                templatevariables = GetTemplateVariables(templatevariables, mystrSubject);

                                temp = GetUpdatedTemplate(props, templatevariables, temp, null, null, null, assessmentObj);

                                var ToList = ReplaceEmails(stateAction.Template.TemplateTo, businessEntitiesList);
                                var CcList = ReplaceEmails(stateAction.Template.TemplateCc, businessEntitiesList);
                                var BccList = ReplaceEmails(stateAction.Template.TemplateBcc, businessEntitiesList);

                                if (ToList.Count() > 0)
                                {
                                    temp.ToEmailId.AddRange(ToList);
                                }

                                if (CcList.Count() > 0)
                                {
                                    temp.CcEmailId.AddRange(CcList);
                                }

                                if (BccList.Count() > 0)
                                {
                                    temp.BccEmailId.AddRange(BccList);
                                }
                                temp.Type = "" + stateAction.Template.TemplateTitle;
                            }
                            result.Add(temp);
                        }
                    }
                }
            }
            if(SingleEntity != null)
            {                 
                    foreach (var ut in SingleEntity.AssessmentLogList)
                    {
                       CustomNotificationOutputNewDto temp = new CustomNotificationOutputNewDto();
                        DateTime compareString = new DateTime();
                        DateTime currentDate = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy HH:mm"));
                        DateTime todate = Convert.ToDateTime(DateTime.Now.AddMinutes(10).ToString("dd-MMM-yyyy HH:mm"));
                        var approverEmailList = new List<string>();
                        var businessEntitiesList = new List<BusinessEntity>();
                        businessEntitiesList.Add(ut.Assessment.BusinessEntity);
                        var assessmentTemplateDto = new AssessmentTemplateDto();
                        if (ut.Assessment != null)
                        {
                        assessmentTemplateDto.Name = ut.Assessment.Name;
                        assessmentTemplateDto.EntityGroupId = ut.Assessment.EntityGroupId;
                        assessmentTemplateDto.ReportingDate = ut.Assessment.ReportingDeadLine;
                        assessmentTemplateDto.ScheduleDetailId = ut.Assessment.ScheduleDetailId;
                        assessmentTemplateDto.AssessmentDate = ut.Assessment.Date;
                        assessmentTemplateDto.AuthoritativeDocumentId = ut.Assessment.AuthoritativeDocumentId;
                        assessmentTemplateDto.AssessmentTypeId = ut.Assessment.AssessmentTypeId;
                        assessmentTemplateDto.AssessmentType = ut.Assessment.AssessmentType;
                        assessmentTemplateDto.Info = ut.Assessment.Info;
                        assessmentTemplateDto.SendSmsNotification = ut.Assessment.SendSmsNotification;
                        assessmentTemplateDto.SendEmailNotification = ut.Assessment.SendEmailNotification;
                        assessmentTemplateDto.Feedback = ut.Assessment.Feedback;
                        assessmentTemplateDto.AuthoritativeDocumentName = ut.Assessment.AuthoritativeDocumentName;
                        assessmentTemplateDto.Code = ut.Assessment.Code;
                        assessmentTemplateDto.Status = ut.Assessment.Status;
                        assessmentTemplateDto.BusinessEntityName = ut.Assessment.BusinessEntityName;
                        assessmentTemplateDto.BusinessEntityId = ut.Assessment.BusinessEntityId;
                        assessmentTemplateDto.ReviewScore = ut.Assessment.ReviewScore;
                        assessmentTemplateDto.EntityType = ut.Assessment.BusinessEntity.EntityType;

                        var sb = "";
                        if (businessEntitiesList.Count > 0)
                        {

                            sb = sb + "<div style='float:center !important'>";

                            sb = sb + "<table style='width:100%; font-size: 12px;border-collapse: collapse'>";
                            sb = sb + "<tr style='background-color:sandybrown; border:solid 1px black; font-size: 8px !important'>";
                            sb = sb + "<th style='width:15%; border:solid 1px black;font-family:Arial, Helvetica;text-align: center; font-size: 12px !important'>LICENSE NUMBER</th>";
                            sb = sb + "<th style='width:55%; border:solid 1px black;font-family:Arial, Helvetica;text-align: center; font-size: 12px !important'>FACILITY NAME</th>";
                           
                                sb = sb + "<tr style='border:solid 1px black'>";
                                sb = sb + "<td style='border: solid 1px black; font-family:Arial, Helvetica, sans-serif;text-align: center;font-size: 8px !important'>" + ut.Assessment.BusinessEntity.LicenseNumber + "</td>";
                                sb = sb + "<td style='border:solid 1px black; font-family:Arial, Helvetica, sans-serif;text-align: center;font-size: 8px !important'>" + ut.Assessment.BusinessEntity.CompanyName + "</td>";
                                sb = sb + "</tr>";
                            
                            sb = sb + "</table></div>";
                        }
                        assessmentTemplateDto.EntityLists = sb;
                        var getyear = Convert.ToDateTime(assessmentTemplateDto.ReportingDate).Year;                       
                        if (Convert.ToDateTime(assessmentTemplateDto.ReportingDate).Month == 3)
                        {

                            assessmentTemplateDto.Quater = "Q1 - " + getyear;
                        }
                        else if (Convert.ToDateTime(assessmentTemplateDto.ReportingDate).Month == 6)
                        {

                            assessmentTemplateDto.Quater = "Q2 - " + getyear;
                        }
                        else if (Convert.ToDateTime(assessmentTemplateDto.ReportingDate).Month == 9)
                        {

                            assessmentTemplateDto.Quater = "Q3 - " + getyear;
                        }
                        else if (Convert.ToDateTime(assessmentTemplateDto.ReportingDate).Month == 12)
                        {

                            assessmentTemplateDto.Quater = "Q4 - " + getyear;
                        }
                    }
                        var assessmentObj = assessmentTemplateDto;
                        var tempObj = assessmentObj;
                        var props = tempObj.GetType().GetProperties();

                        if (ut.ActionDate != null)
                        {
                            var tempDate = ((DateTime)ut.ActionDate).AddDays(dealLineCalculation.Day).AddHours(dealLineCalculation.Hr).AddMinutes(dealLineCalculation.Min).ToString("dd-MMM-yyyy HH:mm");
                            compareString = Convert.ToDateTime(tempDate);
                        }
                        if (currentDate < compareString && compareString <= todate)
                        {

                            temp.Subject = stateAction.Template.TemplateSubject;
                            temp.Body = stateAction.Template.TemplateBody;

                            temp.FileJson = stateAction.Template.TemplateDescription;
                            var mystrBody = stateAction.Template.TemplateBody;
                            var mystrSubject = stateAction.Template.TemplateSubject;
                            List<string> templatevariables = new List<string>();

                            templatevariables = GetTemplateVariables(templatevariables, mystrBody);
                            templatevariables = GetTemplateVariables(templatevariables, mystrSubject);

                            temp = GetUpdatedTemplate(props, templatevariables, temp, null, null, null, assessmentObj);

                            var ToList = ReplaceEmails(stateAction.Template.TemplateTo, businessEntitiesList);
                            var CcList = ReplaceEmails(stateAction.Template.TemplateCc, businessEntitiesList);
                            var BccList = ReplaceEmails(stateAction.Template.TemplateBcc, businessEntitiesList);

                            if (ToList.Count() > 0)
                            {
                                temp.ToEmailId.AddRange(ToList);
                            }

                            if (CcList.Count() > 0)
                            {
                                temp.CcEmailId.AddRange(CcList);
                            }

                            if (BccList.Count() > 0)
                            {
                                temp.BccEmailId.AddRange(BccList);
                            }
                            temp.Type = "" + stateAction.Template.TemplateTitle;

                        result.Add(temp);
                        }
                       
                    }
                
            }            
            return output;
        }


        // For Business Risk

        private async Task<List<CustomNotificationOutputNewDto>> GetBusinessRiskStatusLogEmailerList(List<CustomNotificationOutputNewDto> result, List<User> userList)
        {
            List<string> replacerList = new List<string>();

            var stateActionList = await _stateActionRepository.GetAll().Include(x => x.State).ThenInclude(x => x.WorkFlowPage).Include(x => x.Template)
                  .Where(x => x.State.WorkFlowPage.PageName == "Business Risks" && x.State.IsStateOpen == false && x.State.ActionTimeType == ActionTimeType.Days).ToListAsync();

            var allAssessmentStatusLogInfo = await _businessRiskLogRepository.GetAll().Include(x => x.BusinessRisk).ThenInclude(x => x.BusinessEntity)
                .Where(x => x.StatusId == x.BusinessRisk.StatusId).ToListAsync();

            var classProperties = await _customTemplateAppServiceEmailer.GetClassProperties(3);
            var getBusinessEntitiesProperties = await _customTemplateAppServiceEmailer.GetBusinessEntitiesProperties();
            getBusinessEntitiesProperties.ForEach(x =>
            {
                classProperties.Add(x);
            });

            replacerList = GetReplacerList(replacerList, classProperties);

            foreach (var item in stateActionList)
            {
                if (item.State.IsStateActive)
                {
                    result = await FilterByDateBusinessRiskStatusLog(allAssessmentStatusLogInfo, result, item, userList, replacerList);
                }
            }

            return result;
        }

        private async Task<List<CustomNotificationOutputNewDto>> FilterByDateBusinessRiskStatusLog(List<BusinessRiskStatusLog> allBusinessRiskStatusLog, List<CustomNotificationOutputNewDto> output, StateAction stateAction, List<User> actors, List<string> replacerLists)
        {
            List<CustomNotificationOutputNewDto> result = output;
            List<string> replacerList = replacerLists;
            var filterBusinessRiskStatusLogList = new List<BusinessRiskStatusLog>();
            var dealLineCalculation = new HHMMDDDto();
            var query = allBusinessRiskStatusLog.Where(x => x.StatusId == stateAction.State.AuditProjectStatus).ToList();

            filterBusinessRiskStatusLogList = query.OrderBy(x => x.ActionDate).GroupBy(x => x.BusinessRiskId)
                .Select(y => y.GroupBy(x => x.Status)
                .Select(x => x.OrderByDescending(y => y.ActionDate).FirstOrDefault()).FirstOrDefault())
                .ToList();

            dealLineCalculation = GetStateDeadlines(stateAction, dealLineCalculation);
            dealLineCalculation = GetDaysDeadlines(stateAction, dealLineCalculation);

            filterBusinessRiskStatusLogList.ForEach(x =>
            {
                DateTime compareString = new DateTime();
                DateTime currentDate = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy HH:mm"));
                DateTime todate = Convert.ToDateTime(DateTime.Now.AddMinutes(10).ToString("dd-MMM-yyyy HH:mm"));
                var approverEmailList = new List<string>();
                var businessEntitiesList = new List<BusinessEntity>();
                businessEntitiesList.Add(x.BusinessRisk.BusinessEntity);

                var businessRiskObj = x.BusinessRisk;
                var tempObj = x.BusinessRisk;
                var props = tempObj.GetType().GetProperties();

                if (x.ActionDate != null)
                {
                    var tempDate = ((DateTime)x.ActionDate).AddDays(dealLineCalculation.Day).AddHours(dealLineCalculation.Hr).AddMinutes(dealLineCalculation.Min).ToString("dd-MMM-yyyy HH:mm");
                    compareString = Convert.ToDateTime(tempDate);
                }
                if (currentDate < compareString && compareString <= todate)
                {
                    CustomNotificationOutputNewDto temp = new CustomNotificationOutputNewDto();
                    temp.Subject = stateAction.Template.TemplateSubject;
                    temp.Body = stateAction.Template.TemplateBody;
                    temp.FileJson = stateAction.Template.TemplateDescription;
                    var mystrBody = stateAction.Template.TemplateBody;
                    var mystrSubject = stateAction.Template.TemplateSubject;
                    List<string> templatevariables = new List<string>();

                    templatevariables = GetTemplateVariables(templatevariables, mystrBody);
                    templatevariables = GetTemplateVariables(templatevariables, mystrSubject);

                    temp = GetUpdatedTemplate(props, templatevariables, temp, null, null, businessRiskObj,null);

                    var ToList = ReplaceEmails(stateAction.Template.TemplateTo, businessEntitiesList);
                    var CcList = ReplaceEmails(stateAction.Template.TemplateCc, businessEntitiesList);
                    var BccList = ReplaceEmails(stateAction.Template.TemplateBcc, businessEntitiesList);

                    if (ToList.Count() > 0)
                    {
                        //  temp.ToEmailId.AddRange(ToList);
                    }

                    if (CcList.Count() > 0)
                    {
                        // temp.CcEmailId.AddRange(CcList);
                    }

                    if (BccList.Count() > 0)
                    {
                        // temp.BccEmailId.AddRange(BccList);
                    }
                    temp.Type = "" + stateAction.Template.TemplateTitle;

                    result.Add(temp);
                }
            });

            return output;
        }


    }
}
