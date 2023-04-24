
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using LockthreatCompliance.ControlRequirements.Dtos;

namespace LockthreatCompliance.Questions.Dtos
{
    public class CreateOrEditQuestionDto : EntityDto<int?>
    {

        public string Code { get; set; }


        public string Name { get; set; }


        public string Description { get; set; }

        public AnswerType AnswerType { get; set; }
        public bool Mandatory { get; set; }

        public List<AnswerOptionDto> AnswerOptions { get; set; }

        public List<int> removedAnswers { get; set; }

    }
}