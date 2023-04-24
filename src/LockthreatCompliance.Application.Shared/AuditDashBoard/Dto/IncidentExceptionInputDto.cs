using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditDashBoard.Dto
{
  public  class IncidentExceptionInputDto
    {
        public DateTime?  StartDate  { get; set; }
        public DateTime? EndDate { get; set; }
        public int? BusinessEntityId { get; set; }
    }
}
