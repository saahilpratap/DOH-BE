using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.Dto;
using LockthreatCompliance.FindingReports.Dtos;
using LockthreatCompliance.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FindingReports.Export
{
    public class FindingReportExternalExporter : EpPlusExcelExporterBase, IFindingReportExternalExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FindingReportExternalExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFileExternalFinding(List<ImportExternalFindingDto> externalFindingDto)
        {
            return CreateExcelPackage(
              "ExternalFindingSheet.xlsx",
              excelPackage =>
              {
                  var sheet = excelPackage.Workbook.Worksheets.Add(L("ExternalFindingSheet"));
                  sheet.OutLineApplyStyle = true;

                  AddHeader(
                      sheet,                   
                      ("Stage"),
                      ("Control Ref."),
                      ("Audit Question Subject"),
                      ("Entity compliance"),
                      ("Finding description"),
                      ("References"),
                      ("Date Found")
                     
                      );

                  AddObjects(
                      sheet, 2, externalFindingDto,                     
                      _ => _.Stage,
                      _ => _.ControlRequirementId,
                      _ => _.Title,
                       _ => _.Response,
                      _ => _.Description,
                      _ => _.Reference,
                       _ => _.DateFound                    
                      );


                  for (var i = 0; i < 7; i++)
                  {
                      sheet.Cells.AutoFitColumns(i);
                  }

              });
        }


        public FileDto ExportCapaStatus(List<ImportExternalCapaDto> exportCapa)
        {
            return CreateExcelPackage(
             "ExportFindingCapa.xlsx",
             excelPackage =>
             {
                 var sheet = excelPackage.Workbook.Worksheets.Add(L("ExportFindingCapa"));
                 sheet.OutLineApplyStyle = true;

                 AddHeader(
                     sheet,                   
                     ("Control Ref."),
                     ("Corrective Action Plan"),
                     ("Root Cause"),                   
                     ("Status"),
                     ("Finding Status"),
                     ("Expected Closure Date")

                     );

                 AddObjects(
                     sheet, 2, exportCapa,                    
                     _ => _.ControlRequirementId,
                     _ => _.CorrectiveAction,
                      _ => _.RootCause,
                     _ => _.Status,
                     _ => _.FindingStatus,
                      _ => _.ExpectedClosedDate
                     );


                 for (var i = 0; i < 7; i++)
                 {
                     sheet.Cells.AutoFitColumns(i);
                 }

             });
        }

    }

}
