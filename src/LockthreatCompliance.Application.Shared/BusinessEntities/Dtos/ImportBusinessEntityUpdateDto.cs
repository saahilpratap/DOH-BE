using Abp.DynamicEntityParameters;
using LockthreatCompliance.AuthoritativeDocuments;
using System;
using System.Collections.Generic;
using System.Text;
using LockthreatCompliance.FacilityTypes;
using Abp.Organizations;

namespace LockthreatCompliance.BusinessEntities.Dtos
{
   public class ImportBusinessEntityUpdateDto
    {
        public int? TenantId { get; set; }

   
        public string CompanyName { get; set; }

        public string CompanyLegalName { get; set; }

        public int? FacilityTypeId { get; set; }

        public int? FacilityTypeSize { get; set; }

        public string CompanyWebsite { get; set; }

        public string Logo { get; set; }
        public int NumberOfYearsInBusiness { get; set; }

        public bool IsGovernmentOwned { get; set; }

        public bool IsCompanyLicensed { get; set; }

        public string LicenseNumber { get; set; }

        public string CompanyAddress { get; set; }

        public int CountryId { get; set; }

        public string CityOrDisctrict { get; set; }

        public string PostalCode { get; set; }

        public bool IsAuditableEntity { get; set; }

        public int? ParentCompanyId { get; set; }

        public bool IsParentReportingEnabled { get; set; }

        public long? ParentOrganizationId { get; set; }
        public string AdminName { get; set; }

        public string AdminSurname { get; set; }
        public string AdminPosition { get; set; }

        public string AdminMobile { get; set; }

        public string AdminEmail { get; set; }

        public bool IsSuspended { get; set; }

        public bool IsPreAssessmentQuestionaire { get; set; }

        public int? TotalPersonnel { get; set; }
        public int? NumberEmpWork { get; set; }
        public int? ITSecurityStaff { get; set; }
        public int? ContractPersonnel { get; set; }

        public ControlType ComplianceType { get; set; }
        public EntityTypeStatus Status { get; set; }

   

        //public IEnumerable<BusinessEntityWorkFlowActor> GetNotifiers()
        //{
        //    return Actors.Where(e => e.Type == BusinessEntityWorkflowActorType.Notifier);
        //}

        public OrganizationUnit OrganizationUnit { get; set; }

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
        public long? OrganizationUnitId { get; set; }
        public bool HasAdminGenerated { get; set; }
        public bool IsOrphan { get; set; }

        public string GroupName { get; set; }

        public int? ThirdPartyId { get; set; }
        public DynamicParameterValue ThirdParty { get; set; }


        public string Facility_EN { get; set; }

        public bool IsPublic { get; set; }

        public int? DistrictId { get; set; }
        public DynamicParameterValue District { get; set; }

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

        public string CISO_EN { get; set; }

        public string CISO_Email { get; set; }

        public string CISO_Mobile { get; set; }

        public string PrimaryContactName { get; set; }
        public string Designation { get; set; }
        public string ContactNumber { get; set; }
        public string OfficialEmail { get; set; }

        public string BackupContactName { get; set; }
        public string BackupDesignation { get; set; }
        public string BackupContactNumber { get; set; }
        public string BackupOfficialEmail { get; set; }

        public string RowName { get; set; }

        public string Exception { get; set; }

        public bool CanBeImported { get; set; }

        public string CountryName { get; set; }

        public string Message { get; set; }

        public bool IsLicenseValid { get; set; }

        public bool InvalidCount { get; set; }

        public string InvalidName { get; set; }

        public string FacilityTypeName { get; set; }

        public string FacilitySubTypeName { get; set; }
        public bool IsCityValid { get; set; }

    }
}
