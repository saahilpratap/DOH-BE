using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.DynamicEntityParameters;
using Abp.Extensions;
using Abp.Linq.Extensions;
using LockthreatCompliance.AssessmentSchedules.Dto;
using LockthreatCompliance.AssessmentSchedules.InternalAsssementSchedules;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using LockthreatCompliance.AuthoritativeDocuments.Dtos;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.AuditProjects.Dtos;
using MimeKit.Cryptography;
using Abp.UI;

namespace LockthreatCompliance.AssessmentSchedules
{
    public class AssementScheduleAppService : LockthreatComplianceAppServiceBase, IAssementScheduleAppService
    {
        private readonly IRepository<DynamicParameterValue> _dynamicParameterValueRepository;
        private readonly IRepository<DynamicParameter> _dynamicParameterManager;
        private readonly IRepository<InternalAssessmentSchedule> _internalAssessmentScheduleRepository;
        private readonly IRepository<InternalAssessmentScheduleDetail> _internalAssessmentScheduleDetailRepository;
        private readonly IRepository<Assessment> _assessmentRepository;
        private readonly IRepository<InternalAssessmentScheduleBusinessEntity> _internalAssessmentScheduleBusinessEntityRepository;
        private readonly IRepository<ReviewData> _reviewdataRepository;

        public AssementScheduleAppService(IRepository<DynamicParameterValue> dynamicParameterValueRepository,
            IRepository<ReviewData> reviewdataRepository,
            IRepository<DynamicParameter> dynamicParameterManager, IRepository<Assessment> assessmentRepository,
            IRepository<InternalAssessmentSchedule> internalAssessmentScheduleRepository,
            IRepository<InternalAssessmentScheduleBusinessEntity> internalAssessmentScheduleBusinessEntityRepository,
            IRepository<InternalAssessmentScheduleDetail> internalAssessmentScheduleDetailRepository)
        {
            _reviewdataRepository = reviewdataRepository;

            _dynamicParameterValueRepository = dynamicParameterValueRepository;
            _dynamicParameterManager = dynamicParameterManager;
            _internalAssessmentScheduleRepository = internalAssessmentScheduleRepository;
            _internalAssessmentScheduleDetailRepository = internalAssessmentScheduleDetailRepository;
            _assessmentRepository = assessmentRepository;
            _internalAssessmentScheduleBusinessEntityRepository = internalAssessmentScheduleBusinessEntityRepository;
        }

