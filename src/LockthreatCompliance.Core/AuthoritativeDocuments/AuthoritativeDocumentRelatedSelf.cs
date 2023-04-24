using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuthoritativeDocuments
{
public  class AuthoritativeDocumentRelatedSelf : FullAuditedEntity
    {
        public int? AuthoritativeDocumentId { get; set; }
        public AuthoritativeDocument AuthoritativeDocument { get; set; }

        public int? RelatedADId  { get; set; }
    }
}
