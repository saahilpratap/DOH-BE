using LockthreatCompliance.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.Collections.Generic;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.CustomExceptions;
using System.Linq;
using LockthreatCompliance.AssessmentSchedules.ExternalAsssementSchedules;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.EntityGroups;
using LockthreatCompliance.Extensions;
using Abp.UI;

namespace LockthreatCompliance.ExternalAssessments
{
    [Table("ExternalAssessments")]
    public class ExternalAssessment : Entity, IMayHaveTenant, IFullAudited
    {
        public ExternalAssessment()
        {
            AuthoritativeDocuments = new List<ExternalAssessmentAuthoritativeDocument>();
            ExternalAssessmentAuditWorkPapers = new List<ExternalAssessmentAuditWorkPaper>();
            Reviews = new List<ReviewData>();
            Status = AssessmentStatus.Initialized;
        }

        [NotMapped]
        public virtual string Code { get { return "EXT-" + Id.GetCodeEnding(); } }
        public int? TenantId { get; set; }

        public virtual string Name { get; set; }

        public int FiscalYear { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }


        public virtual ExternalAssessmentType Type { get; set; }

        public List<ExternalAssessmentAuthoritativeDocument> AuthoritativeDocuments { get; set; }

        public List<ReviewData> Reviews { get; set; }

        public int BusinessEntityId { get; set; }
        public BusinessEntity BusinessEntity { get; set; }

        public int? EntityGroupId  { get; set; }
        public EntityGroup EntityGroup { get; set; }

        public long? LeadAssessorId { get; set; }
        public User LeadAssessor { get; set; }

        public bool HasQuestionaireGenerated { get; set; }

        public int? VendorId { get; set; }
        public BusinessEntity Vendor { get; set; }



        public long? BusinessEntityLeadAssessorId { get; set; }
        public User BusinessEntityLeadAssessor { get; set; }

        public long? CreatorUserId { get; set; }
        public User CreatorUser { get; set; }

        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }

        public AssessmentStatus Status { get; set; }

        public virtual long? ScheduleDetailId { get; set; }
        public ExternalAssessmentScheduleDetail ExternalAssessmentScheduleDetail { get; set; }

        public List<ExternalAssessmentAuditWorkPaper> ExternalAssessmentAuditWorkPapers { get; set; }

        public bool SendSmsNotification { get; set; }
        public bool SendEmailNotification { get; set; }

        public string Feedback { get; set; }

        public virtual int? AssessmentTypeId { get; set; }
        public DynamicParameterValue AssessmentType { get; set; }

        public string AuditorTeam { get; set; }

        public string AuditeeTeam { get; set; }

        public long? AuditProjectId { get; set; }

        public AuditProject AuditProject { get; set; }

        public int? GeneralComplianceAssessmentId { get; set; }

        public GeneralComplianceAssessment GeneralComplianceAssessment { get; set; }


        public void SubmitReview(int reviewId, string comment, string clarification, ReviewDataResponseType reviewDataResponseType, ReviewDataResponseType lastResponseValue, List<ReviewQuestion> reviewQuestions)
        {
            var review = getReviewDataById(reviewId);
            review.ReviewQuestions = reviewQuestions;
            review.ResponseType = reviewDataResponseType;
            review.LastResponseType = lastResponseValue;
            review.RequestComment = clarification;
            review.Comment = comment;
        }
        private ReviewData getReviewDataById(int reviewId)
        {
            var reviewData = Reviews.FirstOrDefault(e => e.Id == reviewId);
            if (reviewData == null)
            {
                throw new UserFriendlyException($"Couldn't find Review with ID {reviewId}");
            }
            return reviewData;
        }
    }
}