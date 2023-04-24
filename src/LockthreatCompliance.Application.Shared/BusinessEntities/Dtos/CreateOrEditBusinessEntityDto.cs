
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.Enums;
using System.Collections.Generic;
using LockthreatCompliance.DynamicEntityParameters.Dto;

namespace LockthreatCompliance.BusinessEntities.Dtos
{
    public class CreateOrEditBusinessEntityDto : EntityDto<int?>
    {

        [Required]
        public string Name { get; set; }

        public string Logo { get; set; }
        public string Address { get; set; }
        public int NumberOfYearsInBusiness { get; set; }
        public string WebsiteUrl { get; set; }

        public string PublicIPAddress { get; set; }

        public string Suppliers { get; set; }

        public string VIPUsers { get; set; }

        public string MobileAppName { get; set; }

        public string LegalName { get; set; }
        public bool IsGovernmentOwned { get; set; }

        public bool IsLicensed { get; set; }


        public string LicenseNumber { get; set; }

        public EntityType EntityType { get; set; }
        public bool IsAuditable { get; set; }

        public bool ParentReportingEnabled { get; set; }

        public int? ParentCompanyId { get; set; }

        public long? ParentOrganizationId { get; set; }

        public string EntityId { get; set; }

        public string CityOrDistrict { get; set; }

        public int CountryId { get; set; }

        public int? BusinessTypeId { get; set; }

        public int? FacilityTypeId { get; set; }
     

        public int? FacilityTypeSize { get; set; }

        public string PostalCode { get; set; }

        public string AdminName { get; set; }

        public string AdminSurname { get; set; }
        public string AdminPosition { get; set; }

        public string AdminMobile { get; set; }

        [Required]
        public string AdminEmail { get; set; }

        public bool IsSuspended { get; set; }

        public ControlType ComplianceType { get; set; }

        //Needed for external registration

        public string ParentCompanyName { get; set; }

        public string GroupName { get; set; }

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

        public bool IsPreAssessmentQuestionaire { get; set; }


        public List<long> ApproverIds { get; set; }

        public List<long> ReviewerIds { get; set; }

        //public List<long> NotifierIds { get; set; }

        public List<int> NotifierIds { get; set; }

        public List<long> AuthorityIds { get; set; }

        public int? AuthoritativeDocumentId { get; set; }

        public long? WorkFlowNameId { get; set; }

        public int? ThirdPartyId { get; set; }

        public List<DynamicNameValueDto> ThirdParties { get; set; }

        public int? ConnectivityId { get; set; }


        public bool ScannerConnectivity { get; set; }

        public string VerificationCode { get; set; }

        public string Facility_EN { get; set; }

        public bool IsPublic { get; set; }

        public int? DistrictId { get; set; }


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

        public long? PrimaryReviewerId { get; set; }

        public long? PrimaryApproverId { get; set; }

        public int? TotalPersonnel { get; set; }
        public int? NumberEmpWork { get; set; }
        public int? ITSecurityStaff { get; set; }
        public int? ContractPersonnel { get; set; }

        public string SecurityAdvisoryEmailList { get; set; }

    }
}