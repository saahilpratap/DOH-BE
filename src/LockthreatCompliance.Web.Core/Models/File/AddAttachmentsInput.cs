using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockthreatCompliance.Web.Models.File
{
    public class AddAttachmentsInput
    {
        public List<AttachmentInput> Attachments { get; set; }
    }

    public class AttachmentInput
    {
        public IFormFile File { get; set; }

        public string Title { get; set; }
    }
}
