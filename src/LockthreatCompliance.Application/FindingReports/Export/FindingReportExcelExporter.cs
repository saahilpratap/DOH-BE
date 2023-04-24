using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.Dto;
using LockthreatCompliance.FindingReports.Dtos;
using LockthreatCompliance.Storage;
using NPOI.HSSF.Record;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FindingReports.Export
{
    public class FindingReportExcelExporter : EpPlusExcelExporterBase, IFindingReportExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FindingReportExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }
        public FileDto ExportToFile(List<FindingReportDto> findingReports)
        {
            return CreateExcelPackage(
                "FindingReport.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("FindingReport"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("TenantId"),
                        L("BusinessEntityName"),
                        L("Title"),
                        L("ClassificationName")
                        );

                    AddObjects(
                        sheet, 2, findingReports,
                        _ => _.Code,
                        _ => _.TenantId,
                        _ => _.BusinessEntityName,
                        _ => _.Title,
                        _ => _.ClassificationName                      
                        );

                   
                    for (var i = 0; i < 5; i++)
                    {
                        sheet.Cells.AutoFitColumns(i);
                    }

                });
        }

        public FileDto ExportToFileExternalFinding(List<ImportExternalFindingDto> externalFindingDto)
        {
            return CreateExcelPackage(
              "ExternalFindingErrorSheet.xlsx",
              excelPackage =>
              {
                  var sheet = excelPackage.Workbook.Worksheets.Add(L("ExternalFindingErrorSheet"));
                  sheet.OutLineApplyStyle = true;

                  AddHeader(
                      sheet,                     
                      ("Stage"),
                      ("Control Ref."),                    
                      ("Audit Question Subject"),
                      ("Entity compliance"),
                      ("Finding description"),
                      ("References"),                   
                      ("Date Found"),
                      ("Message"),
                      ("Row No")
                      );

                  AddObjects(
                      sheet, 2, externalFindingDto,                       
                      _ => _.Stage,
                      _ => _.ControlRequirementId,
                      _ => _.Title,
                       _ => _.Response,
                      _ => _.Description,
                      _ => _.Reference,                        
                       _ => _.DateFound,
                      _ => _.Message,
                      _ => _.RowNo
                      );


                  for (var i = 0; i < 7; i++)
                  {
                      sheet.Cells.AutoFitColumns(i);
                  }

              });
        }
        public FileDto ExportToFileExternalCapa(List<ImportExternalCapaDto> externalCapaDto)
        {
            return CreateExcelPackage(
              "ExternalFindingErrorSheet.xlsx",
              excelPackage =>
              {
                  var sheet = excelPackage.Workbook.Worksheets.Add(L("ExternalFindingErrorSheet"));
                  sheet.OutLineApplyStyle = true;

                  AddHeader(
                      sheet,
                      ("Control Ref."),
                      ("Root Cause"),
                      ("Corrective Action Plan"),
                      ("Accepted/Rejected"),
                      ("Expected Closed Date"),
                      ("Status"),
                      ("Message"),
                      ("Row No")
                      );

                  AddObjects(
                      sheet, 2, externalCapaDto,
                      _ => _.ControlRequirementId,
                      _ => _.RootCause,
                      _ => _.CorrectiveAction,
                      _ => _.IsAccepted,
                      _ => _.ExpectedClosedDate,
                      _ => _.Status,
                      _ => _.Message,
                      _ => _.RowNo
                      );


                  for (var i = 0; i < 8; i++)
                  {
                      sheet.Cells.AutoFitColumns(i);
                  }

              });
        }
    }
}
