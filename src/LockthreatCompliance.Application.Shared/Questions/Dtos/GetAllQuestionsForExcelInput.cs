using Abp.Application.Services.Dto;
using System;

namespace LockthreatCompliance.Questions.Dtos
{
    public class GetAllQuestionsForExcelInput
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string NameFilter { get; set; }

		public string DescriptionFilter { get; set; }



    }
}