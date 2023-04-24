using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.AuditVendors.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Storage;

namespace LockthreatCompliance.AuditVendors.Exporting
{
    public class AuditVendorsExcelExporter : EpPlusExcelExporterBase, IAuditVendorsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AuditVendorsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAuditVendorForViewDto> auditVendors)
        {
            return CreateExcelPackage(
                "AuditVendors.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("AuditVendors"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("RegistrationDate"),
                        L("Phone"),
                        L("Email"),
                        L("Website"),
                        L("Address"),
                        L("City"),
                        L("State"),
                        L("PostalCode"),
                        L("Country"),
                        L("Description")
                        );

                    AddObjects(
                        sheet, 2, auditVendors,
                        _ => _.AuditVendor.Name,
                        _ => _timeZoneConverter.Convert(_.AuditVendor.RegistrationDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.AuditVendor.Phone,
                        _ => _.AuditVendor.Email,
                        _ => _.AuditVendor.Website,
                        _ => _.AuditVendor.Address,
                        _ => _.AuditVendor.City,
                        _ => _.AuditVendor.State,
                        _ => _.AuditVendor.PostalCode,
                        _ => _.AuditVendor.Country,
                        _ => _.AuditVendor.Description
                        );

					var registrationDateColumn = sheet.Column(2);
                    registrationDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					registrationDateColumn.AutoFit();
					

                });
        }
    }
}
