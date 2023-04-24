using Abp.Application.Services.Dto;
using LockthreatCompliance.Questions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FeedBacks.Dtos
{
    public class CreateOrEditFeedbackQuestionDto : EntityDto<int?>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Mandatory { get; set; }
        public AnswerType AnswerType { get; set; }

        public List<FeedbackQuestionAnswerOptionDto> AnswerOptions { get; set; }
        public List<int> removedAnswers { get; set; }
    }
}
