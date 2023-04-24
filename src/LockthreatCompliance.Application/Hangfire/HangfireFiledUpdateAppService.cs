using Abp.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using LockthreatCompliance.WrokFlows;
using LockthreatCompliance.Authorization.Users;
using Microsoft.EntityFrameworkCore;
using Abp.Collections.Extensions;
using System.Linq;
using LockthreatCompliance.Hangfire.Dto;
using LockthreatCompliance.AuditProjects;
using System;
using LockthreatCompliance.WorkFlow;
using System.Reflection;
using AutoMapper;
using System.Text.RegularExpressions;
using LockthreatCompliance.BusinessEntities;

namespace LockthreatCompliance.Hangfire
{
    public class HangfireFiledUpdateAppService : LockthreatComplianceAppServiceBase, IHangfireFiledUpdateAppService
    {
        private readonly IRepository<State, long> _stateRepository;
        private readonly IRepository<AuditProjectStatus, long> _auditProjectStatusRepository;
        private readonly IRepository<AuditProject, long> _auditProjectRepository;
        private readonly IRepository<Assessment> _assessmentRepository;

        public HangfireFiledUpdateAppService(IRepository<State, long> stateRepository, IRepository<AuditProjectStatus, long> auditProjectStatusRepository, IRepository<AuditProject, long> auditProjectRepository, IRepository<Assessment> assessmentRepository)
        {
            _stateRepository = stateRepository;
            _auditProjectStatusRepository = auditProjectStatusRepository;
            _auditProjectRepository = auditProjectRepository;
            _assessmentRepository = assessmentRepository;
        }

        public async Task UpdateFiled()
        {
            List<string> replacerList = new List<string>();

            var notSubmitAssessment = await _assessmentRepository.GetAll()
                .Where(x => x.Status == AssessmentStatus.Initialized)
                .Where(x=> x.BusinessEntity.Status == EntityTypeStatus.Active)
                .Where(x => Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy")) > Convert.ToDateTime(x.ReportingDeadLine.ToString("dd-MMM-yyyy")))
                .ToListAsync();

            var tempNotSubmitAssessment = notSubmitAssessment;
            tempNotSubmitAssessment.ForEach(x =>
            {
                var temp = x;
                temp.Status = AssessmentStatus.NotSubmit;
                _assessmentRepository.UpdateAsync(temp);
            });

            var stateList = await _stateRepository.GetAll().Include(x => x.WorkFlowPage).Where(x => x.WorkFlowPage.PageName == "Audit Project" && x.IsStateOpen == true && x.TargetFiled != 0).ToListAsync();

            var allAuditProjectStatusInfo = await _auditProjectStatusRepository.GetAll().Include(x => x.AuditProject).ToListAsync();

            foreach (var item in stateList)
            {
                if (item.IsStateActive && item.TargetFiled != 0)
                {
                    var dealLineCalculation = new HHMMDDDto();
                    var query = allAuditProjectStatusInfo.Where(x => (int)x.StatusId == item.AuditProjectStatus && x.StatusId == x.AuditProject.AuditStatusId).ToList();

                    var filterAuditProjectList = query.OrderBy(x => x.ActionDate).GroupBy(x => x.AuditProjectId)
                         .Select(y => y.GroupBy(x => x.Status)
                         .Select(x => x.OrderByDescending(y => y.ActionDate).FirstOrDefault()).FirstOrDefault())
                         .ToList();

                    dealLineCalculation = GetStateDeadlines(item, dealLineCalculation);

                    filterAuditProjectList.ForEach(x =>
                    {
                        DateTime compareString = new DateTime();
                        DateTime currentDate = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy HH:mm"));
                        DateTime todate = Convert.ToDateTime(DateTime.Now.AddMinutes(20).ToString("dd-MMM-yyyy HH:mm"));

                        if (x.ActionDate != null)
                        {
                            var tempDate = ((DateTime)x.ActionDate).AddDays(dealLineCalculation.Day).AddHours(dealLineCalculation.Hr).AddMinutes(dealLineCalculation.Min).ToString("dd-MMM-yyyy HH:mm");
                            compareString = Convert.ToDateTime(tempDate);
                        }

                        if (currentDate < compareString && compareString <= todate)
                        {
                            var tempAuditProjectObject = x.AuditProject;
                            tempAuditProjectObject.AuditStatusId = item.TargetFiled;
                            _auditProjectRepository.UpdateAsync(tempAuditProjectObject);
                        }
                    });

                }
            }

        }

        private HHMMDDDto GetStateDeadlines(State sa, HHMMDDDto output)
        {
            HHMMDDDto result = output;
            switch (sa.ActionTimeType)
            {
                case ActionTimeType.Days:
                    result.Day += sa.StateDeadline;
                    break;
                case ActionTimeType.Weeks:
                    result.Day += sa.StateDeadline * 7;
                    break;
                case ActionTimeType.Months:
                    result.Day += sa.StateDeadline * 30;
                    break;
                case ActionTimeType.Hours:
                    result.Hr += sa.StateDeadline;
                    break;
                case ActionTimeType.Minutes:
                    result.Min += sa.StateDeadline;
                    break;
                default:
                    break;
            }
            return result;
        }

    }
}
