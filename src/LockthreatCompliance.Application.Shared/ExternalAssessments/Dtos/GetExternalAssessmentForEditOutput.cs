using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.ExternalAssessments.Dtos
{
    public class GetExternalAssessmentForEditOutput
    {
		public CreateOrEditExternalAssessmentDto ExternalAssessment { get; set; }

        public string LeadAssessorEmail { get; set; }

        public string LeadAssessorPhone { get; set; }

        public string AuditeeEmail { get; set; }

        public string AuditeeName { get; set; }

        public string AuditeeSurname { get; set; }

        public string AuditeePhone { get; set; }
    }
}