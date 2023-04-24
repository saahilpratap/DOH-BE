using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace LockthreatCompliance.Localization
{
    public static class LockthreatComplianceLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    LockthreatComplianceConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(LockthreatComplianceLocalizationConfigurer).GetAssembly(),
                        "LockthreatCompliance.Localization.LockthreatCompliance"
                    )
                )
            );
        }
    }
}