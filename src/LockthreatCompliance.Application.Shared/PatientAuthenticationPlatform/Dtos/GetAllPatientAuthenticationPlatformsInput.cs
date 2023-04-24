using Abp.Application.Services.Dto;
using System;

namespace LockthreatCompliance.PatientAuthenticationPlatform.Dtos
{
    public class GetAllPatientAuthenticationPlatformsInput : PagedAndSortedResultRequestDto
    {
        public int? StatusId { get; set; }
        public string Filter { get; set; }	 
    }
    public class GetAllPatientAuthenticationPlatformContactInformationInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
    public class GetAllPatientAuthenticationPlatformLogInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}