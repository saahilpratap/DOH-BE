using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.DynamicEntityParameters;
using Abp.Extensions;
using Abp.Linq.Extensions;
using LockthreatCompliance.AssessmentSchedules.Dto;
using LockthreatCompliance.AssessmentSchedules.ExternalAsssementSchedules;
using LockthreatCompliance.AuthoritativeDocuments.Dtos;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using LockthreatCompliance.ExternalAssessments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using PayPalCheckoutSdk.Orders;
using LockthreatCompliance.AuditProjects.Dtos;
using Abp.UI;

namespace LockthreatCompliance.AssessmentSchedules
{
    public class ExtAssementScheduleAppService : LockthreatComplianceAppServiceBase, IExtAssementScheduleAppService
    {
        private readonly IRepository<DynamicParameterValue> _dynamicParameterValueRepository;
        private readonly IRepository<DynamicParameter> _dynamicParameterManager;
        private readonly IRepository<ExternalAssessmentSchedule, long> _externalAssessmentScheduleRepository;
        private readonly IRepository<ExternalAssessmentScheduleDetail, long> _externalAssessmentScheduleDetailRepository;
        private readonly IRepository<ExternalAssessment> _extAssessmentRepository;
        private readonly IRepository<ExternalAssessmentScheduleEntityGroup> _externalAssessmentScheduleEntityGroupRepository;
        private readonly IRepository<ExtAssSchDetailAuthoritativeDocument> _externalScheduleAuthDocumentRepository;
        public ExtAssementScheduleAppService(IRepository<DynamicParameterValue> dynamicParameterValueRepository,
            IRepository<ExtAssSchDetailAuthoritativeDocument> externalScheduleAuthDocumentRepository,
            IRepository<ExternalAssessmentScheduleEntityGroup> externalAssessmentScheduleEntityGroupRepository,
            IRepository<DynamicParameter> dynamicParameterManager, IRepository<ExternalAssessment> extAssessmentRepository,
            IRepository<ExternalAssessmentSchedule, long> externalAssessmentScheduleRepository,
            IRepository<ExternalAssessmentScheduleDetail, long> externalAssessmentScheduleDetailRepository)
        {
            _externalScheduleAuthDocumentRepository = externalScheduleAuthDocumentRepository;
            _externalAssessmentScheduleEntityGroupRepository = externalAssessmentScheduleEntityGroupRepository;
            _dynamicParameterValueRepository = dynamicParameterValueRepository;
            _dynamicParameterManager = dynamicParameterManager;
            _externalAssessmentScheduleRepository = externalAssessmentScheduleRepository;
            _externalAssessmentScheduleDetailRepository = externalAssessmentScheduleDetailRepository;
            _extAssessmentRepository = extAssessmentRepository;
        }


