﻿using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace LockthreatCompliance.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
