using LockthreatCompliance.ControlStandards;
using LockthreatCompliance.Enums;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LockthreatCompliance.ControlRequirements.Exporting;
using LockthreatCompliance.ControlRequirements.Dtos;
using LockthreatCompliance.Dto;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.EntityFrameworkCore.RepositoryExtensions;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.FindingReports;
using Abp.UI;

namespace LockthreatCompliance.ControlRequirements
{
    [AbpAuthorize]
    public class ControlRequirementsAppService : LockthreatComplianceAppServiceBase, IControlRequirementsAppService
    {
        private readonly IRepository<ControlRequirement> _controlRequirementRepository;
        private readonly IControlRequirementsExcelExporter _controlRequirementsExcelExporter;
        private readonly IRepository<ControlStandard> _controlStandardRepository;
        private readonly IRepository<ReviewData> _reviewDataRepository;
        private readonly IRepository<FindingReport> _findingReportRepository;


        public ControlRequirementsAppService(IRepository<ControlRequirement> controlRequirementRepository, IControlRequirementsExcelExporter controlRequirementsExcelExporter, IRepository<ControlStandard> controlStandardRepository, IRepository<ReviewData> reviewDataRepository, IRepository<FindingReport> findingReportRepository)
        {
            _controlRequirementRepository = controlRequirementRepository;
            _controlRequirementsExcelExporter = controlRequirementsExcelExporter;
            _controlStandardRepository = controlStandardRepository;
            _reviewDataRepository = reviewDataRepository;
            _findingReportRepository = findingReportRepository;

        }

        public async Task<IReadOnlyList<ControlRequirementDto>> GetAllForLookUp()
        {
            var controlRequirements = await _controlRequirementRepository.GetAll().Select(e => new ControlRequirementDto
            {
                Id = e.Id,
                Code = e.OriginalId
            }).ToListAsync();
            return controlRequirements.AsReadOnly();
        }

