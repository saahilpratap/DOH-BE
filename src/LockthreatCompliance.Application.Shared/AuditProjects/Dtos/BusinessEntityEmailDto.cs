using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects.Dtos
{
   public class BusinessEntityEmailDto
    {        
        public string Business_Entity_Admin_Email { get; set; }
        public string Audit_Agency_Admin_Email { get; set; }
        public string Owner_Email { get; set; }
        public string Director_Incharge_Email { get; set; }
        public string CISO_Email { get; set; }
        public string Primary_Contact_Email { get; set; }
        public string Secondary_Contact_Email  { get; set; }
        public string LeadAuditor_Email { get; set; }
        public string Group_Admin { get; set; }

    }
}
