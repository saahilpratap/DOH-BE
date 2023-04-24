using Abp.Application.Services.Dto;
using LockthreatCompliance.ControlRequirements.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Questions.Dtos
{
    public class ExternalQuestionDto : EntityDto
    {
        public int TenantId { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string AnswerType { get; set; }
        public bool Mandatory { get; set; }

        public string AnswerOptionsWithScores { get; set; }
    }
}
