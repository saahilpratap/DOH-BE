using LockthreatCompliance.Enums;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LockthreatCompliance.Incidents.Exporting;
using LockthreatCompliance.Incidents.Dtos;
using LockthreatCompliance.Dto;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using LockthreatCompliance.EntityFrameworkCore.RepositoryExtensions;
using LockthreatCompliance.Sessions;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.Storage;
using LockthreatCompliance.IRMRelations;
using LockthreatCompliance.IRMRelations.Dtos;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Common;
using System.Runtime.InteropServices;

namespace LockthreatCompliance.Incidents
{
    [AbpAuthorize]
    public class IncidentsAppService : LockthreatComplianceAppServiceBase, IIncidentsAppService
    {
        private readonly IRepository<Incident> _incidentRepository;
        private readonly IIncidentsExcelExporter _incidentsExcelExporter;
        private readonly IRepository<DocumentPath> _documentPathRepository;
        private readonly ApplicationSession _appSession;
        private readonly IRepository<IRMRelation, long> _irmRelationRepository;
        private readonly IRepository<IRMUserRelation, long> _irmUserRelationRepository;
        private readonly IRepository<IncidentRemediation> _incidentRemediationrepository;
        private readonly ICommonLookupAppService _commonlookupManagerRepository;
        private readonly IRepository<IncidentStatusLog, long> _incidentStatusLogRepository;

        public IncidentsAppService(IRepository<DocumentPath> documentPathRepository, ICommonLookupAppService commonlookupManagerRepository,
            IRepository<Incident> incidentRepository, IIncidentsExcelExporter incidentsExcelExporter,
            ApplicationSession appSession, IRepository<IRMRelation, long> irmRelationRepository,
            IRepository<IRMUserRelation, long> irmUserRelationRepository, IRepository<IncidentRemediation> incidentRemediationrepository,
            IRepository<IncidentStatusLog, long> incidentStatusLogRepository)
        {
            _commonlookupManagerRepository = commonlookupManagerRepository;
            _incidentRemediationrepository = incidentRemediationrepository;
            _documentPathRepository = documentPathRepository;
            _incidentRepository = incidentRepository;
            _incidentsExcelExporter = incidentsExcelExporter;
            _appSession = appSession;
            _irmRelationRepository = irmRelationRepository;
            _irmUserRelationRepository = irmUserRelationRepository;
            _incidentStatusLogRepository = incidentStatusLogRepository;
        }
       
        [AbpAllowAnonymous]
        public async Task<IReadOnlyList<IncidentDto>> GetAllForLookUp(int? businessEntityId)
        {
            var currentUser = await GetCurrentUserAsync();
            var incidents = await _incidentRepository.GetAll().Where(i => i.BusinessEntityId == businessEntityId)
                //.WhereIf((_appSession.UserOriginType != UserOriginType.Authority && _appSession.UserOriginType != UserOriginType.admin), e => e.BusinessEntityId == currentUser.BusinessEntityId)
                //.WhereIf(_appSession.UserOriginType == UserOriginType.Authority, e => e.BusinessEntityId == businessEntityId.Value)
                .Select(e => new IncidentDto
                {
                    Id = e.Id,
                    Title = e.Title
                })
                .ToListAsync();
            return incidents.AsReadOnly();
        }

