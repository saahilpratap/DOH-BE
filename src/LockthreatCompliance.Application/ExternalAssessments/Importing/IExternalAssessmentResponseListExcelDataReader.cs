using Abp.Dependency;
using LockthreatCompliance.Assessments.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.ExternalAssessments.Importing
{
   public interface IExternalAssessmentResponseListExcelDataReader : ITransientDependency
    {
        List<ImportSelfAssessmentDto> GetAssessmentResponseFromExcel(byte[] fileBytes, int? tenantId);
    }
}
