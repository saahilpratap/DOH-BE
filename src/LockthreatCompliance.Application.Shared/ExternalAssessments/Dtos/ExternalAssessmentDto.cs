using LockthreatCompliance.Enums;

using System;
using Abp.Application.Services.Dto;

namespace LockthreatCompliance.ExternalAssessments.Dtos
{
    public class ExternalAssessmentDto : EntityDto
    {

        public virtual string Code { get; set; }     
        public string Name { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public ExternalAssessmentType Type { get; set; }

        public bool HasQuestionaireGenerated { get; set; }

        public string BusinessEntityName { get; set; }
    }

    public class ExternalAssessmentWIthPrimaryEnrityDto : ExternalAssessmentDto
    {
        public bool IsPrimaryEntity { get; set; }
        public string EntityName { get; set; }
    }
}