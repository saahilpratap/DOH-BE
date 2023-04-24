using Abp.Dependency;
using LockthreatCompliance.Assessments.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Assessments.Importing
{
  public interface ISelfAssessmentResponseListExcelDataReader :  ITransientDependency
    {
        List<ImportSelfAssessmentDto> GetAssessmentResponseFromExcel(byte[] fileBytes, int? tenantId);
    }
}
