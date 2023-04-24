using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using LockthreatCompliance.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects.Dtos
{
    public class GetAllTemplateChecklistInput : PagedAndSortedResultRequestDto , IShouldNormalize
    {
        public string Filter { get; set; }

        public int TabId  { get; set; }

        public long? AuditProjectId  { get; set; }
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
