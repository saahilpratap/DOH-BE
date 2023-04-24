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

namespace LockthreatCompliance.Hangfire
{
    public class HangfireCustomAppService : LockthreatComplianceAppServiceBase, IHangfireCustomAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<StateAction, long> _stateActionRepository;
        private readonly IRepository<WorkFlowPage, long> _WorkFlowPageRepository;
        private readonly IRepository<BusinessRisk, int> _businessRiskRepository;
        private readonly IRepository<Incident> _incidentRepository;
        private readonly IRepository<FindingReport> _findingReportsRepository;
        private readonly IRepository<LockthreatCompliance.Exceptions.Exception> _exceptionRepository;
        private readonly IUserEmailer _userEmailer;
        private readonly ICustomTemplateAppService _customTemplateAppServiceEmailer;
        private readonly IRepository<EntityApplicationSetting> _entityApplicationSettingRepository;


        public HangfireCustomAppService(IRepository<User, long> userRepository,
            IRepository<StateAction, long> stateActionRepository, IRepository<BusinessRisk, int> businessRiskRepository,
            IUserEmailer userEmailer, ICustomTemplateAppService customTemplateAppServiceEmailer,
            IRepository<LockthreatCompliance.Exceptions.Exception> exceptionRepository, IRepository<Incident> incidentRepository,
            IRepository<FindingReport> findingReportsRepository, IRepository<EntityApplicationSetting> entityApplicationSettingRepository

           )
        {
            _userRepository = userRepository;
            _businessRiskRepository = businessRiskRepository;
            _stateActionRepository = stateActionRepository;
            _userEmailer = userEmailer;
            _customTemplateAppServiceEmailer = customTemplateAppServiceEmailer;
            _exceptionRepository = exceptionRepository;
            _incidentRepository = incidentRepository;
            _findingReportsRepository = findingReportsRepository;
            _entityApplicationSettingRepository = entityApplicationSettingRepository;
        }

        public async Task SendMailDaily()
        {
            List<CustomNotificationOutputDto> result = new List<CustomNotificationOutputDto>();
            var userList = await _userRepository.GetAll().IgnoreQueryFilters().ToListAsync();
            var entityApplicationSetting = await _entityApplicationSettingRepository.GetAll().FirstOrDefaultAsync();

            result = await GetBusinessRiskEmailerList(result, userList);

            result = await GetExceptionEmailerList(result, userList);

            result = await GetIncidenceEmailerList(result, userList);

            foreach (var emailObj in result)
            {
                await _userEmailer.SendHourlyDailyAsync(emailObj.ToEmailId, 1, emailObj.Body, emailObj.Subject, entityApplicationSetting.Attachmentpath + "`" + emailObj.FileJson);
            }

        }

        private async Task<List<CustomNotificationOutputDto>> GetBusinessRiskEmailerList(List<CustomNotificationOutputDto> result, List<User> userList)
        {

            var stateActionList = await _stateActionRepository.GetAll().Include(x => x.State).ThenInclude(x => x.WorkFlowPage).Include(x => x.Template)
                  .Where(x => x.State.WorkFlowPageId == 3 && x.State.ActionTimeType == ActionTimeType.Days).ToListAsync();
            var query = await _businessRiskRepository.GetAll().Include(x => x.BusinessEntity).ThenInclude(x => x.Actors).IgnoreQueryFilters().ToListAsync();

            foreach (var item in stateActionList)
            {
                if (item.State.IsStateActive)
                {
                    result = await FilterByDate(query, result, item, userList);
                }
            }
            return result;
        }

