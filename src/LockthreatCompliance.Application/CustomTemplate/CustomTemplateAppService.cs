using Abp.Domain.Repositories;
using LockthreatCompliance.WorkFllows.Dto;
using LockthreatCompliance.WrokFlows;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Entities;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using System.Collections.Generic;
using System.Linq;
using Abp.Linq.Extensions;
using Abp.UI;
using LockthreatCompliance.WorkFllows;
using LockthreatCompliance.CustomTemplate.Dto;
using LockthreatCompliance.Exceptions.Dtos;
using System;
using LockthreatCompliance.Incidents.Dtos;
using LockthreatCompliance.BusinessRisks.Dtos;
using LockthreatCompliance.ExternalAssessments.Dtos;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.FindingReports.Dtos;
using LockthreatCompliance.Assessments.Dto;



namespace LockthreatCompliance.CustomTemplate
{
    public class CustomTemplateAppService : LockthreatComplianceAppServiceBase, ICustomTemplateAppService
    {
        private readonly IRepository<Template, long> _templateserviceRepository;
        private readonly IRepository<WorkFlowPage, long> _workFlowPageRepository;   
        public CustomTemplateAppService(IRepository<Template, long> templateserviceRepository, IRepository<WorkFlowPage, long> workFlowPageRepository)
        {
            _templateserviceRepository = templateserviceRepository;
            _workFlowPageRepository = workFlowPageRepository;   
        }

