using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.ControlRequirements.Dtos
{
    public class ControlRequirementList
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public string Description { get; set; }

        public string ControlStandardName { get; set; }

        public virtual string DomainName { get; set; }

        public virtual string OriginalId { get; set; }

        public string ItemName { get; set; }

        public bool Iscored { get; set; }

    }

    public class ControlRequirementGroup
    {
        public string DomainName { get; set; }

        public List<ControlRequirementList> ControlRequirementList { get; set; }
    }
}
