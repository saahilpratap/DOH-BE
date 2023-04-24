﻿using Abp.Application.Services.Dto;
using LockthreatCompliance.Enums;
using System;

namespace LockthreatCompliance.BusinessEntities.Dtos
{
    public class GetAllBusinessEntitiesForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string AddressFilter { get; set; }
     
        public string WebsiteUrlFilter { get; set; }

        public string LegalNameFilter { get; set; }

        public bool? IsGovernmentOwnedFilter { get; set; }

        public bool? IsLicensedFilter { get; set; }

        public string LicenseNumberFilter { get; set; }

        public bool? IsAuditableFilter { get; set; }


        //public int BusinessTypeFilter { get; set; }

        public int? FacilityTypeFilter { get; set; }

        public int? FacilitySubTypeFilter { get; set; }

        public EntityTypeStatus? Status { get; set; }

        public EntityType EntityType { get; set; }

        public bool ShowOnlyDeleted { get; set; }


    }
}