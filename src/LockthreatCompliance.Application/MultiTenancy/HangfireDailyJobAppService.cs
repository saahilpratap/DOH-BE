﻿using Abp.BackgroundJobs;
using Abp.Dependency;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Hangfire;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.MultiTenancy
{
    public class HangfireDailyJobAppService : BackgroundJob<int>, ITransientDependency
    {
        private readonly IHangfireCustomAppService _hangfireCustomAppService;
        private readonly IEntityApplicationSettingAppService _entityApplicationSettingAppService;

        public HangfireDailyJobAppService(IHangfireCustomAppService hangfireCustomAppService, IEntityApplicationSettingAppService entityApplicationSettingAppService)
        {
            _hangfireCustomAppService = hangfireCustomAppService;
            _entityApplicationSettingAppService = entityApplicationSettingAppService;
        }

        public override void Execute(int args)
        {
            var TriggerValue = _entityApplicationSettingAppService.GetWorkFlowTriggerValue();

            if (TriggerValue)
            {
               _hangfireCustomAppService.SendMailDaily();
            }
        }
    }
}
