using LockthreatCompliance.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.AuthoritativeDocuments;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.FacilityTypes;
using LockthreatCompliance.PreregistrationEntity;

namespace LockthreatCompliance.BusinessEntities.Exporting
{
    public class BusinessEntitiesExcelDto
    {
        public int? TenantId { get; set; }

        public string Name { get; set; }

        public string EntityGroupName { get; set; }

        public bool IsPrimaryEntity { get; set; }

        public int FacilityTypeSize { get; set; }

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

        public int? FacilityTypeId { get; set; }

        public int? FacilitySubTypeId { get; set; }

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
        public int NumberOfYearsInBusiness { get; set; }

        public string Exception { get; set; }

        public bool CanBeImported { get; set; }

        public string FacilityTypeName { get; set; }
        public string FacilitySubTypeName { get; set; }

        public string DistrictName { get; set; }
        public string RowName { get; set; }

        public EntityTypeStatus Status { get; set; }

        public int? CountryId { get; set; }

        public string CityOrDisctrict { get; set; }

        public bool HasParentReporting { get; set; }

        public string CityName { get; set; }

        public string CountryName { get; set; }
    }
}
