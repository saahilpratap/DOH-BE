using Abp.BackgroundJobs;
using Abp.Dependency;
using LockthreatCompliance.Hangfire;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.MultiTenancy
{
    public class HangfireHourlyJobAppService : BackgroundJob<int>, ITransientDependency
    {
        private readonly IHangfireCustomAppService _hangfireCustomAppService;

        public HangfireHourlyJobAppService(IHangfireCustomAppService hangfireCustomAppService)
        {
            _hangfireCustomAppService = hangfireCustomAppService;
        }

        public override void Execute(int args)
        {
           // _hangfireCustomAppService.SendMail();
        }
    }
}
