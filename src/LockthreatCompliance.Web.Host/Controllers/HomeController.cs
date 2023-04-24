using Abp.Auditing;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using LockthreatCompliance.Web.DXServices.Reports;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace LockthreatCompliance.Web.Controllers
{    
    public class HomeController : LockthreatComplianceControllerBase
    {
        [DisableAuditing]
        public IActionResult Index()
        {
            //return RedirectToAction("Index", "Ui");
            return RedirectToAction("", "Error");

        }
    }
}
