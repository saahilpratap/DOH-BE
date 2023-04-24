using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.AuthoritityDepartments.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Storage;

namespace LockthreatCompliance.AuthoritityDepartments.Exporting
{
    public class AuthorityDepartmentsExcelExporter : EpPlusExcelExporterBase, IAuthorityDepartmentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AuthorityDepartmentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAuthorityDepartmentForViewDto> authorityDepartments)
        {
            return CreateExcelPackage(
                "AuthorityDepartments.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("AuthorityDepartments"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name")
                        );

                    AddObjects(
                        sheet, 2, authorityDepartments,
                        _ => _.AuthorityDepartment.Name
                        );

					

                });
        }
    }
}
