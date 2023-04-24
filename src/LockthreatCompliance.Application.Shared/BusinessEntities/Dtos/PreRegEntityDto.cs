using Abp.DynamicEntityParameters;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.FacilityTypes;
using LockthreatCompliance.PreregistrationEntity;
namespace LockthreatCompliance.BusinessEntities
{
   public class PreRegEntityDto : FullAuditedEntity
    {
       public string License_Number { get; set; }
       public string Facility_Name { get; set; }
       public string Group { get; set; }
       public string IS_PUBLIC_FACILITY { get; set; }
       public string FacilityType { get; set; }
       public string FacilitySubType { get; set; }
       public string LicenseStatus { get; set; }
       public string Issue_date { get; set; }
       public string DIRECTOR_INCHARGE_NAME { get; set; }
       public string DIRECTOR_INCHARGE_TELEPHONE { get; set; }
       public string DIRECTOR_INCHARGE_EMAIL { get; set; }
       public string OWNERNAME_EN { get; set; }
       public string OWNER_TELEPHONE { get; set; }
       public string OWNER_EMAIL { get; set; }
       public string INPATIENT_BED_CAPACITY { get; set; }
       public string PRO_NAME { get; set; }
       public string PRO_EMAIL { get; set; }
       public string PRO_TELEPHONE { get; set; }
       public string Region { get; set; }
       public string City { get; set; }
       public bool IsUpdateEntity { get; set; }
       public bool Newentity { get; set; }
    }
}