        public async Task AddorUpdateAssessmentSchedule(InternalAssessmentScheduleDto input)
        {
            try
            {
                input.TenantId = AbpSession.TenantId;
                var checkAssessmentInfo = await _internalAssessmentScheduleRepository.GetAll().Where(x => x.AssessmentInfo.Trim().ToLower() == input.AssessmentInfo.Trim().ToLower()).FirstOrDefaultAsync();
                if (checkAssessmentInfo == null)
                {
                    var scheduleId = await _internalAssessmentScheduleRepository.InsertOrUpdateAndGetIdAsync(ObjectMapper.Map<InternalAssessmentSchedule>(input));
                    var allScheduleDetails = await _internalAssessmentScheduleDetailRepository.GetAll().Where(s => s.ScheduleId == scheduleId).ToListAsync();
                    int cnt = 0;
                    int currentYear = 0;
                    int PrevYear = 0;
                    int count = 0;

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
                        var scheduleDetailData = new InternalAssessmentScheduleDetailDto
                        {
                            ScheduleId = scheduleId,
                            AssementScheduleId = "",
                            AssessmentInfo = input.AssessmentInfo,
                            //AssessmentName = input.AssessmentName.Replace("x", cnt.ToString()),
                            AssessmentName = "ADHICS-" + input.ScheduleTypeName + cnt.ToString() + "-" + +date.Year + "-" + input.AssessmentTypeName,
                            AssessmentTypeId = input.AssessmentTypeId,
                            AuthoritativeDocumentId = input.AuthoritativeDocumentId,
                            FeedBack = input.FeedBack,
                            ScheduleName = input.ScheduleName,
                            ScheduleTypeId = input.ScheduleTypeId,
                            SendEmailNotify = input.SendEmailNotify,
                            SendSmsNotify = input.SendSmsNotify,
                            TenantId = input.TenantId
                        };
                        scheduleDetailData.ScheduleId = scheduleId;
                        if (input.ScheduleTypeName == "Daily")
                        {
                            scheduleDetailData.StartDate = date;
                            scheduleDetailData.EndDate = date;
                            await _internalAssessmentScheduleDetailRepository.InsertAsync(ObjectMapper.Map<InternalAssessmentScheduleDetail>(scheduleDetailData));
                        }
                        else
                        {
                            if (input.ScheduleTypeName == "Weekly")
                            {
                                var index = input.ScheduledDates.FindIndex(d => d == date);
                                scheduleDetailData.StartDate = date;
                                scheduleDetailData.EndDate = date.AddDays(6);
                                await _internalAssessmentScheduleDetailRepository.InsertAsync(ObjectMapper.Map<InternalAssessmentScheduleDetail>(scheduleDetailData));
                            }
                            else
                            {
                                if (input.ScheduleTypeName == "Monthly")
                                {
                                    var index = input.ScheduledDates.FindIndex(d => d == date);
                                    scheduleDetailData.StartDate = date;
                                    scheduleDetailData.EndDate = date.AddMonths(1).AddDays(-1);
                                    await _internalAssessmentScheduleDetailRepository.InsertAsync(ObjectMapper.Map<InternalAssessmentScheduleDetail>(scheduleDetailData));
                                }
                                else
                                {

                                    if (input.ScheduleTypeName == "Quarterly")
                                    {
                                        var index = input.ScheduledDates.FindIndex(d => d == date);
                                        scheduleDetailData.StartDate = date;
                                        if (date.Month == 10)
                                        {
                                            scheduleDetailData.EndDate = date.AddMonths(2).AddDays(30);
                                        }
                                        else
                                        {
                                            scheduleDetailData.EndDate = date.AddMonths(3).AddDays(-1);
                                        }

                                        await _internalAssessmentScheduleDetailRepository.InsertAsync(ObjectMapper.Map<InternalAssessmentScheduleDetail>(scheduleDetailData));

                                    }
                                    else
                                    {
                                        if (input.ScheduleTypeName == "Bi-Annual")
                                        {
                                            var index = input.ScheduledDates.FindIndex(d => d == date);
                                            scheduleDetailData.StartDate = date;
                                            scheduleDetailData.EndDate = date.AddMonths(6);
                                            await _internalAssessmentScheduleDetailRepository.InsertAsync(ObjectMapper.Map<InternalAssessmentScheduleDetail>(scheduleDetailData));
                                        }
                                        else
                                        {
                                            if (input.ScheduleTypeName == "Annual")
                                            {
                                                var index = input.ScheduledDates.FindIndex(d => d == date);
                                                scheduleDetailData.StartDate = date;
                                                string iDate = "" + date.Year + "-12-31";
                                                scheduleDetailData.EndDate = DateTime.Parse(iDate);
                                                await _internalAssessmentScheduleDetailRepository.InsertAsync(ObjectMapper.Map<InternalAssessmentScheduleDetail>(scheduleDetailData));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    throw new UserFriendlyException("Assessment Info Already Exist!");
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public async Task AddorUpdatInternalAssessmentScheduleBusinessentity(InternalAssessmentScheduleDetailBusinessEntityDto input)
        {
            try
            {
                //long tenantId = input.TenantId == 0 ? 
                await _internalAssessmentScheduleBusinessEntityRepository.HardDeleteAsync(x => x.InternalAssessmentScheduleDetailId == input.InternalAssessmentScheduleDetailId && x.ExtGenerated == false && x.TenantId == AbpSession.TenantId);
                input.BusinessEnityies.ForEach(obj =>
                {
                    var items = new InternalAssessmentScheduleBusinessEntity();
                    items.ExtGenerated = false;
                    items.TenantId =  AbpSession.TenantId;
                    items.EntityGroupId = obj.EntityGroupId == 0 ? null : obj.EntityGroupId;
                    items.BusinessEntityId = obj.Id;
                    items.InternalAssessmentScheduleDetailId = input.InternalAssessmentScheduleDetailId;
                    items.EntityType = input.EntityType;
                    _internalAssessmentScheduleBusinessEntityRepository.InsertAsync(items);
                });

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<DynamicNameValueDto>> GetScheduleTypes()
        {
            var getScheduleTypes = new List<DynamicNameValueDto>();
            try
            {
                var getcheckId = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == "Schedule Types");
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
                var getcheckId = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == "Assessment Types"); // Assessmet type
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

        public async Task<PagedResultDto<InternalAssessmentScheduleDto>> GetAllScheduledAssessments(GetAllScheduleInput input)
        {
            var query = _internalAssessmentScheduleRepository.GetAll().Include(d => d.ScheduleType).Include(d => d.AssessmentType)
                .Include(a => a.AuthoritativeDocument).WhereIf(
                   !input.Filter.IsNullOrWhiteSpace(), u =>
                        u.ScheduleName.Contains(input.Filter));
             
            var totalCount = await query.CountAsync();
            var pagedAndFilteredBusinessEntities = await query
            .OrderBy(input.Sorting)
            .PageBy(input)
            .ToListAsync();

            var schedules = pagedAndFilteredBusinessEntities.Select(
                              e => new InternalAssessmentScheduleDto
                              {
                                  Id = e.Id,
                                  ScheduleName = e.ScheduleName,
                                  AssessmentInfo = e.AssessmentInfo,
                                  AssessmentName = e.AssessmentName,
                                  AssessmentTypeId = e.AssessmentTypeId,
                                  AuthoritativeDocumentId = e.AuthoritativeDocumentId,
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
                                   //  AuthoritativeDocument = new AuthoritativeDocumentDto { Id = e.AuthoritativeDocument.Id, Name = e.AuthoritativeDocument.Name, Code = e.AuthoritativeDocument.Code },
                                   // ScheduleDetails = ObjectMapper.Map<List<InternalAssessmentScheduleDetailDto>>(_internalAssessmentScheduleDetailRepository.GetAll().Where(s => s.ScheduleId == e.Id).ToList())
                               });

            return new PagedResultDto<InternalAssessmentScheduleDto>(totalCount, schedules.ToList());
        }

        public async Task<InternalAssessmentScheduleDto> GetSchedulesAssessmentDetails(int id)
        {
            var schedule = await _internalAssessmentScheduleRepository.GetAll().Where(s => s.Id == id).Include(d => d.ScheduleType).Include(d => d.AssessmentType)
                .Include(a => a.AuthoritativeDocument).FirstOrDefaultAsync();

            var result = new InternalAssessmentScheduleDto
            {
                Id = schedule.Id,
                ScheduleName = schedule.ScheduleName,
                AssessmentInfo = schedule.AssessmentInfo,
                AssessmentName = schedule.AssessmentName,
                AssessmentTypeId = schedule.AssessmentTypeId,
                AuthoritativeDocumentId = schedule.AuthoritativeDocumentId,
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
                AuthoritativeDocument = new AuthoritativeDocumentDto { Id = schedule.AuthoritativeDocument.Id, Name = schedule.AuthoritativeDocument.Name, Code = schedule.AuthoritativeDocument.Code }
            };

            return result;
        }

        public async Task DeleteScheduledAssessment(int id)
        {
            var schedule = await _internalAssessmentScheduleRepository.GetAll().Where(s => s.Id == id).FirstOrDefaultAsync();

            var scheduleDetails = await _internalAssessmentScheduleDetailRepository.GetAll().Where(s => s.ScheduleId == schedule.Id).ToListAsync();

            foreach (var sch in scheduleDetails)
            {
                await DeleteScheduledAssessmentDetails(sch.Id);
            }
            await _internalAssessmentScheduleRepository.DeleteAsync(schedule);
        }

        public async Task<PagedResultDto<InternalAssessmentScheduleDetailDto>> GetAllScheduledDetailAssessments(GetAllScheduleInput input)
        {
            var query = _internalAssessmentScheduleDetailRepository.GetAll().Where(s => s.ScheduleId == input.ScheduleId).Include(d => d.ScheduleType).Include(d => d.AssessmentType)
                .Include(a => a.AuthoritativeDocument);

            var schedule = await _internalAssessmentScheduleRepository.FirstOrDefaultAsync(s => s.Id == input.ScheduleId);

            var pagedAndFilteredSchedules = await query
                    .OrderBy(input.Sorting)
                    .PageBy(input).ToListAsync();

            var schedules = pagedAndFilteredSchedules.Select(
                               e => new InternalAssessmentScheduleDetailDto
                               {
                                   Id = e.Id,
                                   ScheduleCode = schedule.ScheduleId,
                                   ScheduleName = e.ScheduleName,
                                   AssessmentInfo = e.AssessmentInfo,
                                   AssessmentName = e.AssessmentName,
                                   AssessmentTypeId = e.AssessmentTypeId,
                                   AssementScheduleId = e.AssementScheduleId,
                                   AuthoritativeDocumentId = e.AuthoritativeDocumentId,
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
                                   AuthoritativeDocument = new AuthoritativeDocumentDto { Id = e.AuthoritativeDocument.Id, Name = e.AuthoritativeDocument.Name, Code = e.AuthoritativeDocument.Code },

                               });


            var totalCount = await query.CountAsync();

            return new PagedResultDto<InternalAssessmentScheduleDetailDto>(totalCount, schedules.ToList());
        }


        public async Task <List<InternalAssessmentScheduleDetailDto>> GetAllScheduledDetailAssessmentById(int id)
        {
            var result = new List<InternalAssessmentScheduleDetailDto>();
            var query = _internalAssessmentScheduleDetailRepository.GetAll().Where(s => s.ScheduleId == id).Include(d => d.ScheduleType).Include(d => d.AssessmentType)
                .Include(a => a.AuthoritativeDocument);

            var schedule = await _internalAssessmentScheduleRepository.FirstOrDefaultAsync(s => s.Id == id);

            result = query.Select(
                               e => new InternalAssessmentScheduleDetailDto
                               {
                                   Id = e.Id,
                                   ScheduleCode = schedule.ScheduleId,
                                   ScheduleName = e.ScheduleName,
                                   AssessmentInfo = e.AssessmentInfo,
                                   AssessmentName = e.AssessmentName,
                                   AssessmentTypeId = e.AssessmentTypeId,
                                   AssementScheduleId = e.AssementScheduleId,
                                   AuthoritativeDocumentId = e.AuthoritativeDocumentId,
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
                                   AuthoritativeDocument = new AuthoritativeDocumentDto { Id = e.AuthoritativeDocument.Id, Name = e.AuthoritativeDocument.Name, Code = e.AuthoritativeDocument.Code },

                               }).ToList();

            return result;
        }

        public async Task<List<BusinessEnityGroupWiesDto>> GetAllBusinessEntityByScheduleDetailId(int Id)
        {
            var query = new List<BusinessEnityGroupWiesDto>();
            try
            {
                query = await _internalAssessmentScheduleBusinessEntityRepository.GetAll().Where(x => x.InternalAssessmentScheduleDetailId == Id).Include(x => x.BusinessEntity)
                               .Select(b => new BusinessEnityGroupWiesDto()
                               {
                                   Id = ((int)b.BusinessEntityId),
                                   CompanyName = b.BusinessEntity.CompanyName,
                                   EntityGroupId = (int)b.EntityGroupId,
                                   EntityType = b.EntityType,
                                   ExtGenerated = b.ExtGenerated,
                                   FacilityTypeId = b.BusinessEntity.FacilityTypeId,
                                   FacilitySubTypeId = b.BusinessEntity.FacilitySubTypeId
                               }).ToListAsync();
                                                          
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteScheduledAssessmentDetails(int id)
        {
            var assmentschedules = await _assessmentRepository.GetAll().Where(s => s.ScheduleDetailId == id).ToListAsync();
            foreach (var item in assmentschedules)
            {
                await DeteleReviewData(item.Id);
                item.IsDeleted = true;
                await _assessmentRepository.DeleteAsync(item);
            }

            var internalAssessmentScheduleBusinessEntity = await _internalAssessmentScheduleBusinessEntityRepository.GetAll().Where(x => x.InternalAssessmentScheduleDetailId == id).ToListAsync();

            foreach (var items in internalAssessmentScheduleBusinessEntity)
            {
                await _internalAssessmentScheduleBusinessEntityRepository.DeleteAsync(items);
            }
            var schDetail = await _internalAssessmentScheduleDetailRepository.FirstOrDefaultAsync(s => s.Id == id);
            await _internalAssessmentScheduleDetailRepository.DeleteAsync(schDetail);
        }

        public async Task DeteleReviewData(int assessmentId)
        {
            var reviewData = await _reviewdataRepository.GetAll().Where(s => s.AssessmentId == assessmentId).ToListAsync();
            foreach (var item in reviewData)
            {
                await _reviewdataRepository.DeleteAsync(item);
            }
        }

    }
}
