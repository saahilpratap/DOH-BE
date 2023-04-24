using Abp.Runtime.Validation;
using LockthreatCompliance.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities.Dtos
{
    public class GetPreRegisterInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }

        public int FacilityTypeId { get; set; }

        public int FacilitySubTypeId { get; set; }

        public int DistrictId { get; set; }
        public bool? IsVerificationDone { get; set; }

        public bool? IsRequestApproved { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id,Name,CompanyName,AdminEmail";
            }

            Filter = Filter?.Trim();
        }

    }
}
