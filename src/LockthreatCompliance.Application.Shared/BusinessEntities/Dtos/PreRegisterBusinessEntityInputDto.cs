using Abp.Application.Services.Dto;
using Abp.Runtime.Security;
using Abp.Runtime.Validation;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using LockthreatCompliance.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace LockthreatCompliance.BusinessEntities.Dtos
{
    public class PreRegisterBusinessEntityInputDto : EntityDto, IShouldNormalize
    {
        public int? TenantId { get; set; }

        public string Name { get; set; }

        public string CompanyName { get; set; }

        public string AdminEmail { get; set; }

        public string AdminMobile { get; set; }

        public string VerificationCode { get; set; }

        public bool IsVerificationDone { get; set; }

        public bool IsRequestApproved { get; set; }

        public EntityType EntityType { get; set; }

        public ControlType ControlType { get; set; }
        public string c { get; set; }

        public int? ThirdPartyId { get; set; }

        public List<DynamicNameValueDto> ThirdParties { get; set; }

        public string LicenseNumber { get; set; }

        public string Facility_EN { get; set; }

        public bool IsPublic { get; set; }

        public int? DistrictId { get; set; }

        public int? FacilityTypeId { get; set; }

        public int? FacilitySubTypeId { get; set; }

        public string HFLName { get; set; }

        public bool IsActive { get; set; }

        public string Facility_Email { get; set; }

        public string Owner_EN { get; set; }

        public string Owner_Email { get; set; }

        public string Owner_Mobile { get; set; }

        public string Director_Incharge_EN { get; set; }

        public string Director_Incharge_Email { get; set; }
        public string Director_Incharge_Mobile { get; set; }

        public string Pro_EN { get; set; }
        public string Pro_Email { get; set; }
        public string Pro_Mobile { get; set; }

        public string PrimaryContactName { get; set; }
        public string Designation { get; set; }
        public string ContactNumber { get; set; }
        public string OfficialEmail { get; set; }

        public string BackupContactName { get; set; }
        public string BackupDesignation { get; set; }
        public string BackupContactNumber { get; set; }
        public string BackupOfficialEmail { get; set; }

        public string AdminName { get; set; }
        public string AdminSurname { get; set; }

        public int? CountryId { get; set; }

        public string CityOrDisctrict { get; set; }
        public void Normalize()
        {
            ResolveParameters();
        }



        protected virtual void ResolveParameters()
        {
            if (!string.IsNullOrEmpty(c))
            {
                var parameters = SimpleStringCipher.Instance.Decrypt(c);
                var query = HttpUtility.ParseQueryString(parameters);

                if (query["email"] != null)
                {
                    AdminEmail = query["email"];
                }

                if (query["verificationCode"] != null)
                {
                    VerificationCode = query["verificationCode"];
                }
            }
        }
    }
}
