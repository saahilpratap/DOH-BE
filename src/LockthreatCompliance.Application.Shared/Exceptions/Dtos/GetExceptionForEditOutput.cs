using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using LockthreatCompliance.Assessments.Dto;
using System.Collections.Generic;

namespace LockthreatCompliance.Exceptions.Dtos
{
    public class GetExceptionForEditOutput
    {
		public CreateOrEditExceptionDto Exception { get; set; }
        public string RequestorName { get; set; }

        public ExceptionReviewStatus ReviewStatus { get; set; }

        public DateTime RequestedDate { get; set; }

        public List<ExceptionRemediationDto> SelectedExceptionRemediations { get; set; }
        public List<AttachmentDto> Attachments { get; set; }

    }
}