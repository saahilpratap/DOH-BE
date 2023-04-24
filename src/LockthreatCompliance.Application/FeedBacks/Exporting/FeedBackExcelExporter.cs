using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.Dto;
using LockthreatCompliance.FeedBacks.Dtos;
using LockthreatCompliance.Storage;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using OfficeOpenXml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LockthreatCompliance.FeedBacks.Exporting
{
    public class FeedBackExcelExporter : EpPlusExcelExporterBase, IFeedBackExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FeedBackExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
      base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<FeedBackExcelDto> feedbackresponse)
        {
            return CreateExcelPackage(
               "FeedbackResponse.xlsx",
               excelPackage =>
               {
                   var sheet = excelPackage.Workbook.Worksheets.Add(L("FeedBackResponse"));

                   AddHeader(
                       sheet,
                       ("Question"),
                       ("Response With Count")
                       
                       );

                   AddObjects(
                     sheet, 2, feedbackresponse,
                     _ => _.QuestionName,
                     _ => _.OptionWithCount
                     );

                   for (var i = 0; i < 3; i++)
                   {
                       sheet.Cells.AutoFitColumns(i);
                   }
               });
        }

    }
}
