using Abp.Application.Services.Dto;
using Abp.Configuration;
using Abp.Runtime.Validation;
using LockthreatCompliance.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.PatientAuthenticationPlatform.Dtos
{
    public class PatientAuthenticationPlatformGlobalAttachmentDto : EntityDto<long?>
    { 
       public  PatientAuthenticationPlatformGlobalAttachmentDto()
        {

        }
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public bool Static { get; set; }
    }


    public class CreateorEditPatientAuthenticationPlatformGlobalAttachmentDto 
    {
        public CreateorEditPatientAuthenticationPlatformGlobalAttachmentDto()
        {
            PatientAuthenticationPlatformGlobalAttachmentDto = new List<PatientAuthenticationPlatformGlobalAttachmentDto>();
        }
        public List<PatientAuthenticationPlatformGlobalAttachmentDto> PatientAuthenticationPlatformGlobalAttachmentDto { get; set; }

    }


   

}
