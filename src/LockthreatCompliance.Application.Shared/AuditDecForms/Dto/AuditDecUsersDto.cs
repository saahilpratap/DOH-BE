using LockthreatCompliance.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditDecForms.Dto
{
  public  class AuditDecUsersDto
    {
        public int Id { get; set; }
        public int? AuditDecFormId { get; set; }
        public long? MemberNameId { get; set; }
        public  string Signature { get; set; }
        public string Name { get; set; }
        public  BusinessEntityWorkflowActorType Type { get; set; }
    }
}
