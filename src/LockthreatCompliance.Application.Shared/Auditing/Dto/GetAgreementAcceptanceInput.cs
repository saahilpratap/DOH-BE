using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.Runtime.Validation;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.Auditing.Dto
{
    public class GetAgreementAcceptanceInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public int? UserId { get; set; }

        public int? HealthCareEntityName { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "id DESC";
            }          
        }
    }

    public class GetAgreementInput
    {
        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public int? UserId { get; set; }

        public int? HealthCareEntityName { get; set; }
    }
}
