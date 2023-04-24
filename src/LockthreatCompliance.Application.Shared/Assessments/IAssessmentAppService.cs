using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Assessments.Dto;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.Domains.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.FindingReports;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.Assessments
{
    public interface IAssessmentAppService : IApplicationService
    {

        Task<AssessmentDto> GetById(EntityDto input);
        Task<PagedResultDto<AssessmentWIthPrimaryEnrityDto>> GetAllAssessment(GetAllInputFilter input);
        Task<bool> GetcheckAssessment(int assessmentId);
        Task<int> CreateAssement(CreateOrEditAssessmentInput input);
        Task<int> CreateOrEdit(CreateOrEditAssessmentInput input);

        Task SaveAssessmentReviews(SubmitAssessmentInput input, bool IsPrimary, string percentageString);

        Task<PagedResultDto<AssessmentWIthPrimaryEnrityDto>> GetAll(GetAllInputFilter input);

        Task ApproveAssessment(ApproveAssessmentInput input);

        Task<FileDto> GetAssessmentExportToExcel(GetAllInputFilter input);

        Task SaveAssessmentReviewsAsVersion(SubmitAssessmentInput input);

        Task<List<IdNameDto>> AssessmentBusinessEntity();

        Task<ReviewDataForDashboardDto> GetReviewDataByBusinessEntityId(int input);

        Task<List<DashboardDOmainGraphDto>> DashboardDOmainGraphEntityId(int input);
        Task ImportSelfAssessmentResponse(List<ImportAssessmentResponse> input, int assessmentId);
        Task SetAssessmentStatus(SetAssessmentStatusInputDto input);

        Task<List<AssessmentWithBusinessEntityNameDto>> GetCopyToChildInputOfAssessment(int input, bool flag);

        Task<int> CopyToChildAssessmentReviews(CopyToChildInputDto input, string percentageString);
        Task<BEAdminAndBEGAdminDto> GetSendToAuthorityButtonValues(int assessmentId);
        Task AcceptMultipleAgreementTerms(MultipleAssessmentAgreementResponseInputDto input);
        Task<List<AssessmentWithBusinessEntityNameDto>> GetBusinessEntityGroupWise(int input, AssessmentStatus updatedStatus);
        Task<List<AssessmentWithBusinessEntityNameDto>> GetBusinessEntityGroupWiseForSubmitForReview(int input);
        Task<SelfAssessmentEntrypOutputDto> GetEncryptAssessmentParameter(int assessmentId, bool flag);
        Task<SelfAssessmentDecryptOutputDto> GetDecriptAssessmentParameter(string encryptedAssessmentId, string encryptedFlag);
        Task<bool> GetAssessmentImportButton(int assessmentId);
        Task<int> CreateAndUpdateRequestClarification(List<AssessmentRequestClarificationDto> input);
        Task<ClarificationOutPutDto> GetAllClarificationAssessment(int assessmentId, int crqid);
        Task<ResponseAndRequestCrqIds> RequestClarificationButton(int assessmentId);
        Task<int> SetStatusAsNeedsClarification(int assessmentId);
        Task<int> CreateOfUpdateAssessmentStatusLog(int assessmentId, AssessmentStatus status);
        Task UpdateAssessmentStatusLogInitial(int input);
        Task UpdateScheduleAssessmentStatusLogInitial(int input);
        Task<string> SendNotSubmittedEmail(List<AssessmentWIthPrimaryEnrityDto> input);
        Task<List<string>> OpenFindingValidation(OpenFindingValidationInputDto input);
        Task<List<string>> OpenFindingValidationForGroup(SetAssessmentStatusInputDto input);
    }
}
