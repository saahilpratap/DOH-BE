﻿using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.Hangfire
{
    public interface IHangfireFiledUpdateAppService : IApplicationService
    {
        Task UpdateFiled();
    }
}
