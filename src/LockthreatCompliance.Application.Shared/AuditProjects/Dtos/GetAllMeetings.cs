using Abp.Runtime.Validation;
using LockthreatCompliance.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects.Dtos
{
    public class GetAllMeetings : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }
        public int AuditVendorId { get; set; }
        public int AuditOrgId { get; set; }
        public int AuditProjectId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "MeetingTitle,StartDate,EndDate";
            }

            Filter = Filter?.Trim();
        }
    }
}
