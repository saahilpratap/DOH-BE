using System.Collections.Generic;
using LockthreatCompliance.AuthoritativeDocuments.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.AuthoritativeDocuments.Exporting
{
    public interface IAuthoritativeDocumentsExcelExporter
    {
        FileDto ExportToFile(List<GetAuthoritativeDocumentForViewDto> authoritativeDocuments);
    }
}