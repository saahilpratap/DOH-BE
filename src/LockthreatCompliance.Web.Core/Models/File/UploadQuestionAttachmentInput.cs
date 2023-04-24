using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockthreatCompliance.Web.Models.File
{
    public class UploadQuestionAttachmentInput
    {
        public int ReviewQuestionId { get; set; }

        public IFormFile File { get; set; }
    }
}
