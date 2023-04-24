

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LockthreatCompliance.AuthoritativeDocuments.Exporting;
using LockthreatCompliance.AuthoritativeDocuments.Dtos;
using LockthreatCompliance.Dto;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using LockthreatCompliance.AuthoritityDepartments;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using Abp.DynamicEntityParameters;
using Abp.UI;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.Enums;
using LockthreatCompliance.Domains.Dtos;
using LockthreatCompliance.Domains;
using LockthreatCompliance.AssessmentSchedules.InternalAsssementSchedules;
using LockthreatCompliance.ExternalAssessments;

namespace LockthreatCompliance.AuthoritativeDocuments
{
    [AbpAuthorize]
    public class AuthoritativeDocumentsAppService : LockthreatComplianceAppServiceBase, IAuthoritativeDocumentsAppService
    {
        private readonly IRepository<AuthoritativeDocument> _authoritativeDocumentRepository;
        private readonly IAuthoritativeDocumentsExcelExporter _authoritativeDocumentsExcelExporter;
        private readonly IRepository<AuthorityDepartment> _authorityDeaprtmentRepository;
        private readonly IRepository<DynamicParameterValue> _dynamicParameterValueRepository;
        private readonly IRepository<DynamicParameter> _dynamicParameterManager;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IRepository<AuthoritativeDocumentRelatedSelf> _authoritativeDocumentRelatedSelfRepository;
        private readonly IRepository<AuthoritativeDocumentAuditType> _authoritativeDocumentAuditTypeRepository;
        private readonly IRepository<Domain> _domainRepository;
        private readonly IRepository<InternalAssessmentScheduleDetail> _internalAssessmentScheduleDetailRepository;
        private readonly IRepository<ExternalAssessmentAuthoritativeDocument> _externalAssessmentAuthoritativeDocumentRepository;

        public AuthoritativeDocumentsAppService(IRepository<AuthoritativeDocument> authoritativeDocumentRepository,
            IRepository<AuthoritativeDocumentRelatedSelf> authoritativeDocumentRelatedSelfRepository,
            IRepository<AuthoritativeDocumentAuditType> authoritativeDocumentAuditTypeRepository,
            IAuthoritativeDocumentsExcelExporter authoritativeDocumentsExcelExporter,
            IRepository<BusinessEntity> businessEntityRepository,
            IRepository<InternalAssessmentScheduleDetail> internalAssessmentScheduleDetailRepository,
            IRepository<ExternalAssessmentAuthoritativeDocument> externalAssessmentAuthoritativeDocumentRepository,
            IRepository<DynamicParameterValue> dynamicParameterValueRepository, IRepository<DynamicParameter> dynamicParameterManager,
            IRepository<AuthorityDepartment> authorityDeaprtmentRepository,
            IRepository<Domain> domainRepository)
        {
            _authoritativeDocumentRelatedSelfRepository = authoritativeDocumentRelatedSelfRepository;
            _authoritativeDocumentAuditTypeRepository = authoritativeDocumentAuditTypeRepository;
            _businessEntityRepository = businessEntityRepository;
            _dynamicParameterValueRepository = dynamicParameterValueRepository;
            _dynamicParameterManager = dynamicParameterManager;
            _authoritativeDocumentRepository = authoritativeDocumentRepository;
            _authoritativeDocumentsExcelExporter = authoritativeDocumentsExcelExporter;
            _authorityDeaprtmentRepository = authorityDeaprtmentRepository;
            _domainRepository = domainRepository;
            _internalAssessmentScheduleDetailRepository = internalAssessmentScheduleDetailRepository;
            _externalAssessmentAuthoritativeDocumentRepository = externalAssessmentAuthoritativeDocumentRepository;
            
        }

        public async Task<PagedResultDto<GetAuthoritativeDocumentForViewDto>> GetAll(GetAllAuthoritativeDocumentsInput input)
        {
            try
            {
                var userId = AbpSession.UserId;
                var query = _authoritativeDocumentRepository.GetAll().Include(d => d.AuthorityDepartment).Include(x => x.DocumentType).Include(x=>x.Category)
                           .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter.Trim().ToLower()) || e.AuthorityDepartment.Name.Contains(input.Filter.Trim().ToLower()))
                              .WhereIf(!input.AuthDocuName.IsNullOrWhiteSpace(),
                         u => u.Name.Contains(input.AuthDocuName.Trim().ToLower()));
                var pagedAndFilteredAuthoritativeDocuments = query
                    .OrderBy(input.Sorting)
                    .PageBy(input);
                var authoritativeDocuments = pagedAndFilteredAuthoritativeDocuments.Select(doc => new GetAuthoritativeDocumentForViewDto
                {
                    AuthoritativeDocument = ObjectMapper.Map<AuthoritativeDocumentDto>(doc),
                    DepartmentName = doc.AuthorityDepartment == null ? "" : doc.AuthorityDepartment.Name.ToString(),
                });

