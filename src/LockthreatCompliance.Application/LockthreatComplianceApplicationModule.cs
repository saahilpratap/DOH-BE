using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using LockthreatCompliance.Authorization;
using LockthreatCompliance.DtoMappers;
namespace LockthreatCompliance
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(LockthreatComplianceApplicationSharedModule),
        typeof(LockthreatComplianceCoreModule)
        )]
    public class LockthreatComplianceApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
            Configuration.Modules.AbpAutoMapper().Configurators.Add(AuthoritativeDocumentDtoMapper.CreateMappings);
            Configuration.Modules.AbpAutoMapper().Configurators.Add(BusinessEntityDtoMapper.CreateMappings);
            Configuration.Modules.AbpAutoMapper().Configurators.Add(AssessmentDtoMapper.CreateMappings);
            Configuration.Modules.AbpAutoMapper().Configurators.Add(FindingReportDtoMapper.CreateMappings);
            Configuration.Modules.AbpAutoMapper().Configurators.Add(ExternalAssessmentDtoMapper.CreateMappings);

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LockthreatComplianceApplicationModule).GetAssembly());
        }
    }
}