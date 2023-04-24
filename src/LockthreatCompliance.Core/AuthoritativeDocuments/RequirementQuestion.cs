using Abp.Domain.Entities;
using LockthreatCompliance.ControlRequirements;
using LockthreatCompliance.Questions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuthoritativeDocuments
{
    public class RequirementQuestion : Entity, IMayHaveTenant
    {
        public int QuestionId { get; set; }

        public Question Question { get; set; }

        public int ControlRequirementId { get; set; }

        public ControlRequirement ControlRequirement { get; set; }

        public int? TenantId { get; set; }
    }
}