                return new PagedResultDto<GetAuthoritativeDocumentForViewDto>(
                     await query.CountAsync(),
                    await authoritativeDocuments.ToListAsync()
                );
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<GetAuthoritativeDocumentForViewDto>> GetAllAuthoritativeDocument()
        {
            var authoritativeDocument = await (from b in _authoritativeDocumentRepository.GetAll().Where(x=>x.Status==AuthritativeDocumentStatus.Approved)
                                               select new GetAuthoritativeDocumentForViewDto()
                                               {
                                                   AuthoritativeDocument = ObjectMapper.Map<AuthoritativeDocumentDto>(b)
                                               }).ToListAsync();


            return authoritativeDocument;
        }

        public async Task<GetAuthoritativeDocumentForViewDto> GetAuthoritativeDocumentForView(int id)
        {
            var authoritativeDocument = await _authoritativeDocumentRepository.GetAsync(id);

            var output = new GetAuthoritativeDocumentForViewDto { AuthoritativeDocument = ObjectMapper.Map<AuthoritativeDocumentDto>(authoritativeDocument) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ComplianceManagement_AuthoritativeDocuments_Edit)]
        public async Task<GetAuthoritativeDocumentForEditOutput> GetAuthoritativeDocumentForEdit(EntityDto input)
        {
            var authoritativeDocument = new GetAuthoritativeDocumentForEditOutput();
            var authoritativeDocumentById = new AuthoritativeDocument();
            try
            {
                if (input.Id > 0)
                {
                    authoritativeDocumentById = await _authoritativeDocumentRepository.GetAll().FirstOrDefaultAsync(p => p.Id == input.Id);
                }
                if (authoritativeDocumentById.Id > 0)
                {
                    authoritativeDocument.AuthoritativeDocument = ObjectMapper.Map<CreateOrEditAuthoritativeDocumentDto>(authoritativeDocumentById);
                    authoritativeDocument.SelectedAuthoritativeDocumentAuditTypes = ObjectMapper.Map<List<AuthoritativeDocumentAuditTypeDto>>(await _authoritativeDocumentAuditTypeRepository.GetAll().Where(p => p.AuthoritativeDocumentId == authoritativeDocumentById.Id).ToListAsync());
                    authoritativeDocument.SelectedAuthoritativeDocumentRelatedSelfs = ObjectMapper.Map<List<AuthoritativeDocumentRelatedSelfDto>>(await _authoritativeDocumentRelatedSelfRepository.GetAll().Where(p => p.AuthoritativeDocumentId == authoritativeDocumentById.Id).ToListAsync());
                }
                //var authoritativeDocument = await _authoritativeDocumentRepository.FirstOrDefaultAsync(input.Id);
                //var output = new GetAuthoritativeDocumentForEditOutput
                //{ AuthoritativeDocument = ObjectMapper.Map<CreateOrEditAuthoritativeDocumentDto>(authoritativeDocument) };

                return authoritativeDocument;
            }
            catch(Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task CreateOrEdit(CreateOrEditAuthoritativeDocumentDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ComplianceManagement_AuthoritativeDocuments_Create)]
        protected virtual async Task Create(CreateOrEditAuthoritativeDocumentDto input)
        {
            var authoritativeDocument = ObjectMapper.Map<AuthoritativeDocument>(input);


            if (AbpSession.TenantId != null)
            {
                authoritativeDocument.TenantId = (int?)AbpSession.TenantId;
            }


            await _authoritativeDocumentRepository.InsertAsync(authoritativeDocument);
        }

        [AbpAuthorize(AppPermissions.Pages_ComplianceManagement_AuthoritativeDocuments_Edit)]
        protected virtual async Task Update(CreateOrEditAuthoritativeDocumentDto input)
        {
            if (input.Id > 0)
            {
                if (AbpSession.TenantId != null)
                {
                    input.TenantId = (int?)AbpSession.TenantId;
                }
                if (input.RemovedAuthoritativeDocumentAuditType != null)
                {
                    foreach (var unitId in input.RemovedAuthoritativeDocumentAuditType)
                    {
                        bool exist = _authoritativeDocumentAuditTypeRepository.GetAll().Any(t => t.Id == unitId);
                        if (exist)
                        {
                            await RemovedAuthoritativeDocumentAuditType(unitId);
                        }
                    }
                }

                if (input.RemovedAuthoritativeDocumentRelatedSelf != null)
                {
                    foreach (var ext in input.RemovedAuthoritativeDocumentRelatedSelf)
                    {
                        bool exist = _authoritativeDocumentRelatedSelfRepository.GetAll().Any(t => t.Id == ext);
                        if (exist)
                        {
                            await RemovedAuthoritativeDocumentRelatedSelf(ext);
                        }
                    }
                }             
            }

            await _authoritativeDocumentRepository.InsertOrUpdateAsync(ObjectMapper.Map<AuthoritativeDocument>(input));

          //  var authoritativeDocument = await _authoritativeDocumentRepository.FirstOrDefaultAsync((int)input.Id);
          //  ObjectMapper.Map(input, authoritativeDocument);
        }


        public async Task RemovedAuthoritativeDocumentAuditType (int id)
        {
            try
            {
                var auditType  = await _authoritativeDocumentAuditTypeRepository.FirstOrDefaultAsync(e => e.Id == id);
                await _authoritativeDocumentAuditTypeRepository.DeleteAsync(auditType);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task RemovedAuthoritativeDocumentRelatedSelf (int id)
        {
            try
            {
                var employee = await _authoritativeDocumentRelatedSelfRepository.FirstOrDefaultAsync(e => e.Id == id);
                await _authoritativeDocumentRelatedSelfRepository.DeleteAsync(employee);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_ComplianceManagement_AuthoritativeDocuments_Delete)]
        public async Task Delete(EntityDto input)
        {

            var check = _internalAssessmentScheduleDetailRepository.GetAll().Any(x => x.AuthoritativeDocumentId == input.Id);

            if (check)
            {
                throw new UserFriendlyException("The related records of the following record still exist. Please delete child records to delete this ! ");
            }
            else
            {

                var athorative = await _authoritativeDocumentRepository.GetAll().Where(p => p.Id == input.Id).Include(p => p.SelectedAuthoritativeDocumentAuditTypes).Include(x => x.SelectedAuthoritativeDocumentRelatedSelfs).FirstOrDefaultAsync();

                if (athorative.SelectedAuthoritativeDocumentAuditTypes.Count > 0)
                {
                    foreach (var item in athorative.SelectedAuthoritativeDocumentAuditTypes)
                    {
                        await RemovedAuthoritativeDocumentAuditType(item.Id);
                    }
                }

                if (athorative.SelectedAuthoritativeDocumentRelatedSelfs.Count > 0)
                {
                    foreach (var item in athorative.SelectedAuthoritativeDocumentRelatedSelfs)
                    {
                        await RemovedAuthoritativeDocumentRelatedSelf(item.Id);
                    }
                }

                //  await _authoritativeDocumentRepository.DeleteAsync(athorative);

                await _authoritativeDocumentRepository.DeleteAsync(input.Id);
            }
        }

        public async Task<FileDto> GetAuthoritativeDocumentsToExcel(GetAllAuthoritativeDocumentsForExcelInput input)
        {

            var filteredAuthoritativeDocuments = _authoritativeDocumentRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var query = (from o in filteredAuthoritativeDocuments
                         select new GetAuthoritativeDocumentForViewDto()
                         {
                             AuthoritativeDocument = new AuthoritativeDocumentDto
                             {
                                 Code = o.Code,
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });


            var authoritativeDocumentListDtos = await query.ToListAsync();

            return _authoritativeDocumentsExcelExporter.ExportToFile(authoritativeDocumentListDtos);
        }

        [AbpAllowAnonymous]
        public async Task<List<DynamicNameValueDto>> GetDynamicEntityDocumentType(string dynamicEntityName)
        {
            var documentType = new List<DynamicNameValueDto>();
            try
            {
                var getcheckId = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == dynamicEntityName);
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
                        documentType = ObjectMapper.Map<List<DynamicNameValueDto>>(getother);
                    }
                    return documentType;
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
            return documentType;
        }


        [AbpAllowAnonymous]
        public async Task<List<DynamicNameValueDto>> GetDynamicEntityCategory(string dynamicEntityName)
        {
            var Category = new List<DynamicNameValueDto>();
            try
            {
                var getcheckId = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == dynamicEntityName);
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
                        Category = ObjectMapper.Map<List<DynamicNameValueDto>>(getother);
                    }
                    return Category;
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
            return Category;
        }

        [AbpAllowAnonymous]
        public async Task<List<DynamicNameValueDto>> GetDynamicEntityAuditType(string dynamicEntityName)
        {
            var auditType = new List<DynamicNameValueDto>();
            try
            {
                var getcheckId = _dynamicParameterManager.FirstOrDefault(x => x.ParameterName.ToLower().Trim() == dynamicEntityName);
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
                        auditType = ObjectMapper.Map<List<DynamicNameValueDto>>(getother);
                    }
                    return auditType;
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
            return auditType;
        }


        [AbpAllowAnonymous]
        public async Task<List<BusinessEntityDto>> GetAllBusinessEntity()
        {
            try
            {
                var businessentity = new List<BusinessEntityDto>();
                var getother = await _businessEntityRepository.GetAll().Where(l => l.EntityType == EntityType.ExternalAudit && l.IsActive == true && l.IsSuspended == false)
                       .Select(x => new BusinessEntityDto()
                       {
                           Id = x.Id,
                           Name = x.CompanyName,
                       }).ToListAsync();
                if (getother.Count() != 0)
                {
                    businessentity = ObjectMapper.Map<List<BusinessEntityDto>>(getother);
                }
                return businessentity;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [AbpAllowAnonymous]
        public async Task<List<AuthoritativeDocumentListDto>> GetAllAuthorativeDocument()
        {
            try
            {
                var getbusinesslookup  = new List<AuthoritativeDocumentListDto>();

                var getother = await _dynamicParameterValueRepository.GetAll().Where(l => l.Value.ToLower().Trim() == ("Authoritative Document").ToLower().Trim()).FirstOrDefaultAsync();

                if (getother != null)
                {
                    getbusinesslookup =await  _authoritativeDocumentRepository.GetAll().Where(l => l.CategoryId == getother.Id && l.Status==AuthritativeDocumentStatus.Approved)
                           .Select(x => new AuthoritativeDocumentListDto()
                           {
                               Id = x.Id,
                               Title = x.Name,
                           }).ToListAsync();

                   
                }
                return getbusinesslookup;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<GetAuthoritativeDocumentForViewDto>> GetAllAuthoritativeDocumentsByCategogy()
         {
            var authoritativeDocument = await (from b in _authoritativeDocumentRepository.GetAll().Where(x=>x.Status==AuthritativeDocumentStatus.Approved)
                                               join d in _dynamicParameterValueRepository.GetAll()
                                               on b.CategoryId equals d.Id where d.Value== "Authoritative Document"
                                               select new GetAuthoritativeDocumentForViewDto()
                                               {
                                                   AuthoritativeDocument = ObjectMapper.Map<AuthoritativeDocumentDto>(b)
                                               }).ToListAsync();


            return authoritativeDocument;
        }

        public async Task<List<AuthoritativeDocumentDto>> GetallAuthorativeDocuments()
        {
            var query = new List<AuthoritativeDocumentDto>();
            try
            {

                // var adId =await _domainRepository.GetAll().Select(x => x.AuthoritativeDocumentId).Distinct().ToListAsync();

                query = await _authoritativeDocumentRepository.GetAll().Where(x => x.Status == AuthritativeDocumentStatus.Approved)
                              .Select(x => new AuthoritativeDocumentDto()
                              {
                                  Id=x.Id,
                                  Name=x.Name
                              }).ToListAsync();

                //query = await (from b in _authoritativeDocumentRepository.GetAll().Where(x=> adId.Contains(x.Id))
                //                                   join d in _dynamicParameterValueRepository.GetAll()
                //                                   on b.CategoryId equals d.Id
                //                                   where d.Value == "Authoritative Document"
                //                                   select new AuthoritativeDocumentDto()
                //                                   {
                //                                       Id = b.Id,
                //                                       Name=b.Name
                //                                  }).ToListAsync();

                return query;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<IdNameDto>> GetAllAuthoritativeDocuments()
        {
            var query = await _authoritativeDocumentRepository.GetAll().ToListAsync();
            return ObjectMapper.Map<List<IdNameDto>>(query);
        }
    }
}