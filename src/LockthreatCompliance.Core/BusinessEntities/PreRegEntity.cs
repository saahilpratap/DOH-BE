using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.Countries;
using LockthreatCompliance.Enums;
using LockthreatCompliance.FacilityTypes;
using LockthreatCompliance.PreregistrationEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities
{
    public class PreRegEntity : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public string Name { get; set; }

        public string CompanyName { get; set; }

        public string AdminEmail { get; set; }

        public string AdminMobile { get; set; }

        public string VerificationCode { get; set; }

        public bool IsVerificationDone { get; set; }
        public bool IsRequestApproved { get; set; }

        public EntityType EntityType { get; set; }

        public ControlType ControlType { get; set; }

        public int? ThirdPartyId { get; set; }
        public DynamicParameterValue ThirdParty { get; set; }

        public string LicenseNumber { get; set; }

        public string Facility_EN { get; set; }

        public bool IsPublic { get; set; }

        public int? DistrictId { get; set; }
        public DynamicParameterValue District { get; set; }

        public string FacilityTypeName { get; set; }

        public FacilityType FacilityType { get; set; }

        public string FacilitySubTypeName { get; set; }
        public FacilitySubType FacilitySubType { get; set; }

        public string HFLName { get; set; }

        public bool IsActive { get; set; }

        public string Facility_Email { get; set; }

        public string Owner_EN { get; set; }

        public string Owner_Email { get; set; }

        public string Owner_Mobile { get; set; }

        public string Director_Incharge_EN { get; set; }

        public string Director_Incharge_Email { get; set; }
        public string Director_Incharge_Mobile { get; set; }

        public string Pro_EN { get; set; }
        public string Pro_Email { get; set; }
        public string Pro_Mobile { get; set; }

        public string PrimaryContactName { get; set; }
        public string Designation { get; set; }
        public string ContactNumber { get; set; }
        public string OfficialEmail { get; set; }

        public string BackupContactName { get; set; }
        public string BackupDesignation { get; set; }
        public string BackupContactNumber { get; set; }
        public string BackupOfficialEmail { get; set; }
        public string AdminName { get; set; }
        public string AdminSurname { get; set; }
        public PreregistrationStatus Status { get; set; }
        public int? CountryId { get; set; }
        public Country Country { get; set; }
        public string CityOrDisctrict { get; set; }
        public string Group { get; set; }
        public DateTime? IssueDate { get; set; }
        public string INPATIENT_BED_CAPACITY { get; set; }
        public string Region { get; set; }
        public bool Newentity { get; set; }
        public bool IsUpdateEntity { get; set; }

    }
}
