using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.ExternalAssessments.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Assessments.Dto;
using System.Collections.Generic;

namespace LockthreatCompliance.ExternalAssessments
{
    public interface IExternalAssessmentsAppService : IApplicationService
    {
        Task<bool> GetcheckControlforFinding(int controlRequirementId, int externalassessmentId);
        Task GetExternalAssessment(int Id);
        Task<List<ExternalAssessmentDto>> GetAllExternalAssessmentsByBessinessEntity(int businessEntityId);
        Task<PagedResultDto<ExternalAssessmentDto>> GetAllExternalAssementByProjectId(ExtrernalAssementInput input);
        Task<PagedResultDto<ExternalAssessmentWIthPrimaryEnrityDto>> GetAll(GetAllExternalAssessmentsInput input);

        Task<GetExternalAssessmentForEditOutput> GetExternalAssessmentForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditExternalAssessmentDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetExternalAssessmentsToExcel(GetAllExternalAssessmentsForExcelInput input);

        Task<GetExternalControlRequirementForEditOutput> GetExternalAssessmentCRQuestions(int externalAssessmentId, int controlRequirementId);

        Task AddOrUpdateExternalAssessmentCRQuestions(CreateOrEditExternalAssessmentCRQuestionDto input);

        Task AddOrUpdateExternalCRQuestions(CreateOrEditExternalAssessmentCRQuestionDto input);

        Task<GetExternalControlRequirementForEditOutput> GetExternalCRQuestions(int authDocumentId, int controlRequirementId);

        Task AddExternalAssessmentWorkPaper(ExternalAssessmentAuditWorkPaperDto input);

        Task DeleteAddedExternalAssessmentWorkPaper(long id);

        Task GenerateScheduledExtAssemments(CreateOrEditExternalAssessmentDto input);
        Task SaveAssessmentReviews(SubmitAssessmentInput input, bool copyToChild);
        Task<bool> ContainSingleResponse(int input);
        Task UpdateLastResonse(ReviewDataDto input, int externalAssessmentId);
    }
}