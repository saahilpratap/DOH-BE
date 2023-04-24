
using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using LockthreatCompliance.Enums;

namespace LockthreatCompliance.BusinessEntities.Dtos
{
    public class BusinessEntityDto : EntityDto
    {
        public BusinessEntityDto()
        {
            ThirdParties = new List<DynamicNameValueDto>();
           
        }

        public string Name { get; set; }
        public string Logo { get; set; }
        public string Code { get; set; }
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

        public bool IsAuditable { get; set; }


        public int BusinessTypeId { get; set; }

        public int? FacilityTypeId { get; set; }

        public int? FacilityTypeSize { get; set; }

        public string GroupName { get; set; }

        public int? ParentCompanyId { get; set; }
        public string ParentCompanyName { get; set; }
        public EntityTypeStatus Status { get; set; }

        public long OrganizationUnitId { get; set; }

        public EntityType Type { get; set; }

        public bool HasAdminGenerated { get; set; }
        public bool IsOrphan { get; set; }
        public int? ThirdPartyId { get; set; }

        public List<DynamicNameValueDto> ThirdParties { get; set; }

        public int? ConnectivityId { get; set; }
        

        public bool ScannerConnectivity { get; set; }

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

        public string CISO_EN { get; set; }

        public string CISO_Email { get; set; }

        public string CISO_Mobile { get; set; }

        public bool IsPreAssessmentQuestionaire { get; set; }

        public long? PrimaryReviewerId { get; set; }

        public long? PrimaryApproverId { get; set; }

        public int? TotalPersonnel { get; set; }
        public int? NumberEmpWork { get; set; }
        public int? ITSecurityStaff { get; set; }
        public int? ContractPersonnel { get; set; }

        public ControlType ComplianceType { get; set; }
        public string SecurityAdvisoryEmailList { get; set; }

    }

    public class BusinessEntitiesListDto: EntityDto
    {
        public string Name { get; set; }
        public long OrganizationUnitId { get; set; }

        public int? FacilitySubTypeId { get; set; }

        public int? FacilityTypeId { get; set; }

    }

    public class GroupEntityOutputDto
    {
        public bool IsGroup { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsSecondary { get; set; }
         
    }

    public class UpdateEntitiesProfileDto
    {
        public List<int> BusinessEntitiesId { get; set; }
        public CreateOrEditBusinessEntityDto CreateOrEditBusinessEntityDto { get; set; }
        
    }
}