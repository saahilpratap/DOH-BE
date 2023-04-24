

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LockthreatCompliance.BusinessRisks.Exporting;
using LockthreatCompliance.BusinessRisks.Dtos;
using LockthreatCompliance.Dto;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.Sessions;
using LockthreatCompliance.Storage;
using LockthreatCompliance.EntityFrameworkCore.RepositoryExtensions;
using LockthreatCompliance.IRMRelations;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.IRMRelations.Dtos;
using LockthreatCompliance.Common;
using Abp.DynamicEntityParameters;

namespace LockthreatCompliance.BusinessRisks
{
    [AbpAuthorize]
    public class BusinessRisksAppService : LockthreatComplianceAppServiceBase, IBusinessRisksAppService
    {
        private readonly IRepository<BusinessRisk> _businessRiskRepository;
        private readonly IBusinessRisksExcelExporter _businessRisksExcelExporter;
        private readonly IRepository<DocumentPath> _documentPathRepository;
        private readonly ApplicationSession _appSession;
        private readonly IRepository<IRMRelation, long> _irmRelationRepository;
        private readonly IRepository<IRMUserRelation, long> _irmUserRelationRepository;
        private readonly IRepository<BusinessRiskRemediation> _BusinessRiskRemediationRepository;
        private readonly ICommonLookupAppService _commonlookupManagerRepository;

