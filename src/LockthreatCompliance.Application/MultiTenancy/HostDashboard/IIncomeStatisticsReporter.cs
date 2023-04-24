using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LockthreatCompliance.MultiTenancy.HostDashboard.Dto;

namespace LockthreatCompliance.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}