using LockthreatCompliance.BusinessTypes;
using LockthreatCompliance.FacilityTypes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using LockthreatCompliance.Countries;
using System.Collections.Generic;
using Abp.Organizations;
using Abp.Events.Bus;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.Extensions;
using LockthreatCompliance.Enums;
using LockthreatCompliance.Authorization.Users;

using System.Linq;
using LockthreatCompliance.BusinessEntities.Events;
using Abp.DynamicEntityParameters;

namespace LockthreatCompliance.BusinessEntities
{

    [Table("BusinessEntities")]
    public class BusinessEntity : Entity, IMayHaveTenant, IFullAudited, IMayHaveOrganizationUnit, ISoftDelete
    {
        public BusinessEntity()
        {
            Assessments = new List<Assessment>();
            Actors = new List<BusinessEntityWorkFlowActor>();
            Users = new List<User>();
            Status = EntityTypeStatus.Active;
            HasAdminGenerated = false;
            IsOrphan = false;
        }
        public int? TenantId { get; set; }
       
        [NotMapped]
        public virtual string Code { get { return "ENT-" + Id.GetCodeEnding(); } }
        [Required]
        public string CompanyName { get; set; }

        public string CompanyLegalName { get; set; }
        
        public int? FacilityTypeId { get; set; }

        public FacilityType FacilityType { get; set; }

        public int? FacilityTypeSize { get; set; }

        public string CompanyWebsite { get; set; }

        public string PublicIPAddress { get; set; }

        public string Suppliers { get; set; }

        public string VIPUsers { get; set; }

        public string MobileAppName { get; set; }

        public string Logo { get; set; }
        public int NumberOfYearsInBusiness { get; set; }

        public bool IsGovernmentOwned { get; set; }

        public bool IsCompanyLicensed { get; set; }

        public string LicenseNumber { get; set; }

        public string CompanyAddress { get; set; }

        public int CountryId { get; set; }

        public Country Country { get; set; }

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

        public EntityType EntityType { get; set; }

        [Required]
        public string AdminEmail { get; set; }

        public bool IsSuspended { get; set; }

        public bool IsPreAssessmentQuestionaire { get; set; }

        public int? TotalPersonnel { get; set; }
        public int? NumberEmpWork { get; set; }
        public int? ITSecurityStaff { get; set; }
        public int? ContractPersonnel { get; set; }

        public ControlType ComplianceType { get; set; }
        public EntityTypeStatus Status { get; set; }
        public List<Assessment> Assessments { get; set; }
        public bool IsOrphan { get; set; }
        
        public ICollection<BusinessEntityWorkFlowActor> Actors { get; set; }

        public IEnumerable<BusinessEntityWorkFlowActor> GetReviewers()
        {
            return Actors.Where(e => e.Type == BusinessEntityWorkflowActorType.Reviewer);
        }

        public IEnumerable<BusinessEntityWorkFlowActor> GetApprovers()
        {
            return Actors.Where(e => e.Type == BusinessEntityWorkflowActorType.Approver);
        }

        public IEnumerable<BusinessEntityWorkFlowActor> GetNotifiers()
        {
            return Actors.Where(e => e.Type == BusinessEntityWorkflowActorType.Notifier);
        }

        public IEnumerable<BusinessEntityWorkFlowActor> GetAuthoritys()
        {
            return Actors.Where(e => e.Type == BusinessEntityWorkflowActorType.Authority);
        }

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

        public string GroupName { get; set; }
        public List<User> Users { get; set; }

        public int? ThirdPartyId { get; set; }
        public DynamicParameterValue ThirdParty { get; set; }


        public int? ConnectivityId  { get; set; }
        public DynamicParameterValue Connectivity { get; set; }

        public bool ScannerConnectivity { get; set; }

        public string Facility_EN { get; set; }

        public bool IsPublic { get; set; }

        public int? DistrictId { get; set; }
        public DynamicParameterValue District { get; set; }

        public int? FacilitySubTypeId { get; set; }
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

        public int? CategoryId { get; set; }
        public DynamicParameterValue Category { get; set; }
        public string SecurityAdvisoryEmailList { get; set; }

        public void Deactivate()
        {
            if (Status == EntityTypeStatus.Active)
            {
                Status = EntityTypeStatus.InActive;
                IsActive = false;
                EventBus.Default.Trigger(new BusinessEntityDeactivatedEvent
                {
                    BusinessEntity = this
                });
            }
        }

        public void Activate()
        {
            if (Status != EntityTypeStatus.Active)
            {
                Status = EntityTypeStatus.Active;
                IsActive = true;
                EventBus.Default.Trigger(new BusinessEntityActivatedEvent
                {
                    BusinessEntity = this
                });
            }
        }
    }
}