        private readonly IRepository<DynamicParameterValue> _dynamicParameterValueRepository;
        private readonly IRepository<DynamicParameter> _dynamicParameterManager;
        private readonly IRepository<BusinessRiskStatusLog, long> _businessRiskStatusLogRepository;
        public BusinessRisksAppService(IRepository<DocumentPath> documentPathRepository,
            ApplicationSession appSession, IRepository<BusinessRisk> businessRiskRepository,
             ICommonLookupAppService commonlookupManagerRepository,
            IRepository<BusinessRiskRemediation> BusinessRiskRemediationRepository,
            IBusinessRisksExcelExporter businessRisksExcelExporter, IRepository<IRMRelation, long> irmRelationRepository,
            IRepository<IRMUserRelation, long> irmUserRelationRepository,
            IRepository<DynamicParameterValue> dynamicParameterValueRepository,
            IRepository<DynamicParameter> dynamicParameterManager,
            IRepository<BusinessRiskStatusLog, long> businessRiskStatusLogRepository)
        {
            _commonlookupManagerRepository = commonlookupManagerRepository;
            _BusinessRiskRemediationRepository = BusinessRiskRemediationRepository;
            _documentPathRepository = documentPathRepository;
            _businessRiskRepository = businessRiskRepository;
            _businessRisksExcelExporter = businessRisksExcelExporter;
            _appSession = appSession;
            _irmRelationRepository = irmRelationRepository;
            _irmUserRelationRepository = irmUserRelationRepository;
            _dynamicParameterValueRepository = dynamicParameterValueRepository;
            _dynamicParameterManager = dynamicParameterManager;
            _businessRiskStatusLogRepository = businessRiskStatusLogRepository;
        }
        [AbpAllowAnonymous]
        public async Task<IReadOnlyList<BusinessRiskDto>> GetAllForLookUp(int? businessEntityId)
        {
            var currentUser = await GetCurrentUserAsync();
            var businessRisks = await _businessRiskRepository.GetAll().Where(b => b.BusinessEntityId == businessEntityId)
                //.WhereIf((_appSession.UserOriginType != UserOriginType.Authority && _appSession.UserOriginType != UserOriginType.admin), e => e.BusinessEntityId == currentUser.BusinessEntityId.Value)
                //.WhereIf(_appSession.UserOriginType == UserOriginType.Authority, e => e.BusinessEntityId == businessEntityId.Value)
                .Select(e => new BusinessRiskDto
                {
                    Id = e.Id,
                    Title = e.Title
                }).ToListAsync();
            return businessRisks.AsReadOnly();
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessRisks)]
        public async Task<PagedResultDto<GetBusinessRiskForViewDto>> GetAll(GetAllBusinessRisksInput input)
        {
            var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();


            var filteredBusinessRisks = _businessRiskRepository.GetAll()
                                       .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count >0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                         .WhereIf(_appSession.UserOriginType == UserOriginType.BusinessEntity || _appSession.UserOriginType == UserOriginType.ExternalAuditor, e => e.BusinessEntityId == GetCurrentUser().BusinessEntityId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter) || e.Vulnerability.Contains(input.Filter) || e.RemediationPlan.Contains(input.Filter) || e.BusinessEntity.CompanyLegalName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title == input.TitleFilter)
                        .WhereIf(input.MinIdentificationDateFilter != null, e => e.IdentificationDate >= input.MinIdentificationDateFilter)
                        .WhereIf(input.MaxIdentificationDateFilter != null, e => e.IdentificationDate <= input.MaxIdentificationDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VulnerabilityFilter), e => e.Vulnerability == input.VulnerabilityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RemediationPlanFilter), e => e.RemediationPlan == input.RemediationPlanFilter)
                        .WhereIf(input.MinExpectedClosureDateFilter != null, e => e.ExpectedClosureDate >= input.MinExpectedClosureDateFilter)
                        .WhereIf(input.MaxExpectedClosureDateFilter != null, e => e.ExpectedClosureDate <= input.MaxExpectedClosureDateFilter)
                       .WhereIf(input.MinCompletionDateFilter != null, e => e.CompletionDate >= input.MinCompletionDateFilter)
                        .WhereIf(input.MaxCompletionDateFilter != null, e => e.CompletionDate <= input.MaxCompletionDateFilter)
                        .WhereIf(input.IsRemediationCompletedFilter > -1, e => (input.IsRemediationCompletedFilter == 1 && e.IsRemediationCompleted) || (input.IsRemediationCompletedFilter == 0 && !e.IsRemediationCompleted))
                        .Include(x => x.BusinessEntity);

            var pagedAndFilteredBusinessRisks = filteredBusinessRisks
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var businessRisks = from o in pagedAndFilteredBusinessRisks
                                select new GetBusinessRiskForViewDto()
                                {
                                    BusinessRisk = new BusinessRiskDto
                                    {
                                        Code = o.Code,
                                        Title = o.Title,
                                        BusinessEntityName = o.BusinessEntity.CompanyName,
                                      IdentificationDate = o.IdentificationDate,
                                        Vulnerability = o.Vulnerability,
                                        RemediationPlan = o.RemediationPlan,
                                       ExpectedClosureDate = o.ExpectedClosureDate,
                                       CompletionDate = o.CompletionDate,
                                        IsRemediationCompleted = o.IsRemediationCompleted,
                                        Id = o.Id,
                                        StatusId = o.StatusId

                                    }
                                };

            var totalCount = await filteredBusinessRisks.CountAsync();

            return new PagedResultDto<GetBusinessRiskForViewDto>(
                totalCount,
                await businessRisks.ToListAsync()
            );
        }

        public async Task<GetBusinessRiskForViewDto> GetBusinessRiskForView(int id)
        {
            var businessRisk = await _businessRiskRepository.GetAsync(id);
            var output = new GetBusinessRiskForViewDto { BusinessRisk = ObjectMapper.Map<BusinessRiskDto>(businessRisk) };
            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessRisks_Edit)]
        public async Task<GetBusinessRiskForEditOutput> GetBusinessRiskForEdit(EntityDto input)
        {
            var businessRisk = await _businessRiskRepository.GetIncluding(e => e.Id == input.Id,
                "Attachments",
                "IRMRelations",
                "IRMRelations.Actors",
                "RelatedExceptions",
                "RelatedIncidents",
                "RelatedFindings",
                "SelectedBusinessRiskRemediations",
                "BusinessRisksCompensatingControls",
                "BusinessRisksImpactedControls",
                "BusinessRisksMonitoringControls");

            var output = new GetBusinessRiskForEditOutput
            {
                BusinessRisk = ObjectMapper.Map<CreateOrEditBusinessRiskDto>(businessRisk),
                SelectedBusinessRiskRemediations = ObjectMapper.Map<List<BusinessRiskRemediationDto>>(businessRisk.SelectedBusinessRiskRemediations)
            };

            foreach (var item in businessRisk.IRMRelations)
            {
                if (item.IRMUserType == IRMUserType.EntityUser)
                {
                    output.BusinessRisk.EntityIRMRelations = ObjectMapper.Map<IRMRelationDto>(item);
                    output.BusinessRisk.EntityIRMRelations.EntityReviewers = item.Actors.Where(a => a.EntityReviewerId != null).Select(a => a.EntityReviewerId.Value).ToList();
                    output.BusinessRisk.EntityIRMRelations.EntityReviewersSignature = item.Actors.Where(a => a.EntityReviewerId != null).Select(a => a.Signature).ToList();
                    output.BusinessRisk.EntityIRMRelations.EntityApprovers = item.Actors.Where(a => a.EntityApproverId != null).Select(a => a.EntityApproverId.Value).ToList();
                    output.BusinessRisk.EntityIRMRelations.EntityApproversSignature = item.Actors.Where(a => a.EntityApproverId != null).Select(a => a.Signature).ToList();
                    output.BusinessRisk.EntityIRMRelations.Signature = item.Actors.Where(a => a.EntityApproverId == AbpSession.UserId).Select(a => a.Signature ).ToList().FirstOrDefault();
                }
                if (item.IRMUserType == IRMUserType.AuthorityUser)
                {
                    output.BusinessRisk.AuthorityIRMRelations = ObjectMapper.Map<IRMRelationDto>(item);
                    output.BusinessRisk.AuthorityIRMRelations.AuthorityReviewers = item.Actors.Where(a => a.AuthorityReviewerId != null).Select(a => a.AuthorityReviewerId.Value).ToList();
                    output.BusinessRisk.AuthorityIRMRelations.AuthorityReviewersSignature = item.Actors.Where(a => a.AuthorityReviewerId != null).Select(a => a.Signature).ToList();
                    output.BusinessRisk.AuthorityIRMRelations.AuthorityApprovers = item.Actors.Where(a => a.AuthorityApproverId != null).Select(a => a.AuthorityApproverId.Value).ToList();
                    output.BusinessRisk.AuthorityIRMRelations.AuthorityApproversSignature = item.Actors.Where(a => a.AuthorityApproverId != null).Select(a => a.Signature).ToList();
                   output.BusinessRisk.AuthorityIRMRelations.Signature = item.Actors.Where(a => a.AuthorityApproverId == AbpSession.UserId).Select(a => a.Signature).ToList().FirstOrDefault();
                }
            }

            output.BusinessRisk.RelatedExceptions = businessRisk.RelatedExceptions.Select(b => b.ExceptionId).ToList();
            output.BusinessRisk.RelatedIncidents = businessRisk.RelatedIncidents.Select(b => b.IncidentId.Value).ToList();
            output.BusinessRisk.RelatedFindings = businessRisk.RelatedFindings.Select(b => b.FindingReportId.Value).ToList();
            output.BusinessRisk.SelectedBusinessRisksCompensatingControls = businessRisk.BusinessRisksCompensatingControls.Select(b => b.ControlRequirementId).ToList();
            output.BusinessRisk.SelectedBusinessRisksImpactedControls = businessRisk.BusinessRisksImpactedControls.Select(b => b.ControlRequirementId).ToList();
            output.BusinessRisk.SelectedBusinessRisksMonitoringControls = businessRisk.BusinessRisksMonitoringControls.Select(b => b.ControlRequirementId).ToList();
            output.BusinessRisk.SelectedBusinessRiskRemediations = businessRisk.SelectedBusinessRiskRemediations.Select(b => b.RemediationId.Value).ToList();

            output.Attachments = businessRisk.Attachments.Select(e => new FindingReports.Dtos.AttachmentWithTitleDto
            {
                Code = e.Code,
                Title = e.FileName
            }).ToList();
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBusinessRiskDto input)
        {
            try
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
            catch(Exception ex)
            {

            }
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessRisks_Create)]
        protected virtual async Task Create(CreateOrEditBusinessRiskDto input)
        {

            var businessRiskStatusObj = await _dynamicParameterManager.GetAll().Where(x => x.ParameterName.ToLower().Trim() == LockthreatComplianceConsts.BusinessRiskStatus.Trim().ToLower()).FirstOrDefaultAsync();
            if (businessRiskStatusObj != null)
            {
                var getbusinessRiskInitialStatus = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == businessRiskStatusObj.Id).
                    Where(x => x.Value.ToLower().Trim() == "New (Draft)".Trim().ToLower()).FirstOrDefaultAsync();
                if (getbusinessRiskInitialStatus != null)
                {
                    input.StatusId = getbusinessRiskInitialStatus.Id;
                }
            }

            var businessRisk = ObjectMapper.Map<BusinessRisk>(input);
            if (AbpSession.TenantId != null)
            {
                businessRisk.TenantId = (int?)AbpSession.TenantId;
            }

            businessRisk.CreationTime = DateTime.Now;
           

            var businessRiskId = await _businessRiskRepository.InsertAndGetIdAsync(businessRisk);

            var businessStatusLog = await CreateBusinessRiskStatusLog(businessRiskId,(int) businessRisk.StatusId);

            if (input.EntityIRMRelations != null)
            {
                input.EntityIRMRelations.BusinessRiskId = businessRiskId;
                input.EntityIRMRelations.TenantId = AbpSession.TenantId;
                input.EntityIRMRelations.IRMUserType = IRMUserType.EntityUser;
                var eirmId = await _irmRelationRepository.InsertAndGetIdAsync(ObjectMapper.Map<IRMRelation>(input.EntityIRMRelations));

                if (input.EntityIRMRelations.EntityReviewers != null)
                {
                    foreach (var e in input.EntityIRMRelations.EntityReviewers)
                    {
                        var ERuser = new IRMUserRelation
                        {
                            EntityReviewerId = e,
                            IRMRelationId = eirmId,
                            IRMUserType = IRMUserType.EntityUser
                        };

                        await _irmUserRelationRepository.InsertAsync(ERuser);
                    }
                }
                if (input.EntityIRMRelations.EntityApprovers != null)
                {
                    foreach (var e in input.EntityIRMRelations.EntityApprovers)
                    {
                        var EAuser = new IRMUserRelation
                        {
                            EntityApproverId = e,
                            IRMRelationId = eirmId,
                            IRMUserType = IRMUserType.EntityUser
                        };
                        await _irmUserRelationRepository.InsertAsync(EAuser);
                    }
                }
            }

            if (input.AuthorityIRMRelations != null)
            {
                input.AuthorityIRMRelations.BusinessRiskId = businessRiskId;
                input.AuthorityIRMRelations.TenantId = AbpSession.TenantId;
                input.AuthorityIRMRelations.IRMUserType = IRMUserType.AuthorityUser;
                var airmId = await _irmRelationRepository.InsertAndGetIdAsync(ObjectMapper.Map<IRMRelation>(input.AuthorityIRMRelations));
                if (input.AuthorityIRMRelations.AuthorityReviewers != null)
                {
                    foreach (var e in input.AuthorityIRMRelations.AuthorityReviewers)
                    {
                        var ARuser = new IRMUserRelation
                        {
                            AuthorityReviewerId = e,
                            IRMRelationId = airmId,
                            IRMUserType = IRMUserType.AuthorityUser
                        };
                        await _irmUserRelationRepository.InsertAsync(ARuser);
                    }
                }
                if (input.AuthorityIRMRelations.AuthorityApprovers != null)
                {
                    foreach (var e in input.AuthorityIRMRelations.AuthorityApprovers)
                    {
                        var Auser = new IRMUserRelation
                        {
                            AuthorityApproverId = e,
                            IRMRelationId = airmId,
                            IRMUserType = IRMUserType.AuthorityUser
                        };
                        await _irmUserRelationRepository.InsertAsync(Auser);
                    }
                }
            }


            if (input.Attachments.Any())
            {
                var documents = await _documentPathRepository.GetAll()
                    .Where(e => input.Attachments.Select(x => x.Code).Any(a => a == e.Code))
                    .ToListAsync();
                foreach (var document in documents)
                {
                    document.BusinessRiskId = businessRiskId;
                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessRisks_Edit)]
        protected virtual async Task Update(CreateOrEditBusinessRiskDto input)
        {
            try
            {
                var businessRisk = await _businessRiskRepository.GetIncluding(e => e.Id == input.Id,
                   "RelatedExceptions",
                   "RelatedIncidents",
                   "RelatedFindings",
                   "SelectedBusinessRiskRemediations",
                   "BusinessRisksCompensatingControls",
                   "BusinessRisksImpactedControls",
                   "BusinessRisksMonitoringControls");
                ObjectMapper.Map(input, businessRisk);
                if (input.Attachments.Any())
                {
                    var documents = await _documentPathRepository.GetAll()
                        .Where(e => input.Attachments.Select(x => x.Code).Any(a => a == e.Code))
                        .ToListAsync();
                    foreach (var document in documents)
                    {
                        document.BusinessRiskId = businessRisk.Id;
                        document.Title = input.Attachments.FirstOrDefault(y => y.Code == document.Code)?.Title;
                    }
                }

                if (input.EntityIRMRelations != null)
                {
                    input.EntityIRMRelations.BusinessRiskId = businessRisk.Id;
                    input.EntityIRMRelations.TenantId = AbpSession.TenantId;
                    input.EntityIRMRelations.IRMUserType = IRMUserType.EntityUser;
                    var eirmId = await _irmRelationRepository.InsertOrUpdateAndGetIdAsync(ObjectMapper.Map<IRMRelation>(input.EntityIRMRelations));
                    // await _irmUserRelationRepository.HardDeleteAsync(r => r.IRMRelationId == eirmId);
                    var getIrmUserRelation = await _irmUserRelationRepository.GetAll().Where(x => x.IRMRelationId == eirmId).ToListAsync();
                    if (input.EntityIRMRelations.EntityReviewers != null)
                    {
                        foreach (var e in input.EntityIRMRelations.EntityReviewers)
                        {
                            foreach (var t in getIrmUserRelation)
                            {
                                if (t.EntityReviewerId == e)
                                {
                                    var getvalue = _irmUserRelationRepository.GetAll().Where(x => x.Id == t.Id).ToList().FirstOrDefault();
                                    if (getvalue != null)
                                    {
                                        getvalue.EntityReviewerId = e;
                                        getvalue.IRMRelationId = eirmId;
                                        getvalue.IRMUserType = IRMUserType.EntityUser;
                                        await _irmUserRelationRepository.UpdateAsync(getvalue);
                                    }
                                }
                            }
                        }

                        if (input.EntityIRMRelations.EntityApprovers != null)
                        {
                            foreach (var e in input.EntityIRMRelations.EntityApprovers)
                            {
                                foreach (var t in getIrmUserRelation)
                                {
                                    if (t.EntityApproverId == e)
                                    {
                                        var getvalue = _irmUserRelationRepository.GetAll().Where(x => x.Id == t.Id).ToList().FirstOrDefault();
                                        if (getvalue != null)
                                        {
                                            getvalue.EntityApproverId = e;
                                            getvalue.IRMRelationId = eirmId;
                                            if(e == AbpSession.UserId)
                                            {
                                                getvalue.Signature = input.EntityIRMRelations.Signature;
                                            }
                                            getvalue.IRMUserType = IRMUserType.EntityUser;

                                          await _irmUserRelationRepository.UpdateAsync(getvalue);
                                        }

                                    }

                                }
                            }
                        }
                    }
                }

                if (input.AuthorityIRMRelations != null)
                {
                    input.AuthorityIRMRelations.BusinessRiskId = businessRisk.Id;
                    input.AuthorityIRMRelations.TenantId = AbpSession.TenantId;
                    input.AuthorityIRMRelations.IRMUserType = IRMUserType.AuthorityUser;
                    var airmId = await _irmRelationRepository.InsertOrUpdateAndGetIdAsync(ObjectMapper.Map<IRMRelation>(input.AuthorityIRMRelations));
                    var getIrmUserRelation = await _irmUserRelationRepository.GetAll().Where(x => x.IRMRelationId == airmId).ToListAsync();
                    if (input.AuthorityIRMRelations.AuthorityReviewers != null)
                    {
                        foreach (var e in input.AuthorityIRMRelations.AuthorityReviewers)
                        {
                            foreach (var t in getIrmUserRelation)
                            {
                                if (t.AuthorityReviewerId == e)
                                {
                                    var getvalue = _irmUserRelationRepository.GetAll().Where(x => x.Id == t.Id).ToList().FirstOrDefault();
                                    if (getvalue != null)
                                    {

                                        getvalue.AuthorityReviewerId = e;
                                        getvalue.IRMRelationId = airmId;   
                                        
                                        getvalue.IRMUserType = IRMUserType.AuthorityUser;
                                     
                                         await _irmUserRelationRepository.UpdateAsync(getvalue);
                                    }
                                }
                            }
                        }
                    }
                    if (input.AuthorityIRMRelations.AuthorityApprovers != null)
                    {
                        foreach (var e in input.AuthorityIRMRelations.AuthorityApprovers)
                        {
                            foreach (var t in getIrmUserRelation)
                            {
                                if (t.AuthorityApproverId == e)
                                {
                                    var getvalue = _irmUserRelationRepository.GetAll().Where(x => x.Id == t.Id).ToList().FirstOrDefault();
                                    if (getvalue != null)
                                    {
                                        getvalue.AuthorityApproverId = e;
                                        getvalue.IRMRelationId = airmId;
                                        if (e == AbpSession.UserId)
                                        {
                                          getvalue.Signature = input.AuthorityIRMRelations.Signature;
                                        }
                                        getvalue.IRMUserType = IRMUserType.AuthorityUser;

                                        await _irmUserRelationRepository.UpdateAsync(getvalue);
                                    }
                                  
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessRisks_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _businessRiskRepository.DeleteAsync(input.Id);
        }
        public async Task<FileDto> GetBusinessRisksToExcel(GetAllBusinessRisksForExcelInput input)
        {

            var filteredBusinessRisks = _businessRiskRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter) || e.Vulnerability.Contains(input.Filter) || e.RemediationPlan.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title == input.TitleFilter)
                        .WhereIf(input.MinIdentificationDateFilter != null, e => e.IdentificationDate >= input.MinIdentificationDateFilter)
                        .WhereIf(input.MaxIdentificationDateFilter != null, e => e.IdentificationDate <= input.MaxIdentificationDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VulnerabilityFilter), e => e.Vulnerability == input.VulnerabilityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RemediationPlanFilter), e => e.RemediationPlan == input.RemediationPlanFilter)
                       .WhereIf(input.MinExpectedClosureDateFilter != null, e => e.ExpectedClosureDate >= input.MinExpectedClosureDateFilter)
                       .WhereIf(input.MaxExpectedClosureDateFilter != null, e => e.ExpectedClosureDate <= input.MaxExpectedClosureDateFilter)
                       .WhereIf(input.MinCompletionDateFilter != null, e => e.CompletionDate >= input.MinCompletionDateFilter)
                        .WhereIf(input.MaxCompletionDateFilter != null, e => e.CompletionDate <= input.MaxCompletionDateFilter)
                        .WhereIf(input.IsRemediationCompletedFilter > -1, e => (input.IsRemediationCompletedFilter == 1 && e.IsRemediationCompleted) || (input.IsRemediationCompletedFilter == 0 && !e.IsRemediationCompleted));

            var query = (from o in filteredBusinessRisks
                         select new GetBusinessRiskForViewDto()
                         {
                             BusinessRisk = new BusinessRiskDto
                             {
                                 Title = o.Title,
                                IdentificationDate = o.IdentificationDate,
                                 Vulnerability = o.Vulnerability,
                                 RemediationPlan = o.RemediationPlan,
                                 ExpectedClosureDate = o.ExpectedClosureDate,
                                 CompletionDate = o.CompletionDate,
                                 IsRemediationCompleted = o.IsRemediationCompleted,
                                 Id = o.Id,
                                 StatusId = o.StatusId
                             }
                         });


            var businessRiskListDtos = await query.ToListAsync();

            return _businessRisksExcelExporter.ExportToFile(businessRiskListDtos);
        }

        public async Task<int> CreateBusinessRiskStatusLog(int businessRiskId, int statusId) {

            var businessRiskStatusLogObj = new CreateBusinessRiskStatusLogDto()
            {
                BusinessRiskId = businessRiskId,
                ActionDate = DateTime.Now,
                StatusId = statusId,
                UserActedId = AbpSession.UserId
            };
            var businessRiskStatusId = await _businessRiskStatusLogRepository.InsertAsync(ObjectMapper.Map<BusinessRiskStatusLog>(businessRiskStatusLogObj));

            return businessRiskId;

        }

    }
}