        public async Task AddorUpdateAssessmentSchedule(ExternalAssessmentScheduleDto input)
        {
            input.TenantId = AbpSession.TenantId;
            var checkExternalAssessmentInfo = await _externalAssessmentScheduleRepository.GetAll().Where(x => x.AssessmentInfo.Trim().ToLower() == input.AssessmentInfo.Trim().ToLower()).FirstOrDefaultAsync();

            if (checkExternalAssessmentInfo == null)
            {
                var scheduleId = await _externalAssessmentScheduleRepository.InsertOrUpdateAndGetIdAsync(ObjectMapper.Map<ExternalAssessmentSchedule>(input));
                var allScheduleDetails = await _externalAssessmentScheduleDetailRepository.GetAll().Where(s => s.ScheduleId == scheduleId).ToListAsync();
                int cnt = 0;
                int currentYear = 0;
                int PrevYear = 0;
                foreach (var date in input.ScheduledDates)
                {
                    cnt++;
                    currentYear = date.Year;
                    if (PrevYear == 0)
                    {
                        PrevYear = currentYear;
                    }
                    if (PrevYear != currentYear)
                    {
                        cnt = 1;
                        PrevYear = currentYear;
                    }
                    var scheduleDetailData = new ExternalAssessmentScheduleDetailDto
                    {
                        ScheduleId = scheduleId,
                        AssementScheduleId = "",
                        AssessmentInfo = input.AssessmentInfo,
                        //AssessmentName = input.AssessmentName.Replace("x", cnt.ToString()),
                        AssessmentName = "ADHICS-" + input.ScheduleTypeName + cnt.ToString() + "-" + +date.Year + "-" + input.AssessmentTypeName,
                        AssessmentTypeId = input.AssessmentTypeId,
                        AuthoritativeDocumentIds = input.AuthoritativeDocumentIds,
                        FeedBack = input.FeedBack,
                        ScheduleName = input.ScheduleName,
                        ScheduleTypeId = input.ScheduleTypeId,
                        SendEmailNotify = input.SendEmailNotify,
                        SendSmsNotify = input.SendSmsNotify,
                        TenantId = input.TenantId
                    };
                    scheduleDetailData.ScheduleId = scheduleId;

                    if (input.ScheduleTypeName.Trim().ToLower() == "OnDemand".Trim().ToLower())
                    {
                        scheduleDetailData.StartDate = date;
                        scheduleDetailData.EndDate = input.EndDate;


                        var getId = await _externalAssessmentScheduleDetailRepository.InsertAndGetIdAsync(ObjectMapper.Map<ExternalAssessmentScheduleDetail>(scheduleDetailData));

                        var SetData = new ExtAssSchDetailAuthoritativeDocument()
                        {
                            AuthoritativeDocumentId = input.AuthoritativeDocumentIds.FirstOrDefault(),
                            ExternalAssessmentScheduleDetailId = getId

                        };
                        await _externalScheduleAuthDocumentRepository.InsertAsync(SetData);


                    }
                    else
                    {
                        if (input.ScheduleTypeName == "Annual")
                        {
                            var index = input.ScheduledDates.FindIndex(d => d == date);
                            scheduleDetailData.StartDate = date;
                            string iDate = "" + date.Year + "-12-31";
                            scheduleDetailData.EndDate = DateTime.Parse(iDate);
                            var SheduleDetailsId = await _externalAssessmentScheduleDetailRepository.InsertAndGetIdAsync(ObjectMapper.Map<ExternalAssessmentScheduleDetail>(scheduleDetailData));

                            var SetData = new ExtAssSchDetailAuthoritativeDocument()
                            {
                                AuthoritativeDocumentId = input.AuthoritativeDocumentIds.FirstOrDefault(),
                                ExternalAssessmentScheduleDetailId = SheduleDetailsId

                            };
                            await _externalScheduleAuthDocumentRepository.InsertAsync(SetData);
                        }
                    }
                }
            }
            else
            {
                throw new UserFriendlyException("ExternalAssessment Info Already Exist!");
            }

        }

