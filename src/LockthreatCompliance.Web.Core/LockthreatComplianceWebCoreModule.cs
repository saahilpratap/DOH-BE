﻿using System;
using System.IO;
using System.Text;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.SignalR;
using Abp.AspNetZeroCore.Licensing;
using Abp.AspNetZeroCore.Web;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.IO;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Caching.Redis;
using Abp.Zero.Configuration;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using LockthreatCompliance.Chat;
using LockthreatCompliance.Configuration;
using LockthreatCompliance.EntityFrameworkCore;
using LockthreatCompliance.Startup;
using LockthreatCompliance.Web.Authentication.JwtBearer;
using LockthreatCompliance.Web.Authentication.TwoFactor;
using LockthreatCompliance.Web.Chat.SignalR;
using LockthreatCompliance.Web.Configuration;
using LockthreatCompliance.Web.DashboardCustomization;

namespace LockthreatCompliance.Web
{
    [DependsOn(
        typeof(LockthreatComplianceApplicationModule),
        typeof(LockthreatComplianceEntityFrameworkCoreModule),
        typeof(AbpAspNetZeroCoreWebModule),
        typeof(AbpAspNetCoreSignalRModule),
        typeof(LockthreatComplianceGraphQLModule),
        typeof(AbpRedisCacheModule), //AbpRedisCacheModule dependency (and Abp.RedisCache nuget package) can be removed if not using Redis cache
        typeof(AbpHangfireAspNetCoreModule) //AbpHangfireModule dependency (and Abp.Hangfire.AspNetCore nuget package) can be removed if not using Hangfire
    )]
    public class LockthreatComplianceWebCoreModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public LockthreatComplianceWebCoreModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            //Set default connection string
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                LockthreatComplianceConsts.ConnectionStringName
            );

            //Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(LockthreatComplianceApplicationModule).GetAssembly()
                );

            Configuration.Caching.Configure(TwoFactorCodeCacheItem.CacheName, cache =>
            {
                cache.DefaultAbsoluteExpireTime = TimeSpan.FromMinutes(2);
            });

            if (_appConfiguration["Authentication:JwtBearer:IsEnabled"] != null && bool.Parse(_appConfiguration["Authentication:JwtBearer:IsEnabled"]))
            {
                ConfigureTokenAuth();
            }

            IocManager.Register<DashboardViewConfiguration>();

            Configuration.ReplaceService<IAppConfigurationAccessor, AppConfigurationAccessor>();

            //Uncomment this line to use Hangfire instead of default background job manager (remember also to uncomment related lines in Startup.cs file(s)).
            //Configuration.BackgroundJobs.UseHangfire();

            //Uncomment this line to use Redis cache instead of in-memory cache.
            //See app.config for Redis configuration and connection string
            //Configuration.Caching.UseRedis(options =>
            //{
            //    options.ConnectionString = _appConfiguration["Abp:RedisCache:ConnectionString"];
            //    options.DatabaseId = _appConfiguration.GetValue<int>("Abp:RedisCache:DatabaseId");
            //});
        }

        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.AccessTokenExpiration = AppConsts.AccessTokenExpiration;
            tokenAuthConfig.RefreshTokenExpiration = AppConsts.RefreshTokenExpiration;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LockthreatComplianceWebCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            SetAppFolders();

            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(LockthreatComplianceWebCoreModule).Assembly);
        }

        private void SetAppFolders()
        {
            var appFolders = IocManager.Resolve<AppFolders>();

            appFolders.SampleProfileImagesFolder = Path.Combine(_env.WebRootPath, $"Common{Path.DirectorySeparatorChar}Images{Path.DirectorySeparatorChar}SampleProfilePics");
            appFolders.WebLogsFolder = Path.Combine(_env.ContentRootPath, $"App_Data{Path.DirectorySeparatorChar}Logs");
        }
    }
}