        [AbpAuthorize(AppPermissions.Pages_ComplianceManagement_ControlRequirements)]
        public async Task<PagedResultDto<GetControlRequirementForViewDto>> GetAll(GetAllControlRequirementsInput input)
        {
            var controlTypeFilter = (ControlType)input.ControlTypeFilter;

            var filteredControlRequirements = _controlRequirementRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.OriginalId.Contains(input.Filter.Trim().ToLower()) || e.DomainName.Contains(input.Filter.Trim().ToLower()) || e.ControlStandardName.Contains(input.Filter.Trim().ToLower()))                     
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OriginalIdFilter), e => e.OriginalId.Trim().ToLower().Contains(input.OriginalIdFilter.Trim().ToLower()))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DomainNameFilter), e => e.DomainName.Trim().ToLower().Contains(input.DomainNameFilter.Trim().ToLower()))
                        .WhereIf(input.ControlTypeFilter > -1, e => e.ControlType == controlTypeFilter)
                         .WhereIf(!string.IsNullOrWhiteSpace(input.ControlRequirementFilter), e => e.Description.Trim().ToLower().Contains(input.ControlRequirementFilter.Trim().ToLower()))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ControlStandardNameFilter), e => e.ControlStandardName.Trim().ToLower().Contains(input.ControlStandardNameFilter.Trim().ToLower()));

            var pagedAndFilteredControlRequirements = filteredControlRequirements
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var controlRequirements = pagedAndFilteredControlRequirements.Select(ent => new GetControlRequirementForViewDto
            {
                ControlRequirement = new ControlRequirementDto
                {
                    DomainName = ent.DomainName,
                    Code = ent.Code,
                    ControlStandardId = ent.ControlStandardId,
                    ControlType = ent.ControlType,
                    ControlRequirement = ent.Description,
                    Id = ent.Id,
                    OriginalId = ent.OriginalId,
                    IndustryMandated = ent.IndustryMandated
                },
                ControlStandardName = ent.ControlStandardName
            });

            var totalCount = await filteredControlRequirements.CountAsync();

            return new PagedResultDto<GetControlRequirementForViewDto>(
                totalCount,
                await controlRequirements.ToListAsync()
            );
        }




        [AbpAuthorize(AppPermissions.Pages_ComplianceManagement_ControlRequirements_Edit)]
        public async Task<GetControlRequirementForEditOutput> GetControlRequirementForEdit(EntityDto input)
        {
            var controlRequirement = await _controlRequirementRepository.GetIncluding(e => e.Id == input.Id, "RequirementQuestions.Question");
            var output = new GetControlRequirementForEditOutput
            {
                ControlStandardName = controlRequirement.ControlStandardName,
                ControlRequirement = new CreateOrEditControlRequirementDto
                {
                    Code = controlRequirement.Code,
                    ControlStandardId = controlRequirement.ControlStandardId,
                    ControlRequirement = controlRequirement.Description,
                    ControlType = controlRequirement.ControlType,
                    Id = controlRequirement.Id,
                    OriginalId = controlRequirement.OriginalId,
                    IndustryMandated = controlRequirement.IndustryMandated,
                    Iscored=controlRequirement.Iscored,
                    RequirementQuestions = controlRequirement.RequirementQuestions.Select(e => new RequirementQuestionDto
                    {
                        QuestionDescription = e.Question.Description,
                        QuestionId = e.QuestionId
                    }).ToList()

                }
            };
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditControlRequirementDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ComplianceManagement_ControlRequirements_Create)]
        protected virtual async Task Create(CreateOrEditControlRequirementDto input)
        {
            var controlStandard = await _controlStandardRepository.FirstOrDefaultAsync(input.ControlStandardId);
            var controlRequirement = new ControlRequirement
            {
                ControlStandardId = input.ControlStandardId,
                ControlStandardName = controlStandard.Name,
                AuthoritativeDocumentId = controlStandard.AuthoritativeDocumentId,
                ControlType = input.ControlType,
                Description = input.ControlRequirement,
                DomainName = controlStandard.DomainName,
                OriginalId = input.OriginalId,
                IndustryMandated = input.IndustryMandated,
                Iscored = input.Iscored,

                RequirementQuestions = input.RequirementQuestions.Select(e => new RequirementQuestion
                {
                    QuestionId = e.QuestionId,
                    TenantId = AbpSession.TenantId
                }).ToList()
            };

            if (AbpSession.TenantId != null)
            {
                controlRequirement.TenantId = (int?)AbpSession.TenantId;
            }


            await _controlRequirementRepository.InsertAsync(controlRequirement);
        }

        [AbpAuthorize(AppPermissions.Pages_ComplianceManagement_ControlRequirements_Edit)]
        protected virtual async Task Update(CreateOrEditControlRequirementDto input)
        {
            var controlRequirement = await _controlRequirementRepository.GetIncluding(e => e.Id == input.Id, "RequirementQuestions");
            var controlStandard = await _controlStandardRepository.FirstOrDefaultAsync(input.ControlStandardId);

            controlRequirement.ControlStandardId = input.ControlStandardId;
            controlRequirement.ControlStandardName = controlStandard.Name;
            controlRequirement.ControlType = input.ControlType;
            controlRequirement.Description = input.ControlRequirement;
            controlRequirement.DomainName = controlStandard.DomainName;
            controlRequirement.OriginalId = input.OriginalId;
            controlRequirement.AuthoritativeDocumentId = controlStandard.AuthoritativeDocumentId;
            controlRequirement.IndustryMandated = input.IndustryMandated;
            controlRequirement.Iscored = input.Iscored;
            controlRequirement.RequirementQuestions = input.RequirementQuestions.Select(e => new RequirementQuestion
            {
                QuestionId = e.QuestionId,
                TenantId = AbpSession.TenantId
            }).ToList();
        }

        [AbpAuthorize(AppPermissions.Pages_ComplianceManagement_ControlRequirements_Delete)]
        public async Task Delete(EntityDto input)
        {
            var check = _reviewDataRepository.GetAll().Any(x => x.ControlRequirementId == input.Id);
            var check2 = _findingReportRepository.GetAll().Any(x => x.ControlRequirementId == input.Id);
            if (check)
            {
                throw new UserFriendlyException("The related records of the following record still exist. Please delete child records to delete this ! ");

            }
            else if (check2)
            {
                throw new UserFriendlyException("The related records of the following record still exist. Please delete child records to delete this ! ");
            }
            else
            {
                await _controlRequirementRepository.DeleteAsync(input.Id);
            }
        }

        public async Task<FileDto> GetControlRequirementsToExcel(GetAllControlRequirementsForExcelInput input)
        {
            var controlTypeFilter = (ControlType)input.ControlTypeFilter;

            var filteredControlRequirements = _controlRequirementRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.OriginalId.Contains(input.Filter) || e.DomainName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OriginalIdFilter), e => e.OriginalId.ToLower().Trim().Contains(input.OriginalIdFilter.ToLower().Trim()))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DomainNameFilter), e => e.DomainName.ToLower().Trim().Contains(input.DomainNameFilter.ToLower().Trim()))
                        .WhereIf(input.ControlTypeFilter > -1, e => e.ControlType == controlTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ControlRequirementFilter), e => e.Description.Trim().ToLower().Contains(input.ControlRequirementFilter.Trim().ToLower()))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ControlStandardNameFilter), e => e.ControlStandardName.Trim().ToLower().Contains(input.ControlStandardNameFilter.Trim().ToLower()));

            var query = filteredControlRequirements.Select(ent => new ImportControlRequirementDto
            {
                
                    Code = ent.Code,
                    TenantId = ent.TenantId,
                    OriginalId = ent.OriginalId,
                    Description = ent.Description,
                    ControlStandardName = ent.ControlStandardName,                
                    DomainName = ent.DomainName,
                    ControlType = (int)ent.ControlType,
                    ControlStandardId = ent.ControlStandardId,
                    AuthoritativeDocumentId=ent.AuthoritativeDocumentId,
                    IndustryMandated=ent.IndustryMandated                  
               
            });

            var controlRequirementListDtos = await query.ToListAsync();

            return _controlRequirementsExcelExporter.ExportToFile(controlRequirementListDtos);
        }

        public async Task<List<ControlRequirementList>> GetControlRequirementLists()
        {
            try
            {
                var op = ObjectMapper.Map<List<ControlRequirementList>>(await _controlRequirementRepository.GetAll().ToListAsync());
                return op;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ControlRequirementGroup>> GetControlRequirementListByAuthDocumentId(int authDocumentId)
        {
            try
            {
                var data = ObjectMapper.Map<List<ControlRequirementList>>(await _controlRequirementRepository.GetAll().Where(c => c.AuthoritativeDocumentId == authDocumentId).OrderBy(c => c.DomainName).ToListAsync());

                var op = data.GroupBy(c => c.DomainName).Select(c =>
                  {
                      var cr = new ControlRequirementGroup();

                      cr.DomainName = c.Key;
                      cr.ControlRequirementList = c.Select(e => e).ToList();
                      return cr;
                  }).ToList();

                return op;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ControlRequirementGroup>> GetControlRequirementListByGrouping()
        {
            try
            {
                var data = ObjectMapper.Map<List<ControlRequirementList>>(await _controlRequirementRepository.GetAll().OrderBy(c => c.DomainName).ToListAsync());
                var op = data.GroupBy(c => c.DomainName).Select(c =>
                {
                    var cr = new ControlRequirementGroup();
                    cr.DomainName = c.Key;
                    cr.ControlRequirementList = c.Select(e => e).ToList();
                    return cr;
                }).ToList();
                return op;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ControlRequirementList>> GetControlRequirementList()
        {
            try
            {
                var data = ObjectMapper.Map<List<ControlRequirementList>>(await _controlRequirementRepository.GetAll().OrderBy(c => c.DomainName).ToListAsync());
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}