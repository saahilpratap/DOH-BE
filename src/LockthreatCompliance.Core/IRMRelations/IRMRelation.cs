using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.BusinessRisks;
using LockthreatCompliance.FindingReports;
using LockthreatCompliance.Incidents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LockthreatCompliance.IRMRelations
{
    public class IRMRelation : FullAuditedEntity<long>, IMayHaveTenant
    {
        public IRMRelation()
        {
            Actors = new List<IRMUserRelation>();
        }

        public int? TenantId { get; set; }
        [MaxLength(500)]
        public string ReviewComments { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? NextReviewDate { get; set; }
        [MaxLength(9999)]
        public string Signature { get; set; }
        public int? IncidentId { get; set; }
        public Incident Incident { get; set; }
        public int? ExceptionId { get; set; }
        public LockthreatCompliance.Exceptions.Exception Exception { get; set; }
        public int? BusinessRiskId { get; set; }
        public BusinessRisk BusinessRisk { get; set; }
        public int? FindingReportId { get; set; }
        public FindingReport FindingReport { get; set; }

        public List<IRMUserRelation> Actors { get; set; }

        public IRMUserType IRMUserType { get; set; }

        public IEnumerable<IRMUserRelation> GetUserRelations()
        {
            return Actors;
        }

        public void AddUserRelations(List<IRMUserRelation> UserRelations)
        {
            Actors.AddRange(UserRelations);
        }

        public bool CheckEntityReviewerAdded(long id)
        {
            return Actors.Any(a => a.EntityReviewerId == id);
        }

        public bool CheckEntityApproverAdded(long id)
        {
            return Actors.Any(a => a.EntityApproverId == id);
        }

        public bool CheckAuthorityReviewerAdded(long id)
        {
            return Actors.Any(a => a.AuthorityReviewerId == id);
        }

        public bool CheckAuthorityApproverAdded(long id)
        {
            return Actors.Any(a => a.AuthorityApproverId == id);
        }
    }
}
