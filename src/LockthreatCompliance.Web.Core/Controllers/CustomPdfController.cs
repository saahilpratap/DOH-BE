using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using DinkToPdf.Contracts;
using LockthreatCompliance.AuditProjects;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using DinkToPdf;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace LockthreatCompliance.Web.Controllers
{
    public class CustomPdfController : LockthreatComplianceControllerBase
    {
        private IConverter _converter;
        private readonly IRepository<AuditMeeting, long> _auditMeetingAppService;

        public CustomPdfController(IConverter converter, IRepository<AuditMeeting, long> auditMeetingAppService)
        {
            _converter = converter;
            _auditMeetingAppService= auditMeetingAppService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<FileResult> AuditPlanReportPDF(long Id)
        {

            string data = "";
            var query = await _auditMeetingAppService.GetAll().Where(m => m.Id == Id).FirstOrDefaultAsync();
            if (query != null)
            {
                data = query.EditorData == null ? "" : query.EditorData;
            }
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings() { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                DocumentTitle = "Audit Plan Report",

            };
            StringBuilder sb = new StringBuilder(data);
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
            };
           
                var objectSettings = new ObjectSettings
                {

                    PagesCount = true,
                    HtmlContent = data,
                   WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                    HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                    //FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Emirsec Technologies" }
                };
                pdf.Objects.Add(objectSettings);
           
            try
            {
                var file = _converter.Convert(pdf);
                return File(file, "application/pdf");

            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }
    }
}
