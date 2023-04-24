﻿using Abp.Application.Services.Dto;
using System;

namespace LockthreatCompliance.Domains.Dtos
{
    public class GetAllDomainsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string NameFilter { get; set; }


		 public string AuthoritativeDocumentNameFilter { get; set; }

		 
    }
}