        private async Task<List<CustomNotificationOutputDto>> FilterByDate(List<BusinessRisk> allBusinessRisks, List<CustomNotificationOutputDto> output, StateAction stateAction, List<User> actors)
        {
            List<CustomNotificationOutputDto> result = output;
            List<string> replacerList = new List<string>();

            var filterBusinessRiskList = allBusinessRisks
                        .WhereIf(stateAction.State.StateName == "New" || stateAction.State.StateName == "NewAndEdit", e => e.LastModificationTime == null)
                        .WhereIf(stateAction.State.StateName == "Edit" || stateAction.State.StateName == "NewAndEdit", e => e.LastModificationTime != null && e.IsDeleted == false)
                        .WhereIf(stateAction.State.StateName == "Delete", e => e.IsDeleted == true).ToList();

            int substractDay = GetDays(stateAction);

            #region MyRegion
            //int substractDay = 0;
            //switch (stateAction.ActionCategory)
            //{
            //    case ActionCategory.Before:
            //        substractDay = (0 - stateAction.ActionTime);
            //        break;
            //    case ActionCategory.Onday:
            //        substractDay = 0;
            //        break;
            //    case ActionCategory.After:
            //        substractDay = stateAction.ActionTime;
            //        break;
            //    case ActionCategory.Escalation:
            //        substractDay = stateAction.ActionTime;
            //        break;
            //    default:
            //        break;
            //}
            #endregion

            var classProperties = await _customTemplateAppServiceEmailer.GetClassProperties(3);
            replacerList = GetReplacerList(replacerList, classProperties);

            //classProperties.ForEach(x =>
            //{
            //    replacerList.Add("{" + x + "}");
            //});

            filterBusinessRiskList.ForEach(x =>
            {
                var compareString = "";
                var approverList = x.BusinessEntity.Actors.Where(x => x.Type == BusinessEntityWorkflowActorType.Approver);
                var approverEmailList = new List<string>();

                if (approverList.Count() != 0)
                {
                    approverEmailList = approverList.Select(x => x.User.EmailAddress).ToList();
                }

                var businessObj = x;
                var props = businessObj.GetType().GetProperties();
               // compareString = GetCompareString(props, stateAction.State.FilterField, substractDay, 3);

                foreach (var column in props)
                {
                    if (column.Name == stateAction.State.FilterField)
                    {
                        if (column.GetValue(businessObj) != null)
                        {
                            var tempDate = (DateTime)column.GetValue(businessObj);
                            compareString = tempDate.AddDays(substractDay).ToString("dd-MMM-yyyy");
                        }
                    }
                }

                if (compareString == DateTime.Now.ToString("dd-MMM-yyyy"))
                {
                    CustomNotificationOutputDto temp = new CustomNotificationOutputDto();
                    temp.Subject = stateAction.Template.TemplateSubject;
                    temp.Body = stateAction.Template.TemplateBody;
                    temp.FileJson = stateAction.Template.TemplateDescription;
                    var mystrBody = stateAction.Template.TemplateBody;
                    var mystrSubject = stateAction.Template.TemplateSubject;
                    List<string> templatevariables = new List<string>();

                    templatevariables = GetTemplateVariables(templatevariables, mystrBody);
                    templatevariables = GetTemplateVariables(templatevariables, mystrSubject);

                    #region MyRegion 
                    //while (mystrBody.Contains("{"))
                    //{
                    //    templatevariables.Add("{" + mystrBody.Split('{', '}')[1] + "}");
                    //    mystrBody = mystrBody.Replace("{" + mystrBody.Split('{', '}')[1] + "}", "");
                    //};

                    //while (mystrSubject.Contains("{"))
                    //{
                    //    templatevariables.Add("{" + mystrSubject.Split('{', '}')[1] + "}");
                    //    mystrSubject = mystrSubject.Replace("{" + mystrSubject.Split('{', '}')[1] + "}", "");
                    //};

                    //templatevariables.ForEach(x =>
                    //{
                    //    foreach (var column in props)
                    //    {
                    //        if ("{" + column.Name + "}" == x)
                    //        {
                    //            var newValue = "" + column.GetValue(businessObj);
                    //            temp.Subject = temp.Subject.Replace(x, newValue);
                    //            temp.Body = temp.Body.Replace(x, newValue);
                    //        }
                    //    }
                    //});
                    #endregion


                    temp = GetUpdatedTemplate(props, templatevariables, temp, 3, businessObj, new Incident(), new LockthreatCompliance.Exceptions.Exception());

                    //foreach (var item in approverEmailList)
                    //    temp.ToEmailId.Add(item);

                    result.Add(temp);
                }
            });

            return output;
        }

        private async Task<List<CustomNotificationOutputDto>> GetExceptionEmailerList(List<CustomNotificationOutputDto> result, List<User> userList)
        {

            var stateActionList = await _stateActionRepository.GetAll().Include(x => x.State).ThenInclude(x => x.WorkFlowPage).Include(x => x.Template)
                  .Where(x => x.State.WorkFlowPageId == 7 && x.State.ActionTimeType == ActionTimeType.Days).ToListAsync();
            var query = await _exceptionRepository.GetAll().Include(x => x.BusinessEntity).ThenInclude(x => x.Actors).IgnoreQueryFilters().ToListAsync();

            foreach (var item in stateActionList)
            {
                if (item.State.IsStateActive)
                    result = await FilterByDateException(query, result, item, userList);
            }
            return result;
        }

