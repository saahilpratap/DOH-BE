using Abp.Dependency;
using LockthreatCompliance.ControlRequirements.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.ControlRequirements.Importing
{
   public interface IControlRequirementsListExcelDataReader :  ITransientDependency
    {
        List<ImportControlRequirementDto> GetControlRequirementFromExcel(byte[] fileBytes, int? tenantId);
    }
}
