using DevExpress.Compatibility.System.Web;
using DevExpress.XtraReports.Web.ReportDesigner;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockthreatCompliance.Web.Controllers
{
    [Route("api/[controller]")]
    public class ReportDesignersController : Controller
    {
        [HttpPost("[action]")]
        public object GetReportDesignerModel([FromForm] string reportUrl)
        {
            string modelJsonScript = new ReportDesignerClientSideModelGenerator(HttpContext.RequestServices).GetJsonModelScript(reportUrl, null, "/DXXRD", "/DXXRDV", "/DXXQB");
            return new JavaScriptSerializer().Deserialize<object>(modelJsonScript);
        }
    }
}
