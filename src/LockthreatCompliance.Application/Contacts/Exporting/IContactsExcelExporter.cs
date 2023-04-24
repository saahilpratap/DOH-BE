using System.Collections.Generic;
using LockthreatCompliance.Contacts.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.Contacts.Exporting
{
    public interface IContactsExcelExporter
    {
        FileDto ExportToFile(List<ImportContactDto> contacts);
    }
}