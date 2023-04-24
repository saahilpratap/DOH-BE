using Abp.Runtime.Validation;
using LockthreatCompliance.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects.Dtos
{
    public class GetAllAuditProject : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }
        public int lockType { get; set; }
        public string FilterCode { get; set; }

        public string filterTitle { get; set; }

        public string filterYear { get; set; }

        public int AuditAreaId { get; set; }

        public int AuditTypeId { get; set; }

        public long? AuditManagerId { get; set; }
        public long? AuditCoordinatorId { get; set; }

        public long? LeadAuditorId { get; set; }

        public long? LeadAuditeeId { get; set; }

        public int? CountryId { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string LicenseNumber { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "id DESC";
            }

            Filter = Filter?.Trim();
        }
    }
    public class GetAllAuditProjectInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "id DESC";
            }

            Filter = Filter?.Trim();
        }
    }

    public class GetCertificateImportByLicenseNumberInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }
        public string LicenseNumber { get; set; }
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
