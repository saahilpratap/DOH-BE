using Abp.Application.Services.Dto;
using LockthreatCompliance.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Assessments.Dto
{
    public class GetAllInputFilter : PagedAndSortedResultRequestDto
    {
        public AssessmentStatus? Status { get; set; }
        public int? BusinessEntityId { get; set; }
        public string Filter { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public long? AuditProjectId { get; set; }

    }
}
