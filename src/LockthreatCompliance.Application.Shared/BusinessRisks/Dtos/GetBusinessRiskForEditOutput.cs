using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using LockthreatCompliance.FindingReports.Dtos;

namespace LockthreatCompliance.BusinessRisks.Dtos
{
    public class GetBusinessRiskForEditOutput
    {
		public CreateOrEditBusinessRiskDto BusinessRisk { get; set; }

        public List<AttachmentWithTitleDto> Attachments { get; set; }

        public List<BusinessRiskRemediationDto> SelectedBusinessRiskRemediations { get; set; }
    }
}