using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.Contacts.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Storage;

namespace LockthreatCompliance.Contacts.Exporting
{
    public class ContactsExcelExporter : EpPlusExcelExporterBase, IContactsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ContactsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<ImportContactDto> contacts)
        {
            return CreateExcelPackage(
                "Contacts.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Contacts"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        ("FirstName"),
                        ("LastName"),
                        ("JobTitle"),
                        ("Mobile"),
                        ("DirectPhone"),
                        ("CompanyName"),
                        ("BusinessEntityId"),
                        ("ContactTypeId"),
                        ("ContactOwnerType"),
                        ("Email")
                        );

                    AddObjects(
                        sheet, 2, contacts,
                        _ => _.FirstName,
                        _ => _.LastName,
                        _ => _.JobTitle,
                        _ => _.Mobile,
                        _ => _.DirectPhone,
                        _ => _.CompanyName,
                        _ => _.BusinessEntityId,
                        _ => _.ContactTypeId,
                        _ => _.ContactOwnerType,
                        _ => _.Email
                        );

                    for (var i = 0; i < 13; i++)
                    {
                        sheet.Cells.AutoFitColumns(i);
                    }

                });
        }
    }
}
