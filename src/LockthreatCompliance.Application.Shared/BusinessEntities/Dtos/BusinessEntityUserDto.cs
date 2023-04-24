using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities.Dtos
{
    public class BusinessEntityUserDto: EntityDto<long>
    {
        public string Name { get; set; }

        public BusinessEntityWorkflowActorType Type { get; set; }

    }
}
