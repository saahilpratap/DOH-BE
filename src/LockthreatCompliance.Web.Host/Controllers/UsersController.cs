using Abp.AspNetCore.Mvc.Authorization;
using LockthreatCompliance.Authorization;
using LockthreatCompliance.Storage;
using Abp.BackgroundJobs;

namespace LockthreatCompliance.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}