using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using LockthreatCompliance.Extensions;

namespace LockthreatCompliance.PatientAuthenticationPlatform
{
    public class PatientAuthenticationPlatformGlobalAttachment: FullAuditedEntity<long>, IMayHaveTenant
    {
        public PatientAuthenticationPlatformGlobalAttachment()
        {

        }

        public PatientAuthenticationPlatformGlobalAttachment(string fileName, string title = null)
        {

            Code = Guid.NewGuid().ToString() + "." + fileName.ReverseChars().GetUntil('.').ReverseChars();
            FileName = fileName;
            Title = title;

        }
        public int? TenantId { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }          
        public bool Static { get; set; }

    }
}
