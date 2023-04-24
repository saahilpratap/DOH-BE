using Abp.Runtime.Validation;
using LockthreatCompliance.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditDecForms
{
 public class AuditDecisionInputDto: PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "decision,id Desc";
            }

            Filter = Filter?.Trim();
        }
    }
}
