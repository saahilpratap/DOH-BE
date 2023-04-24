using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.ExternalAssessments;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities
{
    public class GeneralComplianceAssessment : Entity, IHasCreationTime
    {
        public GeneralComplianceAssessment()
        {
            CreationTime = DateTime.UtcNow;
            Assessments = new List<Assessment>();
            ExternalAssessments = new List<ExternalAssessment>();
        }

        public List<Assessment> Assessments { get; set; }

        public List<ExternalAssessment> ExternalAssessments { get; set; }

        public void AddExtAssessment(ExternalAssessment assessment)
        {
            ExternalAssessments.Add(assessment);
            AllCount++;
        }

        public void AddAssessment(Assessment assessment)
        {
            Assessments.Add(assessment);
            AllCount++;
        }

        public void IncrementSubmitted()
        {
            SubmittedCount++;
            ResetPercentages();
        }

        public void DecrementSubmitted()
        {
            SubmittedCount--;
            ResetPercentages();
        }

        public void IncrementApproved()
        {
            ApprovedCount++;
            ResetPercentages();
        }

        public void DecrementApproved()
        {
            ApprovedCount--;
            ResetPercentages();
        }

        private void ResetPercentages()
        {
            ApprovedPercentage = ApprovedCount / AllCount * 100;
            SubmittedPercentage = SubmittedCount / AllCount * 100;
        }

        public DateTime CreationTime { get; set; }

        public int SubmittedCount { get; set; }

        public int AllCount { get; set; }

        public double SubmittedPercentage { get; set; }
        public double ApprovedPercentage { get; set; }
        public int ApprovedCount { get; set; }
    }
}
