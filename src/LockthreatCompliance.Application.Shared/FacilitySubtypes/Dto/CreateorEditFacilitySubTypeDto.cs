using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using LockthreatCompliance.AuthoritativeDocuments;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FacilitySubtypes.Dto
{
    public class CreateorEditFacilitySubTypeDto : FullAuditedEntityDto
    {
        public  CreateorEditFacilitySubTypeDto()
            {

            }
        public int? TenantId { get; set; }
        public int? FacilityTypeId { get; set; }    
        public string FacilitySubTypeName { get; set; }
        public ControlType ControlType { get; set; }
        
    }
}
