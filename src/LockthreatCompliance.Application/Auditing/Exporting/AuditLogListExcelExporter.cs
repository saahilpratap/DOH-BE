using System.Collections.Generic;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.Auditing.Dto;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.DataExporting.Excel.NPOI;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Storage;

namespace LockthreatCompliance.Auditing.Exporting
{
    public class AuditLogListExcelExporter : EpPlusExcelExporterBase, IAuditLogListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;
        
        public AuditLogListExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportAcceptanceLogToFile(List<AgreementAcceptanceDto> agreementAcceptanceDto)
        {
            return CreateExcelPackage(
                "AuditLogs.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("AuditLogs"));

                    AddHeader(
                        sheet,
                        L("Date"),
                        L("AssessmentName"),                      
                        L("EntityId"),
                        L("EntityName"),
                        L("HasAccepted"),
                        L("Username")
                    );

                    AddObjects(
                        sheet, 2, agreementAcceptanceDto,
                        _ => _.Date == null ? "" : _.Date.ToString("yyyy-mm-dd hh:mm:ss"),
                        _ => _.AssessmentName,
                        _ => _.Signature,
                        _ => _.EntityId,
                        _ => _.EntityName,
                        _ => _.HasAccepted,
                        _ => _.Username
                        );
                    

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.Cells.AutoFitColumns(i);
                    }
                });
        }

        public FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos)
        {
            return CreateExcelPackage(
                "AuditLogs.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("AuditLogs"));

                    AddHeader(
                        sheet,
                        L("Time"),
                        L("UserName"),
                        L("Service"),
                        L("Action"),
                        L("Parameters"),
                        L("Duration"),
                        L("IpAddress"),
                        L("Client"),
                        L("Browser"),
                        L("ErrorState")
                    );

                    AddObjects(
                        sheet, 2, auditLogListDtos,
                        _ => _.ExecutionTime == null ? "" : _.ExecutionTime.ToString("yyyy-MM-dd hh:mm:ss"),
                        _ => _.UserName,
                        _ => _.ServiceName,
                        _ => _.MethodName,
                        _ => _.Parameters,
                        _ => _.ExecutionDuration,
                        _ => _.ClientIpAddress,
                        _ => _.ClientName,
                        _ => _.BrowserInfo,
                        _ => _.Exception.IsNullOrEmpty() ? L("Success") : _.Exception
                        );
                   

                    for (var i = 0; i < 11; i++)
                    {
                        sheet.Cells.AutoFitColumns(i);
                    }
                });
        }

        public FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos)
        {
            return CreateExcelPackage(
                "DetailedLogs.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("DetailedLogs"));

                    AddHeader(
                        sheet,
                        L("Action"),
                        L("Object"),
                        L("UserName"),
                        L("Time")
                    );

                    AddObjects(
                        sheet, 2, entityChangeListDtos,
                        _ => _.ChangeType.ToString(),
                        _ => _.EntityTypeFullName,
                        _ => _.UserName,
                        _ => _.ChangeTime == null ? "" : _.ChangeTime.ToString("yyyy-mm-dd hh:mm:ss")
                    );

                    for (var i = 0; i < 5; i++)
                    {
                        sheet.Cells.AutoFitColumns(i);
                    }
                });
        }
    }
}