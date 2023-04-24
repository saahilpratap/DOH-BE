using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Domains.Dtos
{
    public class IdNameDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DomainIdNameDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AuthoritativeDocumentId { get; set; }
    }

    public class EntityWithAssessmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int assessmentId { get; set; }
    }

    public class IdNameAndPrimaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EntityId { get; set; }
    }

    public class IdAndExternalAssessment
    {
        public int Id { get; set; }
        public int Assemm { get; set; }
    }
}
