using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.MultiTenancy.Dto
{
    public class CommonTopStatForHostAndTenant
    {
        public int TotalActiveEntities { get; set; }

        public int TotalActiveEntitiesCounter { get; set; }

        public int TotalActiveEntitiesChange { get; set; }

        public int TotalActiveEntitiesChangeCounter { get; set; }

        public int NewAssessmentsSubmitted { get; set; }

        public int NewAssessmentsSubmittedCounter { get; set; }

        public int NewAssessmentsSubmittedChange { get; set; }

        public int NewAssessmentsSubmittedChangeCounter { get; set; }

        public int NewExternalAssessments { get; set; }

        public int NewExternalAssessmentsCounter { get; set; }

        public int NewExternalAssessmentsChange { get; set; }

        public int NewExternalAssessmentsChangeCounter { get; set; }

        public int NewUsers { get; set; }

        public int NewUsersCounter { get; set; }

        public int NewUsersChange { get; set; }

        public int NewUsersChangeCounter { get; set; }

    }
}
