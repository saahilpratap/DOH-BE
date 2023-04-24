using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.Questions.Dtos
{
    public class GetQuestionForEditOutput
    {
		public CreateOrEditQuestionDto Question { get; set; }
    }

    public class GetEditExternalQuestionForEditOutput
    {
        public CreateOrEditExternalAssessmentQuestionDto Question { get; set; }
    }
}