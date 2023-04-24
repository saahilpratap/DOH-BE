using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities.Dtos
{
    public class CreateAssessmentStatusLogDto : EntityDto<long>
    {
        public int AssessmentId { get; set; }
        public AssessmentStatus Status { get; set; }
        public long? UserActedId { get; set; }
        public DateTime? ActionDate { get; set; }
    }

}
