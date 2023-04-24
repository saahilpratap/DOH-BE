using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.ThirdpartyApi.Dto
{
   public class BusinessEntityThirdPartyDto : EntityDto
    {
        public int? TenantId { get; set; }
        public string LICENSE_NO { get; set; }

        public string STATUS {get;set; }

        public string FACILITY_CATEGORY { get; set; }

        public string HAS_INSURANCE { get; set; }

        public string IS_PUBLIC_FACILITY { get; set; }

        public string FACILITY_GROUP { get; set; }

        public string DED_LICENSE { get; set; }

        public string DED_TEMPORARY_NUMBER { get; set; }

        public string FACILITY_NAME  { get; set; } 

        public string FACILITY_TYPE  { get; set; }

        public string FACILITY_SUBTYPE  { get; set; }

        public string ADDRESS  { get; set; }

        public string CITY  { get; set; }

        public string TELEPHONE  { get; set; }

        public string EMAIL  { get; set; }

        public string OWNERNAME_EN  { get; set; }

        public string OWNER_TELEPHONE { get; set; }

        public string OWNER_EMAIL  { get; set; }

        public string OWNER_EID  { get; set; }

        public string DIRECTOR_INCHARGE_ENGLISH_NAME{ get; set; }

        public string DIRECTOR_INCHARGE_TELEPHONE{ get; set; }

        public string DIRECTOR_INCHARGE_EMAIL{ get; set; }

        public string PRO_NAME { get; set; }

        public string PRO_TELEPHONE  { get; set; }

        public string PRO_EMAIL { get; set; }
    

    }
}