        public async Task AddorUpdateExternalAssessmentScheduleEntityGroup(ExternalAssessmentScheduleEntityGroupDto input)
        {
            try
            {
                   await _externalAssessmentScheduleEntityGroupRepository.HardDeleteAsync(x => x.ExternalAssessmentScheduleId == input.ExternalAssessmentScheduleId && x.ExtGenerated == false);
                    input.BusinessEnityies.ForEach(obj =>
                    {
                        var items = new ExternalAssessmentScheduleEntityGroup();
                        items.ExtGenerated = false;
                        items.TenantId = AbpSession.TenantId;
                        items.EntityGroupId = obj.EntityGroupId == 0 ? null : obj.EntityGroupId;
                        items.BusinessEntityId = obj.Id;
                        items.ExternalAssessmentScheduleId = input.ExternalAssessmentScheduleId;
                        items.EntityType = input.EntityType;
                        _externalAssessmentScheduleEntityGroupRepository.InsertAsync(items);
                    });                
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<DynamicNameValueDto>> GetScheduleTypes()
        {
            var getScheduleTypes = new List<DynamicNameValueDto>();
            try
            {
                var getcheckId = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == "External Schedule Types");
                if (getcheckId != null)
                {

                    var getother = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getcheckId.Id)
                        .Select(x => new DynamicNameValueDto()
                        {
                            Id = x.Id,
                            Name = x.Value,
                        }).ToListAsync();
                    if (getother.Count() != 0)
                    {
                        getScheduleTypes = ObjectMapper.Map<List<DynamicNameValueDto>>(getother);
                    }
                    return getScheduleTypes;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return getScheduleTypes;
        }

        public async Task<List<DynamicNameValueDto>> GetAssessmentTypes()
        {
            var getAssessmentTypes = new List<DynamicNameValueDto>();
            try
            {
                var getcheckId = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == "External Audit Types");
                if (getcheckId != null)
                {
                    var getother = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getcheckId.Id)
                        .Select(x => new DynamicNameValueDto()
                        {
                            Id = x.Id,
                            Name = x.Value,
                        }).ToListAsync();
                    if (getother.Count() != 0)
                    {
                        getAssessmentTypes = ObjectMapper.Map<List<DynamicNameValueDto>>(getother);
                    }
                    return getAssessmentTypes;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return getAssessmentTypes;
        }

        public async Task<List<DynamicNameValueDto>> GetExternalAssessmentTypes()
        {
            var getAssessmentTypes = new List<DynamicNameValueDto>();
            try
            {
                var getcheckId = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == "External Assessment Types");
                if (getcheckId != null)
                {
                    var getother = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getcheckId.Id)
                        .Select(x => new DynamicNameValueDto()
                        {
                            Id = x.Id,
                            Name = x.Value,
                        }).ToListAsync();
                    if (getother.Count() != 0)
                    {
                        getAssessmentTypes = ObjectMapper.Map<List<DynamicNameValueDto>>(getother);
                    }
                    return getAssessmentTypes;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return getAssessmentTypes;
        }

        public async Task<PagedResultDto<ExternalAssessmentScheduleDto>> GetAllScheduledAssessments(GetAllExtScheduleInput input)
        {
            var query = _externalAssessmentScheduleRepository.GetAll().Include(d => d.ScheduleType).Include(d => d.AssessmentType)
                        .Include(d => d.AuthoritativeDocuments).WhereIf(
                        !input.Filter.IsNullOrWhiteSpace(), u =>
                        u.ScheduleName.Contains(input.Filter));
            var totalCount = await query.CountAsync();
            var pagedAndFilteredSchedules = await query
                      .OrderBy(input.Sorting)
                      .PageBy(input).ToListAsync();

            var externalAssessmentScheduleDetailList = await _externalAssessmentScheduleDetailRepository.GetAll().Include(x=>x.ExternalAssessmentSchedule).Include(d => d.ScheduleType)
                    .Include(d => d.AssessmentType).Include(d => d.AuthoritativeDocuments).ToListAsync();

            var schedulesdetailsList = externalAssessmentScheduleDetailList.Select(
                                       e => new ExternalAssessmentScheduleDetailDto
                                       {
                                           Id = e.Id,
                                           ScheduleCode = e.ExternalAssessmentSchedule == null ? null : e.ExternalAssessmentSchedule.ScheduleId,
                                           ScheduleName = e.ScheduleName,
                                           AssessmentInfo = e.AssessmentInfo,
                                           AssessmentName = e.AssessmentName,
                                           AssessmentTypeId = e.AssessmentTypeId,
                                           AssementScheduleId = e.AssementScheduleId,
                                           EndDate = e.EndDate,
                                           FeedBack = e.FeedBack,
                                           RecurringJobId = e.RecurringJobId,
                                           ScheduleId = e.ScheduleId,
                                           ScheduleTypeId = e.ScheduleTypeId,
                                           SendEmailNotify = e.SendEmailNotify,
                                           SendSmsNotify = e.SendSmsNotify,
                                           StartDate = e.StartDate,
                                           TenantId = e.TenantId,
                                           ScheduleType = new DynamicNameValueDto { Id = e.ScheduleType.Id, Name = e.ScheduleType.Value },
                                           AssessmentType = new DynamicNameValueDto { Id = e.AssessmentType.Id, Name = e.AssessmentType.Value },
                                           AuthoritativeDocumentIds = e.AuthoritativeDocuments.Select(a => a.AuthoritativeDocumentId).ToList(),
                                           //  BusinessEnityGroupWiesDetails = getdata.ToList(),
                                       }).ToList();
            var schedules = pagedAndFilteredSchedules.Select(
                               e => new ExternalAssessmentScheduleDto
                               {
                                   Id = e.Id,
                                   ScheduleName = e.ScheduleName,
                                   AssessmentInfo = e.AssessmentInfo,
                                   AssessmentName = e.AssessmentName,
                                   AssessmentTypeId = e.AssessmentTypeId,
                                   EndDate = e.EndDate,
                                   FeedBack = e.FeedBack,
                                   RecurringJobId = e.RecurringJobId,
                                   ScheduleId = e.ScheduleId,
                                   ScheduleTypeId = e.ScheduleTypeId,
                                   SendEmailNotify = e.SendEmailNotify,
                                   SendSmsNotify = e.SendSmsNotify,
                                   StartDate = e.StartDate,
                                   TenantId = e.TenantId,
                                   ScheduleType = new DynamicNameValueDto { Id = e.ScheduleType.Id, Name = e.ScheduleType.Value },
                                   AssessmentType = new DynamicNameValueDto { Id = e.AssessmentType.Id, Name = e.AssessmentType.Value },
                                   AuthoritativeDocumentIds = e.AuthoritativeDocuments.Select(a => a.AuthoritativeDocumentId).ToList(),
                                   ScheduleDetails = schedulesdetailsList.Where(s => s.ScheduleId == e.Id).ToList()
                               }).OrderByDescending(x=>x.Id).ToList();
           
            return new PagedResultDto<ExternalAssessmentScheduleDto>(totalCount, schedules);
        }

        public async Task<ExternalAssessmentScheduleDto> GetSchedulesAssessmentDetails(long id)
        {
            var schedule = await _externalAssessmentScheduleRepository.GetAll().Where(s => s.Id == id).Include(d => d.ScheduleType)
                .Include(d => d.AssessmentType).Include(d => d.AuthoritativeDocuments).FirstOrDefaultAsync();

            var result = new ExternalAssessmentScheduleDto
            {
                Id = schedule.Id,
                ScheduleName = schedule.ScheduleName,
                AssessmentInfo = schedule.AssessmentInfo,
                AssessmentName = schedule.AssessmentName,
                AssessmentTypeId = schedule.AssessmentTypeId,
                EndDate = schedule.EndDate,
                FeedBack = schedule.FeedBack,
                RecurringJobId = schedule.RecurringJobId,
                ScheduleId = schedule.ScheduleId,
                ScheduleTypeId = schedule.ScheduleTypeId,
                SendEmailNotify = schedule.SendEmailNotify,
                SendSmsNotify = schedule.SendSmsNotify,
                StartDate = schedule.StartDate,
                TenantId = schedule.TenantId,
                ScheduleType = new DynamicNameValueDto { Id = schedule.ScheduleType.Id, Name = schedule.ScheduleType.Value },
                AssessmentType = new DynamicNameValueDto { Id = schedule.AssessmentType.Id, Name = schedule.AssessmentType.Value },
                AuthoritativeDocumentIds = schedule.AuthoritativeDocuments.Select(a => a.AuthoritativeDocumentId).ToList(),
            };

            return result;
        }

        public async Task<PagedResultDto<ExternalAssessmentScheduleDetailDto>> GetAllScheduledDetailAssessments(GetAllExtScheduleInput input)
        {
            var schedule = new ExternalAssessmentSchedule();
            var totalCount = 0;

            try
            {
               
                
                    var query = _externalAssessmentScheduleDetailRepository.GetAll().WhereIf(input.ScheduleId > 0,s => s.ScheduleId == input.ScheduleId).Include(d => d.ScheduleType)
                    .Include(d => d.AssessmentType).Include(d => d.AuthoritativeDocuments).WhereIf(
                       !input.Filter.IsNullOrWhiteSpace(), u =>
                            u.ScheduleName.Contains(input.Filter)).OrderBy(s => s.StartDate);
                if (input.ScheduleId > 0)
                {
                    schedule = await _externalAssessmentScheduleRepository.FirstOrDefaultAsync(s => s.Id == input.ScheduleId);



                    var pagedAndFilteredSchedules = await query
                                .OrderBy(input.Sorting)
                                .PageBy(input).ToListAsync();

                    var schedules = pagedAndFilteredSchedules.Select(
                                       e => new ExternalAssessmentScheduleDetailDto
                                       {
                                           Id = e.Id,
                                           ScheduleCode = schedule.ScheduleId,
                                           ScheduleName = e.ScheduleName,
                                           AssessmentInfo = e.AssessmentInfo,
                                           AssessmentName = e.AssessmentName,
                                           AssessmentTypeId = e.AssessmentTypeId,
                                           AssementScheduleId = e.AssementScheduleId,
                                           EndDate = e.EndDate,
                                           FeedBack = e.FeedBack,
                                           RecurringJobId = e.RecurringJobId,
                                           ScheduleId = e.ScheduleId,
                                           ScheduleTypeId = e.ScheduleTypeId,
                                           SendEmailNotify = e.SendEmailNotify,
                                           SendSmsNotify = e.SendSmsNotify,
                                           StartDate = e.StartDate,
                                           TenantId = e.TenantId,
                                           ScheduleType = new DynamicNameValueDto { Id = e.ScheduleType.Id, Name = e.ScheduleType.Value },
                                           AssessmentType = new DynamicNameValueDto { Id = e.AssessmentType.Id, Name = e.AssessmentType.Value },
                                           AuthoritativeDocumentIds = e.AuthoritativeDocuments.Select(a => a.AuthoritativeDocumentId).ToList(),
                                           //  BusinessEnityGroupWiesDetails = getdata.ToList(),
                                       });

                     totalCount = await query.CountAsync();


                    return new PagedResultDto<ExternalAssessmentScheduleDetailDto>(totalCount, schedules.ToList());
                }
                return new PagedResultDto<ExternalAssessmentScheduleDetailDto>(totalCount, null);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }


        public async Task<List<BusinessEnityGroupWiesDto>> GetAllBusinessEntityByScheduleId(int Id)
        {
            var query = new List<BusinessEnityGroupWiesDto>();
            try
            {
                query = (from b in _externalAssessmentScheduleEntityGroupRepository.GetAll().Where(x => x.ExternalAssessmentScheduleId == Id).Include(x=>x.BusinessEntity)
                         select new BusinessEnityGroupWiesDto()
                         {
                             Id = ((int)b.BusinessEntityId),
                             CompanyName = b.BusinessEntity.CompanyName,
                             EntityGroupId = b.EntityGroupId==null?0:(int)b.EntityGroupId,
                             ExtGenerated = b.ExtGenerated,
                             EntityType = b.EntityType,
                             FacilityTypeId=(b.BusinessEntity.FacilityTypeId!=null)?b.BusinessEntity.FacilityTypeId:0,
                             FacilitySubTypeId=(b.BusinessEntity.FacilitySubTypeId!=null)?b.BusinessEntity.FacilitySubTypeId:0,
                         }).ToList();
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteScheduledAssessmentDetails(long id)
        {
            var assmentschedules = await _extAssessmentRepository.GetAll().Where(s => s.ScheduleDetailId == id).ToListAsync();
            foreach (var item in assmentschedules)
            {
                await _extAssessmentRepository.DeleteAsync(item);
            }

            var schDetail = await _externalAssessmentScheduleDetailRepository.FirstOrDefaultAsync(s => s.Id == id);
            await _externalAssessmentScheduleDetailRepository.DeleteAsync(schDetail);
        }

        public async Task DeleteScheduledAssessment(long id)
        {
            var schedule = await _externalAssessmentScheduleRepository.GetAll().Where(s => s.Id == id).FirstOrDefaultAsync();

            var scheduleDetails = await _externalAssessmentScheduleDetailRepository.GetAll().Where(s => s.ScheduleId == schedule.Id).ToListAsync();

            foreach (var sch in scheduleDetails)
            {
                await DeleteScheduledAssessmentDetails(sch.Id);
            }


            await _externalAssessmentScheduleRepository.DeleteAsync(schedule);
        }
    }

}
