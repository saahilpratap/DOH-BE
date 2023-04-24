using Abp.Domain.Entities.Auditing;
using Abp.DynamicEntityParameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuthoritativeDocuments
{
 public  class AuthoritativeDocumentAuditType : FullAuditedEntity
    {
        public int? AuthoritativeDocumentId  { get; set; }
        public AuthoritativeDocument AuthoritativeDocument { get; set; }
        public int? AuditTypeId  { get; set; }
        public DynamicParameterValue AuditType { get; set; }

    }
}