        public async Task DeleteCustomTemplate(long input)
        {
            try
            {
                var stateAction = await _templateserviceRepository.FirstOrDefaultAsync(a => a.Id == input);
                await _templateserviceRepository.DeleteAsync(stateAction);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        public async Task<List<CustomTemplateWithPageNameDto>> GetAllCustomTemplate()
        {
            try
            {
                var query = await _templateserviceRepository.GetAll().Select(x => new CustomTemplateWithPageNameDto
                {
                    Id = x.Id,
                    TemplateTitle = x.TemplateTitle,
                    TemplateDescription = x.TemplateDescription,
                    TemplateSubject = x.TemplateSubject,
                    TemplateBody = x.TemplateBody,
                    TemplateType = x.TemplateType,
                    StateId = x.StateId,
                    WorkFlowPageId = x.WorkFlowPageId,
                    ActivitiesId = x.ActivitiesId,
                    PageName = "",
                    TemplateTo = x.TemplateTo,
                    TemplateCc = x.TemplateCc,
                    TemplateBcc = x.TemplateBcc,
                }).ToListAsync();
                return query;
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }

        }

        public async Task CreateOrUpdateTemplate(CustomTemplateDto input)
        {
            try
            {
                if (input.Id!=0)
                {
                    var getTemplate = await _templateserviceRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefaultAsync();

                    getTemplate.WorkFlowPageId = input.WorkFlowPageId;
                    getTemplate.TemplateTo = input.TemplateTo;
                    getTemplate.TemplateCc = input.TemplateCc;
                    getTemplate.TemplateBcc = input.TemplateBcc;
                    getTemplate.TemplateBody = input.TemplateBody;
                    getTemplate.TemplateDescription = input.TemplateDescription;
                    getTemplate.TemplateSubject = input.TemplateSubject;
                    getTemplate.TemplateTitle = input.TemplateTitle;
                    await _templateserviceRepository.InsertOrUpdateAsync(getTemplate);
                }
                else
                {
                    var id = _templateserviceRepository.InsertOrUpdateAndGetId(ObjectMapper.Map<Template>(input));
                }


             
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }

        }

        public async Task<CustomTemplateDto> GetCustomTemplateById(long input)
        {
            var templateObject = await _templateserviceRepository.GetAll().Where(x => x.Id == input).FirstOrDefaultAsync();
            return ObjectMapper.Map<CustomTemplateDto>(templateObject);
        }

        public async Task<List<string>> GetClassProperties(long typeId)
        {
            var result = new List<string>();
            switch (typeId)
            {
                case 1:
                    var input1 = new CreateOrEditExternalAssessmentDto();
                    foreach (var prp in input1.GetType().GetProperties())
                    {
                        result.Add(prp.Name);
                    }
                    break;
                case 2:
                    var input2 = new AuditProjectDto();
                    foreach (var prp in input2.GetType().GetProperties())
                    {
                        result.Add(prp.Name);
                    }
                    break;
                case 3:
                    var input3 = new CreateOrEditBusinessRiskDto();
                    foreach (var prp in input3.GetType().GetProperties())
                    {
                        result.Add(prp.Name);
                    }
                    break;
                case 4:
                case 5:
                    var input4 = new CreateOrEditFindingReportDto();
                    foreach (var prp in input4.GetType().GetProperties())
                    {
                        result.Add(prp.Name);
                    }
                    break;
                //case 5:
                //    var input5 = new CreateOrEditExceptionDto();
                //    foreach (var prp in input5.GetType().GetProperties())
                //    {
                //        result.Add(prp.Name);
                //    }
                case 6:
                    var input6 = new AssessmentTemplateDto();
                    foreach (var prp in input6.GetType().GetProperties())
                    {
                        result.Add(prp.Name);
                    }
                    break;
                case 7:
                    var input7 = new CreateOrEditExceptionDto();
                    foreach (var prp in input7.GetType().GetProperties())
                    {
                        result.Add(prp.Name);
                    }
                    break;
                case 8:
                    var input8 = new CreateOrEditIncidentDto();
                    foreach (var prp in input8.GetType().GetProperties())
                    {
                        result.Add(prp.Name);
                    }
                    break;
                //case 19:
                //    var input13 = new AuditProjectDto();
                //    foreach (var prp in input13.GetType().GetProperties())
                //    {
                //        result.Add(prp.Name);
                //    }
                //    break;
                //case 20:
                //    var input14 = new AuditProjectDto();
                //    foreach (var prp in input14.GetType().GetProperties())
                //    {
                //        result.Add(prp.Name);
                //    }
                //    break;
                default:
                    //var inputdefault = new AuditProjectDto();
                    //foreach (var prp in inputdefault.GetType().GetProperties())
                    //{
                    //    result.Add(prp.Name);
                    //}
                    break;
            }
            return result;
        }


        public async Task<List<string>> GetAuditProjectMeetingDto()
        {
            var result = new List<string>();
            var input2 = new AuditProjectMeetingEmailDto();
            foreach (var prp in input2.GetType().GetProperties())
            {
                result.Add(prp.Name);
            }

            return result;
        }
        public async Task<List<String>> GetAuditProjectClassProperties(long typeId)
        {
            var result = new List<string>();

            var getpageName = _workFlowPageRepository.GetAll().Where(x => x.Id == typeId).Select(x => x.PageName).FirstOrDefault();
            if (getpageName != null)
            {
                string workflowpage = getpageName.Trim().ToLower();
                switch (workflowpage)
                {
                    case "feedback":
                        {
                            var input12 = new FeedBackLinkDto();
                            foreach (var prp in input12.GetType().GetProperties())
                            {
                                result.Add(prp.Name);
                            }
                            break;
                        }

                    case "external assessment":
                        var input1 = new CreateOrEditExternalAssessmentDto();
                        foreach (var prp in input1.GetType().GetProperties())
                        {
                            result.Add(prp.Name);
                        }
                        break;
                    case "audit project":
                        var input2 = new AuditProjectEmailDto();
                        foreach (var prp in input2.GetType().GetProperties())
                        {
                            result.Add(prp.Name);
                        }
                        break;
                    case "business risks":
                        var input3 = new CreateOrEditBusinessRiskDto();
                        foreach (var prp in input3.GetType().GetProperties())
                        {
                            result.Add(prp.Name);
                        }
                        break;

                    case "external findings":
                        var input4 = new CreateOrEditFindingReportDto();
                        foreach (var prp in input4.GetType().GetProperties())
                        {
                            result.Add(prp.Name);
                        }
                        break;
                    case "findings":
                        var input5 = new CreateOrEditFindingReportDto();
                        foreach (var prp in input5.GetType().GetProperties())
                        {
                            result.Add(prp.Name);
                        }
                        break;                
                                                 
                    default:
                        var input11 = new FeedBackLinkDto();
                        foreach (var prp in input11.GetType().GetProperties())
                        {
                            result.Add(prp.Name);
                        }

                        break;

                }
            }

            return result;
        }




        public async Task<List<string>> GetBusinessEntitiesProperties()
        {
            var result = new List<string>();

            var query = new BusinessEntityEmailDto();
            foreach (var prp in query.GetType().GetProperties())
            {
                result.Add(prp.Name);
            }
            return result;

        }

        public async Task<List<string>> GetAuditReportType()
        {
            var result = new List<string>();

            var query = new AuditReportTypeDto();
            foreach (var prp in query.GetType().GetProperties())
            {
                result.Add(prp.Name);
            }
            return result;
        }
    }

}
