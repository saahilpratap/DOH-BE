using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.EmailReminderTemplates.Dto
{
  public  class GetAllForLookupEmailReminderTemplateInputDto: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
    
}
