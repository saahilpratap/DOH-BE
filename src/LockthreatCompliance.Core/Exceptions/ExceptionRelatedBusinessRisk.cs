using Abp.Domain.Entities;
using LockthreatCompliance.BusinessRisks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.Exceptions
{
    [Table("ExceptionRelatedBusinessRisks")]
    public class ExceptionRelatedBusinessRisk : Entity
    {
        public int ExceptionId { get; set; }

        public Exception Exception { get; set; }

        public int BusinessRiskId { get; set; }

        public BusinessRisk BusinessRisk { get; set; }
    }
}
