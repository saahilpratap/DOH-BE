using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.AspNetCore.Reporting.QueryBuilder;
using DevExpress.AspNetCore.Reporting.QueryBuilder.Native.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DevExpress.AspNetCore.Reporting.ReportDesigner;
using DevExpress.AspNetCore.Reporting.ReportDesigner.Native.Services;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer.Native.Services;

namespace LockthreatCompliance.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("DXXQB")]
    public class CustomQueryBuilderController : QueryBuilderController
    {
        public CustomQueryBuilderController(IQueryBuilderMvcControllerService controllerService) : base(controllerService) { }
        public override Task<IActionResult> Invoke()
        {
            return base.Invoke();
        }
    }
}
