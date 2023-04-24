using LockthreatCompliance.Enums;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using LockthreatCompliance.AuthoritativeDocuments;
using System.Collections.Generic;

namespace LockthreatCompliance.ControlRequirements.Dtos
{
    public class CreateOrEditControlRequirementDto : EntityDto<int?>
    {

        public string OriginalId { get; set; }


        public ControlType ControlType { get; set; }


        public string Code { get; set; }
        public string ControlRequirement { get; set; }


        public int ControlStandardId { get; set; }

        public bool IndustryMandated { get; set; }

        public List<RequirementQuestionDto> RequirementQuestions { get; set; }

        public bool Iscored { get; set; }
    }
}