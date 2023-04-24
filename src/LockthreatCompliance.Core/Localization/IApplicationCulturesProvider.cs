using System.Globalization;

namespace LockthreatCompliance.Localization
{
    public interface IApplicationCulturesProvider
    {
        CultureInfo[] GetAllCultures();
    }
}