using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.AuthoritativeDocuments.Dtos;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.DynamicEntityParameters;
using LockthreatCompliance.Enums;
using LockthreatCompliance.FacilityTypes;
using LockthreatCompliance.Remediations.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using Abp.UI;
using LockthreatCompliance.FindingReports.Dtos;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.ExternalAssessments;

namespace LockthreatCompliance.AuditProjects
{
    public class TemplateChecklistAppService : LockthreatComplianceAppServiceBase, ITemplateChecklistAppService
    {
        private readonly IRepository<AuthoritativeDocument> _authoritativeDocumentRepository;
        private readonly IRepository<TemplateChecklist, long> _templateChecklistRepository;
        private readonly IRepository<TemplateChecklistAuthoritativeDocument> _templateChecklistAuthoritativeDocumentRepository;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private IFacilityTypesAppService  _facilityTypesAppService;
        private ICustomDynamicAppService _iCustomDynamicAppService;
        private IAuthoritativeDocumentsAppService _iAuthoritativeDocumentsAppService;
        private readonly IRepository<TemplateChecklistAttachment, long> _templateCheckRepository;
        private readonly IRepository<DynamicParameterValue> _dynamicParameterValueRepository;
        private readonly IRepository<DynamicParameter> _dynamicParameterManager;
        private readonly IRepository<AuditProject, long> _auditProjectRepository;
        private readonly IRepository<AuditProjectAuthoritativeDocument, int> _auditprojectAuthRepository;
        private readonly IRepository<ExternalAssessment> _externalAssessmentRepository;
        public TemplateChecklistAppService(IRepository<TemplateChecklist, long> templateChecklistRepository, IRepository<AuditProject, long> auditProjectRepository,
            IRepository<AuditProjectAuthoritativeDocument, int> auditprojectAuthRepository, IRepository<ExternalAssessment> externalAssessmentRepository,
            IRepository<BusinessEntity> businessEntityRepository, IRepository<TemplateChecklistAttachment, long> templateCheckRepository,
            IFacilityTypesAppService facilityTypesAppService,
            IAuthoritativeDocumentsAppService iAuthoritativeDocumentsAppService,
            ICustomDynamicAppService iCustomDynamicAppService,
            IRepository<AuthoritativeDocument> authoritativeDocumentRepository,
            IRepository<TemplateChecklistAuthoritativeDocument> templateChecklistAuthoritativeDocumentRepository,
            IRepository<DynamicParameterValue> dynamicParameterValueRepository, IRepository<DynamicParameter> dynamicParameterManager)
        {
            _externalAssessmentRepository = externalAssessmentRepository;
            _auditProjectRepository = auditProjectRepository;
            _auditprojectAuthRepository = auditprojectAuthRepository;
            _templateCheckRepository = templateCheckRepository;
            _iAuthoritativeDocumentsAppService = iAuthoritativeDocumentsAppService;
            _iCustomDynamicAppService = iCustomDynamicAppService;
            _facilityTypesAppService = facilityTypesAppService;
           _authoritativeDocumentRepository = authoritativeDocumentRepository;
            _businessEntityRepository = businessEntityRepository;
            _templateChecklistAuthoritativeDocumentRepository = templateChecklistAuthoritativeDocumentRepository;
            _templateChecklistRepository = templateChecklistRepository;
            _dynamicParameterValueRepository = dynamicParameterValueRepository;
            _dynamicParameterManager = dynamicParameterManager;
        }


