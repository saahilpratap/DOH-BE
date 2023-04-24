using Abp.Domain.Entities;
using LockthreatCompliance.BusinessRisks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.FindingReports
{
    [Table("FindingReportRelatedBusinessRisks")]
    public class FindingReportRelatedBusinessRisk : Entity
    {
        public int? FindingReportId { get; set; }

        public FindingReport FindingReport { get; set; }

        public int BusinessRiskId { get; set; }

        public BusinessRisk BusinessRisk { get; set; }
    }
}
