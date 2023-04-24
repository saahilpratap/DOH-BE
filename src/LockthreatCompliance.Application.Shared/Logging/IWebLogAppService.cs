using Abp.Application.Services;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Logging.Dto;

namespace LockthreatCompliance.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
