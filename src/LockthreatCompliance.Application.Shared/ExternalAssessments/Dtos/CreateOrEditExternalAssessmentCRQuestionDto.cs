using Abp.Application.Services.Dto;
using LockthreatCompliance.AuthoritativeDocuments;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.ExternalAssessments.Dtos
{
    public class CreateOrEditExternalAssessmentCRQuestionDto : EntityDto<int?>
    {

        public int ExternalAssessmentId { get; set; }

        public int ControlRequirementId { get; set; }

        public string OriginalId { get; set; }

        public int AuthoritativeDocumentId { get; set; }

        public ControlType ControlType { get; set; }


        public string Code { get; set; }
        public string ControlRequirement { get; set; }


        public int ControlStandardId { get; set; }

        public List<ExternalRequirementQuestionDto> ExternalRequirementQuestions { get; set; }

        public List<int> RemovedQuestions { get; set; }
    }
}