        private async Task<List<CustomNotificationOutputDto>> FilterByDateException(List<LockthreatCompliance.Exceptions.Exception> allExceptions, List<CustomNotificationOutputDto> output, StateAction stateAction, List<User> actors)
        {
            List<CustomNotificationOutputDto> result = output;
            List<string> replacerList = new List<string>();

            var filterExceptionsList = allExceptions
                       .WhereIf(stateAction.State.StateName == "New" || stateAction.State.StateName == "NewAndEdit", e => e.LastModificationTime == null)
                       .WhereIf(stateAction.State.StateName == "Edit" || stateAction.State.StateName == "NewAndEdit", e => e.LastModificationTime != null && e.IsDeleted == false)
                       .WhereIf(stateAction.State.StateName == "Delete", e => e.IsDeleted == true).ToList();

            int substractDay = GetDays(stateAction);

            var classProperties = await _customTemplateAppServiceEmailer.GetClassProperties(7);
            replacerList = GetReplacerList(replacerList, classProperties);

            allExceptions.ForEach(x =>
            {
                var compareString = "";
                var approverList = x.BusinessEntity.Actors.Where(x => x.Type == BusinessEntityWorkflowActorType.Approver);
                var approverEmailList = new List<string>();

                if (approverList.Count() != 0)
                {
                    approverEmailList = approverList.Select(x => x.User.EmailAddress).ToList();
                }

                var businessObj = x;
                var props = businessObj.GetType().GetProperties();
                foreach (var column in props)
                {
                    if (column.Name == stateAction.State.FilterField)
                    {
                        if (column.GetValue(businessObj) != null)
                        {
                            var tempDate = (DateTime)column.GetValue(businessObj);
                            compareString = tempDate.AddDays(substractDay).ToString("dd-MMM-yyyy");
                        }
                    }
                }
                if (compareString == DateTime.Now.ToString("dd-MMM-yyyy"))
                {
                    CustomNotificationOutputDto temp = new CustomNotificationOutputDto();
                    temp.Subject = stateAction.Template.TemplateSubject;
                    temp.Body = stateAction.Template.TemplateBody;
                    temp.FileJson = stateAction.Template.TemplateDescription;

                    var mystrBody = stateAction.Template.TemplateBody; 
                    var mystrSubject = stateAction.Template.TemplateSubject;
                    List<string> templatevariables = new List<string>();

                    templatevariables = GetTemplateVariables(templatevariables, mystrBody);
                    templatevariables = GetTemplateVariables(templatevariables, mystrSubject);

                    temp = GetUpdatedTemplate(props, templatevariables, temp, 7, new BusinessRisk(), new Incident(), businessObj);

                    //foreach (var item in approverEmailList)
                    //    temp.ToEmailId.Add(item);

                    result.Add(temp);
                }
            });

            return output;
        }

        private async Task<List<CustomNotificationOutputDto>> GetIncidenceEmailerList(List<CustomNotificationOutputDto> result, List<User> userList)
        {

            var stateActionList = await _stateActionRepository.GetAll().Include(x => x.State).ThenInclude(x => x.WorkFlowPage).Include(x => x.Template)
                  .Where(x => x.State.WorkFlowPageId == 8 && x.State.ActionTimeType == ActionTimeType.Days).ToListAsync();
            var query = await _incidentRepository.GetAll().Include(x => x.BusinessEntity).ThenInclude(x => x.Actors).IgnoreQueryFilters().ToListAsync();

            foreach (var item in stateActionList)
            {
                if (item.State.IsStateActive)
                    result = await FilterByDateIncidence(query, result, item, userList);
            }
            return result;
        }

