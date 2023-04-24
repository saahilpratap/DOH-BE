using System.Collections.Generic;
using LockthreatCompliance.AuthoritityDepartments.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.AuthoritityDepartments.Exporting
{
    public interface IAuthorityDepartmentsExcelExporter
    {
        FileDto ExportToFile(List<GetAuthorityDepartmentForViewDto> authorityDepartments);
    }
}