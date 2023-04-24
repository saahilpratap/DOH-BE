using Abp.Runtime.Validation;
using LockthreatCompliance.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Remediations.Dto
{
    public class RemediationInputDto : PagedAndSortedInputDto, IShouldNormalize
    {
               public int AuditProjectId { get; set; }
        public string Filter { get; set; } 
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Title DESC";
            }
            Filter = Filter?.Trim();
        }
    }

}
