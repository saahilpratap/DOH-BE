using Abp.Dependency;
using LockthreatCompliance.Contacts.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Contacts.Importing
{
  public  interface IContactListExcelDataReader : ITransientDependency
    {
        List<ImportContactDto> GetContactFromExcel(byte[] fileBytes, int? tenantId);
    }
}
