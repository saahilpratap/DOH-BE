using Abp.Domain.Entities;
using Abp.Events.Bus;

using LockthreatCompliance.CustomExceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockthreatCompliance.Extensions;
using LockthreatCompliance.BusinessEntities.DomainEvents;
using LockthreatCompliance.AssessmentSchedules.InternalAsssementSchedules;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.EntityGroups;

namespace LockthreatCompliance.BusinessEntities
{
    [Table("Assessments")]
    public class Assessment : Entity, IMayHaveTenant, ISoftDelete
    {
        public Assessment()
        {
            Reviews = new List<ReviewData>();
            Status = AssessmentStatus.Initialized;
            ReviewScore = 0;
        }

        [NotMapped]
        public virtual string Code { get { return "ASS-" + Id.GetCodeEnding(); } }

        public int? TenantId { get; set; }

        public int BusinessEntityId { get; set; }

        public long OrganizationUnitId { get; set; }
        public BusinessEntity BusinessEntity { get; set; }
        public string BusinessEntityName { get; set; }

        public DateTime ReportingDeadLine { get; set; }

        public DateTime Date { get; set; }

        public bool SendEmailNotification { get; set; }

        public bool SendSmsNotification { get; set; }

        public string Info { get; set; }

        public string Name { get; set; }

        public int AuthoritativeDocumentId { get; set; }

        public string AuthoritativeDocumentName { get; set; }

        public virtual int? AssessmentTypeId { get; set; }
        public DynamicParameterValue AssessmentType { get; set; }

        public string Feedback { get; set; }

        public float ReviewScore { get; set; }

        public List<ReviewData> Reviews { get; set; }

        public AssessmentStatus Status { get; set; }
        public bool IsDeleted { get; set; }

        public bool HasFetchedLastAnswers { get; set; }
        public bool AllResponseCompleted { get; set; }

        public virtual int? ScheduleDetailId { get; set; }
        public InternalAssessmentScheduleDetail InternalAssessmentScheduleDetail { get; set; }

        public int? EntityGroupId { get; set; }
        public EntityGroup EntityGroup { get; set; }

        public bool IsAssessmentSubmitted { get; set; }

        public void Merge(List<ReviewData> previousReviews)
        {
            Reviews.ForEach(review =>
            {
                var previousReview = previousReviews.FirstOrDefault(e => e.ControlRequirementId == review.ControlRequirementId);
                if (previousReview != null)
                {
                    review.LastResponseType = previousReview.ResponseType;
                }
            });
        }

        public int GeneralComplianceAssessmentId { get; set; }

        public GeneralComplianceAssessment GeneralComplianceAssessment { get; set; }

        public async Task ApproveAsync()
        {
            Status = AssessmentStatus.Approved;
            await EventBus.Default.TriggerAsync(new AssessmentApprovedDomainEvent(this));
        }

        public async Task MakeInReviewAsync()
        {
            Status = AssessmentStatus.InReview;
            await EventBus.Default.TriggerAsync(new AssessmentSubmittedDomainEvent(this));
        }

        public async Task MakeBeAdminReviewAsync()
        {
            Status = AssessmentStatus.BEAdminReview;
            await EventBus.Default.TriggerAsync(new AssessmentSubmittedDomainEvent(this));
        }

        public async Task MakeEntityGroupAdminReviewAsync()
        {
            Status = AssessmentStatus.EntityGroupAdminReview;
            await EventBus.Default.TriggerAsync(new AssessmentSubmittedDomainEvent(this));
        }

        public void Publish()
        {
            Status = AssessmentStatus.SentToAuthority;
        }
        public void SetEGAReviewStatus()
        {
            Status = AssessmentStatus.EntityGroupAdminReview;
        }


        public void SubmitReview(int reviewId, string comment, string clarification, ReviewDataResponseType reviewDataResponseType, List<ReviewQuestion> reviewQuestions)
        {
            var review = getReviewDataById(reviewId);
            review.ReviewQuestions = reviewQuestions;
            review.ResponseType = reviewDataResponseType;
            review.LastResponseType = reviewDataResponseType;
            review.RequestComment = clarification;
            review.Comment = comment;
            if (review.ResponseType != ReviewDataResponseType.NotSelected && review.ResponseType != review.LastResponseType)
            {
                HasFetchedLastAnswers = true;
            }
        }

        public void RequestReviewItemClarification(int reviewId, string requestComment)
        {
            var review = getReviewDataById(reviewId);
            review.RequestComment = requestComment;
            review.Status = ReviewDataStatus.NeedsClarification;
            Status = AssessmentStatus.NeedsClarification;
        }

        public async Task RequestClarification()
        {
            Status = AssessmentStatus.NeedsClarification;
        }

        private ReviewData getReviewDataById(int reviewId)
        {
            var reviewData = Reviews.FirstOrDefault(e => e.Id == reviewId);
            if (reviewData == null)
            {
                throw new NotFoundException($"Couldn't find Review with ID {reviewId}");
            }
            return reviewData;
        }


    }
}
