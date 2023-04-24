using LockthreatCompliance.Enums;
using System;
using Abp.Application.Services.Dto;

namespace LockthreatCompliance.Exceptions.Dtos
{
    public class ExceptionDto : EntityDto
    {
        public string Code { get; set; }
		public string Title { get; set; }       
        public string TypeName { get; set; }
        public ExceptionReviewStatus ReviewStatus { get; set; }

        public string BusinessEntityName { get; set; }
        public DateTime RequestDate { get; set; }
    }
}