using Abp.Domain.Entities;
using LockthreatCompliance.Exceptions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.FindingReports
{
    [Table("FindingReportRelatedExceptions")]
    public class FindingReportRelatedException : Entity
    {
        public int? FindingReportId { get; set; }

        public FindingReport FindingReport { get; set; }

        public int ExceptionId { get; set; }

        public Exception Exception { get; set; }
    }
}
