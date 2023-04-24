using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using LockthreatCompliance.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FacilitySubtypes.Dto
{
  public  class FacilitySubTypeinputDto : PagedAndSortedInputDto, IShouldNormalize
    {

        public string Filter { get; set; }

        public int? FacilityTypeId { get; set; }

        public int? ControlTypeId { get; set; }
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
