using LockthreatCompliance.BusinessTypes;
using LockthreatCompliance.FacilityTypes;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LockthreatCompliance.BusinessEntities.Exporting;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.Dto;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using LockthreatCompliance.EntityFrameworkCore.RepositoryExtensions;
using Abp.Organizations;
using Abp.Events.Bus;
using LockthreatCompliance.BusinessEntities.Events;
using LockthreatCompliance.CustomExceptions;
using Abp.Domain.Uow;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.Common;
using LockthreatCompliance.Enums;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.Sessions;
using LockthreatCompliance.Url;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using Abp.DynamicEntityParameters;
using Abp.UI;
using LockthreatCompliance.EntityGroups;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.Contacts;
using LockthreatCompliance.ExternalAssessments;
using Twilio.Clients;
using LockthreatCompliance.BusinessRisks;
using LockthreatCompliance.Incidents;
using LockthreatCompliance.FindingReports;
using LockthreatCompliance.EntityGroups.Dtos;
using Abp.Authorization.Users;
using NPOI.SS.Formula.Functions;
using PayPalCheckoutSdk.Orders;
using System.Runtime.InteropServices.WindowsRuntime;
using LockthreatCompliance.Countries;
using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.PreregistrationEntity;
using LockthreatCompliance.WrokFlows;
using LockthreatCompliance.AuthoritityDepartments;
using LockthreatCompliance.Authorization.Users.Dto;
using Abp.Runtime.Security;
using System.Text.RegularExpressions;
using System.Text;
using Abp.Collections.Extensions;

namespace LockthreatCompliance.BusinessEntities
{
    [AbpAuthorize]
    public class BusinessEntitiesAppService : LockthreatComplianceAppServiceBase, IBusinessEntitiesAppService
    {
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IBusinessEntitiesExcelExporter _businessEntitiesExcelExporter;
        private readonly IRepository<User, long> _userRepository;
        private readonly ICommonLookupAppService _commonlookupManagerRepository;
        private readonly IRepository<FacilityType, int> _lookup_facilityTypeRepository;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IEntityUserCreator _entityUserCreator;
        private readonly ApplicationSession _applicationSession;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<PreRegisterBusinessEntity> _preRegisterEntityRepository;
        private readonly IUserEmailer _userEmailer;
        private readonly IRepository<DynamicParameterValue> _dynamicParameterValueRepository;
        private readonly IRepository<DynamicParameter> _dynamicParameterManager;
        private readonly RoleManager _roleManager;
        private readonly IEntityApplicationSettingAppService _ientityApplicationSettingAppService;
        private readonly IRepository<EntityGroup> _entityGrpMemberRepository;
        private readonly IRepository<AuditProject, long> _auditProjectRepository;
        private readonly IRepository<EntityGroupMember> _entityGroupMemberRepository;
        private readonly IRepository<EntityApplicationSetting> _settingRepository;
        private readonly IRepository<Contact> _contactRepository;
        private readonly IRepository<BusinessEntityWorkFlowActor> _businessEntityWorkFlowActorRepository;
        private readonly IRepository<Assessment> _assessmentRepository;
        private readonly IRepository<ExternalAssessment> _extAssessmentRepository;
        private readonly IRepository<ReviewData> _reviewDataRepository;
        private readonly IRepository<EntityGroupMember> _entityGroupMembersRepository;
        private readonly IRepository<BusinessRisk> _businessRisksRepository;
        private readonly IRepository<Exceptions.Exception> _exceptionRepository;
        private readonly IRepository<Incident> _incidentRepository;
        private readonly IRepository<FindingReport> _findingReportRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<Country> _countriesRepository;
        private const string defaultPassword = "123qwe";
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepository<WorkFlowPage, long> _workflowpageRepository;
        private readonly IRepository<Authorityworkflowactor> _authorityworkflowRepository;
        private readonly IRepository<BusinessEntityUser> _businessEntityUserRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;

        private readonly IRepository<Template, long> _templateRepository;
        public IAppUrlService AppUrlService { get; set; }
        public BusinessEntitiesAppService(IRepository<Country> countriesRepository,
            IRepository<Authorityworkflowactor> authorityworkflowRepository,
            IRepository<WorkFlowPage, long> workflowpageRepository,
            IRepository<Role> roleRepository, IPasswordHasher<User> passwordHasher,
            IRepository<ExternalAssessment> extAssessmentRepository,
            IRepository<EntityGroupMember> entityGroupMembersRepository,
            IRepository<User, long> userRepository,
            IRepository<Assessment> assessmentRepository,
            IRepository<BusinessEntityWorkFlowActor> businessEntityWorkFlowActorRepository,
            ICommonLookupAppService commonlookupManagerRepository,
            IRepository<BusinessEntity> businessEntityRepository,
            IBusinessEntitiesExcelExporter businessEntitiesExcelExporter,

            IRepository<FacilityType, int> lookup_facilityTypeRepository,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            OrganizationUnitManager organizationUnitManager,
            IEntityUserCreator entityUserCreator,
            ApplicationSession applicationSession,
            IRepository<Template, long> templateRepository,
            IRepository<AuditProject, long> auditProjectRepository,
            IUnitOfWorkManager unitOfWorkManager, IRepository<PreRegisterBusinessEntity> preRegisterEntityRepository,
            IUserEmailer userEmailer, IRepository<DynamicParameterValue> dynamicParameterValueRepository,
          IRepository<DynamicParameter> dynamicParameterManager, RoleManager roleManager,
          IRepository<Contact> contactRepository,
          IRepository<EntityGroupMember> entityGroupMemberRepository, IRepository<EntityApplicationSetting> settingRepository,
          IEntityApplicationSettingAppService ientityApplicationSettingAppService, IRepository<EntityGroup> entityGrpMemberRepository,
           IRepository<ReviewData> reviewDataRepository,
           IRepository<Exceptions.Exception> exceptionRepository,
           IRepository<Incident> incidentRepository,
           IRepository<BusinessRisk> businessRisksRepository,
           IRepository<FindingReport> findingReportRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<BusinessEntityUser> businessEntityUserRepository,
            IRepository<UserRole, long> userRoleRepository
           )
        {
            _templateRepository = templateRepository;
            _authorityworkflowRepository = authorityworkflowRepository;
            _workflowpageRepository = workflowpageRepository;
            _passwordHasher = passwordHasher;
            _countriesRepository = countriesRepository;
            _roleRepository = roleRepository;
            _findingReportRepository = findingReportRepository;
            _businessRisksRepository = businessRisksRepository;
            _incidentRepository = incidentRepository;
            _exceptionRepository = exceptionRepository;
            _reviewDataRepository = reviewDataRepository;

            _entityGroupMembersRepository = entityGroupMembersRepository;
            _extAssessmentRepository = extAssessmentRepository;
            _assessmentRepository = assessmentRepository;
            _businessEntityWorkFlowActorRepository = businessEntityWorkFlowActorRepository;
            _contactRepository = contactRepository;
            _commonlookupManagerRepository = commonlookupManagerRepository;
            _entityGroupMemberRepository = entityGroupMemberRepository;
            _userRepository = userRepository;
            _applicationSession = applicationSession;
            _entityUserCreator = entityUserCreator;
            _businessEntityRepository = businessEntityRepository;
            _businessEntitiesExcelExporter = businessEntitiesExcelExporter;
            // _lookup_businessTypeRepository = lookup_businessTypeRepository;
            _lookup_facilityTypeRepository = lookup_facilityTypeRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _organizationUnitManager = organizationUnitManager;
            _unitOfWorkManager = unitOfWorkManager;
            _settingRepository = settingRepository;
            _preRegisterEntityRepository = preRegisterEntityRepository;
            _userEmailer = userEmailer;
            _dynamicParameterValueRepository = dynamicParameterValueRepository;
            _dynamicParameterManager = dynamicParameterManager;
            _roleManager = roleManager;
            _ientityApplicationSettingAppService = ientityApplicationSettingAppService;
            _entityGrpMemberRepository = entityGrpMemberRepository;
            _auditProjectRepository = auditProjectRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _businessEntityUserRepository = businessEntityUserRepository;
            _userRoleRepository = userRoleRepository;
        }

        #region CRUD
        [AbpAuthorize(AppPermissions.Pages_HealthCareEntities_All, AppPermissions.Pages_AuditManagement_Entities_All)]
        public async Task<PagedResultDto<GetBusinessEntitiesExcelDto>> GetAll(GetAllBusinessEntitiesInput input)
        {
            try
            {
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();

                var filteredBusinessEntities = _businessEntityRepository.GetAll()
                 .Include(e => e.FacilityType)
                 .Include(e => e.Country).Where(e => e.IsDeleted == input.ShowOnlyDeleted && e.EntityType == input.EntityType)
                 .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.Id))
                 .WhereIf(input.Status != null, e => e.Status == input.Status)
                 .WhereIf(input.IsLicensedFilter != null, e => e.IsCompanyLicensed == input.IsLicensedFilter)
                 .WhereIf(input.IsAuditableFilter != null, e => e.IsAuditableEntity == input.IsAuditableFilter)
                 .WhereIf(input.IsGovernmentOwnedFilter != null, e => e.IsGovernmentOwned == input.IsGovernmentOwnedFilter)
                 .WhereIf(input.MinNumberOfYearsInBusinessFilter != null, e => e.NumberOfYearsInBusiness <= input.MinNumberOfYearsInBusinessFilter)
                 .WhereIf(input.MaxNumberOfYearsInBusinessFilter != null, e => e.NumberOfYearsInBusiness >= input.MaxNumberOfYearsInBusinessFilter)
                 .WhereIf(input.FacilityTypeFilter > 0, e => e.FacilityTypeId == input.FacilityTypeFilter)
                 .WhereIf(input.FacilitySubTypeFilter > 0, e => e.FacilitySubTypeId == input.FacilitySubTypeFilter)
                 .WhereIf(!input.Filter.IsNullOrWhiteSpace(), u => u.CompanyName.Contains(input.Filter.Trim().ToLower()) ||
                       u.LicenseNumber.Contains(input.Filter.Trim().ToLower()) || u.AdminEmail.Contains(input.Filter.Trim().ToLower()) || u.CompanyAddress.Contains(input.Filter.Trim().ToLower()))
                  .WhereIf(!input.LegalNameFilter.IsNullOrWhiteSpace(),
                  u => u.CompanyLegalName.Contains(input.LegalNameFilter.Trim().ToLower()))
                  .WhereIf(!input.NameFilter.IsNullOrWhiteSpace(),
                  u => u.CompanyName.Contains(input.NameFilter.Trim().ToLower()))
                  .WhereIf(!input.AddressFilter.IsNullOrWhiteSpace(),
                  u => u.CompanyAddress.Contains(input.AddressFilter.Trim().ToLower()))
                  .WhereIf(!input.LicenseNumberFilter.IsNullOrWhiteSpace(),
                  u => u.GroupName.Contains(input.LicenseNumberFilter.Trim().ToLower()))
                  .WhereIf(!input.WebsiteUrlFilter.IsNullOrWhiteSpace(),
                  u => u.CompanyWebsite.Contains(input.WebsiteUrlFilter.Trim().ToLower()));


                var totalCount = await filteredBusinessEntities.CountAsync();

                var pagedAndFilteredBusinessEntities = await filteredBusinessEntities
               .OrderBy(input.Sorting)
               .PageBy(input)
               .ToListAsync();

                var businessEntities = pagedAndFilteredBusinessEntities.Select(x => new GetBusinessEntitiesExcelDto
                {

                    FacilityTypeName = (x.FacilityType != null) ? x.FacilityType.Name : null,
                    BusinessEntity = new BusinessEntityDto
                    {
                        Code = x.Code,
                        Address = x.CompanyAddress,
                        Type = x.EntityType,
                        HasAdminGenerated = x.HasAdminGenerated,
                        FacilityTypeId = (x.FacilityType != null) ? x.FacilityTypeId.Value : 0,
                        Id = x.Id,
                        IsAuditable = x.IsAuditableEntity,
                        IsGovernmentOwned = x.IsGovernmentOwned,
                        IsLicensed = x.IsCompanyLicensed,
                        LegalName = x.CompanyLegalName,
                        LicenseNumber = x.LicenseNumber,
                        Name = x.CompanyLegalName,
                        NumberOfYearsInBusiness = x.NumberOfYearsInBusiness,
                        WebsiteUrl = x.CompanyWebsite,
                        Status = x.Status,
                        GroupName = x.GroupName,
                        ComplianceType = x.ComplianceType,
                        IsOrphan = x.IsOrphan,
                        ParentCompanyId = (x.ParentCompanyId != null) ? x.ParentCompanyId : 0,
                        ParentCompanyName = (x.ParentCompanyId != null) ? (_organizationUnitRepository.GetAll().Where(y => y.Id == x.ParentOrganizationId).FirstOrDefault().DisplayName) : null,
                        OrganizationUnitId = x.OrganizationUnitId == null ? 0 : (long)x.OrganizationUnitId

                    }
                });