        public async Task<PagedResultDto<GetIncidentForViewDto>> GetAll(GetAllIncidentsInput input)
        {
            IReadOnlyList<long> userOrganizationUnits = null;
            var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
            userOrganizationUnits = await GetOrganizationUnitIds();

            var filteredIncidents = _incidentRepository.GetAll().Include("IncidentType").Include(x => x.BusinessEntity)
                .WhereIf(input.statusId != 0, e => e.Status == (IncidentStatus)input.statusId)
                .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                .WhereIf(input.TypeFilter > 0, e => e.IncidentTypeId == (int)input.TypeFilter)
                .WhereIf(input.statusId > 0, e => e.Status == (IncidentStatus)input.statusId)
               .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), a =>
                             a.Title.Contains(input.TitleFilter.Trim().ToLower()))
               .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), a =>
                             a.Title.Contains(input.DescriptionFilter.Trim().ToLower()))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), a =>
                             a.Title.Contains(input.Filter.Trim().ToLower())||a.BusinessEntity.CompanyLegalName.Contains(input.Filter.Trim().ToLower()))

               .WhereIf(input.PriorityFilter > 0, e => e.Priority == (IncidentPriority)input.PriorityFilter)
               .WhereIf(input.SeverityFilter > 0, e => e.Severity == (IncidentSeverity)input.SeverityFilter);
               

            var pagedAndFilteredIncidents = filteredIncidents
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

            var incidents = pagedAndFilteredIncidents.Select(e => new GetIncidentForViewDto
            {
                Incident = new IncidentDto
                {
                    Id = e.Id,
                    BusinessEntityName = e.BusinessEntity.CompanyName,
                    Code = e.Code,
                    Title = e.Title,
                    Description = e.Description,
                    Typename = e.IncidentType.Name,
                    Severity = e.Severity,
                    Priority = e.Priority,
                    Status = e.Status
                }
            });

            var totalCount = await filteredIncidents.CountAsync();

            return new PagedResultDto<GetIncidentForViewDto>(
                totalCount,
                await incidents.ToListAsync()
            );

        }


        [AbpAllowAnonymous]
        public async Task<List<IncidentDto>> GetIncidntPdf()
        {
            var incidents = new List<IncidentDto>();

            var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();

            var filteredIncidents = _incidentRepository.GetAll().Include("IncidentType").Include(x => x.BusinessEntity)
                .WhereIf(!getcheckUser.Isadmin, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId));

            incidents = await (filteredIncidents.Select(e => new IncidentDto
            {
                Id = e.Id,
                BusinessEntityName = e.BusinessEntity.CompanyName,
                Code = e.Code,
                Title = e.Title,
                Description = e.Description,
                Typename = e.IncidentType.Name,
                Severity = e.Severity,
                Priority = e.Priority,
                Status = e.Status
            })).ToListAsync();

            return incidents;

        }


        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Incidents_Edit)]
        public async Task<GetIncidentForEditOutput> GetIncidentForEdit(EntityDto input)
        {
            var incident = await _incidentRepository.GetIncluding(e => e.Id == input.Id,
                "Reviewers",
                "Attachments",
                "IRMRelations",
                "IRMRelations.Actors",
                "SelectedIncidentRemediations",
                "RelatedBusinessRisks",
                "RelatedExceptions",
                "RelatedFindings");

            var output = new GetIncidentForEditOutput
            {
                Incident = ObjectMapper.Map<CreateOrEditIncidentDto>(incident),
                SelectedIncidentRemediations = ObjectMapper.Map<List<IncidentRemediationDto>>(incident.SelectedIncidentRemediations)
            };
            foreach (var item in incident.IRMRelations)
            {
                if (item.IRMUserType == IRMUserType.EntityUser)
                {
                    output.Incident.EntityIRMRelations = ObjectMapper.Map<IRMRelationDto>(item);
                    output.Incident.EntityIRMRelations.EntityReviewers = item.Actors.Where(a => a.EntityReviewerId != null).Select(a => a.EntityReviewerId.Value).ToList();
                    output.Incident.EntityIRMRelations.EntityReviewersSignature = item.Actors.Where(a => a.EntityReviewerId != null).Select(a => a.Signature).ToList();
                    output.Incident.EntityIRMRelations.EntityApprovers = item.Actors.Where(a => a.EntityApproverId != null).Select(a => a.EntityApproverId.Value).ToList();
                    output.Incident.EntityIRMRelations.EntityApproversSignature = item.Actors.Where(a => a.EntityApproverId != null).Select(a => a.Signature).ToList();
                    output.Incident.EntityIRMRelations.Signature = item.Actors.Where(a => a.EntityApproverId == AbpSession.UserId).Select(a => a.Signature).ToList().FirstOrDefault();
                }
                if (item.IRMUserType == IRMUserType.AuthorityUser)
                {
                    output.Incident.AuthorityIRMRelations = ObjectMapper.Map<IRMRelationDto>(item);
                    output.Incident.AuthorityIRMRelations.AuthorityReviewers = item.Actors.Where(a => a.AuthorityReviewerId != null).Select(a => a.AuthorityReviewerId.Value).ToList();
                    output.Incident.AuthorityIRMRelations.AuthorityReviewersSignature = item.Actors.Where(a => a.AuthorityReviewerId != null).Select(a => a.Signature).ToList();
                    output.Incident.AuthorityIRMRelations.AuthorityApprovers = item.Actors.Where(a => a.AuthorityApproverId != null).Select(a => a.AuthorityApproverId.Value).ToList();
                    output.Incident.AuthorityIRMRelations.AuthorityApproversSignature = item.Actors.Where(a => a.AuthorityApproverId != null).Select(a => a.Signature).ToList();
                    output.Incident.AuthorityIRMRelations.Signature = item.Actors.Where(a => a.AuthorityApproverId == AbpSession.UserId).Select(a => a.Signature).ToList().FirstOrDefault();
                }
            }

            output.Incident.RelatedBusinessRisks = incident.RelatedBusinessRisks.Select(b => b.BusinessRiskId).ToList();
            output.Incident.RelatedExceptions = incident.RelatedExceptions.Select(b => b.ExceptionId).ToList();
            output.Incident.RelatedFindings = incident.RelatedFindings.Select(b => b.FindingReportId.Value).ToList();
            output.Incident.SelectedIncidentRemediations = incident.SelectedIncidentRemediations.Select(b => b.RemediationId.Value).ToList();

            output.Attachments = incident.Attachments.Select(e => new FindingReports.Dtos.AttachmentWithTitleDto
            {
                Code = e.Code,
                Title = e.Title
            }).ToList();
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditIncidentDto input)
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

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Incidents_Create)]
        protected virtual async Task Create(CreateOrEditIncidentDto input)
        {
            var incident = ObjectMapper.Map<Incident>(input);
            incident.DetectionDateTime = incident.DetectionDateTime.Date + new TimeSpan(0, 0, 0);//.AddHours(0).AddMinutes(0);
                                                                                                 //   incident.CloseDate = incident.CloseDate.Date + new TimeSpan(23, 55, 0);//.AddHours(23).AddMinutes(55);

            if (AbpSession.TenantId != null)
            {
                incident.TenantId = (int?)AbpSession.TenantId;
            }
            if (incident.Id == 0)
            {
                incident.Status = IncidentStatus.New;
            }
            var incidentId = await _incidentRepository.InsertAndGetIdAsync(incident);

            //IncidentStatusLog Table

            var CreateOrEditIncidentStatusLogObj = new CreateOrEditIncidentStatusLogDto();
            CreateOrEditIncidentStatusLogObj.Id = 0;
            CreateOrEditIncidentStatusLogObj.IncidentId = incidentId;
            CreateOrEditIncidentStatusLogObj.UserActedId = AbpSession.UserId;
            CreateOrEditIncidentStatusLogObj.Status = IncidentStatus.New;
            CreateOrEditIncidentStatusLogObj.ActionDate = DateTime.Now;
            var incidentStatusId = await _incidentStatusLogRepository.InsertOrUpdateAndGetIdAsync(ObjectMapper.Map<IncidentStatusLog>(CreateOrEditIncidentStatusLogObj));

            //End IncidentStatusLog Table

            if (input.EntityIRMRelations != null)
            {
                input.EntityIRMRelations.IncidentId = incidentId;
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
                input.AuthorityIRMRelations.IncidentId = incidentId;
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
                    document.IncidentId = incidentId;
                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Incidents_Edit)]
        protected virtual async Task Update(CreateOrEditIncidentDto input)
        {
            var incident = await _incidentRepository.GetIncluding(e => e.Id == input.Id, "Reviewers", "SelectedIncidentRemediations",
                "RelatedBusinessRisks",
                "RelatedExceptions",
                "RelatedFindings");

            ObjectMapper.Map(input, incident);
            if (input.Attachments.Any())
            {
                var documents = await _documentPathRepository.GetAll()
                    .Where(e => input.Attachments.Select(x => x.Code).Any(a => a == e.Code))
                    .ToListAsync();
                foreach (var document in documents)
                {
                    document.IncidentId = incident.Id;
                    document.Title = input.Attachments.FirstOrDefault(y => y.Code == document.Code)?.Title;
                }
            }

            if (input.EntityIRMRelations != null)
            {
                input.EntityIRMRelations.IncidentId = incident.Id;
                input.EntityIRMRelations.TenantId = AbpSession.TenantId;
                input.EntityIRMRelations.IRMUserType = IRMUserType.EntityUser;
                var eirmId = await _irmRelationRepository.InsertOrUpdateAndGetIdAsync(ObjectMapper.Map<IRMRelation>(input.EntityIRMRelations));
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
                                    if (e == AbpSession.UserId)
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

            if (input.AuthorityIRMRelations != null)
            {
                input.AuthorityIRMRelations.IncidentId = incident.Id;
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

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Incidents_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _incidentRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetIncidentsToExcel(GetAllIncidentsForExcelInput input)
        {
            var priorityFilter = (IncidentPriority)input.PriorityFilter;
            var severityFilter = (IncidentSeverity)input.SeverityFilter;

            var filteredIncidents = _incidentRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title == input.TitleFilter)
                        .WhereIf(input.PriorityFilter > -1, e => e.Priority == priorityFilter)
                        .WhereIf(input.SeverityFilter > -1, e => e.Severity == severityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var query = (from o in filteredIncidents
                         select new GetIncidentForViewDto()
                         {
                             Incident = new IncidentDto
                             {
                                 Title = o.Title,
                                 Typename = o.IncidentType.Name,
                                 Priority = o.Priority,
                                 Severity = o.Severity,
                                 Description = o.Description,
                                 Id = o.Id
                             }
                         });


            var incidentListDtos = await query.ToListAsync();

            return _incidentsExcelExporter.ExportToFile(incidentListDtos);
        }

        public async Task CreateOrUpdateIncidentStatusLog(CreateOrEditIncidentStatusLogDto input)
        {
            var incidentStatusId = await _incidentStatusLogRepository.InsertOrUpdateAndGetIdAsync(ObjectMapper.Map<IncidentStatusLog>(input));
        }

        public async Task<List<IdAndName>> GetIncidentStatusList()
        {
            var result = new List<IdAndName>();

            Array enumValueArray = Enum.GetValues(typeof(IncidentStatus));
            foreach (int enumValue in enumValueArray)
            {
                result.Add(new IdAndName() { Id = enumValue, Name = Enum.GetName(typeof(IncidentStatus), enumValue) });
            }

            return result;
        }
    }
}