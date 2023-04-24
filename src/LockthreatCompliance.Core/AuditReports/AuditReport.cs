using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using LockthreatCompliance.Extensions;
using LockthreatCompliance.AuditProjects;
using Microsoft.AspNetCore.Components.Server.Circuits;
using LockthreatCompliance.Authorization.Users;

namespace LockthreatCompliance.AuditReports
{
  public  class AuditReport : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [NotMapped]
        public virtual string Code { get { return "ARP-" + Id.GetCodeEnding(); } }
        public long AuditProjectId { get; set; }
        public AuditProject AuditProject { get; set; }
        public virtual int? NumberofAuditors { get; set; }
        public virtual string OnsiteRemote { get; set; }
        public virtual string Desktop { get; set; }
        public long? LeadAuditorId { get; set; }
        public User LeadAuditor  { get; set; }        
        public  string AuditConclusions { get; set; }
        public string ClosureFinding { get; set; }
        public string AreaImprovement  { get; set; }
        public string Recommendation { get; set; }

        public string Performance1 { get; set; }
        public string Performance2 { get; set; }



    }
}
