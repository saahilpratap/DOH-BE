using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Domains.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.PatientAuthenticationPlatform.Dtos;
using LockthreatCompliance.Storage;

namespace LockthreatCompliance.PatientAuthenticationPlatform.Dtos
{
    public class PatientAuthenticationPlatformDto :  EntityDto<long>
    {
        public PatientAuthenticationPlatformDto() {
            PatientAuthenticationPlatformContactInformationDtos = new List<PatientAuthenticationPlatformContactInformationDto>();
            PatientAuthenticationPlatformAttachmentDtos = new List<PatientAuthenticationPlatformAttachmentDto>();
            PatientAuthenticationPlatformSelectedEntityDtos = new List<PatientAuthenticationPlatformSelectedEntityDto>();
        }
        public virtual string Code { get; set; }
        public int TenantId { get; set; }

        public string GroupName { get; set; }
        public bool Connecting { get; set; }
        public string FacilityLicenseNumber { get; set; }
        public virtual string AdditionalInformation { get; set; }
        public int? StatusId { get; set; }
        public virtual string Comment1 { get; set; }
        public virtual string Comment2 { get; set; }
        public List<PatientAuthenticationPlatformContactInformationDto> PatientAuthenticationPlatformContactInformationDtos { get; set; }
        public List<PatientAuthenticationPlatformAttachmentDto> PatientAuthenticationPlatformAttachmentDtos { get; set; }
        public List<PatientAuthenticationPlatformSelectedEntityDto> PatientAuthenticationPlatformSelectedEntityDtos { get; set; }

    }
    public class PatientAuthenticationPlatformListDto : EntityDto<long>
    {
       public PatientAuthenticationPlatformListDto()
        {
        }
        public virtual string Code { get; set; }
        public string GroupName { get; set; }
        public bool Connecting { get; set; }
        public string FacilityLicenseNumber { get; set; }
        public virtual string AdditionalInformation { get; set; }
        public string Status { get; set; }

        public DateTime? CreateationTime { get; set; }

    }

    public class PatientAuthenticationPlatformContactInformationDto : EntityDto<long>
    {
        public int TenantId { get; set; }
        public virtual bool IsDeleted { get; set; }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string MobilePhoneNumber { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual long PAPId { get; set; }

    }

    public class PatientAuthenticationPlatformLogDto : EntityDto<long>
    {
        public int? TenantId { get; set; }
        public long? LogUserId { get; set; }
        public virtual string Action { get; set; }
        public virtual long PAPId { get; set; }
        public int? StatusId { get; set; }

    }

    public class BusinessEntitiListDto {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool Ischeck { get; set; }

    }

    public class PatientAuthenticationPlatformAttachmentDto : EntityDto<int>
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public PAPAttachmentType PAPAttachmentType { get; set; }
        public long PAPId { get; set; }
        public bool Static { get; set; }
    }

    public class PatientAuthenticationPlatformSelectedEntityDto : EntityDto<long>
    {
        public virtual int BusinessEntityId { get; set; }
        public virtual long PAPId { get; set; }

    }

}
