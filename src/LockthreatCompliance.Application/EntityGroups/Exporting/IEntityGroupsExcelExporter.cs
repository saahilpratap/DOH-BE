using System.Collections.Generic;
using LockthreatCompliance.EntityGroups.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.EntityGroups.Exporting
{
    public interface IEntityGroupsExcelExporter
    {
        FileDto ExportToFile(List<GetEntityGroupForViewDto> entityGroups);
    }
}