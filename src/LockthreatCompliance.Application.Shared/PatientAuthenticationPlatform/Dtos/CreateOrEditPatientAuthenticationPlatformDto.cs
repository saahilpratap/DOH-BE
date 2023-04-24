
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LockthreatCompliance.PatientAuthenticationPlatform.Dtos
{
    public class CreateOrEditPatientAuthenticationPlatformDto : EntityDto<long?>
    {
        public CreateOrEditPatientAuthenticationPlatformDto() {
            PatientAuthenticationPlatformContactInformationDtos = new List<CreateOrEditPatientAuthenticationPlatformContactInformationDto>();
            PatientAuthenticationPlatformSelectedEntityDtos = new List<CreateOrEditPatientAuthenticationPlatformSelectedEntityDto>();

        }
        public int TenantId { get; set; }
        public virtual string Code { get; set; }
        public string GroupName { get; set; }
        public bool Connecting { get; set; }
        public string FacilityLicenseNumber { get; set; }
        public virtual string AdditionalInformation { get; set; }
        public int? StatusId { get; set; }
        public virtual string Comment1 { get; set; }
        public virtual string Comment2 { get; set; }

       
        public List<CreateOrEditPatientAuthenticationPlatformContactInformationDto> PatientAuthenticationPlatformContactInformationDtos { get; set; }
        public List<CreateOrEditPatientAuthenticationPlatformSelectedEntityDto> PatientAuthenticationPlatformSelectedEntityDtos { get; set; }
    }
    public class CreateOrEditPatientAuthenticationPlatformContactInformationDto : EntityDto<long?>
    {
        public int TenantId { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string MobilePhoneNumber { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual long PAPId { get; set; }
    }

    public class CreateOrEditPatientAuthenticationPlatformLogDto : EntityDto<long?>
    {
        public int? TenantId { get; set; }
        public long? LogUserId { get; set; }
        public virtual string Action { get; set; }
        public virtual long PAPId { get; set; }
        public int? StatusId { get; set; }
    }

    public class CreateOrEditPatientAuthenticationPlatformSelectedEntityDto : EntityDto<long?>
    {
        public virtual int BusinessEntityId { get; set; }
        public virtual long PAPId { get; set; }
    }
}