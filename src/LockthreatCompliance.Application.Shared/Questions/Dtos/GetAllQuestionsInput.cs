using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using LockthreatCompliance.Dto;
using System;

namespace LockthreatCompliance.Questions.Dtos
{
    public class GetAllQuestionsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id,Name,Description";
            }

            Filter = Filter?.Trim();
        }

    } 
}