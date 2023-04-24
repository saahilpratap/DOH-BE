﻿using System;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Web.Models.AbpUserConfiguration;
using LockthreatCompliance.ApiClient;

namespace LockthreatCompliance.Configuration
{
    public class UserConfigurationService : ITransientDependency
    {
        private readonly AbpApiClient _apiClient;
        private readonly IAccessTokenManager _tokenManager;
        private const string Endpoint = "AbpUserConfiguration/GetAll";

        public Func<Task> OnSessionTimeOut { get; set; }

        public Func<string, Task> OnAccessTokenRefresh { get; set; }

        public UserConfigurationService(AbpApiClient apiClient, IAccessTokenManager tokenManager)
        {
            _apiClient = apiClient;
            _tokenManager = tokenManager;
        }

        public async Task<AbpUserConfigurationDto> GetAsync(bool isUserLoggedIn)
        {
            return isUserLoggedIn
                ? await GetAuthenticatedUserConfig()
                : await _apiClient.GetAnonymousAsync<AbpUserConfigurationDto>(Endpoint);
        }

        private async Task<AbpUserConfigurationDto> GetAuthenticatedUserConfig()
        {
            var userConfig = await _apiClient.GetAsync<AbpUserConfigurationDto>(Endpoint);

            if (userConfig.HasSessionUserId())
            {
                return userConfig;
            }

            if (_tokenManager.IsRefreshTokenExpired)
            {
                return await HandleSessionTimeOut(userConfig);
            }

            return await RefreshAccessTokenAndSendRequestAgain();
        }

        private async Task<AbpUserConfigurationDto> HandleSessionTimeOut(AbpUserConfigurationDto userConfig)
        {
            _tokenManager.Logout();

            if (OnSessionTimeOut != null)
            {
                await OnSessionTimeOut();
            }

            return userConfig;
        }

        private async Task<AbpUserConfigurationDto> RefreshAccessTokenAndSendRequestAgain()
        {
            var newAccessToken = await _tokenManager.RefreshTokenAsync();
            
            if (OnAccessTokenRefresh != null)
            {
                await OnAccessTokenRefresh(newAccessToken);
            }

            return await _apiClient.GetAsync<AbpUserConfigurationDto>(Endpoint);
        }
    }
}