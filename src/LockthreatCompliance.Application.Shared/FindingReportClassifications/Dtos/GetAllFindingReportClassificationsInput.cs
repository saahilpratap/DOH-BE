﻿using Abp.Application.Services.Dto;
using System;

namespace LockthreatCompliance.FindingReportClassifications.Dtos
{
    public class GetAllFindingReportClassificationsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }



    }
}