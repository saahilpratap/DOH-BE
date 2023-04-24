using System;
using System.Collections.Generic;
using System.Text;
using Abp.BackgroundJobs;
using Abp.Dependency;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Hangfire;

namespace LockthreatCompliance.MultiTenancy
{
 public   class HangfireDailyJobBusinessEntityThirdPartyAppService : BackgroundJob<int>, ITransientDependency
    {
        private readonly IHangfireBusinessEntityThirdPartyAppService _hangfireCustomAppService;
        private readonly IEntityApplicationSettingAppService _entityApplicationSettingAppService;

        public HangfireDailyJobBusinessEntityThirdPartyAppService(IHangfireBusinessEntityThirdPartyAppService hangfireCustomAppService, IEntityApplicationSettingAppService entityApplicationSettingAppService)
        {
            _hangfireCustomAppService = hangfireCustomAppService;
            _entityApplicationSettingAppService = entityApplicationSettingAppService;
        }

        public override void Execute(int args)
        {

            var TriggerValue = _entityApplicationSettingAppService.GetWorkFlowTriggerValue();

            if (TriggerValue)
            {

          //      _hangfireCustomAppService.SendForBusinessEntity();
            }
            
        }
    }
}
