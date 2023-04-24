using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Questions.Dtos
{
    public class AnswerOptionDto
    { 
        public int Id { get; set; }
        public string Value { get; set; }

        public double Score { get; set; }
    }

    public class ExternalAssessmentAnswerOptionDto
    { 
        public int Id { get; set; }
        public string Value { get; set; }

        public double Score { get; set; }
    }
}
