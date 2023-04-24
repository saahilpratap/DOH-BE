using Abp.BackgroundJobs;
using Abp.Dependency;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Hangfire;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.MultiTenancy
{
    public class HangfireDailyJobFiledUpdateAppService : BackgroundJob<int>, ITransientDependency
    {
        private readonly IHangfireFiledUpdateAppService _hangfireCustomAppService;
        private readonly IEntityApplicationSettingAppService _entityApplicationSettingAppService;

        public HangfireDailyJobFiledUpdateAppService(IHangfireFiledUpdateAppService hangfireCustomAppService, IEntityApplicationSettingAppService entityApplicationSettingAppService)
        {
            _hangfireCustomAppService = hangfireCustomAppService;
            _entityApplicationSettingAppService = entityApplicationSettingAppService;
        }

        public override void Execute(int args)
        {
            var TriggerValue = _entityApplicationSettingAppService.GetWorkFlowTriggerValue();

            if (TriggerValue)
            {
               _hangfireCustomAppService.UpdateFiled();
            }
        }
    }
}
