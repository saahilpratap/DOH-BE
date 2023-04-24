using LockthreatCompliance.Domains.Dtos;
using LockthreatCompliance.FindingReports.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjectGroups
{
    public class ExternalAssessmentListDto
    {
        public ExternalAssessmentListDto()
        {
            ExternalEntityList = new List<EntityWithAssessmentDto>();
        }
        public int SelectedEntityId { get; set; }
        public List<EntityWithAssessmentDto> ExternalEntityList { get; set; }
    }

    public class GetAllFindingForAuditProjectInputDto
    {
        public int AuditProjectId { get; set; }
        public int EntityId { get; set; }
    }

    public class GetAllFindingForAuditProjectOutputDto
    {
        public List<FindingListAuditDto> FindingListAudits { get; set; }
        public List<int> EntityList { get; set; }
        public int SelectedEntityId { get; set; }

    }

}
