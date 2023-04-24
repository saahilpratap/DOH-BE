using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using LockthreatCompliance.FindingReports.Dtos;

namespace LockthreatCompliance.Incidents.Dtos
{
    public class GetIncidentForEditOutput
    {
		public CreateOrEditIncidentDto Incident { get; set; }

        public List<AttachmentWithTitleDto> Attachments { get; set; }

        public List<IncidentRemediationDto> SelectedIncidentRemediations { get; set; }

    }
}