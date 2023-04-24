using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects.Dtos
{
  public  class AuditReportTypeDto
    {

   public virtual string AUditProjectConsolidate { get; set; }
    public virtual string Audit_Plan_1 { get; set; }
     public virtual string Audit_Plan_2 { get; set; }

    public virtual  string AuditFinding_1 { get; set; }
    public virtual string AuditFinding_2 { get; set; }

    public virtual string CAPA_1 { get; set; }

    public virtual string CAPA_2 { get; set; }

    public virtual string  CertificationProposal { get; set; }

   public virtual string Decision { get; set; }

   public virtual string Certificate { get; set; }

   public virtual string SurviellanceAuditReport { get; set; }

   public virtual string Annexure_1 { get; set; }

   public virtual string Annexure_2 { get; set; }

   public virtual string Annexure_3 { get; set; }
   public virtual string Annexure_4 { get; set; }

   public virtual string Recertification { get; set; }

   public virtual string Stage_1_And_Stage_2_Finding  { get; set; }

   public virtual string Capa_1_And_Capa_2 { get; set; }


    }
}
