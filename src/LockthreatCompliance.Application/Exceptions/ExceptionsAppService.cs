using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LockthreatCompliance.Exceptions.Exporting;
using LockthreatCompliance.Exceptions.Dtos;
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
using LockthreatCompliance.Assessments.Dto;
using Abp.Timing;
using LockthreatCompliance.IRMRelations;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.IRMRelations.Dtos;
using LockthreatCompliance.Authorization.Users.Dto;
using Abp.UI;
using LockthreatCompliance.Common;

namespace LockthreatCompliance.Exceptions
{
    [AbpAuthorize]
    public class ExceptionsAppService : LockthreatComplianceAppServiceBase, IExceptionsAppService
    {
        private readonly IRepository<Exception> _exceptionRepository;
        private readonly IExceptionsExcelExporter _exceptionsExcelExporter;
        private readonly IRepository<DocumentPath> _documentPathRepository;
        private readonly ApplicationSession _appSession;
        private readonly IRepository<IRMRelation, long> _irmRelationRepository;
        private readonly IRepository<IRMUserRelation, long> _irmUserRelationRepository;
        private readonly IRepository<ExceptionRemediation> _exceptionRemiationRepository;
        private readonly ICommonLookupAppService _commonlookupManagerRepository;
        public ExceptionsAppService(IRepository<DocumentPath> documentPathRepository, ICommonLookupAppService commonlookupManagerRepository,
            IRepository<Exception> exceptionRepository, IExceptionsExcelExporter exceptionsExcelExporter,
            ApplicationSession appSession, IRepository<IRMRelation, long> irmRelationRepository,
            IRepository<IRMUserRelation, long> irmUserRelationRepository, IRepository<ExceptionRemediation> exceptionRemiationRepository)
        {
            _commonlookupManagerRepository = commonlookupManagerRepository;
            _exceptionRepository = exceptionRepository;
            _exceptionsExcelExporter = exceptionsExcelExporter;
            _appSession = appSession;
            _documentPathRepository = documentPathRepository;
            _irmRelationRepository = irmRelationRepository;
            _irmUserRelationRepository = irmUserRelationRepository;
            _exceptionRemiationRepository = exceptionRemiationRepository;
        }
        [AbpAllowAnonymous]
        public async Task<IReadOnlyList<ExceptionDto>> GetAllForLookUp(int? businessEntityId)
        {
            var currentUser = await GetCurrentUserAsync();
            var result = await _exceptionRepository.GetAll().Where(e => e.BusinessEntityId == businessEntityId)
                //.WhereIf((_appSession.UserOriginType != UserOriginType.Authority && _appSession.UserOriginType != UserOriginType.admin), e => e.BusinessEntityId == currentUser.BusinessEntityId)
                //.WhereIf(_appSession.UserOriginType == UserOriginType.Authority, e => e.BusinessEntityId == businessEntityId.Value)
                .Select(e => new ExceptionDto
                {
                    Id = e.Id,
                    Title = e.Title
                })
                .ToListAsync();
            return result.AsReadOnly();
        }
        public async Task<PagedResultDto<GetExceptionForViewDto>> GetAll(GetAllExceptionsInput input)
        {
            var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();

            var filteredExceptions = _exceptionRepository.GetAll()                                                                        
                                     .Include("ExceptionType").Include(x => x.BusinessEntity).
                                     WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains((int)e.BusinessEntityId))
                                     .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter) ||  e.BusinessEntity.CompanyLegalName.Contains(input.Filter));
           
            var pagedAndFilteredExceptions = filteredExceptions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var exceptions = pagedAndFilteredExceptions.Select(o =>
                         new GetExceptionForViewDto()
                         {
                             Exception = new ExceptionDto
                             {
                                 Title = o.Title,
                                 BusinessEntityName = o.BusinessEntity.CompanyName,
                                 RequestDate = o.RequestDate,
                                 Code = o.Code,
                                 ReviewStatus = o.ReviewStatus,
                                 TypeName = o.ExceptionType.Name,
                                 Id = o.Id
                             }
                         });

            var totalCount = await filteredExceptions.CountAsync();
            return new PagedResultDto<GetExceptionForViewDto>(
                totalCount,
                await exceptions.ToListAsync()
            );
        }

        [AbpAllowAnonymous]
        public async Task<List<ExceptionDto>> GetAllException()
        {
            var query = new List<ExceptionDto>();
            try
            {

                var filteredExceptions = _exceptionRepository.GetAll()
                       .WhereIf(_appSession.UserOriginType == UserOriginType.BusinessEntity || _appSession.UserOriginType == UserOriginType.ExternalAuditor, e => e.BusinessEntityId == GetCurrentUser().BusinessEntityId)
                   .Include("ExceptionType").Include(x => x.BusinessEntity);


                query = await (filteredExceptions.Select(o =>
                              new ExceptionDto
                              {
                                  Title = o.Title,
                                  BusinessEntityName = o.BusinessEntity.CompanyName,
                                  RequestDate = o.RequestDate,
                                  Code = o.Code,
                                  ReviewStatus = o.ReviewStatus,
                                  TypeName = o.ExceptionType.Name,
                                  Id = o.Id
                              })).ToListAsync();
                return query;
            }
            catch(System.Exception ex )
            {
                throw new System.Exception(ex.Message);
            }
            
        }



        

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Exceptions_Edit)]
        public async Task<GetExceptionForEditOutput> GetExceptionForEdit(EntityDto input)
        {
            var exception = await _exceptionRepository.GetIncluding(e => e.Id == input.Id,
                "ExceptionImpactedControlRequirements",
                "ExceptionRelatedBusinessRisks",
                "ExceptionCompensatingControls",
                "Requestor",
                "Attachments",
                "IRMRelations",
                "IRMRelations.Actors",
                "SelectedExceptionRemediations",
                "ExceptionRelatedBusinessRisks",
                "RelatedIncidents",
                "RelatedFindings",
                "Requestor");

            var output = new GetExceptionForEditOutput
            {
                ReviewStatus = exception.ReviewStatus,
                RequestedDate = exception.RequestDate,
                RequestorName = exception.Requestor.Name,
                Attachments = exception.Attachments.Select(e => new AttachmentDto
                {
                    Code = e.Code,
                    FileName = e.FileName,
                    Title = e.Title
                }).ToList(),
                Exception = new CreateOrEditExceptionDto
                {
                    RequestDate = exception.RequestDate,
                    CreationTime = exception.CreationTime,
                    TypeId = exception.ExceptionTypeId,
                    ApproverId = exception.ApproverId,
                    ExpertReviewerId = exception.ExpertReviewerId,
                    BusinessEntityId = exception.BusinessEntityId.Value,
                    ApprovedTillDate = exception.ApprovedTillDate,
                    Code = exception.Code,
                    Comment = exception.ReviewComment,
                    Id = exception.Id,
                    Title = exception.Title,
                    ExceptionDetails=exception.ExceptionDetails,
                    NextReviewDate = exception.NextReviewDate,
                    CriticalityId = exception.CriticalityId,
                    ReviewPriorityId = exception.ReviewPriorityId,
                    RequestorId = exception.RequestorId,
                    RequestorUser = ObjectMapper.Map<UserListDto>(exception.Requestor),
                    CompensatingControlIds = exception.ExceptionCompensatingControls.Select(e => e.ControlRequirementId).ToList(),
                    ImpactedControlRequirementIds = exception.ExceptionImpactedControlRequirements.Select(e => e.ControlRequirementId).ToList()
                },
                SelectedExceptionRemediations = ObjectMapper.Map<List<ExceptionRemediationDto>>(exception.SelectedExceptionRemediations)
            };

            foreach (var item in exception.IRMRelations)
            {
                if (item.IRMUserType == IRMUserType.EntityUser)
                {
                    output.Exception.EntityIRMRelations = ObjectMapper.Map<IRMRelationDto>(item);
                    output.Exception.EntityIRMRelations.EntityReviewers = item.Actors.Where(a => a.EntityReviewerId != null).Select(a => a.EntityReviewerId.Value).ToList();
                    output.Exception.EntityIRMRelations.EntityReviewersSignature = item.Actors.Where(a => a.EntityReviewerId != null).Select(a => a.Signature).ToList();
                    output.Exception.EntityIRMRelations.EntityApprovers = item.Actors.Where(a => a.EntityApproverId != null).Select(a => a.EntityApproverId.Value).ToList();
                    output.Exception.EntityIRMRelations.EntityApproversSignature = item.Actors.Where(a => a.EntityApproverId != null).Select(a => a.Signature).ToList();
                    output.Exception.EntityIRMRelations.Signature = item.Actors.Where(a => a.EntityApproverId == AbpSession.UserId).Select(a => a.Signature).ToList().FirstOrDefault();
                }
                if (item.IRMUserType == IRMUserType.AuthorityUser)
                {
                    output.Exception.AuthorityIRMRelations = ObjectMapper.Map<IRMRelationDto>(item);
                    output.Exception.AuthorityIRMRelations.AuthorityReviewers = item.Actors.Where(a => a.AuthorityReviewerId != null).Select(a => a.AuthorityReviewerId.Value).ToList();
                    output.Exception.AuthorityIRMRelations.AuthorityReviewersSignature = item.Actors.Where(a => a.AuthorityReviewerId != null).Select(a => a.Signature).ToList();
                    output.Exception.AuthorityIRMRelations.AuthorityApprovers = item.Actors.Where(a => a.AuthorityApproverId != null).Select(a => a.AuthorityApproverId.Value).ToList();
                    output.Exception.AuthorityIRMRelations.AuthorityApproversSignature = item.Actors.Where(a => a.AuthorityApproverId != null).Select(a => a.Signature).ToList();
                    output.Exception.AuthorityIRMRelations.Signature = item.Actors.Where(a => a.AuthorityApproverId == AbpSession.UserId).Select(a => a.Signature).ToList().FirstOrDefault();
                }
            }

            output.Exception.ExceptionRelatedBusinessRisks = exception.ExceptionRelatedBusinessRisks.Select(b => b.BusinessRiskId).ToList();
            output.Exception.RelatedIncidents = exception.RelatedIncidents.Select(b => b.IncidentId.Value).ToList();
            output.Exception.RelatedFindings = exception.RelatedFindings.Select(b => b.FindingReportId.Value).ToList();
            output.Exception.SelectedExceptionRemediations = exception.SelectedExceptionRemediations.Select(b => b.RemediationId.Value).ToList();
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditExceptionDto input)
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

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Exceptions_Create)]
        protected virtual async Task Create(CreateOrEditExceptionDto input)
        {
            var exception = new Exception
            {
                CreationTime = Clock.Now,
                RequestorId = AbpSession.UserId.Value,
                ApproverId = input.ApproverId,
                ExpertReviewerId = input.ExpertReviewerId,
                BusinessEntityId = input.BusinessEntityId,
                Title = input.Title,
                RequestDate = input.RequestDate,
                NextReviewDate = input.NextReviewDate,
                ApprovedTillDate = input.ApprovedTillDate,
                ReviewComment = input.Comment,
                CriticalityId = input.CriticalityId,
                ExceptionDetails=input.ExceptionDetails,
                ReviewPriorityId = input.ReviewPriorityId,
                ExceptionCompensatingControls = input.CompensatingControlIds.Select(e => new ExceptionCompensatingControl
                {
                    ControlRequirementId = e,
                }).ToList(),
                ExceptionTypeId = input.TypeId,
                ExceptionImpactedControlRequirements = input.ImpactedControlRequirementIds.Select(e => new ExceptionImpactedControlRequirement
                {
                    ControlRequirementId = e
                }).ToList(),
            };
            if (AbpSession.TenantId != null)
            {
                exception.TenantId = (int?)AbpSession.TenantId;
            }

            ObjectMapper.Map(input, exception);
            var exceptionId = await _exceptionRepository.InsertAndGetIdAsync(exception);


            if (input.EntityIRMRelations != null)
            {
                input.EntityIRMRelations.ExceptionId = exceptionId;
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
                input.AuthorityIRMRelations.ExceptionId = exceptionId;
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
                    document.ExceptionId = exceptionId;
                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Exceptions_Edit)]
        protected virtual async Task Update(CreateOrEditExceptionDto input)
        {
            var exception = await _exceptionRepository.GetIncluding(e => e.Id == input.Id,
                "ExceptionImpactedControlRequirements", "ExceptionRelatedBusinessRisks",
                "ExceptionCompensatingControls", "SelectedExceptionRemediations",
                "ExceptionRelatedBusinessRisks",
                "RelatedIncidents",
                "RelatedFindings");

            ObjectMapper.Map(input, exception);
            exception.ApproverId = input.ApproverId;
            exception.ExpertReviewerId = input.ExpertReviewerId;
            exception.RequestorId = AbpSession.UserId.Value;
            exception.Title = input.Title;
            exception.ExceptionDetails = input.ExceptionDetails;
            exception.NextReviewDate = input.NextReviewDate;
            exception.ApprovedTillDate = input.ApprovedTillDate;
            exception.ReviewComment = input.Comment;
            exception.ExceptionCompensatingControls = input.CompensatingControlIds.Select(e => new ExceptionCompensatingControl
            {
                ControlRequirementId = e,
            }).ToList();
            exception.ExceptionTypeId = input.TypeId;
            exception.ExceptionImpactedControlRequirements = input.ImpactedControlRequirementIds.Select(e => new ExceptionImpactedControlRequirement
            {
                ControlRequirementId = e
            }).ToList();

            if (input.Attachments.Any())
            {
                var documents = await _documentPathRepository.GetAll()
                    .Where(e => input.Attachments.Select(x => x.Code).Any(a => a == e.Code))
                    .ToListAsync();
                foreach (var document in documents)
                {
                    document.ExceptionId = exception.Id;
                    document.Title = input.Attachments.FirstOrDefault(y => y.Code == document.Code)?.Title;
                }
            }
            if (input.EntityIRMRelations != null)
            {
                input.EntityIRMRelations.ExceptionId = exception.Id;
                input.EntityIRMRelations.TenantId = AbpSession.TenantId;
                input.EntityIRMRelations.IRMUserType = IRMUserType.EntityUser;
                var eirmId = await _irmRelationRepository.InsertOrUpdateAndGetIdAsync(ObjectMapper.Map<IRMRelation>(input.EntityIRMRelations));
                //await _irmUserRelationRepository.HardDeleteAsync(r => r.IRMRelationId == eirmId);
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
                input.AuthorityIRMRelations.ExceptionId = exception.Id;
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

        [AbpAuthorize(AppPermissions.Pages_SystemSetUp_Exceptions_Delete)]
        public async Task Delete(EntityDto input)
        {
            try
            {
                var exception = await _exceptionRepository.GetAll().Where(x => x.Id == input.Id).Include(x=>x.IRMRelations).Include(x => x.ExceptionCompensatingControls).Include(x => x.ExceptionImpactedControlRequirements).Include(x => x.ExceptionRelatedBusinessRisks).Include(x => x.RelatedFindings).Include(x => x.RelatedIncidents).Include(x => x.SelectedExceptionRemediations).FirstOrDefaultAsync();

                await _exceptionRepository.DeleteAsync(exception);
               
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch ( System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }       
        }

    }
}