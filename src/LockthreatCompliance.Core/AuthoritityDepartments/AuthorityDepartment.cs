using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Organizations;
using System.Collections.Generic;
using System.Linq;
using LockthreatCompliance.BusinessEntities;

namespace LockthreatCompliance.AuthoritityDepartments
{
    [Table("AuthorityDepartments")]
    public class AuthorityDepartment : FullAuditedEntity, IMayHaveTenant
    {

        public AuthorityDepartment()
        {
            Actors = new List<Authorityworkflowactor>();
        }
        public int? TenantId { get; set; }
        public virtual string Name { get; set; }

        public ICollection<Authorityworkflowactor> Actors { get; set; }

        public IEnumerable<Authorityworkflowactor> GetReviewers()
        {
            return Actors.Where(e => e.Type == BusinessEntityWorkflowActorType.Reviewer);
        }

        public IEnumerable<Authorityworkflowactor> GetApprovers()
        {
            return Actors.Where(e => e.Type == BusinessEntityWorkflowActorType.Approver);
        }

        public IEnumerable<Authorityworkflowactor> GetNotifiers()
        {
            return Actors.Where(e => e.Type == BusinessEntityWorkflowActorType.Notifier);
        }

        public IEnumerable<Authorityworkflowactor> GetAuthoritys ()
        {
            return Actors.Where(e => e.Type == BusinessEntityWorkflowActorType.Authority);
        }

        //public long? OrganizationUnitId { get; set; }

        //public OrganizationUnit OrganizationUnit { get; set; }

    }
}