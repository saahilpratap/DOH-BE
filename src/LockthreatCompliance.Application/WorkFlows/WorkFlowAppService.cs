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
using LockthreatCompliance.ExternalAssessments.Dtos;
using LockthreatCompliance.Incidents.Dtos;
using LockthreatCompliance.Exceptions.Dtos;
using LockthreatCompliance.Assessments.Dto;
using LockthreatCompliance.FindingReports.Dtos;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.BusinessRisks.Dtos;
using Abp.DynamicEntityParameters;
using System;
using LockthreatCompliance.Incidents;
using LockthreatCompliance.BusinessEntities;

namespace LockthreatCompliance.WorkFlows
{
    public class WorkFlowAppService : LockthreatComplianceAppServiceBase, IWorkFlowAppService
    {
        private readonly IRepository<State, long> _stateServiceResposirtory;
        private readonly IRepository<StateAction, long> _stateActionServiceResposirtory;
        private readonly IRepository<Template, long> _templateserviceRepository;
        private readonly ICustomTemplateAppService _customTemplateAppService;
        private readonly IRepository<DynamicParameterValue> _dynamicParameterValueRepository;
        private readonly IRepository<DynamicParameter> _dynamicParameterManager;
        public WorkFlowAppService(IRepository<State, long> stateServiceResposirtory, IRepository<Template, long> templateserviceRepository,
            IRepository<StateAction, long> stateActionServiceResposirtory, ICustomTemplateAppService customTemplateAppService,
            IRepository<DynamicParameterValue> dynamicParameterValueRepository, IRepository<DynamicParameter> dynamicParameterManager)
        {
            _templateserviceRepository = templateserviceRepository;
            _stateServiceResposirtory = stateServiceResposirtory;
            _stateActionServiceResposirtory = stateActionServiceResposirtory;
            _customTemplateAppService = customTemplateAppService;
            _dynamicParameterValueRepository = dynamicParameterValueRepository;
            _dynamicParameterManager = dynamicParameterManager;
        }
        public async Task<long> CreateOrUpdateState(CreateOrUpdateStateDto input)
        {
            try
            {
                long stateId = 0;
                if (input.Id == 0)
                {
                    var stateObj = await _stateServiceResposirtory.GetAll().Include(x => x.WorkFlowPage).Where(x => x.WorkFlowPageId == input.WorkFlowPageId && x.StateName == input.StateName).OrderByDescending(x=>x.Id).FirstOrDefaultAsync();
                    if (stateObj != null)
                    {

                        if (stateObj.WorkFlowPage.PageName == "Audit Project" || stateObj.WorkFlowPage.PageName == "Self Assessment")
                        {
                            if (stateObj.AuditProjectStatus == input.AuditProjectStatus)
                            {
                                throw new UserFriendlyException("This Status record already present");
                            }
                            else
                            {
                                stateId = await _stateServiceResposirtory.InsertAndGetIdAsync(ObjectMapper.Map<State>(input));
                            }
                        }
                        else
                        {
                            throw new UserFriendlyException("" + stateObj.WorkFlowPage.PageName + " Page -> " + stateObj.StateName + " State record already exist !");
                        }
                    }
                    else
                    {
                        stateId = await _stateServiceResposirtory.InsertAndGetIdAsync(ObjectMapper.Map<State>(input));
                    }
                }
                else
                {
                    var stateObj = await _stateServiceResposirtory.GetAll().Include(x => x.WorkFlowPage).Where(x => x.WorkFlowPageId == input.WorkFlowPageId && x.StateName == input.StateName && x.Id != input.Id).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                    if (stateObj != null)
                    {
                        if (stateObj.WorkFlowPage.PageName == "Audit Project" || stateObj.WorkFlowPage.PageName == "Self Assessment")
                        {
                            if (stateObj.AuditProjectStatus == input.AuditProjectStatus)
                            {
                                throw new UserFriendlyException("This Status record already present");
                            }
                            else
                            {
                                var state = await _stateServiceResposirtory.GetAll().FirstOrDefaultAsync(x => x.Id == input.Id);
                                stateId = state.Id;
                                ObjectMapper.Map(input, state);
                            }
                        }
                        else
                        {
                            throw new UserFriendlyException("" + stateObj.WorkFlowPage.PageName + " Page -> " + stateObj.StateName + " State record already exist !");
                        }
                    }
                    else
                    {
                        var state = await _stateServiceResposirtory.GetAll().FirstOrDefaultAsync(x => x.Id == input.Id);
                        stateId = state.Id;
                        ObjectMapper.Map(input, state);
                    }
                }
                return stateId;
            }
            catch (System.Exception ex)
            {
                throw;
            }


        }
        public async Task<long> CreteOrUpdateStateAction(CreateOrUpdateStateActionDto input)
        {
            long stateId = 0;
            try
            {
                if (input.Id == 0)
                {
                    var stateActionObj = await _stateActionServiceResposirtory.GetAll().Include(x => x.State).ThenInclude(x => x.WorkFlowPage).Where(x => x.StateId == input.StateId && x.ActionCategory == input.ActionCategory).FirstOrDefaultAsync();
                    if (stateActionObj != null)
                    {
                        throw new UserFriendlyException("" + stateActionObj.State.WorkFlowPage.PageName + " Page -> " + stateActionObj.State.StateName + " State -> " + stateActionObj.ActionCategory + " Category record already exist !");
                    }
                    else
                    {
                        long stateAction = await _stateActionServiceResposirtory.InsertAndGetIdAsync(ObjectMapper.Map<StateAction>(input));
                        stateId = (long)input.StateId;
                    }
                }
                else
                {
                    var stateActionObj = await _stateActionServiceResposirtory.GetAll().Include(x => x.State).ThenInclude(x => x.WorkFlowPage).Where(x => x.StateId == input.StateId && x.ActionCategory == input.ActionCategory && x.Id != input.Id).FirstOrDefaultAsync();
                    if (stateActionObj != null)
                    {
                        throw new UserFriendlyException("" + stateActionObj.State.WorkFlowPage.PageName + " Page -> " + stateActionObj.State.StateName + " State -> " + stateActionObj.ActionCategory + " Category record already exist !");
                    }
                    else
                    {
                        var state = await _stateActionServiceResposirtory.GetAll().FirstOrDefaultAsync(x => x.Id == input.Id);
                        stateId = (long)state.StateId;
                        ObjectMapper.Map(input, state);
                    }
                }
                return stateId;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteStateAction(int input)
        {
            try
            {
                var stateAction = await _stateActionServiceResposirtory.FirstOrDefaultAsync(a => a.Id == input);
                await _stateActionServiceResposirtory.DeleteAsync(stateAction);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }
        public async Task<PagedResultDto<StateActionListDto>> GetStateActionList(StateActionInputDto input)
        {
            IQueryable<StateAction> query = null;
            var stateActionLists = new List<StateActionListDto>();
            var stateAction = 0;
            try
            {
                if (input.StateId != 0)
                {
                    query = _stateActionServiceResposirtory.GetAll().Where(x => x.StateId == input.StateId).Include(x => x.State)
                                .WhereIf(input.Filter != null, x => x.State.StateName.ToLower().Trim().ToString() == input.Filter.ToLower().ToString());

                    stateAction = await query.CountAsync();

                    var stateActionList = await query
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    stateActionLists = ObjectMapper.Map<List<StateActionListDto>>(stateActionList);
                }
                return new PagedResultDto<StateActionListDto>(
                      stateAction,
                      stateActionLists
                      );
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        //public async Task CreateOrUpdateTemplate(CreateOrUpdateTemplateDto input)
        //{

        //    if (input.Id == 0)
        //    {
        //        long stateAction = await _stateActionServiceResposirtory.InsertAndGetIdAsync(ObjectMapper.Map<StateAction>(input));
        //        stateId = stateAction;
        //    }
        //    else
        //    {
        //        var state = await _stateActionServiceResposirtory.GetAll().FirstOrDefaultAsync(x => x.Id == input.Id);
        //        stateId = (long)state.StateId;
        //        ObjectMapper.Map(input, state);
        //    }
        //    return stateId;
        //}

        public async Task<List<TemplateDto>> GetAllTemplate()
        {
            var query = new List<TemplateDto>();
            try
            {
                query = await _templateserviceRepository.GetAll().Select(x => new TemplateDto()
                {
                    Id = x.Id,
                    TemplateTitle = x.TemplateTitle
                }).ToListAsync();
                return query;
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }

        }

        public async Task<List<StateDto>> GetAllStates(GetAllSubWorkFlowInput input)
        {
            var getvalue = input.Type != "0" ? true : false;
            List<StateDto> result = new List<StateDto>();
            var query = await _stateServiceResposirtory.GetAll().Include(x => x.WorkFlowPage)
            .WhereIf(input.Type != "null",e=>e.IsStateOpen == getvalue).ToListAsync();
            result = ObjectMapper.Map<List<StateDto>>(query);
            return result;
        }

        public async Task DeleteState(int id)
        {
            try
            {
                var stateObject = await _stateServiceResposirtory.FirstOrDefaultAsync(a => a.Id == id);
                await _stateServiceResposirtory.DeleteAsync(stateObject);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        public async Task<CreateOrUpdateStateDto> GetStateById(int id)
        {
            CreateOrUpdateStateDto result = new CreateOrUpdateStateDto();
            var query = await _stateServiceResposirtory.GetAll().Include(x => x.WorkFlowPage).Where(x => x.Id == id).FirstOrDefaultAsync();
            result = ObjectMapper.Map<CreateOrUpdateStateDto>(query);
            return result;
        }

        public async Task<CreateOrUpdateStateActionDto> GetStateActionById(int id)
        {
            CreateOrUpdateStateActionDto result = new CreateOrUpdateStateActionDto();
            var query = await _stateActionServiceResposirtory.GetAll().Where(x => x.Id == id).FirstOrDefaultAsync();
            result = ObjectMapper.Map<CreateOrUpdateStateActionDto>(query);
            return result;
        }

        public async Task<List<IdAndNameDto>> GetEnumStatusList(int input)
        {
            var result = new List<IdAndNameDto>();
            switch (input)
            {
                case 1:
                    Array enumValueArray1 = Enum.GetValues(typeof(IncidentStatus));
                    foreach (int enumValue in enumValueArray1)
                    {
                        result.Add(new IdAndNameDto() { Id = enumValue, Name = Enum.GetName(typeof(IncidentStatus), enumValue) });
                    }
                    break;
                case 2:
                    Array enumValueArray2 = Enum.GetValues(typeof(AssessmentStatus));
                    foreach (int enumValue in enumValueArray2)
                    {
                        result.Add(new IdAndNameDto() { Id = enumValue, Name = Enum.GetName(typeof(AssessmentStatus), enumValue) });
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        public async Task<List<string>> GetFilterFiledByStateId(int typeId)
        {
            var result = new List<string>();
            switch (typeId)
            {
                case 1:
                    var input1 = new CreateOrEditExternalAssessmentDto();
                    foreach (var prp in input1.GetType().GetProperties())
                    {
                        if (prp.PropertyType.FullName.Contains("System.DateTime"))
                            result.Add(prp.Name);
                    }
                    break;
                case 2:
                    var input2 = new AuditProjectDto();
                    foreach (var prp in input2.GetType().GetProperties())
                    {
                        if (prp.PropertyType.FullName.Contains("System.DateTime"))
                            result.Add(prp.Name);
                    }
                    break;
                case 3:
                    var input3 = new CreateOrEditBusinessRiskDto();
                    foreach (var prp in input3.GetType().GetProperties())
                    {
                        if (prp.PropertyType.FullName.Contains("System.DateTime"))
                            result.Add(prp.Name);
                    }
                    break;
                case 4:
                case 5:
                    var input4 = new CreateOrEditFindingReportDto();
                    foreach (var prp in input4.GetType().GetProperties())
                    {
                        if (prp.PropertyType.FullName.Contains("System.DateTime"))
                            result.Add(prp.Name);
                    }
                    break;
                case 6:
                    var input6 = new CreateOrEditAssessmentInput();
                    foreach (var prp in input6.GetType().GetProperties())
                    {
                        if (prp.PropertyType.FullName.Contains("System.DateTime"))
                            result.Add(prp.Name);
                    }
                    break;
                case 7:
                    var input7 = new CreateOrEditExceptionDto();
                    foreach (var prp in input7.GetType().GetProperties())
                    {
                        if (prp.PropertyType.FullName.Contains("System.DateTime"))
                            result.Add(prp.Name);
                    }
                    break;
                case 8:
                    var input8 = new CreateOrEditIncidentDto();
                    foreach (var prp in input8.GetType().GetProperties())
                    {
                        if (prp.PropertyType.FullName.Contains("System.DateTime"))
                            result.Add(prp.Name);
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        public async Task<List<IdAndNameDto>> GetDynamicParameterStatusList(string input)
        {
            var result = new List<IdAndNameDto>();
            var DynamicParameterObj = await _dynamicParameterManager.GetAll().Where(x => x.ParameterName == input).FirstOrDefaultAsync();

            if (DynamicParameterObj != null)
            {
                var query = await _dynamicParameterValueRepository.GetAll().Where(x => x.DynamicParameterId == DynamicParameterObj.Id).ToListAsync();
                result = query.Select(x => new IdAndNameDto { Id = x.Id, Name = x.Value }).ToList();
            }

            return result;
        }

        public async Task<List<NameAndDataTypeDto>> GetTargetFiledPropertyByStateId(int typeId)
        {
            var result = new List<NameAndDataTypeDto>();
            var systemDateField = new List<string>() { "CreationTime", "LastModificationTime", "DeletionTime" };
            switch (typeId)
            {
                case 2:
                    var input2 = new AuditProjectDto();
                    foreach (var prp in input2.GetType().GetProperties())
                    {
                        if (prp.PropertyType.FullName.Contains("System.DateTime"))
                        {
                            if (!systemDateField.Contains(prp.Name))
                                result.Add(new NameAndDataTypeDto { DataType = "DateTime", Name = prp.Name });
                        }
                        else if (prp.Name == "AuditStatusId")
                        {
                            result.Add(new NameAndDataTypeDto { DataType = "int", Name = prp.Name });
                        }
                    }
                    break;
                case 6:
                    //var input6 = new CreateOrEditAssessmentInput();
                    //foreach (var prp in input6.GetType().GetProperties())
                    //{
                    //    if (prp.PropertyType.FullName.Contains("System.DateTime"))
                    //        result.Add(prp.Name);
                    //}
                    break;
                default:
                    break;
            }
            return result;
        }

        public async Task<List<IdAndNameStringDto>> GetTargetStatusList(string input)
        {
            var result = new List<IdAndNameStringDto>();
            var DynamicParameterObj = await _dynamicParameterManager.GetAll().Where(x => x.ParameterName == input).FirstOrDefaultAsync();

            if (DynamicParameterObj != null)
            {
                var query = await _dynamicParameterValueRepository.GetAll().Where(x => x.DynamicParameterId == DynamicParameterObj.Id).ToListAsync();
                result = query.Select(x => new IdAndNameStringDto { Id = "" + x.Id, Name = x.Value }).ToList();
            }

            return result;
        }


    }
}