                return new PagedResultDto<GetBusinessEntitiesExcelDto>(
                    totalCount,
                     businessEntities.ToList()
                );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Record not found.!");
            }


        }


        /*Get ALL BusinessEnitty */
        public async Task<List<BusinessEntityDto>> GetAllList()
        {
            var result = new List<BusinessEntityDto>();
            try
            {
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();

                var filteredBusinessEntities = _businessEntityRepository.GetAll()
                 .Include(e => e.FacilityType)
                 .Include(e => e.Country);



                result = filteredBusinessEntities.Select(x => new BusinessEntityDto()
                {
                    Code = x.Code,
                    Address = x.CompanyAddress,
                    Type = x.EntityType,
                    HasAdminGenerated = x.HasAdminGenerated,
                    FacilityTypeId = x.FacilityTypeId.Value,
                    Id = x.Id,
                    IsAuditable = x.IsAuditableEntity,
                    IsGovernmentOwned = x.IsGovernmentOwned,
                    IsLicensed = x.IsCompanyLicensed,
                    LegalName = x.CompanyLegalName,
                    LicenseNumber = x.LicenseNumber,
                    Name = x.CompanyLegalName,
                    NumberOfYearsInBusiness = x.NumberOfYearsInBusiness,
                    WebsiteUrl = x.CompanyWebsite,
                    Status = x.Status,
                    GroupName = x.GroupName,
                    ComplianceType = x.ComplianceType,
                    IsOrphan = x.IsOrphan,
                    ParentCompanyId = (x.ParentCompanyId != null) ? x.ParentCompanyId : 0,
                    ParentCompanyName = (x.ParentCompanyId != null) ? (_organizationUnitRepository.GetAll().Where(y => y.Id == x.ParentOrganizationId).FirstOrDefault().DisplayName) : null,
                    OrganizationUnitId = x.OrganizationUnitId == null ? 0 : (long)x.OrganizationUnitId
                }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("ReCord Not Found!");
            }
        }

        public async Task<PagedResultDto<BusinessEntityGridDto>> GetGridAll(GetAllBusinessEntitiesInput input)
        {

            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var getAssisement = await _assessmentRepository.GetAllListAsync();
                var getReview = await _reviewDataRepository.GetAll().Where(x => x.AssessmentId != null).ToListAsync();
                var getBusinessRisk = await _businessRisksRepository.GetAll().ToListAsync();
                var getException = await _exceptionRepository.GetAll().ToListAsync();
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                var getIncident = await _incidentRepository.GetAll().ToListAsync();
                var getFinding = await _findingReportRepository.GetAll().ToListAsync();
                var getExternal = await _extAssessmentRepository.GetAll().ToListAsync();

                var filteredBusinessEntities = _businessEntityRepository.GetAll()
                    .Include(e => e.FacilityType)
                    .Include(e => e.Country).Where(e => e.IsDeleted == input.ShowOnlyDeleted && e.EntityType == input.EntityType)
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0 && input.EntityType == EntityType.HealthcareEntity || input.EntityType == EntityType.InsuranceFacilities, e => getcheckUser.BusinessEntityId.Contains(e.Id))
                    .WhereIf(input.Status != null, e => e.Status == input.Status)
                    .WhereIf(input.IsLicensedFilter != null, e => e.IsCompanyLicensed == input.IsLicensedFilter)
                    .WhereIf(input.IsAuditableFilter != null, e => e.IsAuditableEntity == input.IsAuditableFilter)
                    .WhereIf(input.IsGovernmentOwnedFilter != null, e => e.IsGovernmentOwned == input.IsGovernmentOwnedFilter)
                    .WhereIf(input.MinNumberOfYearsInBusinessFilter != null, e => e.NumberOfYearsInBusiness <= input.MinNumberOfYearsInBusinessFilter ||
                    (input.MaxNumberOfYearsInBusinessFilter != null && e.NumberOfYearsInBusiness >= input.MaxNumberOfYearsInBusinessFilter))
                    .WhereIf(input.FacilityTypeFilter > 0, e => e.FacilityTypeId == input.FacilityTypeFilter)
                     .WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                     u =>
                         u.CompanyName.Contains(input.Filter) ||
                         u.LicenseNumber.Contains(input.Filter) ||
                         u.AdminEmail.Contains(input.Filter) ||
                         u.CompanyAddress.Contains(input.Filter))
                     .WhereIf(!input.LegalNameFilter.IsNullOrWhiteSpace(),
                     u => u.CompanyLegalName.Contains(input.LegalNameFilter))
                     .WhereIf(!input.NameFilter.IsNullOrWhiteSpace(),
                     u => u.CompanyName.Contains(input.NameFilter))
                     .WhereIf(!input.AddressFilter.IsNullOrWhiteSpace(),
                     u => u.CompanyAddress.Contains(input.AddressFilter))
                     .WhereIf(!input.WebsiteUrlFilter.IsNullOrWhiteSpace(),
                     u => u.CompanyWebsite.Contains(input.WebsiteUrlFilter));

                var pagedAndFilteredBusinessEntities = filteredBusinessEntities
               .OrderBy(input.Sorting ?? "id asc")
               .PageBy(input);
                var businessEntityGridDtos = new List<BusinessEntityGridDto>();
                foreach (var item in pagedAndFilteredBusinessEntities)
                {
                    int count = 0;
                    int countTotal = 0;
                    var businessEntityGridDto = new BusinessEntityGridDto();
                    businessEntityGridDto.BusinessEntity.Code = item.Code;
                    businessEntityGridDto.BusinessEntity.Address = item.CompanyAddress;
                    businessEntityGridDto.BusinessEntity.Type = item.EntityType;
                    businessEntityGridDto.BusinessEntity.HasAdminGenerated = item.HasAdminGenerated;
                    businessEntityGridDto.BusinessEntity.IsOrphan = item.IsOrphan;
                    businessEntityGridDto.BusinessEntity.FacilityTypeId = item.FacilityTypeId == null ? 0 : item.FacilityTypeId.Value;
                    businessEntityGridDto.BusinessEntity.Id = item.Id;
                    businessEntityGridDto.BusinessEntity.IsAuditable = item.IsAuditableEntity;
                    businessEntityGridDto.BusinessEntity.IsGovernmentOwned = item.IsGovernmentOwned;
                    businessEntityGridDto.BusinessEntity.IsLicensed = item.IsCompanyLicensed;
                    businessEntityGridDto.BusinessEntity.LegalName = item.CompanyLegalName;
                    businessEntityGridDto.BusinessEntity.LicenseNumber = item.LicenseNumber;
                    businessEntityGridDto.BusinessEntity.Name = item.CompanyLegalName;
                    businessEntityGridDto.BusinessEntity.NumberOfYearsInBusiness = item.NumberOfYearsInBusiness;
                    businessEntityGridDto.BusinessEntity.WebsiteUrl = item.CompanyWebsite;
                    businessEntityGridDto.BusinessEntity.Status = item.Status;
                    businessEntityGridDto.BusinessEntity.OrganizationUnitId = item.OrganizationUnitId == null ? 0 : (long)item.OrganizationUnitId;
                    businessEntityGridDto.BusinessRiskCount = getBusinessRisk.Where(x => x.BusinessEntityId == item.Id).Count();
                    businessEntityGridDto.AssessmentCount = getAssisement.Where(x => x.BusinessEntityId == item.Id).Count();
                    businessEntityGridDto.ExceptionCount = getException.Where(x => x.BusinessEntityId == item.Id).Count();
                    businessEntityGridDto.IncidentCount = getIncident.Where(x => x.BusinessEntityId == item.Id).Count();
                    businessEntityGridDto.FindingCount = getFinding.Where(x => x.BusinessEntityId == item.Id).Count();
                    businessEntityGridDto.AuditFindingCount = getExternal.Where(x => x.BusinessEntityId == item.Id).Count();
                    if (item.FacilityType != null)
                    {
                        businessEntityGridDto.FacilityTypeName = item.FacilityType.Name;
                    }
                    var item3 = getAssisement.Where(x => x.BusinessEntityId == item.Id).OrderByDescending(x => x.Date).FirstOrDefault();
                    if (item3 != null)
                    {
                        count = getReview.Where(x => x.AssessmentId == item3.Id && x.ResponseType != 0).Count();
                        countTotal = getReview.Where(x => x.AssessmentId == item3.Id).Count();
                    }
                    if (countTotal > 0 && count > 0)
                    {
                        businessEntityGridDto.ReviewDataPercent = (count * 100) / countTotal;
                    }
                    else
                    {

                        businessEntityGridDto.ReviewDataPercent = 0;
                    }


                    businessEntityGridDtos.Add(businessEntityGridDto);
                }

                var totalCount = await filteredBusinessEntities.CountAsync();

                return new PagedResultDto<BusinessEntityGridDto>(
                    totalCount,
                    await Task.FromResult(businessEntityGridDtos.ToList())
                );


            }
        }

        public async Task<EntityChartDto> GetBusinessEntityChartValue(int entityTypeID)
        {
            try
            {

                var entityChart = new EntityChartDto();
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                var filteredBusinessEntities = _businessEntityRepository.GetAll()
                    .Include(e => e.FacilityType)
                    .Include(e => e.District)
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.Id));
                if (entityTypeID == (int)EntityType.ExternalAudit)
                {
                    var allBusinessEntityCity = filteredBusinessEntities.Where(x => x.DistrictId != null && x.EntityType == EntityType.ExternalAudit).ToList();
                    var allBusinessEntityFacility = filteredBusinessEntities.Where(x => x.FacilityTypeId != null && x.EntityType == EntityType.ExternalAudit).ToList();
                    var entitytype = filteredBusinessEntities;
                    var getExternalAuditEntityTypeCount = entitytype.Where(x => x.EntityType == EntityType.ExternalAudit).Count();
                    var getDistrictCount = allBusinessEntityCity.GroupBy(item => item.District.Value.ToUpper())
                               .Select(item => new
                               {
                                   Name = item.Key,
                                   Count = item.Count()
                               })
                               .OrderByDescending(item => item.Count)
                               .ThenBy(item => item.Name)
                               .ToList();
                    foreach (var item in getDistrictCount)
                    {
                        var citychartValue = new ChartValue();
                        citychartValue.name = item.Name.ToString();
                        citychartValue.value = item.Count.ToString();
                        entityChart.CityValueChart.Add(citychartValue);
                    }
                    var getFacilityCount = allBusinessEntityFacility.GroupBy(item => item.FacilityType.Name)
                             .Select(item => new
                             {
                                 Name = item.Key,
                                 Count = item.Count()
                             })
                             .OrderByDescending(item => item.Count)
                             .ThenBy(item => item.Name)
                             .ToList();
                    foreach (var item in getFacilityCount)
                    {
                        var facilityTypechart = new ChartValue();
                        facilityTypechart.name = item.Name.ToString();
                        facilityTypechart.value = item.Count.ToString();
                        entityChart.FacilityTypeValueChart.Add(facilityTypechart);
                    }
                    entityChart.ExternalAuditCount = getExternalAuditEntityTypeCount;
                }
                else
                {
                    var allBusinessEntityCity = filteredBusinessEntities.Where(x => x.DistrictId != null && x.EntityType != EntityType.ExternalAudit).ToList();
                    var allBusinessEntityFacility = filteredBusinessEntities.Where(x => x.FacilityTypeId != null && x.EntityType != EntityType.ExternalAudit).ToList();
                    var entitytype = filteredBusinessEntities;

                    var getHealthCareEntityTypeCount = entitytype.Where(x => x.EntityType == EntityType.HealthcareEntity).Count();
                    var getInsuranceEntityCount = entitytype.Where(x => x.EntityType == EntityType.InsuranceFacilities).Count();
                    var getDistrictCount = allBusinessEntityCity.GroupBy(item => item.District.Value.ToUpper())
                               .Select(item => new
                               {
                                   Name = item.Key,
                                   Count = item.Count()
                               })
                               .OrderByDescending(item => item.Count)
                               .ThenBy(item => item.Name)
                               .ToList();
                    foreach (var item in getDistrictCount)
                    {
                        var citychartValue = new ChartValue();
                        citychartValue.name = item.Name.ToString();
                        citychartValue.value = item.Count.ToString();
                        entityChart.CityValueChart.Add(citychartValue);
                    }
                    var getFacilityCount = allBusinessEntityFacility.GroupBy(item => item.FacilityType.Name)
                             .Select(item => new
                             {
                                 Name = item.Key,
                                 Count = item.Count()
                             })
                             .OrderByDescending(item => item.Count)
                             .ThenBy(item => item.Name)
                             .ToList();
                    foreach (var item in getFacilityCount)
                    {
                        var facilityTypechart = new ChartValue();
                        facilityTypechart.name = item.Name.ToString();
                        facilityTypechart.value = item.Count.ToString();
                        entityChart.FacilityTypeValueChart.Add(facilityTypechart);
                    }
                    entityChart.HealthCareEntityCount = getHealthCareEntityTypeCount;
                    entityChart.InsuranceFacilitiesCount = getInsuranceEntityCount;
                }

                return entityChart;
            }
            catch (Exception)
            {
                throw new UserFriendlyException("ReCord not found.!");
            }
        }

        public async Task<IReadOnlyList<BusinessEntityUserDto>> GetAllBusinessUsers(List<int> input)
        {
            try
            {
                var getBusinessentityId = new List<int>();
                var checkPrimarEntity = new List<int>();
                var users = new List<BusinessEntityUserDto>();
                var usersPrimary = new List<BusinessEntityUserDto>();
                var result = new List<BusinessEntityUserDto>();

                getBusinessentityId = await _businessEntityRepository.GetAll().Where(x => input.Contains(x.Id)).Select(x => x.Id).ToListAsync();


                var primaryEntity = await _entityGroupMembersRepository.GetAll().Include(x => x.EntityGroup).Where(x => getBusinessentityId.Contains((int)x.BusinessEntityId)).Select(x => (int)x.EntityGroup.PrimaryEntityId).Distinct().ToListAsync();

                if (primaryEntity.Count > 0)
                {
                    usersPrimary = await _userRepository.GetAll().Where(x => primaryEntity.Contains((int)x.BusinessEntityId)).Include(x => x.BusinessEntity)
                            .Select(x => new BusinessEntityUserDto()
                            {
                                Id = x.Id,
                                Name = x.FullName + "-" + x.BusinessEntity.CompanyName

                            }).ToListAsync();
                }
                users = await _userRepository.GetAll().Where(x => getBusinessentityId.Contains((int)x.BusinessEntityId)).Include(x => x.BusinessEntity)
                             .Select(x => new BusinessEntityUserDto()
                             {
                                 Id = x.Id,
                                 Name = x.FullName + "-" + x.BusinessEntity.CompanyName

                             }).ToListAsync();

                if (users.Count() == 0)
                {
                    //If User Not Available individual Entity That time get GroupMemeber PrimaryEntity User.
                    checkPrimarEntity = await _entityGroupMembersRepository.GetAll().Include(x => x.EntityGroup).Where(x => getBusinessentityId.Contains((int)x.BusinessEntityId)).Select(x => (int)x.EntityGroup.PrimaryEntityId).Distinct().ToListAsync();
                    users = await _userRepository.GetAll().Where(x => checkPrimarEntity.Contains((int)x.BusinessEntityId)).Include(x => x.BusinessEntity)
                           .Select(x => new BusinessEntityUserDto()
                           {
                               Id = x.Id,
                               Name = x.FullName + "-" + x.BusinessEntity.CompanyName

                           }).ToListAsync();
                }

                if (usersPrimary.Count > 0)
                {
                    foreach (var item in usersPrimary)
                    {
                        if (!users.Where(y => y.Id == item.Id).Any())
                        {
                            users.AddRange(usersPrimary);
                        }
                    }
                }
                return users;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }

        }

        [AbpAllowAnonymous]
        public async Task<IReadOnlyList<BusinessEntityUserDto>> GetAllUsers(EntityDto input)
        {


            var businessEntity = await _businessEntityRepository.GetIncluding(e => e.Id == input.Id, "OrganizationUnit");
            var users = await UserManager.GetUsersInOrganizationUnitAsync(businessEntity.OrganizationUnit);

            var result = users.GroupBy(test => test.Id)
                   .Select(grp => grp.First())
                   .ToList();

            return result.Select(e => new BusinessEntityUserDto
            {
                Id = e.Id,
                Name = e.FullName
            }).ToList();



        }

        [AbpAllowAnonymous]
        public async Task<List<BusinessEntityUserDto>> GetAllApprovalUser(EntityDto input)
        {
            var result = new List<BusinessEntityUserDto>();
            try
            {
                var getpage = await _workflowpageRepository.GetAll().ToListAsync();
                if (getpage.Count() > 0)
                {
                    var checkpageNameId = getpage.Where(x => x.PageName.Trim().ToLower() == "global".Trim().ToLower().ToString()).FirstOrDefault();
                    if (checkpageNameId != null)
                    {
                        var query = _businessEntityWorkFlowActorRepository.GetAll().Include(x => x.User).
                               Where(x => x.BusinessEntityId == input.Id && x.UserId != null && x.WorkFlowNameId == checkpageNameId.Id && (x.Type == BusinessEntityWorkflowActorType.Approver || x.Type == BusinessEntityWorkflowActorType.Reviewer)).ToList();
                        if (query.Count > 0)
                        {
                            result = query.Select(y => new BusinessEntityUserDto()
                            {
                                Id = (long)(y.UserId),
                                Name = (y.User != null) ? y.User.FullName : null,
                                Type = y.Type
                            }).Distinct().ToList();
                        }

                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<List<BusinessEntityUserDto>> GetAuthorityUsers(string pageName)
        {
            var result = new List<BusinessEntityUserDto>();
            try
            {
                var getpage = await _workflowpageRepository.GetAll().ToListAsync();
                if (getpage.Count() > 0)
                {
                    var checkpageNameId = getpage.Where(x => x.PageName.Trim().ToLower() == pageName.Trim().ToLower().ToString()).FirstOrDefault();
                    if (checkpageNameId != null)
                    {
                        var checkpage = await _authorityworkflowRepository.GetAll().Where(x => x.WorkFlowNameId == checkpageNameId.Id).FirstOrDefaultAsync();
                        if (checkpage != null)
                        {
                            result = _authorityworkflowRepository.GetAll().Include(x => x.User).Where(x => x.WorkFlowNameId == checkpageNameId.Id && x.UserId != null && (x.Type == BusinessEntityWorkflowActorType.Approver || x.Type == BusinessEntityWorkflowActorType.Reviewer)).
                                 Select(y => new BusinessEntityUserDto()
                                 {
                                     Id = (long)y.UserId,
                                     Name = (y.User != null) ? y.User.FullName : null,
                                     Type = y.Type
                                 }).ToList();
                        }
                        else
                        {
                            checkpageNameId = getpage.Where(x => x.PageName.Trim().ToLower() == "global".Trim().ToLower().ToString()).FirstOrDefault();
                            result = _authorityworkflowRepository.GetAll().Include(x => x.User).Where(x => x.WorkFlowNameId == checkpageNameId.Id && x.UserId != null && (x.Type == BusinessEntityWorkflowActorType.Approver || x.Type == BusinessEntityWorkflowActorType.Reviewer)).
                               Select(y => new BusinessEntityUserDto()
                               {
                                   Id = (long)y.UserId,
                                   Name = (y.User != null) ? y.User.FullName : null,
                                   Type = y.Type
                               }).ToList();
                        }

                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<List<BusinessEntityUserDto>> GetEntityAdminUser(int businessEntityId)
        {
            var query = new List<BusinessEntityUserDto>();

            try
            {
                var checkbusinessEntity = await _businessEntityRepository.GetAll().Where(x => x.Id == businessEntityId).FirstOrDefaultAsync();
                int roleId = 0;
                if (checkbusinessEntity.EntityType == EntityType.HealthcareEntity)
                {
                    roleId = _roleRepository.GetAll().Where(x => x.DisplayName.Trim().ToLower() == "business entity admin".Trim().ToLower()).Select(x => x.Id).FirstOrDefault();
                }
                else if (checkbusinessEntity.EntityType == EntityType.ExternalAudit)
                {
                    roleId = _roleRepository.GetAll().Where(x => x.DisplayName.Trim().ToLower() == "external audit admin".Trim().ToLower()).Select(x => x.Id).FirstOrDefault();

                }
                else if (checkbusinessEntity.EntityType == EntityType.InsuranceFacilities)
                {
                    roleId = _roleRepository.GetAll().Where(x => x.DisplayName.Trim().ToLower() == "insurance entity admin".Trim().ToLower()).Select(x => x.Id).FirstOrDefault();
                }
                else
                {
                    //  roleId = _roleRepository.GetAll().Where(x => x.DisplayName == "Business Entity Admin").Select(x => x.Id).FirstOrDefault();

                }
                query = await _userRepository.GetAll().Include(x => x.Roles)
                     .Where(x => x.BusinessEntityId == businessEntityId &&x.IsActive==true && x.TenantId == AbpSession.TenantId)
                     .WhereIf(roleId!=0, x =>  x.Roles.Select(y => y.RoleId).Contains(roleId)).Select(x => new BusinessEntityUserDto
                     {
                         Id = x.Id,
                         Name = x.FullName
                     }).ToListAsync();

                return query;
            }
            catch (Exception)
            {
                throw new UserFriendlyException("No record Found !");
            }
        }

        [AbpAllowAnonymous]
        public async Task<IReadOnlyList<BusinessEntityUserDto>> GetAllAuthorativeUsers()
        {
            try
            {
                var users = await _userRepository.GetAll().Where(x => x.BusinessEntityId == null && x.TenantId == AbpSession.TenantId && x.Type == UserOriginType.Authority).ToListAsync();
                return users.Select(e => new BusinessEntityUserDto
                {
                    Id = e.Id,
                    Name = e.FullName
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [AbpAllowAnonymous]
        public async Task<IReadOnlyList<BusinessEntityUserDto>> GetAllAggrementsUsers()
        {
            try
            {
                var users = await _userRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).ToListAsync();
                return users.Select(e => new BusinessEntityUserDto
                {
                    Id = e.Id,
                    Name = e.FullName
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [AbpAllowAnonymous]
        public async Task<IReadOnlyList<BusinessEntityFacilityTypeLookupTableDto>> GetAllBusinessEntity()
        {
            try
            {
                var users = await _businessEntityRepository.GetAll().Where(x => x.EntityType != EntityType.HealthcareEntity).ToListAsync();
                return users.Select(e => new BusinessEntityFacilityTypeLookupTableDto
                {
                    Id = e.Id,
                    DisplayName = e.CompanyName
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }



        [AbpAllowAnonymous]
        public async Task<IReadOnlyList<BusinessEntityFacilityTypeLookupTableDto>> GetAllHealthCareEntity()
        {
            try
            {
                var users = await _businessEntityRepository.GetAll().Where(x => x.EntityType == EntityType.HealthcareEntity).ToListAsync();
                return users.Select(e => new BusinessEntityFacilityTypeLookupTableDto
                {
                    Id = e.Id,
                    DisplayName = e.CompanyName
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }




        [AbpAllowAnonymous]
        public async Task<List<BusinessEntityUserDto>> GetTechnicalCommiteuser(int? businessEntityId)
        {
            try
            {
                var users = await _userRepository.GetAll().Where(x => x.BusinessEntityId == businessEntityId && x.TenantId == AbpSession.TenantId && x.Type == UserOriginType.ExternalAuditor).ToListAsync();
                return users.Select(e => new BusinessEntityUserDto
                {
                    Id = e.Id,
                    Name = e.FullName
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        [AbpAllowAnonymous]
        public async Task<IReadOnlyList<GetBusinessEntitiesExcelDto>> GetAllForLookUp(EntityType type, bool showAll = false)
        {
            List<GetBusinessEntitiesExcelDto> data = new List<GetBusinessEntitiesExcelDto>();
            var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();

            var query = await _businessEntityRepository
                .GetAll().Include("Actors").Include("OrganizationUnit")
                .WhereIf(showAll == false, e => e.Status == EntityTypeStatus.Active)
                .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.Id))
                .WhereIf(type == EntityType.HealthcareEntity || type == EntityType.ExternalAudit || type == EntityType.InsuranceFacilities, e => e.EntityType == type)
                .ToListAsync();
            foreach (var item in query)
            {
                var x = new GetBusinessEntitiesExcelDto
                {
                    BusinessEntity = new BusinessEntityDto
                    {
                        Id = item.Id,
                        Name = item.CompanyName,
                        OrganizationUnitId = item.OrganizationUnitId.Value,
                        Type = item.EntityType,
                        PrimaryApproverId = item.GetApprovers().Where(p => p.IsPrimaryUser).Select(r => r.UserId.Value).FirstOrDefault(),
                        PrimaryReviewerId = item.GetReviewers().Where(p => p.IsPrimaryUser).Select(r => r.UserId.Value).FirstOrDefault()
                    }
                };
                data.Add(x);
            }

            return data.AsReadOnly();
        }

        [AbpAllowAnonymous]
        public async Task<IReadOnlyList<GetBusinessEntitiesExcelDto>> GetAllForLookUpByVendor(int vendorId, bool showAll = false)
        {
            var data = new List<GetBusinessEntitiesExcelDto>();

            try
            {
                var query = await _businessEntityRepository
                   .GetAll().Include("Actors").Include("OrganizationUnit")
                   .WhereIf(showAll == false, e => e.Status == EntityTypeStatus.Active && e.Id == vendorId)
                   .ToListAsync();
                foreach (var item in query)
                {
                    var x = new GetBusinessEntitiesExcelDto
                    {
                        BusinessEntity = new BusinessEntityDto
                        {
                            Id = item.Id,
                            Name = item.CompanyName,
                            OrganizationUnitId = item.OrganizationUnitId.Value,
                            Type = item.EntityType,
                            PrimaryApproverId = item.GetApprovers().Where(p => p.IsPrimaryUser).Select(r => r.UserId.Value).FirstOrDefault(),
                            PrimaryReviewerId = item.GetReviewers().Where(p => p.IsPrimaryUser).Select(r => r.UserId.Value).FirstOrDefault()
                        }
                    };
                    data.Add(x);
                }

                return data.AsReadOnly();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<List<BusinessEntityDto>> GetAllUsersByUser(EntityType type, bool showAll = false)
        {
            var data = new List<BusinessEntityDto>();
            try
            {
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();

                data = await _businessEntityRepository
                    .GetAll().Include(x => x.OrganizationUnit)
                    .WhereIf(showAll == false, e => e.Status == EntityTypeStatus.Active)
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.Id))
                    .WhereIf(type == EntityType.HealthcareEntity || type == EntityType.ExternalAudit || type == EntityType.InsuranceFacilities, e => e.EntityType == type)
                    .Select(x => new BusinessEntityDto()
                    {
                        Id = x.Id,
                        Name = x.LicenseNumber + "-" + x.CompanyName,
                        OrganizationUnitId = x.OrganizationUnitId.Value,
                        Type = x.EntityType,
                        //    PrimaryApproverId = x.GetApprovers().Where(p => p.IsPrimaryUser).Select(r => r.UserId.Value).FirstOrDefault(),
                        //    PrimaryReviewerId = x.GetReviewers().Where(p => p.IsPrimaryUser).Select(r => r.UserId.Value).FirstOrDefault()
                    }).ToListAsync();


                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<GetBusinessEntitiesExcelDto>> GetAllVendor(EntityType type, bool showAll = false)
        {
            var data = new List<GetBusinessEntitiesExcelDto>();

            var query = await _businessEntityRepository
                .GetAll().AsNoTracking().Include("Actors").Include("OrganizationUnit")
                .WhereIf(showAll == false, e => e.Status == EntityTypeStatus.Active)
                .WhereIf(type == EntityType.HealthcareEntity || type == EntityType.ExternalAudit || type == EntityType.InsuranceFacilities, e => e.EntityType == type)
                .ToListAsync();
            foreach (var item in query)
            {
                var x = new GetBusinessEntitiesExcelDto
                {
                    BusinessEntity = new BusinessEntityDto
                    {
                        Id = item.Id,
                        Name = item.CompanyName,
                        OrganizationUnitId = item.OrganizationUnitId.Value,
                        Type = item.EntityType,
                        PrimaryApproverId = item.GetApprovers().Where(p => p.IsPrimaryUser).Select(r => r.UserId.Value).FirstOrDefault(),
                        PrimaryReviewerId = item.GetReviewers().Where(p => p.IsPrimaryUser).Select(r => r.UserId.Value).FirstOrDefault()
                    }
                };
                data.Add(x);
            }

            return data;
        }

        public async Task<List<BusinessEntityDto>> GetAllVendors(EntityType type, bool showAll = false)
        {
            var query = new List<BusinessEntityDto>();

            try
            {
                query = await _businessEntityRepository
                    .GetAll().Include("Actors").Include("OrganizationUnit")
                    .WhereIf(showAll == false, e => e.Status == EntityTypeStatus.Active)
                    .WhereIf(type == EntityType.HealthcareEntity || type == EntityType.ExternalAudit || type == EntityType.InsuranceFacilities, e => e.EntityType == type)
                    .Select(item => new BusinessEntityDto()
                    {
                        Id = item.Id,
                        Name = item.CompanyName,
                        OrganizationUnitId = item.OrganizationUnitId.Value,
                        Type = item.EntityType

                    }).ToListAsync();
                return query;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AbpAllowAnonymous]
        public async Task<IReadOnlyList<GetBusinessEntitiesExcelDto>> GetAllForLookups()
        {
            List<GetBusinessEntitiesExcelDto> data = new List<GetBusinessEntitiesExcelDto>();
            var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
            var query = await _businessEntityRepository
                .GetAll().Include("Actors").Include("OrganizationUnit").
                Where(e => e.Status == EntityTypeStatus.Active)
               .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.Id)).ToListAsync();



            foreach (var item in query)
            {
                var x = new GetBusinessEntitiesExcelDto
                {
                    BusinessEntity = new BusinessEntityDto
                    {
                        Id = item.Id,
                        Name = item.CompanyName,
                        OrganizationUnitId = item.OrganizationUnitId.Value,
                        Type = item.EntityType,
                        PrimaryApproverId = item.GetApprovers().Where(p => p.IsPrimaryUser).Select(r => r.UserId.Value).FirstOrDefault(),
                        PrimaryReviewerId = item.GetReviewers().Where(p => p.IsPrimaryUser).Select(r => r.UserId.Value).FirstOrDefault()
                    }
                };

                data.Add(x);
            }

            return data.AsReadOnly();
        }


        public async Task<List<BusinessEntityDto>> GetAllForBusinessEntity()
        {
            try
            {
                var data = new List<BusinessEntityDto>();
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                var query = await _businessEntityRepository
                   .GetAll().Include("Actors").Include("OrganizationUnit").
                   Where(e => e.Status == EntityTypeStatus.Active)
                  .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.Id)).ToListAsync();

                data = query.Select(item => new BusinessEntityDto()
                {
                    Id = item.Id,
                    Name = item.CompanyName,
                    OrganizationUnitId = item.OrganizationUnitId.Value,
                    Type = item.EntityType,
                    PrimaryApproverId = item.GetApprovers().Where(p => p.IsPrimaryUser).Select(r => r.UserId.Value).FirstOrDefault(),
                    PrimaryReviewerId = item.GetReviewers().Where(p => p.IsPrimaryUser).Select(r => r.UserId.Value).FirstOrDefault()
                }).ToList();
                return data;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public async Task<List<BusinessEntityDto>> GetAllBusinessEntityType(EntityType type)
        {
            try
            {
                var data = new List<BusinessEntityDto>();
                var AdminEntityType = new EntityType();
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();

                var currentUser = await GetCurrentUserAsync();
                if (currentUser.Type == UserOriginType.BusinessEntity)
                {
                    type = EntityType.HealthcareEntity;
                    AdminEntityType = EntityType.HealthcareEntity;
                }
                else if (currentUser.Type == UserOriginType.InsuranceEntity)
                {
                    type = EntityType.InsuranceFacilities;
                    AdminEntityType = EntityType.InsuranceFacilities;
                }
                else if (currentUser.Type == UserOriginType.admin || currentUser.Type == UserOriginType.Authority)
                {
                    type = EntityType.HealthcareEntity;
                    AdminEntityType = EntityType.InsuranceFacilities;
                }

                var query = await _businessEntityRepository
               .GetAll().Include("Actors").Include("OrganizationUnit").
               Where(e => e.Status == EntityTypeStatus.Active && e.EntityType == type || e.EntityType == AdminEntityType)
              .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.Id)).ToListAsync();

                data = query.Select(item => new BusinessEntityDto()
                {
                    Id = item.Id,
                    Name = EntityType.HealthcareEntity == item.EntityType ? "HealthcareEntity" + "-" + item.CompanyName + "-" + item.LicenseNumber : "InsuranceEntity" + "-" + item.CompanyName + "-" + item.LicenseNumber,
                    OrganizationUnitId = item.OrganizationUnitId.Value,
                    Type = item.EntityType,
                    PrimaryApproverId = item.GetApprovers().Where(p => p.IsPrimaryUser).Select(r => r.UserId.Value).FirstOrDefault(),
                    PrimaryReviewerId = item.GetReviewers().Where(p => p.IsPrimaryUser).Select(r => r.UserId.Value).FirstOrDefault()
                }).ToList();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<BusinessEntityDto>> GetAllBusinessEntityTypeById(int busiessEntityId)
        {
            try
            {
                var data = new List<BusinessEntityDto>();



                var query = await _businessEntityRepository
                   .GetAll().Include("Actors").Include("OrganizationUnit").
                   Where(e => e.Status == EntityTypeStatus.Active && e.Id == busiessEntityId).ToListAsync();


                data = query.Select(item => new BusinessEntityDto()
                {
                    Id = item.Id,
                    Name = item.CompanyName,
                    OrganizationUnitId = item.OrganizationUnitId.Value,
                    Type = item.EntityType,
                    PrimaryApproverId = item.GetApprovers().Where(p => p.IsPrimaryUser).Select(r => r.UserId.Value).FirstOrDefault(),
                    PrimaryReviewerId = item.GetReviewers().Where(p => p.IsPrimaryUser).Select(r => r.UserId.Value).FirstOrDefault()
                }).ToList();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<List<BusinessEntityDto>> GetAllBusinessEntityTypes()
        {
            try
            {
                var data = new List<BusinessEntityDto>();
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                var query = await _businessEntityRepository
                   .GetAll().Include("Actors").Include("OrganizationUnit").
                   Where(e => e.Status == EntityTypeStatus.Active)
                  .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.Id)).ToListAsync();
                data = query.Select(item => new BusinessEntityDto()
                {
                    Id = item.Id,
                    Name = item.CompanyName,
                    OrganizationUnitId = item.OrganizationUnitId.Value,
                    Type = item.EntityType,
                    PrimaryApproverId = item.GetApprovers().Where(p => p.IsPrimaryUser).Select(r => r.UserId.Value).FirstOrDefault(),
                    PrimaryReviewerId = item.GetReviewers().Where(p => p.IsPrimaryUser).Select(r => r.UserId.Value).FirstOrDefault()
                }).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IReadOnlyList<GetBusinessEntitiesExcelDto>> GetAllAuditablesForLookUp(EntityType type)
        {
            List<GetBusinessEntitiesExcelDto> data = new List<GetBusinessEntitiesExcelDto>();
            var query = await _businessEntityRepository
                .GetAll().Include("Actors").Include("OrganizationUnit")
                .Where(e => e.Status == EntityTypeStatus.Active)
                .WhereIf(type == EntityType.HealthcareEntity || type == EntityType.ExternalAudit || type == EntityType.InsuranceFacilities, e => e.EntityType == type)
                .WhereIf(type == EntityType.HealthcareEntity, e => e.IsAuditableEntity == true)
                .ToListAsync();

            foreach (var item in query)
            {
                var x = new GetBusinessEntitiesExcelDto
                {
                    BusinessEntity = new BusinessEntityDto
                    {
                        Id = item.Id,
                        Name = item.LicenseNumber + "-" + item.CompanyName,
                        OrganizationUnitId = item.OrganizationUnitId.Value,
                        Type = item.EntityType,
                        PrimaryApproverId = item.GetApprovers().Where(p => p.IsPrimaryUser).Select(r => r.UserId.Value).FirstOrDefault(),
                        PrimaryReviewerId = item.GetReviewers().Where(p => p.IsPrimaryUser).Select(r => r.UserId.Value).FirstOrDefault()
                    }
                };

                data.Add(x);
            }

            return data.AsReadOnly();
        }



        public async Task<IReadOnlyList<BusinessEnityGroupWiesDto>> GetBusinessEntityWithGrouporNot(int Id, long AuditProjectId)
        {
            var query = new List<BusinessEnityGroupWiesDto>();
            try
            {
                query = await _entityGroupMemberRepository.GetAll().Where(x => x.EntityGroupId == Id).Include(x => x.BusinessEntity).
                       Select(x => new BusinessEnityGroupWiesDto()
                       {
                           Id = x.BusinessEntity.Id,
                           CompanyName = x.BusinessEntity.CompanyName + "-" + x.BusinessEntity.LicenseNumber + "-" + x.EntityGroup.Name + " " + (x.EntityGroup.PrimaryEntityId == x.BusinessEntityId ? "(Primary Entity)" : ""),
                           EntityGroupId = x.EntityGroupId
                       }).ToListAsync();

                if(query.Count()==0)
                {
                   var querys  = await _extAssessmentRepository.GetAll().Where(x => x.AuditProjectId == AuditProjectId).Select(x => x.BusinessEntityId).ToListAsync();

                    if (querys.Count() != 0)
                    {

                        query = await _businessEntityRepository.GetAll().Where(x => querys.Contains(x.Id)).Select(x => new BusinessEnityGroupWiesDto()
                        {

                            Id = x.Id,
                            CompanyName = x.CompanyName + "-" + x.LicenseNumber,
                            EntityGroupId = Id
                        }).ToListAsync();
                    }
                }

                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AbpAllowAnonymous]
        public async Task<IReadOnlyList<BusinessEnityGroupWiesDto>> GetBusinessEntityGroupWies(int Id)
        {
            var query = new List<BusinessEnityGroupWiesDto>();
            try
            {
                query = await _entityGroupMemberRepository.GetAll().Where(x => x.EntityGroupId == Id).Include(x => x.BusinessEntity).
                       Select(x => new BusinessEnityGroupWiesDto()
                       {
                           Id = x.BusinessEntity.Id,
                           CompanyName = x.BusinessEntity.CompanyName + "-" + x.BusinessEntity.LicenseNumber + "-" + x.EntityGroup.Name + " " + (x.EntityGroup.PrimaryEntityId == x.BusinessEntityId ? "(Primary Entity)" : ""),
                           EntityGroupId = x.EntityGroupId
                       }).ToListAsync();

                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AbpAllowAnonymous]
        public async Task<IReadOnlyList<BusinessEnityGroupWiesDto>> GetBusinessEntityDeletedGroupWies(int Id)
        {
            var query = new List<BusinessEnityGroupWiesDto>();
            try
            {
                using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    query = await _entityGroupMemberRepository.GetAll().Where(x => x.EntityGroupId == Id).Include(x => x.BusinessEntity).
                       Select(x => new BusinessEnityGroupWiesDto()
                       {
                           Id = x.BusinessEntity.Id,
                           CompanyName = x.BusinessEntity.CompanyName + "-" + x.BusinessEntity.LicenseNumber + "-" + x.EntityGroup.Name + " " + (x.EntityGroup.PrimaryEntityId == x.BusinessEntityId ? "(Primary Entity)" : ""),
                           EntityGroupId = x.EntityGroupId
                       }).ToListAsync();
                }
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AbpAllowAnonymous]
        public async Task<IReadOnlyList<BusinessEnityGroupWiesDto>> GetBusinessEntityOFMultipleGroup(List<int> input)
        {
            var query = new List<BusinessEnityGroupWiesDto>();

            try
            {
                query = await _entityGroupMemberRepository.GetAll().
                       Where(x => input.Contains(x.EntityGroupId)).Include(x => x.EntityGroup).
                       Include(x => x.BusinessEntity).
                       Where(y => y.BusinessEntity.Status == EntityTypeStatus.Active).
                       Select(x => new BusinessEnityGroupWiesDto()
                       {
                           Id = x.BusinessEntity.Id,
                           CompanyName = x.BusinessEntity.CompanyName + "-" + x.BusinessEntity.LicenseNumber + "-" + x.EntityGroup.Name + " " + (x.EntityGroup.PrimaryEntityId == x.BusinessEntityId ? "(Primary Entity)" : ""),
                           EntityGroupId = x.EntityGroupId,
                           FacilitySubTypeId = (x.BusinessEntity.FacilitySubTypeId != null) ? x.BusinessEntity.FacilitySubTypeId : 0,
                           FacilityTypeId = (x.BusinessEntity.FacilityTypeId != null) ? x.BusinessEntity.FacilityTypeId : 0,
                           // EntityType=x.en

                       }).OrderByDescending(x => x.Id).ToListAsync();

                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<BusinessEnityGroupWiesDto>> GetAllBusinessEntityswithFacilityType()
        {

            var query = new List<BusinessEnityGroupWiesDto>();
            var query1 = new List<BusinessEnityGroupWiesDto>();
            var result = new List<BusinessEnityGroupWiesDto>();
            try
            {

                query = await _entityGroupMemberRepository.GetAll().Include(x => x.EntityGroup).Include(x => x.BusinessEntity).
                      Where(e => e.BusinessEntity.Status == EntityTypeStatus.Active && e.BusinessEntity.EntityType == EntityType.HealthcareEntity || e.BusinessEntity.EntityType == EntityType.InsuranceFacilities)
                      .Select(x => new BusinessEnityGroupWiesDto()
                      {
                          Id = x.BusinessEntity.Id,
                          CompanyName = x.BusinessEntity.LicenseNumber + "-" + x.BusinessEntity.CompanyName + "-" + x.EntityGroup.Name + " " + (x.EntityGroup.PrimaryEntityId == x.BusinessEntityId ? "(Primary Entity)" : ""),
                          EntityGroupId = x.EntityGroupId,
                          FacilitySubTypeId = (x.BusinessEntity.FacilitySubTypeId != null) ? x.BusinessEntity.FacilitySubTypeId : 0,
                          FacilityTypeId = (x.BusinessEntity.FacilityTypeId != null) ? x.BusinessEntity.FacilityTypeId : 0,
                          // EntityType=x.en

                      }).ToListAsync();


                var entityGroupMembers = await (from gm in _entityGrpMemberRepository.GetAll().Include("Members")
                                                from members in gm.Members
                                                select new EntityGroupMember
                                                {
                                                    Id = members.Id,
                                                    BusinessEntityId = members.BusinessEntityId
                                                }).ToListAsync();

                var list1 = _businessEntityRepository.GetAll()
                    .Where(e => e.Status == EntityTypeStatus.Active && e.EntityType == EntityType.HealthcareEntity || e.EntityType == EntityType.InsuranceFacilities)
                    .ToList();

                query1 = list1.Where(b => !entityGroupMembers.Any(e => e.BusinessEntityId == b.Id))
                     .Select(x => new BusinessEnityGroupWiesDto
                     {
                         Id = x.Id,
                         CompanyName = x.LicenseNumber + "-" + x.CompanyName,
                         EntityGroupId = 0,
                         FacilitySubTypeId = (x.FacilitySubTypeId != null) ? x.FacilitySubTypeId : 0,
                         FacilityTypeId = (x.FacilityTypeId != null) ? x.FacilityTypeId : 0,
                     }).ToList();

                result = query.Concat(query1).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AbpAllowAnonymous]
        public async Task<IReadOnlyList<BusinessEnityGroupWiesDto>> GetBusinessEntityes(int id)
        {
            var query = new List<BusinessEnityGroupWiesDto>();
            try
            {
                query = await _businessEntityRepository.GetAll().Where(x => x.Id == id).Select(x => new BusinessEnityGroupWiesDto()
                {
                    Id = x.Id,
                    CompanyName = x.CompanyName,

                }).ToListAsync();

                return query;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AbpAllowAnonymous]
        public async Task<IReadOnlyList<BusinessEnityGroupWiesDto>> GetBusinessEntityForLoginUser()
        {
            long Id = (long)AbpSession.UserId;
            var currentUser = await GetCurrentUserAsync();
            var role = await _roleManager.Roles.Where(r => r.DisplayName == "Admin").FirstOrDefaultAsync();
            var users = await UserManager.GetUsersInRoleAsync(role.Name);
            bool isAdmin = users.Any(u => u.Id == currentUser.Id);
            var BusinessEntityIds = new List<int>();
            if (!isAdmin)
            {
                var getcheckPrimaryEntity = _entityGrpMemberRepository.FirstOrDefaultAsync(x => x.Id == currentUser.BusinessEntityId).Result;
                if (getcheckPrimaryEntity != null)
                {
                    if (getcheckPrimaryEntity.PrimaryEntityId == currentUser.BusinessEntityId)
                    {
                        BusinessEntityIds = _entityGroupMemberRepository.GetAll().Where(x => x.EntityGroupId == getcheckPrimaryEntity.Id).Select(x => x.BusinessEntityId).ToList();

                    }
                }
                else
                {
                    BusinessEntityIds.Add((int)currentUser.BusinessEntityId);
                }
            }

            var query = new List<BusinessEnityGroupWiesDto>();
            try
            {
                query = await _businessEntityRepository.GetAll().Where(x => x.EntityType == EntityType.HealthcareEntity || x.EntityType == EntityType.InsuranceFacilities)
                    .WhereIf(!isAdmin, x => BusinessEntityIds.Contains(x.Id)).Select(x => new BusinessEnityGroupWiesDto()
                    {
                        Id = x.Id,
                        CompanyName = x.CompanyName,

                    }).ToListAsync();

                return query;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IReadOnlyList<GetBusinessEntitiesExcelDto>> GetAllExcludedEntityGroupMembers(EntityType type)
        {

            var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();


            var entityGroupMembers = await (from gm in _entityGrpMemberRepository.GetAll().AsNoTracking().Include("Members")
                                            from members in gm.Members
                                            select new EntityGroupMember
                                            {
                                                Id = members.Id,
                                                BusinessEntityId = members.BusinessEntityId
                                            }).ToListAsync();

            //var query = _businessEntityRepository.GetAll().Select(b => b.Id).Except(entityGroupMembers.Select(e => e.BusinessEntityId));
            var list1 = await _businessEntityRepository.GetAll().AsNoTracking()
                .Where(e => e.Status == EntityTypeStatus.Active && e.EntityType == type && e.IsAuditableEntity == true)
                 .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.Id)).ToListAsync();
                

            //var data= list1.Where(b => !entityGroupMembers.Any(e => e.BusinessEntityId == b.Id));

            var query = list1.Where(b => !entityGroupMembers.Any(e => e.BusinessEntityId == b.Id)).AsEnumerable()
                  .Select(x => new GetBusinessEntitiesExcelDto
                  {
                      BusinessEntity = new BusinessEntityDto
                      {
                          Id = x.Id,
                          Name = x.LicenseNumber + "-" + x.CompanyName,
                          OrganizationUnitId = x.OrganizationUnitId.Value
                      }
                  }).ToList();

            return query.AsReadOnly();
        }

        public async Task<List<BusinessEntitiesListDto>> GetAllExcludeMembers(EntityType type)
        {
            var query = new List<BusinessEntitiesListDto>();
            try
            {
                var entityGroupMembers = await (from gm in _entityGrpMemberRepository.GetAll().Include("Members")
                                                from members in gm.Members
                                                select new EntityGroupMember
                                                {
                                                    Id = members.Id,
                                                    BusinessEntityId = members.BusinessEntityId
                                                }).ToListAsync();

                var list1 = _businessEntityRepository.GetAll()
                    .Where(e => e.Status == EntityTypeStatus.Active)
                    .WhereIf(type == EntityType.HealthcareEntity || type == EntityType.ExternalAudit || type == EntityType.InsuranceFacilities, e => e.EntityType == type)
                   // .WhereIf(type == EntityType.HealthcareEntity, e => e.IsAuditableEntity == true)
                    .ToList();

                query = list1.Where(b => !entityGroupMembers.Any(e => e.BusinessEntityId == b.Id))
                     .Select(x => new BusinessEntitiesListDto
                     {
                         Id = x.Id,
                         Name = x.LicenseNumber + "-" + x.CompanyName,
                         OrganizationUnitId = x.OrganizationUnitId.Value,
                         FacilityTypeId = x.FacilityTypeId != null ? x.FacilityTypeId : 0,
                         FacilitySubTypeId = x.FacilitySubTypeId != null ? x.FacilitySubTypeId : 0

                     }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public async Task<List<BusinessEnityGroupWiesDto>> GetAllBusinessEntityByType(EntityType type)
        {
            var query = new List<BusinessEnityGroupWiesDto>();
            var query1 = new List<BusinessEnityGroupWiesDto>();
            var result = new List<BusinessEnityGroupWiesDto>();
            try
            {

                query = await _entityGroupMemberRepository.GetAll().Include(x => x.EntityGroup).Include(x => x.BusinessEntity).Where(x => x.BusinessEntity.EntityType == type && x.BusinessEntity.Status == EntityTypeStatus.Active).
                       Select(x => new BusinessEnityGroupWiesDto()
                       {
                           Id = x.BusinessEntity.Id,
                           CompanyName = x.BusinessEntity.CompanyName + "-" + x.BusinessEntity.LicenseNumber + "-" + x.EntityGroup.Name + " " + (x.EntityGroup.PrimaryEntityId == x.BusinessEntityId ? "(Primary Entity)" : ""),
                           EntityGroupId = x.EntityGroupId,
                           FacilitySubTypeId = (x.BusinessEntity.FacilitySubTypeId != null) ? x.BusinessEntity.FacilitySubTypeId : 0,
                           FacilityTypeId = (x.BusinessEntity.FacilityTypeId != null) ? x.BusinessEntity.FacilityTypeId : 0,

                       }).ToListAsync();


                var entityGroupMembers = await (from gm in _entityGrpMemberRepository.GetAll().Include("Members")
                                                from members in gm.Members
                                                select new EntityGroupMember
                                                {
                                                    Id = members.Id,
                                                    BusinessEntityId = members.BusinessEntityId
                                                }).ToListAsync();

                var list1 = _businessEntityRepository.GetAll()
                    .Where(e => e.Status == EntityTypeStatus.Active && e.EntityType == EntityType.InsuranceFacilities)
                    .ToList();

                query1 = list1.Where(b => !entityGroupMembers.Any(e => e.BusinessEntityId == b.Id))
                     .Select(x => new BusinessEnityGroupWiesDto
                     {
                         Id = x.Id,
                         CompanyName = x.LicenseNumber + "-" + x.CompanyName,
                         EntityGroupId = 0,
                         FacilitySubTypeId = (x.FacilitySubTypeId != null) ? x.FacilitySubTypeId : 0,
                         FacilityTypeId = (x.FacilityTypeId != null) ? x.FacilityTypeId : 0,
                     }).ToList();

                result = query.Concat(query1).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_HealthCareEntities_Edit)]
        public async Task<GetBusinessEntityForEditOutput> GetBusinessEntityForEdit(EntityDto input)
        {
            var businessEntity = await _businessEntityRepository.GetIncluding(x => x.Id == input.Id, "FacilityType", "Actors");

            var output = new GetBusinessEntityForEditOutput { BusinessEntity = ObjectMapper.Map<CreateOrEditBusinessEntityDto>(businessEntity) };
            output.BusinessEntity.EntityId = businessEntity.Code;
            output.BusinessEntity.AuthoritativeDocumentId = await _businessEntityWorkFlowActorRepository.GetAll().Where(x => x.BusinessEntityId == input.Id).Select(x => x.AuthoritativeDocumentId).Distinct().FirstOrDefaultAsync();
            output.BusinessEntity.WorkFlowNameId = await _businessEntityWorkFlowActorRepository.GetAll().Where(x => x.BusinessEntityId == input.Id).Select(x => x.WorkFlowNameId).Distinct().FirstOrDefaultAsync();
            output.BusinessEntity.ReviewerIds = businessEntity.GetReviewers().Select(r => r.UserId.Value).ToList();
            output.BusinessEntity.NotifierIds = businessEntity.GetNotifiers().Select(r => r.NotifierUserId.Value).ToList();
            output.BusinessEntity.ApproverIds = businessEntity.GetApprovers().Where(p => p.UserId != null).Select(r => r.UserId.Value).ToList();
            output.BusinessEntity.AuthorityIds = businessEntity.GetAuthoritys().Select(r => r.UserId.Value).ToList();
            output.BusinessEntity.PrimaryApproverId = businessEntity.GetApprovers().Where(p => p.IsPrimaryUser).Select(r => r.UserId.Value).FirstOrDefault();
            output.BusinessEntity.PrimaryReviewerId = businessEntity.GetReviewers().Where(p => p.IsPrimaryUser).Select(r => r.UserId.Value).FirstOrDefault();

            // output.BusinessTypeName = businessEntity.BusinessType?.Name;
            output.FacilityTypeName = businessEntity.FacilityType?.Name;
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBusinessEntityDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        public async Task CreateAdmin(EntityDto input)
        {
            await EventBus.Default.TriggerAsync(new EntityAdminCreationRequestedEvent(input.Id));
        }

        [AbpAllowAnonymous]
        public virtual async Task Register(CreateOrEditBusinessEntityDto input)
        {
            var businessEntity = ObjectMapper.Map<BusinessEntity>(input);
            businessEntity.Status = EntityTypeStatus.NotApproved;
            businessEntity.ThirdPartyId = businessEntity.ThirdPartyId == 0 ? null : businessEntity.ThirdPartyId;
            businessEntity.DistrictId = businessEntity.DistrictId == 0 ? null : businessEntity.DistrictId;
            // businessEntity.FacilitySubTypeId = businessEntity.FacilitySubTypeId == 0 ? null : businessEntity.FacilitySubTypeId;

            businessEntity.TenantId = 1; //  TODO
            await _businessEntityRepository.InsertAsync(businessEntity);

            if (!string.IsNullOrEmpty(input.VerificationCode))
            {
                var data = await _preRegisterEntityRepository.FirstOrDefaultAsync(v => v.VerificationCode == input.VerificationCode);
                data.IsVerificationDone = true;
                data.VerificationCode = null;
                await _preRegisterEntityRepository.UpdateAsync(data);
            }
        }

        [AbpAllowAnonymous]
        protected virtual async Task Create(CreateOrEditBusinessEntityDto input)
        {


            var result = await _businessEntityRepository.GetAll().ToListAsync();
            var check = result.Where(p => p.LicenseNumber.ToString().ToLower() == input.LicenseNumber.Trim().ToString().ToLower()).FirstOrDefault();
            if (check != null)
            {
                throw new UserFriendlyException("License Number already exist");
            }
            else
            {
                var businessEntity = ObjectMapper.Map<BusinessEntity>(input);
                businessEntity.IsAuditableEntity = true;
                businessEntity.Status = EntityTypeStatus.Active;
                var organizationUser = await _entityUserCreator.CreateAsync
                    (
                    businessEntity.AdminEmail,
                    businessEntity.AdminName,
                    businessEntity.AdminSurname,
                    businessEntity.AdminEmail,
                    businessEntity.AdminMobile,
                    AbpSession.TenantId,
                    input.EntityType,
                    businessEntity.CompanyName,
                    input.ParentOrganizationId,
                    false
                    );

                businessEntity.Users.Add(organizationUser.User);
                businessEntity.OrganizationUnit = organizationUser.OrganizationUnit;
                businessEntity.OrganizationUnitId = organizationUser.OrganizationUnit.Id;
                businessEntity.HasAdminGenerated = true;
                if (AbpSession.TenantId != null)
                {
                    businessEntity.TenantId = (int?)AbpSession.TenantId;
                }
                if (input.ParentOrganizationId.HasValue)
                {
                    var parentCompany = await _businessEntityRepository.FirstOrDefaultAsync(e => e.OrganizationUnitId == input.ParentOrganizationId);
                    businessEntity.ParentCompanyId = parentCompany.Id;
                    // businessEntity.ParentCompanyName = parentCompany.CompanyName;
                }
                businessEntity.ThirdPartyId = businessEntity.ThirdPartyId == 0 ? null : businessEntity.ThirdPartyId;
                businessEntity.DistrictId = businessEntity.DistrictId == 0 ? null : businessEntity.DistrictId;
                businessEntity.FacilitySubTypeId = businessEntity.FacilitySubTypeId == 0 ? null : businessEntity.FacilitySubTypeId;
                var newBsinessEntityId = _businessEntityRepository.InsertAndGetId(businessEntity);
                //insert into BusinrssEntityUser Table
                var temp = new BusinessEntityUser()
                {
                    Id = 0,
                    TenantId = (int)AbpSession.TenantId,
                    UserId = organizationUser.User.Id,
                    BusinessEntityId = newBsinessEntityId
                };
                var insertedid = _businessEntityUserRepository.InsertAndGetId(temp);
            }

        }



        [AbpAuthorize(AppPermissions.Pages_HealthCareEntities_Edit, AppPermissions.Pages_AuditManagement_Entities_Edit)]
        protected virtual async Task Update(CreateOrEditBusinessEntityDto input)
        {
            try
            {
                var businessEntity = await _businessEntityRepository.GetIncluding(e => e.Id == input.Id, "Actors", "Users");
                businessEntity.ThirdPartyId = businessEntity.ThirdPartyId == 0 ? null : businessEntity.ThirdPartyId;
                businessEntity.ConnectivityId= input.ConnectivityId == 0 ? null : input.ConnectivityId;

                businessEntity.DistrictId = businessEntity.DistrictId == 0 ? null : businessEntity.DistrictId;
                businessEntity.FacilitySubTypeId = input.FacilitySubTypeId == 0 ? null : input.FacilitySubTypeId;
                var beUser = await UserManager.FindByEmailAsync(businessEntity.AdminEmail);
                if (beUser != null)
                {
                    beUser.Name = input.AdminName;
                    beUser.Surname = input.AdminSurname;
                    beUser.EmailAddress = input.AdminEmail;
                    beUser.PhoneNumber = input.AdminMobile;
                    await UserManager.UpdateAsync(beUser);
                }
                else
                {
                    throw new UserFriendlyException("User Not Found!");
                }
                if (businessEntity.CompanyName != input.Name)
                {
                    var bseOrgUnit = _organizationUnitRepository.GetAll().Where(o => o.Id == businessEntity.OrganizationUnitId).FirstOrDefault();
                    bseOrgUnit.DisplayName = input.Name;
                }
                if (input.ThirdPartyId == 0)
                {
                    input.ThirdPartyId = null;
                }

                if(input.ConnectivityId == 0)
                {
                    input.ConnectivityId = null;
                }
                

                ObjectMapper.Map(input, businessEntity);
                if (input.ParentOrganizationId.HasValue)
                {
                    var parentCompany = await _businessEntityRepository.FirstOrDefaultAsync(e => e.OrganizationUnitId == input.ParentOrganizationId);
                    businessEntity.ParentCompanyId = parentCompany.Id;
                    //  businessEntity.ParentCompanyName = parentCompany.CompanyName;
                }
                List<BusinessEntityWorkFlowActor> businessEntityWorkFlowActors = new List<BusinessEntityWorkFlowActor>();
                if (input.ApproverIds.Count > 0 || input.NotifierIds.Count > 0 || input.ReviewerIds.Count > 0)
                {
                    input.ApproverIds.ForEach(approverId =>
                    {
                        businessEntityWorkFlowActors.Add(new BusinessEntityWorkFlowActor(BusinessEntityWorkflowActorType.Approver, approverId, businessEntity.Id, null, input.PrimaryApproverId == approverId ? true : false, input.AuthoritativeDocumentId, input.WorkFlowNameId));
                    });
                    input.NotifierIds.ForEach(notifierId =>
                    {
                        businessEntityWorkFlowActors.Add(new BusinessEntityWorkFlowActor(BusinessEntityWorkflowActorType.Notifier, null, businessEntity.Id, notifierId, false, input.AuthoritativeDocumentId, input.WorkFlowNameId));
                    });
                    input.ReviewerIds.ForEach(reviewerId =>
                    {
                        businessEntityWorkFlowActors.Add(new BusinessEntityWorkFlowActor(BusinessEntityWorkflowActorType.Reviewer, reviewerId, businessEntity.Id, null, input.PrimaryReviewerId == reviewerId ? true : false, input.AuthoritativeDocumentId, input.WorkFlowNameId));
                    });
                }
                else
                {
                    businessEntityWorkFlowActors.Add(new BusinessEntityWorkFlowActor(BusinessEntityWorkflowActorType.Approver, null, businessEntity.Id, null, false, input.AuthoritativeDocumentId, input.WorkFlowNameId));

                }

                input.AuthorityIds.ForEach(authorityId =>
                {
                    businessEntityWorkFlowActors.Add(new BusinessEntityWorkFlowActor(BusinessEntityWorkflowActorType.Authority, authorityId, businessEntity.Id, null, false, input.AuthoritativeDocumentId, input.WorkFlowNameId));
                });

                businessEntity.Actors = businessEntityWorkFlowActors;

                foreach (var user in businessEntity.Users)
                {
                    if (businessEntity.IsSuspended)
                    {
                        user.IsActive = false;
                    }                  
                }
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_HealthCareEntities_Delete, AppPermissions.Pages_AuditManagement_Entities_Delete)]
        [UnitOfWork]
        public async Task Delete(EntityDto input)
        {
            var businessEntity = await _businessEntityRepository.GetIncluding(e => e.Id == input.Id, "OrganizationUnit");
            if (businessEntity == null)
            {
                throw new NotFoundException($"Couldn't find Business Entity with ID {input.Id}");
            }
            else
            {
                var getchek = _extAssessmentRepository.GetAll().Any(x => x.BusinessEntityId == input.Id);
                if (getchek)
                {
                    throw new UserFriendlyException("The related records of the following record still exist. Please delete child records to delete this ! ");

                }
                else
                {
                    var Isexit = _assessmentRepository.GetAll().Any(x => x.BusinessEntityId == input.Id);
                    var isEntityGroupMember = _entityGroupMembersRepository.GetAll().Any(x => x.BusinessEntityId == input.Id);
                    if (Isexit)
                    {
                        throw new UserFriendlyException("The related records of the following record still exist. Please delete child records to delete this ! ");
                    }

                    else if (isEntityGroupMember)
                    {
                        throw new UserFriendlyException("The related records of the following record still exist. Please delete child records to delete this ! ");

                    }
                }
            }
            await EventBus.Default.TriggerAsync(new BusinessEntityDeletedEvent
            {
                BusinessEntity = businessEntity
            });
            await _businessEntityRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_HealthCareEntities_Edit, AppPermissions.Pages_AuditManagement_Entities_Edit)]
        public async Task Activate(int id)
        {
            var businessEntity = await _businessEntityRepository.GetIncluding(e => e.Id == id, "OrganizationUnit");
            if (businessEntity == null)
            {
                throw new NotFoundException($"Couldn't find Business Entity with ID {id}");
            }
            foreach (var user in businessEntity.Users)
            {
                if (!user.IsActive)
                {
                    user.IsActive = false;
                }
            }
            businessEntity.Activate();
        }
        [AbpAuthorize(AppPermissions.Pages_HealthCareEntities_Edit, AppPermissions.Pages_AuditManagement_Entities_Edit)]
        public async Task Deactivate(int id)
        {
            var businessEntity = await _businessEntityRepository.GetIncluding(e => e.Id == id, "OrganizationUnit", "Users");
            if (businessEntity == null)
            {
                throw new NotFoundException($"Couldn't find Business Entity with ID {id}");
            }
            foreach (var user in businessEntity.Users)
            {
                user.IsActive = false;
            }
            businessEntity.Deactivate();
        }


        public async Task<IReadOnlyList<BusinessEntityUserDto>> GetAllAuditAgencyAdmins(EntityDto input)
        {
            try
            {
                //var userall = new List<User>();
                //var auditAgencyUser = new List<User>();
                //var auditManagmentUsers = new List<User>();

                //var businessEntity = await _businessEntityRepository.GetIncluding(e => e.Id == input.Id, "OrganizationUnit", "Users");
                //var users = await UserManager.GetUsersInOrganizationUnitAsync(businessEntity.OrganizationUnit);
                //var auditAgencyUsers = await UserManager.GetUsersInRoleAsync("External Audit Admin");
                //auditAgencyUser = auditAgencyUsers.ToList();
                //var auditManagmentUser = await UserManager.GetUsersInRoleAsync("External Auditor Management");
                //auditManagmentUsers = auditManagmentUser.ToList();
                //userall.AddRange(auditAgencyUser);
                //userall.AddRange(auditManagmentUsers);

                //var Userses = users.Select(x => x.Id).Distinct();

                var result = await _userRepository.GetAll().Where(x => x.BusinessEntityId == input.Id && x.Type == UserOriginType.ExternalAuditor).Select(x => new BusinessEntityUserDto()
                {
                    Id = x.Id,
                    Name = x.FullName

                }).ToListAsync();

                //var result = (from u1 in Userses
                //              join u2 in userall
                //              on u1 equals u2.Id
                //              select new BusinessEntityUserDto
                //              {
                //                  Id = u1,
                //                  Name = u2.FullName
                //              }).Distinct().ToArray().ToList();



                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AbpAllowAnonymous]
        public async Task<int?> GetTeantId()
        {
            var currentUser = await GetCurrentUserAsync();
            return currentUser.TenantId;
        }

        public async Task<CheckEntityGroupWithRoleDto> EntityGroupExistOrNot(List<int> input)
        {
            CheckEntityGroupWithRoleDto result = new CheckEntityGroupWithRoleDto();
            if (_entityGroupMembersRepository.GetAll().Any(x => input.Contains(x.BusinessEntityId)))
            {
                var query = await _entityGroupMembersRepository.GetAll().Include(x => x.EntityGroup).Where(x => input.Contains(x.BusinessEntityId)).Select(x => x.EntityGroup).FirstOrDefaultAsync();
                result.createOrEditEntityGroupDto = ObjectMapper.Map<CreateOrEditEntityGroupDto>(query);
            }

            var user = await _userRepository.GetAll().Include(x => x.Roles).Where(x => x.Id == (long)AbpSession.UserId).FirstOrDefaultAsync();
            var roleId = user.Roles.Select(x => x.RoleId).FirstOrDefault();
            var role = await _roleManager.GetRoleByIdAsync(roleId);

            result.roleName = role.Name;
            return result;
        }

        public async Task<EntityType> EntityTypeOfUser()
        {
            var user = await _userRepository.GetAll().Include(x => x.BusinessEntity).Where(x => x.Id == (long)AbpSession.UserId).FirstOrDefaultAsync();
            return user.BusinessEntity.EntityType;
        }


        #endregion

        #region IMPORT&EXPORT
        public async Task<FileDto> GetBusinessEntitiesToExcel(GetAllBusinessEntitiesInput input)
        {

            var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();

            var filteredBusinessEntities = _businessEntityRepository.GetAll()
             .Include(e => e.FacilityType)
             .Include(e => e.FacilitySubType)
             .Include(e => e.Country).Where(e => e.IsDeleted == input.ShowOnlyDeleted && e.EntityType == input.EntityType)
             .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.Id))
             .WhereIf(input.Status != null, e => e.Status == input.Status)
             .WhereIf(input.IsLicensedFilter != null, e => e.IsCompanyLicensed == input.IsLicensedFilter)
             .WhereIf(input.IsAuditableFilter != null, e => e.IsAuditableEntity == input.IsAuditableFilter)
             .WhereIf(input.IsGovernmentOwnedFilter != null, e => e.IsGovernmentOwned == input.IsGovernmentOwnedFilter)
             .WhereIf(input.MinNumberOfYearsInBusinessFilter != null, e => e.NumberOfYearsInBusiness <= input.MinNumberOfYearsInBusinessFilter)
             .WhereIf(input.MaxNumberOfYearsInBusinessFilter != null, e => e.NumberOfYearsInBusiness >= input.MaxNumberOfYearsInBusinessFilter)
             .WhereIf(input.FacilityTypeFilter > 0, e => e.FacilityTypeId == input.FacilityTypeFilter)
             .WhereIf(input.FacilitySubTypeFilter > 0, e => e.FacilitySubTypeId == input.FacilitySubTypeFilter)
             .WhereIf(!input.Filter.IsNullOrWhiteSpace(), u => u.CompanyName.Contains(input.Filter.Trim().ToLower()) ||
                   u.LicenseNumber.Contains(input.Filter.Trim().ToLower()) || u.AdminEmail.Contains(input.Filter.Trim().ToLower()) || u.CompanyAddress.Contains(input.Filter.Trim().ToLower()))
              .WhereIf(!input.LegalNameFilter.IsNullOrWhiteSpace(),
              u => u.CompanyLegalName.Contains(input.LegalNameFilter.Trim().ToLower()))
              .WhereIf(!input.NameFilter.IsNullOrWhiteSpace(),
              u => u.CompanyName.Contains(input.NameFilter.Trim().ToLower()))
              .WhereIf(!input.AddressFilter.IsNullOrWhiteSpace(),
              u => u.CompanyAddress.Contains(input.AddressFilter.Trim().ToLower()))
              .WhereIf(!input.LicenseNumberFilter.IsNullOrWhiteSpace(),
              u => u.GroupName.Contains(input.LicenseNumberFilter.Trim().ToLower()))
              .WhereIf(!input.WebsiteUrlFilter.IsNullOrWhiteSpace(),
              u => u.CompanyWebsite.Contains(input.WebsiteUrlFilter.Trim().ToLower()));
            var list = filteredBusinessEntities.Select(x => new BusinessEntitiesExcelDto
            {
                EntityType = x.EntityType,
                LicenseNumber = x.LicenseNumber,
                Facility_EN = x.CompanyName,
                IsPublic = x.IsPublic,
                EntityGroupName = x.GroupName,
                District = x.District == null ? null : x.District,
                FacilityTypeName = x.FacilityType == null ? null : x.FacilityType.Name,
                FacilitySubTypeName = x.FacilitySubType == null ? null : x.FacilitySubType.FacilitySubTypeName,
                IsActive = x.IsActive,
                HFLName = x.CompanyLegalName,
                Facility_Email = x.Facility_Email,
                Owner_EN = x.Owner_EN,
                Owner_Email = x.Owner_Email,
                Owner_Mobile = x.Owner_Mobile,
                Director_Incharge_EN = x.Director_Incharge_EN,
                Director_Incharge_Email = x.Director_Incharge_Email,
                Director_Incharge_Mobile = x.Director_Incharge_Mobile,
                Pro_EN = x.Pro_EN,
                Pro_Email = x.Pro_Email,
                Pro_Mobile = x.Pro_Mobile,
                AdminEmail = x.AdminEmail,
                AdminMobile = x.AdminMobile,
                PrimaryContactName = x.PrimaryContactName,
                ContactNumber = x.ContactNumber,
                Designation = x.Designation,
                OfficialEmail = x.OfficialEmail,
                BackupContactName = x.BackupContactName,
                BackupContactNumber = x.BackupContactNumber,
                BackupDesignation = x.BackupDesignation,
                BackupOfficialEmail = x.BackupOfficialEmail,
                AdminName = x.AdminName,
                AdminSurname = x.AdminSurname,
                NumberOfYearsInBusiness = x.NumberOfYearsInBusiness,
                Status = (EntityTypeStatus)x.Status,
                CityOrDisctrict = x.CityOrDisctrict,
                CountryName = x.Country.Name
            }).ToList();

            return _businessEntitiesExcelExporter.ExportToFile(list);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_PreRegistration_View)]
        public async Task<PagedResultDto<PreRegisterBusinessEntityInputDto>> GetAllPreRegisterRequests(GetPreRegisterInput input)
        {
            try
            {
                var filteredReg = _preRegisterEntityRepository.GetAll()
                    .WhereIf(input.FacilityTypeId > 0, e => e.FacilityTypeId == input.FacilityTypeId).WhereIf(input.FacilitySubTypeId > 0, e => e.FacilitySubTypeId == input.FacilitySubTypeId)
                  .WhereIf(input.DistrictId > 0, e => e.DistrictId == input.DistrictId)
                     .WhereIf(input.IsVerificationDone.HasValue, e => e.IsVerificationDone == input.IsVerificationDone)
                     .WhereIf(input.IsRequestApproved.HasValue, e => e.IsRequestApproved == input.IsRequestApproved)
                    .WhereIf(!input.Filter.IsNullOrWhiteSpace(), u =>
                         u.Name.Contains(input.Filter) ||
                         u.CompanyName.Contains(input.Filter) ||
                         u.AdminEmail.Contains(input.Filter) ||
                         u.LicenseNumber.Contains(input.Filter)
                );


                var pagedAndFilteredReg = filteredReg
                    .OrderBy(input.Sorting)
                    .PageBy(input);

                var data = ObjectMapper.Map<List<PreRegisterBusinessEntityInputDto>>(pagedAndFilteredReg);

                var totalCount = await filteredReg.CountAsync();

                return new PagedResultDto<PreRegisterBusinessEntityInputDto>(
                    totalCount,
                    data
                );
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [AbpAllowAnonymous]
        public async Task<bool> checkUserEmail(PreRegisterBusinessEntityInputDto input)
        {

            try
            {
                if (!UserManager.Users.Any(u => u.EmailAddress == input.AdminEmail))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AbpAllowAnonymous]
        public async Task PreRegistrationVerification(PreRegisterBusinessEntityInputDto input)
        {
            try
            {
                var data = ObjectMapper.Map<PreRegisterBusinessEntity>(input);
                data.VerificationCode = Guid.NewGuid().ToString("N").Truncate(10).ToUpperInvariant();
                data.TenantId = AbpSession.TenantId;
                var appSetting = await _ientityApplicationSettingAppService.GetApplicationSettings();
                if (appSetting.EnablePreRegVerification)
                {
                    if (!string.IsNullOrEmpty(appSetting.PreRegVerificationList))
                    {
                        List<string> approvers = appSetting.PreRegVerificationList.Split(",").ToList();

                        if (approvers.Count > 0)
                        {
                            await _userEmailer.SendPreRegisterApproverMail(approvers, data.AdminEmail, data.VerificationCode, data.TenantId.Value);
                        }
                    }
                }
                else
                {
                    data.IsRequestApproved = true;
                    await _userEmailer.SendPreRegisterVerificationMail(data.AdminEmail, data.VerificationCode, data.TenantId.Value,
                       AppUrlService.CreatePreRegistrationVerifyLink(AbpSession.TenantId.Value));
                }
                data.ThirdPartyId = data.ThirdPartyId == 0 ? null : data.ThirdPartyId;
                data.DistrictId = data.DistrictId == 0 ? null : data.DistrictId;
                data.FacilitySubTypeId = data.FacilitySubTypeId == 0 ? null : data.FacilitySubTypeId;
                await _preRegisterEntityRepository.InsertOrUpdateAsync(data);
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_PreRegistration_Approver)]

        public async Task PreRegistrationApproval(PreRegisterBusinessEntityInputDto input)
        {

            var data = await _preRegisterEntityRepository.FirstOrDefaultAsync(v => v.AdminEmail == input.AdminEmail);
            if (data.VerificationCode != null)
            {
                data.IsRequestApproved = input.IsRequestApproved;
                await _preRegisterEntityRepository.UpdateAsync(data);
                if (input.IsRequestApproved)
                {
                    await _userEmailer.SendPreRegisterVerificationMail(data.AdminEmail, data.VerificationCode, data.TenantId.Value,
                        AppUrlService.CreatePreRegistrationVerifyLink(AbpSession.TenantId.Value));
                }
            }
            else
            {
                throw new UserFriendlyException("verification already Done !");
            }


        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_PreRegistration_Approver)]

        public async Task PreRegistrationSelectedApprovals(List<PreRegisterBusinessEntityInputDto> items)
        {
            try
            {
                foreach (var input in items)
                {
                    var data = await _preRegisterEntityRepository.FirstOrDefaultAsync(v => v.Id == input.Id);
                    if (data.AdminEmail != null && data.AdminName != null && data.AdminSurname != null)
                    {
                        data.IsRequestApproved = true;
                        data.VerificationCode = "00000" + data.Id;
                    }

                    await _preRegisterEntityRepository.UpdateAsync(data);
                    if (!input.IsRequestApproved)
                    {

                        await _userEmailer.SendPreRegisterVerificationMail(data.AdminEmail, data.VerificationCode, data.TenantId.Value,
                            AppUrlService.CreatePreRegistrationVerifyLink(AbpSession.TenantId.Value));
                    }
                }
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public async Task<bool> BusinessEntityApproval(List<PreRegisterBusinessEntityInputDto> items)
        {
            bool isverfied = false;
            int BusinessEntityId = 0;
            long orgId = 0;
            try
            {
                foreach (var item in items)
                {
                    long userid = 0;
                    var checkBusinessentity = _businessEntityRepository.GetAll().ToList();
                    var checkavailable = checkBusinessentity.Where(x => x.LicenseNumber.ToLower() == item.LicenseNumber.Trim().ToString().ToLower()).FirstOrDefault();
                    if (checkavailable == null)
                    {
                        string DisplayName = null;

                        if (item.EntityType == EntityType.HealthcareEntity)
                        {
                            DisplayName = "Healthcare Entities";
                        }
                        else
                        {
                            if (item.EntityType == EntityType.InsuranceFacilities)
                            {
                                DisplayName = "Insurance Facilities";
                            }
                            else
                            {
                                if (item.EntityType == EntityType.ExternalAudit)
                                {
                                    DisplayName = "External Auditors";
                                }
                            }
                        }
                        var getOraganization = await _organizationUnitRepository.GetAll().ToListAsync();
                        var concreteOrganizationUnit = getOraganization.FirstOrDefault(e => e.DisplayName.Trim().ToLower() == (DisplayName).Trim().ToLower().ToString());
                        var getcodes = _organizationUnitManager.GetNextChildCode(concreteOrganizationUnit.Id);

                        var newOrganizatinUnit = new OrganizationUnit()
                        {
                            Id = 0,
                            TenantId = item.TenantId,
                            ParentId = concreteOrganizationUnit.Id,
                            Code = getcodes.ToString(),
                            DisplayName = item.Name
                        };

                        orgId = _organizationUnitRepository.InsertAndGetId(newOrganizatinUnit);
                        var businessentity = new BusinessEntity
                        {
                            CreationTime = DateTime.Now,
                            Id = 0,
                            LicenseNumber = item.LicenseNumber,
                            CompanyName = item.Name,
                            IsActive = item.IsActive,
                            EntityType = item.EntityType,
                            CompanyLegalName = item.Name,
                            IsPreAssessmentQuestionaire = true,
                            AdminEmail = item.AdminEmail,
                            AdminMobile = item.AdminMobile,
                            Owner_EN = item.Owner_EN,
                            Owner_Mobile = item.Owner_Mobile,
                            Owner_Email = item.Owner_Email,
                            FacilityTypeId = item.FacilityTypeId,
                            FacilitySubTypeId = item.FacilitySubTypeId,
                            DistrictId = item.DistrictId,
                            CityOrDisctrict = item.CityOrDisctrict,
                            CountryId = _countriesRepository.GetAll().Where(x => x.TenantId == item.TenantId).FirstOrDefault().Id,
                            ThirdPartyId = item.ThirdPartyId,
                            PrimaryContactName = item.PrimaryContactName,
                            Facility_Email=item.Facility_Email,
                            Facility_EN=item.Facility_EN,
                            ContactNumber = item.ContactNumber,
                            Designation = item.Designation,
                            OfficialEmail = item.OfficialEmail,
                            BackupContactName = item.BackupContactName,
                            BackupContactNumber = item.BackupContactNumber,
                            BackupDesignation = item.BackupDesignation,
                            BackupOfficialEmail = item.BackupOfficialEmail,
                            AdminName = item.AdminName,
                            OrganizationUnitId = orgId,
                            AdminSurname = item.AdminSurname,
                            Status = EntityTypeStatus.Active,
                            IsAuditableEntity = true,
                            HasAdminGenerated = true,
                            TenantId = item.TenantId,
                            ComplianceType = item.ControlType,
                            NumberOfYearsInBusiness = 0,
                            IsGovernmentOwned = false,
                            IsCompanyLicensed = true,
                            IsParentReportingEnabled = false,
                            IsSuspended = false,
                            IsOrphan = false,
                            IsPublic = item.IsPublic,
                            // FacilityTypeSize = item.FacilityTypeSize

                        };

                        if (item.CountryId != null)
                        {
                            businessentity.CountryId = (int)item.CountryId;
                        }
                        else
                        {
                            businessentity.CountryId = _countriesRepository.GetAll().Where(x => x.TenantId == item.TenantId).FirstOrDefault().Id;
                        }

                        BusinessEntityId = _businessEntityRepository.InsertAndGetId(businessentity);
                        var getuserCheck = _userRepository.GetAll().ToList();
                        var checkUser = getuserCheck.Where(x => x.EmailAddress.Trim().ToLower() == item.AdminEmail.Trim().ToLower()).FirstOrDefault();
                        if (checkUser == null)
                        {
                            var user = new User
                            {
                                Id = 0,
                                AccessFailedCount = 0,
                                CreationTime = DateTime.Now,
                                ShouldChangePasswordOnNextLogin = true,
                                UserName = item.AdminEmail.Replace(" ", string.Empty),
                                PhoneNumber = item.AdminMobile,
                                TenantId = item.TenantId,
                                EmailAddress = item.AdminEmail,
                                Name = item.AdminName,
                                NormalizedUserName = item.AdminEmail.ToUpper(),
                                NormalizedEmailAddress = item.AdminEmail.ToUpper(),
                                Surname = item.AdminSurname,
                                IsActive = true,
                                BusinessEntityId = BusinessEntityId,
                                Type = item.EntityType == EntityType.InsuranceFacilities ? UserOriginType.InsuranceEntity : UserOriginType.BusinessEntity
                            };
                            user.Password = _passwordHasher.HashPassword(user, defaultPassword);
                            //  user.EncryptPassword= SimpleStringCipher.Instance.Encrypt(defaultPassword);
                            user.Roles = new Collection<UserRole>();
                            string AssignedRoleNames = null;
                            if (item.EntityType == EntityType.HealthcareEntity)
                            {
                                AssignedRoleNames = "Business Entity Admin";
                            }
                            else
                            {
                                AssignedRoleNames = "Insurance Entity Admin";
                            }

                            var role = _roleManager.GetRoleByNameAsync(AssignedRoleNames);
                            user.Roles.Add(new UserRole(item.TenantId, user.Id, role.Result.Id));
                            userid = _userRepository.InsertAndGetId(user);

                            //var users = await _userRepository.FirstOrDefaultAsync(x => x.Id == userid);
                            //users.SetNewEmailConfirmationCode();
                            //await _userEmailer.CreateUSerAsync(
                            //    users,
                            //    AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId)
                            //);
                        }

                        userid = userid != 0 ? userid : checkUser.Id;
                        //insert into BusinrssEntityUser Table
                        var temp = new BusinessEntityUser()
                        {
                            Id = 0,
                            TenantId = (int)AbpSession.TenantId,
                            UserId = userid,
                            BusinessEntityId = BusinessEntityId
                        };
                        var insertedid = _businessEntityUserRepository.InsertAndGetId(temp);
                        var userOrganizationUnits = new UserOrganizationUnit()
                        {
                            Id = 0,
                            UserId = userid,
                            TenantId = item.TenantId,
                            CreationTime = DateTime.Now,
                            OrganizationUnitId = orgId
                        };
                        await _userOrganizationUnitRepository.InsertAsync(userOrganizationUnits);

                        var preregistration = _preRegisterEntityRepository.GetAll().Where(x => x.Id == item.Id).FirstOrDefault();
                        preregistration.IsVerificationDone = true;
                        preregistration.IsRequestApproved = true;
                        _preRegisterEntityRepository.Update(preregistration);

                        isverfied = true;

                    }
                    else
                    {

                        var getcheckUpdate = _preRegisterEntityRepository.GetAll().Where(x => x.Id == item.Id).FirstOrDefault();
                        getcheckUpdate.IsVerificationDone = true;
                        getcheckUpdate.IsRequestApproved = true;
                        _preRegisterEntityRepository.Update(getcheckUpdate);
                    }
                }
                return isverfied;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        public async Task<bool> SendEntityInform(List<long> items)
        {
            bool isverfied = false;
            try
            {
                var getpage = await _workflowpageRepository.FirstOrDefaultAsync(x => x.PageName.ToLower().Trim() == LockthreatComplianceConsts.GlobalPage.Trim().ToLower());
                if (getpage != null)
                {
                    var checkteamplate = await _templateRepository.FirstOrDefaultAsync(x => x.WorkFlowPageId == getpage.Id);

                    if (checkteamplate != null)
                    {

                        var checkentity = await _businessEntityRepository.GetAll().Where(e => items.Contains(e.Id)).ToListAsync();

                        checkentity.ForEach(x =>
                        {
                            string auditbody = null;
                            string AuditEmailsubject = null;

                            var emails = new HashSet<string>();
                            var ccemail = new HashSet<string>();
                            var bccemail = new HashSet<string>();


                            //  var item =  _businessEntityRepository.GetAll().Where(xx => xx.Id == x.Id).FirstOrDefault();
                            var item1 = _businessEntityRepository.GetAll().Where(xx => xx.Id == x.Id).FirstOrDefault();
                            List<string> templateSubject = new List<string>();
                            var auditprojectsubjectBody = checkteamplate.TemplateSubject;

                            AuditEmailsubject = checkteamplate.TemplateSubject.ToString();

                            while (auditprojectsubjectBody.Contains("{"))
                            {
                                templateSubject.Add("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}");
                                auditprojectsubjectBody = auditprojectsubjectBody.Replace("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}", "");
                            };


                            AuditEmailsubject = ReplaceValueFunction(item1, templateSubject, AuditEmailsubject);

                            var auditTemplate = checkteamplate.TemplateBody;

                            var auditTo = checkteamplate.TemplateTo;
                            List<string> auditToList = checkteamplate.TemplateTo.Split(',').ToList();
                            List<string> templatevariables = new List<string>();

                            auditToList.ForEach(emailid =>
                            {
                                if (emailid.Contains("{"))
                                {
                                    templatevariables.Add("{" + emailid.Split('{', '}')[1] + "}");
                                    //  auditTo = auditTo.Replace("{" + auditTo.Split('{', '}')[1] + "}", "");
                                }
                                else
                                {
                                    string email = emailid.Trim();
                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                    if (isEmail == true)
                                    {
                                        emails.Add(email);
                                    }
                                }
                            });

                            var auditCc = checkteamplate.TemplateCc;
                            List<string> auditCcList = checkteamplate.TemplateCc.Split(',').ToList();
                            List<string> templateCc = new List<string>();

                            auditCcList.ForEach(emailid =>
                            {
                                if (emailid.Contains("{"))
                                {
                                    templateCc.Add("{" + emailid.Split('{', '}')[1] + "}");
                                    //  auditTo = auditTo.Replace("{" + auditTo.Split('{', '}')[1] + "}", "");
                                }
                                else
                                {
                                    string email = emailid.Trim();
                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                    if (isEmail == true)
                                    {
                                        ccemail.Add(email);
                                    }
                                }
                            });

                            templatevariables.ForEach(x =>
                            {
                                switch (x)
                                {
                                    case "{Business_Entity_Admin_Email}":
                                        {
                                            emails.Add(item1.AdminEmail);
                                            break;
                                        }
                                    case "{Audit_Agency_Admin_Email}":
                                        {
                                            //var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                            //if (getbusinessadmin != null)
                                            //{
                                            //    emails.Add(getbusinessadmin.AdminEmail);
                                            //}
                                            break;
                                        }
                                    case "{Owner_Email}":
                                        {
                                            if (item1.Owner_Email != null)
                                            {
                                                var splitEmail = item1.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        emails.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Director_Incharge_Email}":
                                        {
                                            if (item1.Director_Incharge_Email != null)
                                            {
                                                var splitEmail = item1.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        emails.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{CISO_Email}":
                                        {
                                            if (item1.CISO_Email != null)
                                            {
                                                var splitEmail = item1.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        emails.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Primary_Contact_Email}":
                                        {
                                            if (item1.OfficialEmail != null)
                                            {
                                                var splitEmail = item1.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        emails.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Secondary_Contact_Email}":
                                        {
                                            if (item1.BackupOfficialEmail != null)
                                            {
                                                var splitEmail = item1.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        emails.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{LeadAuditor_Email}":
                                        {

                                            break;
                                        }
                                    case "{Group_Admin}":
                                        {
                                            var getGroup = _entityGroupMemberRepository.GetAll().Include(x => x.EntityGroup).Where(x => x.BusinessEntityId == item1.Id).FirstOrDefault();
                                            if (getGroup != null)
                                            {
                                                var getuser = _userRepository.FirstOrDefault(x => x.Id == getGroup.EntityGroup.UserId);

                                                var splitEmail = getuser.EmailAddress.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        emails.Add(i);
                                                    }
                                                }
                                            }
                                            break;

                                        }

                                }
                            });

                            templateCc.ForEach(x =>
                            {
                                switch (x)
                                {
                                    case "{Business_Entity_Admin_Email}":
                                        {
                                            ccemail.Add(item1.AdminEmail);
                                            break;
                                        }
                                    case "{Audit_Agency_Admin_Email}":
                                        {
                                            //var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                            //if (getbusinessadmin != null)
                                            //{
                                            //    ccemail.Add(getbusinessadmin.AdminEmail);
                                            //}
                                            break;
                                        }
                                    case "{Owner_Email}":
                                        {
                                            if (item1.Owner_Email != null)
                                            {
                                                var splitEmail = item1.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        ccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Director_Incharge_Email}":
                                        {
                                            if (item1.Director_Incharge_Email != null)
                                            {
                                                var splitEmail = item1.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        ccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{CISO_Email}":
                                        {
                                            if (item1.CISO_Email != null)
                                            {
                                                var splitEmail = item1.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        ccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Primary_Contact_Email}":
                                        {
                                            if (item1.OfficialEmail != null)
                                            {
                                                var splitEmail = item1.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        ccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Secondary_Contact_Email}":
                                        {
                                            if (item1.BackupOfficialEmail != null)
                                            {
                                                var splitEmail = item1.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        ccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{LeadAuditor_Email}":
                                        {

                                            break;
                                        }
                                    case "{Group_Admin}":
                                        {
                                            var getGroup = _entityGroupMemberRepository.GetAll().Include(x => x.EntityGroup).Where(x => x.BusinessEntityId == item1.Id).FirstOrDefault();
                                            if (getGroup != null)
                                            {
                                                var getuser = _userRepository.FirstOrDefault(x => x.Id == getGroup.EntityGroup.UserId);

                                                var splitEmail = getuser.EmailAddress.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        ccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;

                                        }
                                }
                            });

                            var auditBcc = checkteamplate.TemplateBcc;
                            List<string> auditBccList = checkteamplate.TemplateBcc.Split(',').ToList();
                            List<string> templateBcc = new List<string>();

                            auditBccList.ForEach(emailid =>
                            {
                                if (emailid.Contains("{"))
                                {
                                    templateBcc.Add("{" + emailid.Split('{', '}')[1] + "}");
                                    //  auditTo = auditTo.Replace("{" + auditTo.Split('{', '}')[1] + "}", "");
                                }
                                else
                                {
                                    string email = emailid.Trim();
                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                    if (isEmail == true)
                                    {
                                        bccemail.Add(email);
                                    }
                                }
                            });

                            templateBcc.ForEach(x =>
                            {
                                switch (x)
                                {
                                    case "{Business_Entity_Admin_Email}":
                                        {
                                            bccemail.Add(item1.AdminEmail);
                                            break;
                                        }
                                    case "{Audit_Agency_Admin_Email}":
                                        {

                                            break;
                                        }
                                    case "{Owner_Email}":
                                        {
                                            if (item1.Owner_Email != null)
                                            {
                                                var splitEmail = item1.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        bccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Director_Incharge_Email}":
                                        {
                                            if (item1.Director_Incharge_Email != null)
                                            {
                                                var splitEmail = item1.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        bccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{CISO_Email}":
                                        {
                                            if (item1.CISO_Email != null)
                                            {
                                                var splitEmail = item1.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        bccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Primary_Contact_Email}":
                                        {
                                            if (item1.OfficialEmail != null)
                                            {
                                                var splitEmail = item1.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        bccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Secondary_Contact_Email}":
                                        {
                                            if (item1.BackupOfficialEmail != null)
                                            {
                                                var splitEmail = item1.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        bccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{LeadAuditor_Email}":
                                        {

                                            break;
                                        }
                                    case "{Group_Admin}":
                                        {
                                            var getGroup = _entityGroupMemberRepository.GetAll().Include(x => x.EntityGroup).Where(x => x.BusinessEntityId == item1.Id).FirstOrDefault();
                                            if (getGroup != null)
                                            {
                                                var getuser = _userRepository.FirstOrDefault(x => x.Id == getGroup.EntityGroup.UserId);

                                                var splitEmail = getuser.EmailAddress.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        bccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;

                                        }
                                }

                            });

                            List<string> templateBody = new List<string>();
                            var auditprojectBody = checkteamplate.TemplateBody;

                            auditbody = checkteamplate.TemplateBody.ToString();

                            while (auditprojectBody.Contains("{"))
                            {
                                templateBody.Add("{" + auditprojectBody.Split('{', '}')[1] + "}");
                                auditprojectBody = auditprojectBody.Replace("{" + auditprojectBody.Split('{', '}')[1] + "}", "");
                            };
                            auditbody = ReplaceValueFunction(item1, templateBody, auditbody);

                            _userEmailer.EntityInformNotification(emails, ccemail, bccemail, AuditEmailsubject, (int)item1.TenantId, auditbody, null,
                              null);
                        });


                    }
                    else
                    {
                        throw new UserFriendlyException("Please configure the template");
                    }
                }

                return isverfied;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Please configure the template");
            }


        }

        private string ReplaceValueFunction(BusinessEntity item, List<string> input, string output)
        {
            var Feedbacksubject = output;
            var getbusinessentity = item;

            var mailMessage = new StringBuilder();
            input.ForEach(x =>
            {
                switch (x)
                {

                    case "{Link}":
                        {
                            var link = AppUrlService.CreateFeedbackSubmitUrlFormat(AbpSession.TenantId.Value, 0);
                            var temp = link.Split("/account/");
                            link = "" + temp[0] + "/#/account/" + temp[1];
                            if (!link.IsNullOrEmpty())
                            {
                                //  link = EncryptauditProjectQueryParameters(link);
                            }
                            if (!link.IsNullOrEmpty())
                            {
                                //link = EncryptQueryParameters(link);
                                mailMessage.AppendLine("<br />");
                                //  mailMessage.AppendLine("<a style=\"" + _emailButtonStyle + "\" bg-color=\"" + _emailButtonColor + "\" href=\"" + link + "\">" + L("SubmitFeedback") + "</a>");
                                mailMessage.AppendLine("<br />");
                                mailMessage.AppendLine("<br />");
                                mailMessage.AppendLine("<br />");
                                mailMessage.AppendLine("<span style=\"font-size: 9pt;\">" + L("EmailMessage_CopyTheLinkBelowToYourBrowser") + "</span><br />");
                                mailMessage.AppendLine("<span style=\"font-size: 8pt;\">" + link + "</span>");
                            }

                            Feedbacksubject = Feedbacksubject.Replace("{Link}", mailMessage.ToString());
                            break;
                        }
                }

            });

            return Feedbacksubject;
        }


        [AbpAllowAnonymous]
        public async Task<PreRegisterBusinessEntityInputDto> VerifyBusinessEnity(PreRegisterBusinessEntityInputDto input)
        {
            try
            {
                var data = await _preRegisterEntityRepository.FirstOrDefaultAsync(v => v.VerificationCode == input.VerificationCode);
                if (data != null)
                {
                    if (!data.IsVerificationDone)
                    {
                        var op = ObjectMapper.Map<PreRegisterBusinessEntityInputDto>(data);
                        op.ThirdParties = await GetDynamicEntityDatabyName("Third Parties");
                        return op;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                { return null; }
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [AbpAllowAnonymous]
        public async Task<List<DynamicNameValueDto>> GetDynamicEntityDatabyName(string dynamicEntityName)
        {
            var getIndustrySectors = new List<DynamicNameValueDto>();
            try
            {
                var getcheckId = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == dynamicEntityName.Trim().ToLower());
                if (getcheckId != null)
                {

                    var getother = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getcheckId.Id)
                        .Select(x => new DynamicNameValueDto()
                        {
                            Id = x.Id,
                            Name = x.Value,
                        }).ToListAsync();
                    if (getother.Count() != 0)
                    {
                        getIndustrySectors = ObjectMapper.Map<List<DynamicNameValueDto>>(getother);
                    }
                    return getIndustrySectors;
                }
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return getIndustrySectors;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_PreRegistration_Edit)]
        public async Task<PreRegisterBusinessEntityInputDto> GetPreRegEntryForEdit(int id)
        {
            try
            {
                var data = await _preRegisterEntityRepository.FirstOrDefaultAsync(v => v.Id == id);
                var op = ObjectMapper.Map<PreRegisterBusinessEntityInputDto>(data);
                op.ThirdParties = await GetDynamicEntityDatabyName("Third Parties");
                return op;
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_PreRegistration_Edit)]
        public async Task SavePreRegEntry(PreRegisterBusinessEntityInputDto input)
        {
            try
            {
                var data = ObjectMapper.Map<PreRegisterBusinessEntity>(input);
                data.TenantId = AbpSession.TenantId;
                data.ThirdPartyId = data.ThirdPartyId == 0 ? null : data.ThirdPartyId;
                data.DistrictId = data.DistrictId == 0 ? null : data.DistrictId;
                data.FacilitySubTypeId = data.FacilitySubTypeId == 0 ? null : data.FacilitySubTypeId;
                data.FacilityTypeId = data.FacilityTypeId == 0 ? null : data.FacilityTypeId;
                await _preRegisterEntityRepository.InsertOrUpdateAsync(data);
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_PreRegistration_Delete)]
        public async Task DeletePreRegEntry(int id)
        {
            try
            {
                var entry = await _preRegisterEntityRepository.FirstOrDefaultAsync(id);
                await _preRegisterEntityRepository.HardDeleteAsync(entry);
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_PreRegistration_Delete)]

        public async Task DeleteSelectedPreRegEntry(List<int> ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    var entry = await _preRegisterEntityRepository.FirstOrDefaultAsync(id);
                    await _preRegisterEntityRepository.HardDeleteAsync(entry);
                }
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion


        public async Task<IReadOnlyList<GetBusinessEntitiesExcelDto>> GetUserAssignEntities(EntityType type)
        {

            var userId = (long)AbpSession.UserId;

            var organisationIdList = await _userOrganizationUnitRepository.GetAll().Where(x => x.UserId == userId).Select(x => x.OrganizationUnitId).ToListAsync();

            var entityGroupMembers = await (from gm in _entityGrpMemberRepository.GetAll().Include("Members")
                                            from members in gm.Members
                                            select new EntityGroupMember
                                            {
                                                Id = members.Id,
                                                BusinessEntityId = members.BusinessEntityId
                                            }).ToListAsync();

            var list1 = _businessEntityRepository.GetAll()
                .Where(e => e.Status == EntityTypeStatus.Active)
                .Where(x => organisationIdList.Contains((long)x.OrganizationUnitId))
                .WhereIf(type == EntityType.HealthcareEntity || type == EntityType.ExternalAudit || type == EntityType.InsuranceFacilities, e => e.EntityType == type)
                .WhereIf(type == EntityType.HealthcareEntity, e => e.IsAuditableEntity == true)
                .ToList();

            //var data= list1.Where(b => !entityGroupMembers.Any(e => e.BusinessEntityId == b.Id));
            var query = list1.Where(b => !entityGroupMembers.Any(e => e.BusinessEntityId == b.Id))
                 .Select(x => new GetBusinessEntitiesExcelDto
                 {
                     BusinessEntity = new BusinessEntityDto
                     {
                         Id = x.Id,
                         Name = x.CompanyName,
                         OrganizationUnitId = x.OrganizationUnitId.Value
                     }
                 }).ToList();

            return query.AsReadOnly();
        }

        public async Task<GroupEntityOutputDto> GetGroupInfoByUserId()
        {
            GroupEntityOutputDto result = new GroupEntityOutputDto();
            var userObj = await _userRepository.GetAll().Where(x => x.Id == AbpSession.UserId && x.TenantId == AbpSession.TenantId).FirstOrDefaultAsync();
            if (userObj != null)
            {
                var EntityGroupMember = await _entityGroupMemberRepository.GetAll().Where(x => x.BusinessEntityId == userObj.BusinessEntityId).FirstOrDefaultAsync();
                if (EntityGroupMember != null)
                {
                    var EntityGroupObj = await _entityGrpMemberRepository.GetAll().Where(x => x.Id == EntityGroupMember.EntityGroupId).FirstOrDefaultAsync();
                    if (EntityGroupObj != null)
                    {
                        result.IsGroup = true;
                        if (EntityGroupObj.PrimaryEntityId == userObj.BusinessEntityId)
                            result.IsPrimary = true;
                        else
                            result.IsSecondary = true;
                    }
                }
            }

            return result;
        }
        //Change Business Entity Creation date to Assessment Date
        public async Task<List<GroupEntityPivotGridDto>> GetGroupEntityPivotGrid()
        {
            try
            {
                var result = new List<GroupEntityPivotGridDto>();

                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                result = await _assessmentRepository.GetAll().Include(x => x.BusinessEntity).Include(x => x.BusinessEntity.FacilityType).Where(x => x.Date > DateTime.Now.AddYears(-3) && x.ReviewScore != 0)
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .Select(x => new GroupEntityPivotGridDto
                    {
                        BusinessEntityId = x.Id,
                        CompanyName = x.BusinessEntity.CompanyName,
                        GroupName = x.BusinessEntity.GroupName == null ? "Other" : x.BusinessEntity.GroupName,
                        CreationTime = x.Date.ToString("yyyy"),
                        ReviewScore = ((Math.Round(Convert.ToDouble(x.ReviewScore), 2)) / 100).ToString(),
                        FacilityTypeName = x.BusinessEntity.FacilityType == null ? null : x.BusinessEntity.FacilityType.Name,
                        District = x.BusinessEntity.CityOrDisctrict,
                        Status = ((AssessmentStatus)x.Status).ToString(),
                        AssessmentName = x.Name
                    }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task SendUSerDetails(List<UserListDto> items)
        {
            try
            {

                foreach (var input in items)
                {
                    var user = await _userRepository.FirstOrDefaultAsync(v => v.Id == input.Id);
                    user.SetNewPasswordResetCode();
                    await _userEmailer.SendPasswordResetLinkByUserAsync(
                                 user,
                             AppUrlService.CreatePasswordResetUrlFormat(AbpSession.TenantId)
                            );
                }
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task OrphanEntity(long id)
        {
            try
            {
                var oldEmailId = "";
                var newEmailId = "";
                var oldUserId = 0;
                var newUserId = 0;

                string DirectorEmail = null;

                var groupEntity = await _entityGroupMemberRepository.GetAll().Include(x => x.EntityGroup).Where(x => x.BusinessEntityId == id).FirstOrDefaultAsync();

                var businessEntityObj = await _businessEntityRepository.GetAll().Include(x => x.OrganizationUnit)
                    .Include(x => x.Users).Where(x => x.Id == id).FirstOrDefaultAsync();
                if (businessEntityObj.Director_Incharge_EN != null && businessEntityObj.Director_Incharge_Email != null)
                {
                    businessEntityObj.IsOrphan = true;
                   
                    var splitEmail = businessEntityObj.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    if (splitEmail != null)
                    {
                        string email = splitEmail.Trim();
                        bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                        if (isEmail == true)
                        {
                            DirectorEmail = email;
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException("Director email not valid");
                    }

                    if (groupEntity != null)
                    {
                        if (groupEntity.EntityGroup.PrimaryEntityId != id)
                        {

                            EntityGroupMember temp = groupEntity;
                            await _entityGroupMemberRepository.DeleteAsync(temp);                            
                        }
                        else
                        {
                            throw new UserFriendlyException("Primary Entity of Group can not be convert as Orphan");
                        }
                    }

                    oldEmailId = businessEntityObj.AdminEmail;
                    newEmailId = DirectorEmail;

                    businessEntityObj.AdminEmail = DirectorEmail;
                    businessEntityObj.AdminMobile = businessEntityObj.Director_Incharge_Mobile;
                    businessEntityObj.AdminName = businessEntityObj.Director_Incharge_EN;
                    businessEntityObj.PrimaryContactName = businessEntityObj.Director_Incharge_EN;
                    businessEntityObj.ContactNumber = businessEntityObj.Director_Incharge_Mobile;
                    businessEntityObj.OfficialEmail = businessEntityObj.Director_Incharge_Email;
                    businessEntityObj.BackupContactName = businessEntityObj.Owner_EN;
                    businessEntityObj.BackupContactNumber = businessEntityObj.Owner_Mobile;
                    businessEntityObj.BackupOfficialEmail = businessEntityObj.Owner_Email;
                    businessEntityObj.GroupName = null;
                    businessEntityObj.ParentCompanyId = null;
                    businessEntityObj.ParentOrganizationId = null;
                    var BId = await _businessEntityRepository.InsertOrUpdateAndGetIdAsync(businessEntityObj);

                    var oldUserExist = await _userRepository.GetAll().Where(x => x.EmailAddress.Trim().ToLower()== oldEmailId.Trim().ToLower()).FirstOrDefaultAsync();
                    oldUserId = (int)oldUserExist.Id;

                    var NewUserExist = await _userRepository.GetAll().Where(x => x.EmailAddress.Trim().ToLower() == newEmailId.Trim().ToLower()).FirstOrDefaultAsync();

                    if (NewUserExist != null)
                    {
                        newUserId = (int)NewUserExist.Id;
                        var userOrganizationUnitObj1 = await _userOrganizationUnitRepository.GetAll()
                       .Where(x => x.OrganizationUnitId == businessEntityObj.OrganizationUnitId && x.UserId == oldUserId).FirstOrDefaultAsync();
                        if (userOrganizationUnitObj1 != null)
                        {
                            userOrganizationUnitObj1.UserId = newUserId;
                            await _userOrganizationUnitRepository.UpdateAsync(userOrganizationUnitObj1);
                        }

                        var businessEntityUserObj1 = await _businessEntityUserRepository.GetAll().Where(x => x.UserId == oldUserId && x.BusinessEntityId == businessEntityObj.Id).FirstOrDefaultAsync();
                        if (businessEntityUserObj1 != null)
                        {
                            businessEntityUserObj1.UserId = newUserId;
                            await _businessEntityUserRepository.UpdateAsync(businessEntityUserObj1);
                        }
                        else
                        {
                            BusinessEntityUser temp = new BusinessEntityUser();
                            temp.Id = 0;
                            temp.UserId = newUserId;
                            temp.BusinessEntityId = businessEntityObj.Id;
                            temp.BusinessEntity = businessEntityObj;
                            var Id = await _businessEntityUserRepository.InsertAndGetIdAsync(temp);
                        }

                        UserRole userRole = new UserRole();
                        userRole.TenantId = AbpSession.TenantId;
                        userRole.UserId = newUserId;

                        if (businessEntityObj.EntityType == EntityType.HealthcareEntity)
                        {
                            var role = await _roleRepository.GetAll().Where(x => x.Name == "Business Entity Admin").FirstOrDefaultAsync();
                            userRole.RoleId = role.Id;
                        }
                        else if (businessEntityObj.EntityType == EntityType.InsuranceFacilities)
                        {
                            var role = await _roleRepository.GetAll().Where(x => x.Name == "Insurance Entity Admin").FirstOrDefaultAsync();
                            userRole.RoleId = role.Id;
                        }

                        if (!(_userRoleRepository.GetAll().Any(x => x.UserId == userRole.UserId && x.RoleId == userRole.RoleId)))
                        {
                            var userRoleId = await _userRoleRepository.InsertAndGetIdAsync(userRole);
                        }

                        newUserId = (int)NewUserExist.Id;

                    }
                    else
                    {
                        User u = oldUserExist;
                        u.IsActive = true;
                        u.Id = 0;
                        u.UserName = DirectorEmail.Trim();
                        u.UserName = u.UserName.Replace(" ", String.Empty);
                        u.NormalizedUserName = DirectorEmail.Trim();

                        u.BusinessEntityId = businessEntityObj.Id;
                        u.BusinessEntity = businessEntityObj;
                        u.PhoneNumber = businessEntityObj.Director_Incharge_Mobile;

                        if (businessEntityObj.Director_Incharge_EN.Length > 0)
                        {
                            var nameList = businessEntityObj.Director_Incharge_EN.Split(' ');
                            u.Name = nameList.FirstOrDefault();
                            u.Surname = nameList.LastOrDefault();
                        }

                        u.EmailAddress = DirectorEmail;
                        u.NormalizedEmailAddress = DirectorEmail;
                        u.NormalizedEmailAddress = DirectorEmail;
                       
                        u.BusinessEntityId = businessEntityObj.Id;
                        u.BusinessEntity = businessEntityObj;
                        var Newid = await _userRepository.InsertAndGetIdAsync(u);

                        var userOrganizationUnitObj1 = await _userOrganizationUnitRepository.GetAll()
                      .Where(x => x.OrganizationUnitId == businessEntityObj.OrganizationUnitId && x.UserId == oldUserId).FirstOrDefaultAsync();
                        if (userOrganizationUnitObj1 != null)
                        {
                            userOrganizationUnitObj1.UserId = Newid;
                            await _userOrganizationUnitRepository.UpdateAsync(userOrganizationUnitObj1);
                        }

                        var businessEntityUserObj1 = await _businessEntityUserRepository.GetAll().Where(x => x.UserId == oldUserId && x.BusinessEntityId == businessEntityObj.Id).FirstOrDefaultAsync();
                        if (businessEntityUserObj1 != null)
                        {
                            businessEntityUserObj1.UserId = Newid;
                            await _businessEntityUserRepository.UpdateAsync(businessEntityUserObj1);
                        }
                        else
                        {
                            BusinessEntityUser temp = new BusinessEntityUser();
                            temp.Id = 0;
                            temp.UserId = Newid;
                            temp.BusinessEntityId = businessEntityObj.Id;
                            temp.BusinessEntity = businessEntityObj;
                            var Id = await _businessEntityUserRepository.InsertAndGetIdAsync(temp);
                        }


                        UserRole userRole = new UserRole();
                        userRole.TenantId = AbpSession.TenantId;
                        userRole.UserId = Newid;

                        if (businessEntityObj.EntityType == EntityType.HealthcareEntity)
                        {
                            var role = await _roleRepository.GetAll().Where(x => x.Name == "Business Entity Admin").FirstOrDefaultAsync();
                            userRole.RoleId = role.Id;
                        }
                        else if (businessEntityObj.EntityType == EntityType.InsuranceFacilities)
                        {
                            var role = await _roleRepository.GetAll().Where(x => x.Name == "Insurance Entity Admin").FirstOrDefaultAsync();
                            userRole.RoleId = role.Id;
                        }

                        var userRoleId = await _userRoleRepository.InsertAndGetIdAsync(userRole);
                        newUserId = (int)Newid;
                    }


                    // Send Mail here

                    var getpage = await _workflowpageRepository.FirstOrDefaultAsync(x => x.PageName.ToLower().Trim() == LockthreatComplianceConsts.OrphanPage.Trim().ToLower());

                    if (getpage != null)
                    {
                        var checkteamplate = await _templateRepository.FirstOrDefaultAsync(x => x.WorkFlowPageId == getpage.Id);

                        if (checkteamplate != null)
                        {
                            string auditbody = null;
                            string AuditEmailsubject = null;
                            var emails = new HashSet<string>();
                            var ccemail = new HashSet<string>();
                            var bccemail = new HashSet<string>();

                            var item1 = _businessEntityRepository.GetAll().Where(xx => xx.Id == id).FirstOrDefault();
                            List<string> templateSubject = new List<string>();
                            var auditprojectsubjectBody = checkteamplate.TemplateSubject;

                            AuditEmailsubject = checkteamplate.TemplateSubject.ToString();

                            while (auditprojectsubjectBody.Contains("{"))
                            {
                                templateSubject.Add("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}");
                                auditprojectsubjectBody = auditprojectsubjectBody.Replace("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}", "");
                            };

                            AuditEmailsubject = ReplaceValueFunction(item1, templateSubject, AuditEmailsubject);
                            var auditTemplate = checkteamplate.TemplateBody;

                            var auditTo = checkteamplate.TemplateTo;
                            List<string> auditToList = checkteamplate.TemplateTo.Split(',').ToList();
                            List<string> templatevariables = new List<string>();

                            auditToList.ForEach(emailid =>
                            {
                                if (emailid.Contains("{"))
                                {
                                    templatevariables.Add("{" + emailid.Split('{', '}')[1] + "}");
                                }
                                else
                                {
                                    string email = emailid.Trim();
                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                    if (isEmail == true)
                                    {
                                        emails.Add(email);
                                    }
                                }
                            });

                            var auditCc = checkteamplate.TemplateCc;
                            List<string> auditCcList = checkteamplate.TemplateCc.Split(',').ToList();
                            List<string> templateCc = new List<string>();

                            auditCcList.ForEach(emailid =>
                            {
                                if (emailid.Contains("{"))
                                {
                                    templateCc.Add("{" + emailid.Split('{', '}')[1] + "}");
                                    //  auditTo = auditTo.Replace("{" + auditTo.Split('{', '}')[1] + "}", "");
                                }
                                else
                                {
                                    string email = emailid.Trim();
                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                    if (isEmail == true)
                                    {
                                        ccemail.Add(email);
                                    }
                                }
                            });

                            templatevariables.ForEach(x =>
                            {
                                switch (x)
                                {
                                    case "{Business_Entity_Admin_Email}":
                                        {
                                            emails.Add(item1.AdminEmail);
                                            break;
                                        }
                                    case "{Audit_Agency_Admin_Email}":
                                        {
                                            //var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                            //if (getbusinessadmin != null)
                                            //{
                                            //    emails.Add(getbusinessadmin.AdminEmail);
                                            //}
                                            break;
                                        }
                                    case "{Owner_Email}":
                                        {
                                            if (item1.Owner_Email != null)
                                            {
                                                var splitEmail = item1.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        emails.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Director_Incharge_Email}":
                                        {
                                            if (item1.Director_Incharge_Email != null)
                                            {
                                                var splitEmail = item1.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        emails.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{CISO_Email}":
                                        {
                                            if (item1.CISO_Email != null)
                                            {
                                                var splitEmail = item1.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        emails.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Primary_Contact_Email}":
                                        {
                                            if (item1.OfficialEmail != null)
                                            {
                                                var splitEmail = item1.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        emails.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Secondary_Contact_Email}":
                                        {
                                            if (item1.BackupOfficialEmail != null)
                                            {
                                                var splitEmail = item1.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        emails.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{LeadAuditor_Email}":
                                        {

                                            break;
                                        }
                                    case "{Group_Admin}":
                                        {
                                            var getGroup = _entityGroupMemberRepository.GetAll().Include(x => x.EntityGroup).Where(x => x.BusinessEntityId == item1.Id).FirstOrDefault();
                                            if (getGroup != null)
                                            {
                                                var getuser = _userRepository.FirstOrDefault(x => x.Id == getGroup.EntityGroup.UserId);

                                                var splitEmail = getuser.EmailAddress.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        emails.Add(i);
                                                    }
                                                }
                                            }
                                            break;

                                        }
                                }
                            });

                            templateCc.ForEach(x =>
                            {
                                switch (x)
                                {
                                    case "{Business_Entity_Admin_Email}":
                                        {
                                            ccemail.Add(item1.AdminEmail);
                                            break;
                                        }
                                    case "{Audit_Agency_Admin_Email}":
                                        {
                                            //var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                            //if (getbusinessadmin != null)
                                            //{
                                            //    ccemail.Add(getbusinessadmin.AdminEmail);
                                            //}
                                            break;
                                        }
                                    case "{Owner_Email}":
                                        {
                                            if (item1.Owner_Email != null)
                                            {
                                                var splitEmail = item1.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        ccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Director_Incharge_Email}":
                                        {
                                            if (item1.Director_Incharge_Email != null)
                                            {
                                                var splitEmail = item1.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        ccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{CISO_Email}":
                                        {
                                            if (item1.CISO_Email != null)
                                            {
                                                var splitEmail = item1.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        ccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Primary_Contact_Email}":
                                        {
                                            if (item1.OfficialEmail != null)
                                            {
                                                var splitEmail = item1.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        ccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Secondary_Contact_Email}":
                                        {
                                            if (item1.BackupOfficialEmail != null)
                                            {
                                                var splitEmail = item1.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        ccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{LeadAuditor_Email}":
                                        {
                                            break;
                                        }
                                    case "{Group_Admin}":
                                        {
                                            var getGroup = _entityGroupMemberRepository.GetAll().Include(x => x.EntityGroup).Where(x => x.BusinessEntityId == item1.Id).FirstOrDefault();
                                            if (getGroup != null)
                                            {
                                                var getuser = _userRepository.FirstOrDefault(x => x.Id == getGroup.EntityGroup.UserId);

                                                var splitEmail = getuser.EmailAddress.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        ccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;

                                        }
                                }
                            });

                            var auditBcc = checkteamplate.TemplateBcc;
                            List<string> auditBccList = checkteamplate.TemplateBcc.Split(',').ToList();
                            List<string> templateBcc = new List<string>();

                            auditBccList.ForEach(emailid =>
                            {
                                if (emailid.Contains("{"))
                                {
                                    templateBcc.Add("{" + emailid.Split('{', '}')[1] + "}");
                                }
                                else
                                {
                                    string email = emailid.Trim();
                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                    if (isEmail == true)
                                    {
                                        bccemail.Add(email);
                                    }
                                }
                            });

                            templateBcc.ForEach(x =>
                            {
                                switch (x)
                                {
                                    case "{Business_Entity_Admin_Email}":
                                        {
                                            bccemail.Add(item1.AdminEmail);
                                            break;
                                        }
                                    case "{Audit_Agency_Admin_Email}":
                                        {
                                            break;
                                        }
                                    case "{Owner_Email}":
                                        {
                                            if (item1.Owner_Email != null)
                                            {
                                                var splitEmail = item1.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        bccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Director_Incharge_Email}":
                                        {
                                            if (item1.Director_Incharge_Email != null)
                                            {
                                                var splitEmail = item1.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        bccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{CISO_Email}":
                                        {
                                            if (item1.CISO_Email != null)
                                            {
                                                var splitEmail = item1.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        bccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Primary_Contact_Email}":
                                        {
                                            if (item1.OfficialEmail != null)
                                            {
                                                var splitEmail = item1.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        bccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{Secondary_Contact_Email}":
                                        {
                                            if (item1.BackupOfficialEmail != null)
                                            {
                                                var splitEmail = item1.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        bccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "{LeadAuditor_Email}":
                                        {
                                            break;
                                        }
                                    case "{Group_Admin}":
                                        {
                                            var getGroup = _entityGroupMemberRepository.GetAll().Include(x => x.EntityGroup).Where(x => x.BusinessEntityId == item1.Id).FirstOrDefault();
                                            if (getGroup != null)
                                            {
                                                var getuser = _userRepository.FirstOrDefault(x => x.Id == getGroup.EntityGroup.UserId);

                                                var splitEmail = getuser.EmailAddress.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var i in splitEmail)
                                                {
                                                    string email = i.Trim();
                                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                    if (isEmail == true)
                                                    {
                                                        bccemail.Add(i);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                }

                            });

                            List<string> templateBody = new List<string>();
                            var auditprojectBody = checkteamplate.TemplateBody;
                            auditbody = checkteamplate.TemplateBody.ToString();
                            while (auditprojectBody.Contains("{"))
                            {
                                templateBody.Add("{" + auditprojectBody.Split('{', '}')[1] + "}");
                                auditprojectBody = auditprojectBody.Replace("{" + auditprojectBody.Split('{', '}')[1] + "}", "");
                            };
                            auditbody = ReplaceValueFunction(item1, templateBody, auditbody);

                          //  await _userEmailer.EntityInformNotification(emails, ccemail, bccemail, AuditEmailsubject, (int)item1.TenantId, auditbody, null,
                         //      null);



                        }
                    }

                }
                else
                {
                    throw new UserFriendlyException("Director Info not avaliable");
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task RemoveOrphanEntityField(long id)
        {
            try
            {
                var businessEntityObj = await _businessEntityRepository.GetAll().Where(x => x.Id == id).FirstOrDefaultAsync();
                businessEntityObj.IsOrphan = false;
                var a = await _businessEntityRepository.InsertOrUpdateAndGetIdAsync(businessEntityObj);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task UpdateEntitiesProfile(UpdateEntitiesProfileDto input)
        {
            try
            {
                foreach (var item in input.BusinessEntitiesId)
                {
                    var obj = await _businessEntityRepository.FirstOrDefaultAsync(v => v.Id == item);

                    if ("" + input.CreateOrEditBusinessEntityDto.PrimaryContactName != "")
                        obj.PrimaryContactName = input.CreateOrEditBusinessEntityDto.PrimaryContactName;
                    if ("" + input.CreateOrEditBusinessEntityDto.Designation != "")
                        obj.Designation = input.CreateOrEditBusinessEntityDto.Designation;
                    if ("" + input.CreateOrEditBusinessEntityDto.ContactNumber != "")
                        obj.ContactNumber = input.CreateOrEditBusinessEntityDto.ContactNumber;
                    if ("" + input.CreateOrEditBusinessEntityDto.OfficialEmail != "")
                        obj.OfficialEmail = input.CreateOrEditBusinessEntityDto.OfficialEmail;
                    if ("" + input.CreateOrEditBusinessEntityDto.BackupContactName != "")
                        obj.BackupContactName = input.CreateOrEditBusinessEntityDto.BackupContactName;
                    if ("" + input.CreateOrEditBusinessEntityDto.BackupDesignation != "")
                        obj.BackupDesignation = input.CreateOrEditBusinessEntityDto.BackupDesignation;
                    if ("" + input.CreateOrEditBusinessEntityDto.BackupContactNumber != "")
                        obj.BackupContactNumber = input.CreateOrEditBusinessEntityDto.BackupContactNumber;
                    if ("" + input.CreateOrEditBusinessEntityDto.BackupOfficialEmail != "")
                        obj.BackupOfficialEmail = input.CreateOrEditBusinessEntityDto.BackupOfficialEmail;
                    if ("" + input.CreateOrEditBusinessEntityDto.CISO_EN != "")
                        obj.CISO_EN = input.CreateOrEditBusinessEntityDto.CISO_EN;
                    if ("" + input.CreateOrEditBusinessEntityDto.CISO_Email != "")
                        obj.CISO_Email = input.CreateOrEditBusinessEntityDto.CISO_Email;
                    if ("" + input.CreateOrEditBusinessEntityDto.CISO_Mobile != "")
                        obj.CISO_Mobile = input.CreateOrEditBusinessEntityDto.CISO_Mobile;
                    if ("" + input.CreateOrEditBusinessEntityDto.TotalPersonnel != "")
                        obj.TotalPersonnel = input.CreateOrEditBusinessEntityDto.TotalPersonnel;
                    if ("" + input.CreateOrEditBusinessEntityDto.NumberEmpWork != "")
                        obj.NumberEmpWork = input.CreateOrEditBusinessEntityDto.NumberEmpWork;
                    if ("" + input.CreateOrEditBusinessEntityDto.ITSecurityStaff != "")
                        obj.ITSecurityStaff = input.CreateOrEditBusinessEntityDto.ITSecurityStaff;
                    if ("" + input.CreateOrEditBusinessEntityDto.ContractPersonnel != "")
                        obj.ContractPersonnel = input.CreateOrEditBusinessEntityDto.ContractPersonnel;
                    if ("" + input.CreateOrEditBusinessEntityDto.SecurityAdvisoryEmailList != "")
                        obj.SecurityAdvisoryEmailList = input.CreateOrEditBusinessEntityDto.SecurityAdvisoryEmailList;

                    var updatedId = await _businessEntityRepository.UpdateAsync(obj);
                }
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


    }
}