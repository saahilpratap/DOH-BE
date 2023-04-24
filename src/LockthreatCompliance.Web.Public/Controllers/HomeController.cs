using Microsoft.AspNetCore.Mvc;
using LockthreatCompliance.Web.Controllers;

namespace LockthreatCompliance.Web.Public.Controllers
{
    public class HomeController : LockthreatComplianceControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}