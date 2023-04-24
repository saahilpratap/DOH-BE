using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.ControlRequirements.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Storage;
using LockthreatCompliance.AuthoritativeDocuments;
using Abp.Domain.Repositories;
using System.Linq;

namespace LockthreatCompliance.ControlRequirements.Exporting
{
    public class ControlRequirementsExcelExporter : EpPlusExcelExporterBase, IControlRequirementsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;
        private readonly IRepository<AuthoritativeDocument> _authoritativeDocumentRepository;
        public ControlRequirementsExcelExporter(
            IRepository<AuthoritativeDocument> authoritativeDocumentRepository,
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _authoritativeDocumentRepository = authoritativeDocumentRepository;
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<ImportControlRequirementDto> importControlRequirementDto)
        {
            return CreateExcelPackage(
                "ControlRequirements.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ControlRequirements"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        ("OriginalId"),
                        ("Description"),
                        ("ControlStandardName"),
                        ("DomainName"),
                        ("ControlType"),
                        ("ControlStandardId"),
                        ("AuthoritativeDocument"),
                        ("IndustryMandated"),
                         ("Isscored")
                        );

                    AddObjects(
                        sheet, 2, importControlRequirementDto,
                        _ => _.OriginalId,
                        _ => _.Description,
                        _ => _.ControlStandardName,
                        _ => _.DomainName,
                        _ => ((ControlType)_.ControlType).ToString(),
                        _ => _.ControlStandardId,
                        _ => _.AuthoritativeDocumentId.ToString() == "" ? "" : _authoritativeDocumentRepository.GetAll().Where(x=>x.Id == _.AuthoritativeDocumentId).FirstOrDefault().Name,
                        _ => _.IndustryMandated,
                        _ => _.Iscored 
                        );

                    for (var i = 0; i < 9; i++)
                    {
                        sheet.Cells.AutoFitColumns(i);
                    }

                });
        }

    }
}
