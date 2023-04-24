using Abp.Runtime.Validation;
using LockthreatCompliance.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.QuestionGroups.Dtos
{
    public class GetAllQuestionGroup : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id,QuestionnaireTitle,QuestionnaireType,GroupType";
            }

            Filter = Filter?.Trim();
        }

    }
}
