using Abp.Application.Services.Dto;
using LockthreatCompliance.ControlRequirements.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Questions.Dtos
{
    public class CreateOrEditExternalAssessmentQuestionDto : EntityDto<int?>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AnswerType AnswerType { get; set; }
        public bool Mandatory { get; set; }

        public List<ExternalAssessmentAnswerOptionDto> AnswerOptions { get; set; }
        public int ControlRequirementId { get; set; }

        public List<ControlRequirementList> ControlRequirements { get; set; }

        public List<int> removedAnswers { get; set; }

    }
}
