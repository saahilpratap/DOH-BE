using Abp.Runtime.Validation;
using LockthreatCompliance.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.WorkFllows.Dto
{
 public class StateActionInputDto: PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }

        public long StateId { get; set; }
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
