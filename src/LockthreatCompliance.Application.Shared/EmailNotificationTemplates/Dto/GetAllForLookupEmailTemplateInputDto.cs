using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.EmailNotificationTemplates.Dto
{
  public  class GetAllForLookupEmailTemplateInputDto: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
    
}