        public async Task<TemplateChecklistDto> GetAllTeamplateinfo(long? templateId)
         {
            try
            {
                var templateinfo = new TemplateChecklistDto();
                var templatechech = new TemplateChecklist();
                if (templateId > 0)
                {
                    templatechech = await _templateChecklistRepository.GetAll().FirstOrDefaultAsync(p => p.Id == templateId);
                }
                if (templatechech.Id > 0)
                {
                    templateinfo = ObjectMapper.Map<TemplateChecklistDto>(templatechech);
                    templateinfo.Attachments = ObjectMapper.Map<List<AttachmentWithTitleDto>>(await _templateCheckRepository.GetAll().Where(p => p.TemplateChecklistId == templatechech.Id).ToListAsync());
                    templateinfo.AuthoritativeDocuments = ObjectMapper.Map<List<TemplateChecklistAuthoritativeDocumentDto>>(await _templateChecklistAuthoritativeDocumentRepository.GetAll().Where(p => p.TemplateChecklistId == templatechech.Id).ToListAsync());                 
                }
                 templateinfo.BusinessEntitysList = ObjectMapper.Map<List<BusinessEntitysListDto>>(await _businessEntityRepository.GetAll().Where(x => x.EntityType == EntityType.ExternalAudit && x.Status == EntityTypeStatus.Active).ToListAsync());
                 templateinfo.AuthoritativeDocumentList= ObjectMapper.Map<List<AuthoritativeDocumentListDto>>(await _authoritativeDocumentRepository.GetAll().ToListAsync());
                 templateinfo.FacilityTypeList = await _facilityTypesAppService.GetAllFacilityType();
                 templateinfo.CategoryList = await _iCustomDynamicAppService.GetDynamicEntityDatabyName("Questionnaire Category");
                 templateinfo.TemplateTypeList= await _iCustomDynamicAppService.GetDynamicEntityDatabyName("Template Type");
                return templateinfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task CreateorUpdateTemplateChecklist (TemplateChecklistDto input)
        {
            try
            {                           
                if (input.Id > 0)
                {
                    var templatecheck = await _templateChecklistRepository.
                        GetAll().
                        Include(x => x.AuthoritativeDocuments).FirstOrDefaultAsync(x => x.Id == input.Id);
                    await _templateChecklistAuthoritativeDocumentRepository.DeleteAsync(r => r.TemplateChecklistId == input.Id);
                    ObjectMapper.Map(input, templatecheck);
                   
                    if (input.Attachments.Any())
                    {
                        var documents = await _templateCheckRepository.GetAll()
                            .Where(e => input.Attachments.Select(x => x.Code).Any(a => a == e.Code))
                            .ToListAsync();
                        foreach (var document in documents)
                        {
                            document.TemplateChecklistId = templatecheck.Id;
                            document.Title = input.Attachments.FirstOrDefault(y => y.Code == document.Code)?.Title;
                        }
                    }
                }
                if (input.Id == 0)
                {
                    if (AbpSession.TenantId != null)
                    {
                        input.TenantId = (int?)AbpSession.TenantId;
                    }
                    var templateId = await _templateChecklistRepository.InsertAndGetIdAsync(ObjectMapper.Map<TemplateChecklist>(input));

                   
                    if (input.Attachments != null)
                    {
                        if (input.Attachments.Any())
                        {
                            var documents = await _templateCheckRepository.GetAll()
                                .Where(e => input.Attachments.Select(x => x.Code).Any(a => a == e.Code))
                                .ToListAsync();
                            foreach (var document in documents)
                            {

                                document.Title = input.Attachments.FirstOrDefault(y => y.Code == document.Code)?.Title;
                                document.TemplateChecklistId = templateId;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<PagedResultDto<TemplateListDto>> GetAllTemplate (GetAllTemplateChecklistInput input)
        {
            int? checkVendorId = 0;
             int AuthId = 0;
            if (input.AuditProjectId > 0 && input.AuditProjectId != null)
            {
                 checkVendorId =  _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == input.AuditProjectId).Select(x=>x.VendorId).FirstOrDefault();

                 AuthId = _auditprojectAuthRepository.GetAll().Where(x => x.AuditProjectId == input.AuditProjectId).Select(x=>x.AuthoritativeDocumentId).FirstOrDefault();
            }
                    var query = _templateChecklistRepository.GetAll().Include(x => x.FacilityType)
                                .Include(x => x.TemplateType).Include(x => x.Category).Include(x => x.Vendor).Include(x=>x.AuthoritativeDocuments)
                            .WhereIf(input.Filter != null, x => x.Title == input.Filter).
                            WhereIf(checkVendorId>0 && AuthId >0, x => x.VendorId == checkVendorId && x.AuthoritativeDocuments.Any(y=>y.AuthoritativeDocumentId==AuthId))
                            .WhereIf(input.TabId > 0, x => x.TemplateTypeId == input.TabId);

                    var keyriskindicator = await query.CountAsync();

                    var templateDto = await query
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    var templateListDto = ObjectMapper.Map<List<TemplateListDto>>(templateDto);

                    return new PagedResultDto<TemplateListDto>(
                       keyriskindicator,
                       templateListDto
                       );
                
              
               
           
        }

        public async Task DeleteTemplateCheckList (long id)
        {
            try
            {
                var teamplatecheck  = await _templateChecklistRepository.GetAllIncluding(x=>x.AuthoritativeDocuments).FirstOrDefaultAsync(a => a.Id == id);
                  if(teamplatecheck.AuthoritativeDocuments.Count > 0)
                  {
                    foreach (var item in teamplatecheck.AuthoritativeDocuments)
                    {
                        await RemoveAuthorativeDocument(item.Id);
                    }                  
                   }
                await _templateChecklistRepository.DeleteAsync(teamplatecheck);

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

        public async Task RemoveAuthorativeDocument(int id)
        {
            try
            {
                var releatedAd = await _templateChecklistAuthoritativeDocumentRepository.FirstOrDefaultAsync(e => e.Id == id);
                await _templateChecklistAuthoritativeDocumentRepository.DeleteAsync(releatedAd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }    
}
