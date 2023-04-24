using Abp.Domain.Entities;
using LockthreatCompliance.Incidents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.FindingReports
{
    [Table("FindingReportRelatedIncidents")]
    public  class FindingReportRelatedIncident : Entity
    {
        public int? FindingReportId { get; set; }

        public FindingReport FindingReport { get; set; }

        public int IncidentId { get; set; }

        public Incident Incident { get; set; }
    }
}
