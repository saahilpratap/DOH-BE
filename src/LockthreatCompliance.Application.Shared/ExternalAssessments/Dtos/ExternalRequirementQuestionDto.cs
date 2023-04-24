using LockthreatCompliance.Questions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.ExternalAssessments.Dtos
{
    public class ExternalRequirementQuestionDto
    {
        public int QuestionId { get; set; }

        public string QuestionDescription { get; set; }

        public AnswerType AnswerType { get; set; }
    }
}
