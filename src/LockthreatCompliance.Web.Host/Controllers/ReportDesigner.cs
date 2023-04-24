using DevExpress.XtraReports.Web.ReportDesigner;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DevExpress.DataAccess.Sql;

using DevExpress.XtraReports.Web.Localization;
using System.Globalization;
using System.Threading;
using System.Web;


namespace LockthreatCompliance.Web.Controllers
{
    public class ReportDesigner : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public ActionResult GetReportDesignerModel(string reportUrl)
        {
            //Response("Access-Control-Allow-Origin", "*");

            string modelJsonScript =
                new ReportDesignerClientSideModelGenerator()
                .GetJsonModelScript(
                    reportUrl,                 // The URL of a report that is opened in the Report Designer when the application starts.
                    GetAvailableDataSources(), // Available data sources in the Report Designer that can be added to reports.
                    "ReportDesigner/Invoke",   // The URI path of the controller action that processes requests from the Report Designer.
                    "WebDocumentViewer/Invoke",// The URI path of the controller action that processes requests from the Web Document Viewer.
                    "QueryBuilder/Invoke"      // The URI path of the controller action that processes requests from the Query Builder.
                );
            return Content(modelJsonScript, "application/json");
        }

        Dictionary<string, object> GetAvailableDataSources()
        {
            var dataSources = new Dictionary<string, object>();
            SqlDataSource ds = new SqlDataSource("NWindConnectionString");
            var query = SelectQueryFluentBuilder.AddTable("MainReport").SelectAllColumns().Build("MainReport");
            ds.Queries.Add(query);
            ds.RebuildResultSchema();
            dataSources.Add("SqlDataSource", ds);
            return dataSources;
        }

    }
}
