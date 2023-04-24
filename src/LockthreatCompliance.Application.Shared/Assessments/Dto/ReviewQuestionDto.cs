using LockthreatCompliance.Questions;
using LockthreatCompliance.Questions.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Assessments.Dto
{
    public class ReviewQuestionDto
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }

        public string QuestionDescription { get; set; }

        public AnswerType AnswerType { get; set; }

        public string Comment { get; set; }

        public List<AttachmentDto> Attachments { get; set; }
        public List<AnswerOptionDto> AnswerOptions { get; set; }

        public int? SelectedAnswerOptionId { get; set; }
    }
}