        private async Task<List<CustomNotificationOutputDto>> FilterByDateIncidence(List<Incident> allIncidents, List<CustomNotificationOutputDto> output, StateAction stateAction, List<User> actors)
        {
            List<CustomNotificationOutputDto> result = output;
            List<string> replacerList = new List<string>();

            var filterIncidentsList = allIncidents
                       .WhereIf(stateAction.State.StateName == "New" || stateAction.State.StateName == "NewAndEdit", e => e.LastModificationTime == null)
                       .WhereIf(stateAction.State.StateName == "Edit" || stateAction.State.StateName == "NewAndEdit", e => e.LastModificationTime != null && e.IsDeleted == false)
                       .WhereIf(stateAction.State.StateName == "Delete", e => e.IsDeleted == true).ToList();

            int substractDay =  GetDays(stateAction);

            var classProperties = await _customTemplateAppServiceEmailer.GetClassProperties(8);
            replacerList = GetReplacerList(replacerList, classProperties);

            filterIncidentsList.ForEach(x =>
            {
                var compareString = "";
                var approverList = x.BusinessEntity.Actors.Where(x => x.Type == BusinessEntityWorkflowActorType.Approver);
                var approverEmailList = new List<string>();

                if (approverList.Count() != 0)
                {
                    approverEmailList = approverList.Select(x => x.User.EmailAddress).ToList();
                }

                var incidenceObj = x;
                var props = incidenceObj.GetType().GetProperties();
                foreach (var column in props)
                {
                    if (column.Name == stateAction.State.FilterField)
                    {
                        if (column.GetValue(incidenceObj) != null)
                        {
                            var tempDate = (DateTime)column.GetValue(incidenceObj);
                            compareString = tempDate.AddDays(substractDay).ToString("dd-MMM-yyyy");
                        }
                    }
                }
                if (compareString == DateTime.Now.ToString("dd-MMM-yyyy"))
                {
                    CustomNotificationOutputDto temp = new CustomNotificationOutputDto();
                    temp.Subject = stateAction.Template.TemplateSubject;
                    temp.Body = stateAction.Template.TemplateBody;
                    temp.FileJson = stateAction.Template.TemplateDescription;

                    var mystrBody = stateAction.Template.TemplateBody;
                    var mystrSubject = stateAction.Template.TemplateSubject;
                    List<string> templatevariables = new List<string>();

                    templatevariables = GetTemplateVariables(templatevariables, mystrBody);
                    templatevariables = GetTemplateVariables(templatevariables, mystrSubject);

                    temp = GetUpdatedTemplate(props, templatevariables, temp, 8, new BusinessRisk(), incidenceObj, new LockthreatCompliance.Exceptions.Exception());

                    //foreach (var item in approverEmailList)
                    //    temp.ToEmailId.Add(item);

                    result.Add(temp);
                }
            });

            return output;
        }






        private int GetDays(StateAction sa)
        {
            int result = 0;

            if (sa.ActionTimeType == ActionTimeType.Days)
            {
                switch (sa.ActionCategory)
                {
                    case ActionCategory.Before:
                        result = (0 - sa.ActionTime);
                        break;
                    case ActionCategory.Onday:
                        result = 0;
                        break;
                    case ActionCategory.After:
                        result = sa.ActionTime;
                        break;
                    case ActionCategory.Escalation:
                        result = sa.ActionTime;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                int additionalDays = sa.ActionTimeType == ActionTimeType.Weeks ? 7 : 30;
                switch (sa.ActionCategory)
                {
                    case ActionCategory.Before:
                        result = (0 - (sa.ActionTime + additionalDays));
                        break;
                    case ActionCategory.Onday:
                        result = 0;
                        break;
                    case ActionCategory.After:
                        result = (sa.ActionTime + additionalDays);
                        break;
                    case ActionCategory.Escalation:
                        result = (sa.ActionTime + additionalDays);
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

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

        private CustomNotificationOutputDto GetUpdatedTemplate(PropertyInfo[] ObjectProperties, List<string> templateVariables, CustomNotificationOutputDto templateObj, long classId, BusinessRisk businessRiskObj, Incident incidentObj, LockthreatCompliance.Exceptions.Exception exceptionObj)
        {
            CustomNotificationOutputDto result = new CustomNotificationOutputDto();
            result = templateObj;
            templateVariables.ForEach(x =>
            {
                foreach (var column in ObjectProperties)
                {
                    if ("{" + column.Name + "}" == x)
                    {
                        switch (classId)
                        {
                            case 3:
                                var newValue3 = "" + column.GetValue(businessRiskObj);
                                templateObj.Subject = templateObj.Subject.Replace(x, newValue3);
                                templateObj.Body = templateObj.Body.Replace(x, newValue3);
                                break;
                            case 7:
                                var newValue7 = "" + column.GetValue(exceptionObj);
                                templateObj.Subject = templateObj.Subject.Replace(x, newValue7);
                                templateObj.Body = templateObj.Body.Replace(x, newValue7);
                                break;
                            case 8:
                                var newValue8 = "" + column.GetValue(incidentObj);
                                templateObj.Subject = templateObj.Subject.Replace(x, newValue8);
                                templateObj.Body = templateObj.Body.Replace(x, newValue8);
                                break;
                            default:
                                break;
                        }
                    }
                }
            });

            return result;
        }              

     
    }
}
