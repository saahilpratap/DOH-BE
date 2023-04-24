using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.AuthoritativeDocuments;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FacilityTypes
{
  public class FacilitySubType: FullAuditedEntity, IMayHaveTenant
    {
       public int? TenantId { get; set; }
       public int? FacilityTypeId  { get; set; }
       public FacilityType FacilityType { get; set; }
       public string FacilitySubTypeName { get; set; }
       public ControlType ControlType { get; set; }
    }
}
