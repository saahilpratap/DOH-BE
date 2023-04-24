using System.Threading.Tasks;
using Abp.Application.Services;
using LockthreatCompliance.Install.Dto;

namespace LockthreatCompliance.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}