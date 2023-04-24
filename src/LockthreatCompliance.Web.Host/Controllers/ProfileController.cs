using Abp.AspNetCore.Mvc.Authorization;
using LockthreatCompliance.Storage;

namespace LockthreatCompliance.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
        }
    }
}