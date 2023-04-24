using Abp.Runtime.Validation;
using LockthreatCompliance.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AssessmentSchedules.Dto
{
    public class GetAllScheduleInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }

        public int ScheduleId { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "id DESC";
            }

            Filter = Filter?.Trim();
        }

    }
}
