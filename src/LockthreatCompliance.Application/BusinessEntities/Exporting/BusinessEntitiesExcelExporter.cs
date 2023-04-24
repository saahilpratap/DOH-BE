using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Storage;

namespace LockthreatCompliance.BusinessEntities.Exporting
{
    public class BusinessEntitiesExcelExporter : EpPlusExcelExporterBase, IBusinessEntitiesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BusinessEntitiesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<BusinessEntitiesExcelDto> businessEntities)
        {
            return CreateExcelPackage(
                "BusinessEntities.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("BusinessEntities"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        ("EntityGroup"),
                        ("Entity_Type"),
                        ("IsPrimary"),
                        ("LICENSENUMBER"),
                        ("FACILITY_EN"),
                        ("PUBLIC_PRIVATE"),
                        ("District"),
                        ("FacilityType"),
                        ("Fac_Sub_Type"),
                        ("Ftypesize"),
                        ("LicenseStatus"),
                        ("FHLName"),
                        ("FACILITY_EMAIL"),
                        ("OWNER_EN"),
                        ("OWNER_EMAIL"),
                        ("OWNER_MOBILE"),
                        ("DIRECTOR_INCHARGE_EN"),
                        ("DIRECTOR_INCHARGE_EMAIL"),
                        ("DIRECTOR_INCHARGE_MOBILE"),
                        ("Pro_En"),
                        ("Pro_Email"),
                        ("Pro_Mobile"),
                        ("Admin_mail"),
                        ("Admin_Phone"),
                        ("PrimaryContactName"),
                        ("ContactNumber"),
                        ("Designation"),
                        ("OfficialEmail"),
                        ("BackupContactName"),
                        ("BackupContactNumber"),
                        ("BackupDesignation"),
                        ("BackupOfficialEmail"),
                        ("AdminName"),
                        ("AdminSurName"),
                        ("NumberOfYearsInBusiness"),
                        ("Status"),
                        ("CityOrDisctrict"),
                        ("Country")
                        );

                    AddObjects(
                        sheet, 2, businessEntities,
                        _ => _.EntityGroupName,
                        _ => _.EntityType,
                        _ => _.IsPrimaryEntity,
                        _ => _.LicenseNumber,
                        _ => _.Facility_EN,
                        _ => _.IsPublic,
                        _ => _.District == null ? "": _.District.Value,
                        _ => _.FacilityTypeName == null ? "" :_.FacilityTypeName,
                        _ => _.FacilitySubTypeName == null ? "": _.FacilitySubTypeName,
                        _ => _.FacilityTypeSize,
                        _ => _.IsActive,
                        _ => _.HFLName,
                        _ => _.Facility_Email,
                        _ => _.Owner_EN,
                        _ => _.Owner_Email,
                        _ => _.Owner_Mobile,
                        _ => _.Director_Incharge_EN,
                        _ => _.Director_Incharge_Email,
                        _ => _.Director_Incharge_Mobile,
                        _ => _.Pro_EN,
                        _ => _.Pro_Email,
                        _ => _.Pro_Mobile,
                        _ => _.AdminEmail,
                        _ => _.AdminMobile,
                        _ => _.PrimaryContactName,
                        _ => _.ContactNumber,
                        _ => _.Designation,
                        _ => _.OfficialEmail,
                        _ => _.BackupContactName,
                        _ => _.BackupContactNumber,
                        _ => _.BackupDesignation,
                        _ => _.BackupOfficialEmail,
                        _ => _.AdminName,
                        _ => _.AdminSurname,
                        _ => _.NumberOfYearsInBusiness,
                        _ => _.Status,
                        _ => _.CityOrDisctrict,
                        _ => _.CountryName
                        );



                });
        }

        public FileDto ExportInvalidToFile(List<ImportBusinessEntityUpdateDto> businessEntities)
        {
            return CreateExcelPackage(
                "BusinessEntitiesErrorSheet.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("BusinessEntities"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        ("EntityGroup"),
                        ("Entity_Type"),
                        ("IsPrimary"),
                        ("LICENSENUMBER"),
                        ("FACILITY_EN"),
                        ("PUBLIC_PRIVATE"),
                        ("District"),
                        ("FacilityType"),
                        ("Fac_Sub_Type"),
                        ("Ftypesize"),
                        ("LicenseStatus"),
                        ("FHLName"),
                        ("FACILITY_EMAIL"),
                        ("OWNER_EN"),
                        ("OWNER_EMAIL"),
                        ("OWNER_MOBILE"),
                        ("DIRECTOR_INCHARGE_EN"),
                        ("DIRECTOR_INCHARGE_EMAIL"),
                        ("DIRECTOR_INCHARGE_MOBILE"),
                        ("Pro_En"),
                        ("Pro_Email"),
                        ("Pro_Mobile"),
                        ("Admin_mail"),
                        ("Admin_Phone"),
                        ("PrimaryContactName"),
                        ("ContactNumber"),
                        ("Designation"),
                        ("OfficialEmail"),
                        ("BackupContactName"),
                        ("BackupContactNumber"),
                        ("BackupDesignation"),
                        ("BackupOfficialEmail"),
                        ("AdminName"),
                        ("AdminSurName"),
                        ("NumberOfYearsInBusiness"),
                        ("Status"),
                        ("CityOrDisctrict"),
                        ("Country"),
                        ("Row No"),
                        ("Result")
                        );

                    AddObjects(
                        sheet, 2, businessEntities,
                        _ => "",
                        _ => "",
                        _ => "",
                        _ => _.LicenseNumber,
                        _ => _.Facility_EN,
                        _ => _.IsPublic,
                         _ => _.District == null ? "" : _.District.Value,
                        _ => _.FacilityTypeName == null ? "" : _.FacilityTypeName,
                        _ => _.FacilitySubTypeName == null ? "" : _.FacilitySubTypeName,
                        _ => _.FacilityTypeSize,
                        _ => _.IsActive,
                        _ => _.HFLName,
                        _ => _.Facility_Email,
                        _ => _.Owner_EN,
                        _ => _.Owner_Email,
                        _ => _.Owner_Mobile,
                        _ => _.Director_Incharge_EN,
                        _ => _.Director_Incharge_Email,
                        _ => _.Director_Incharge_Mobile,
                        _ => _.Pro_EN,
                        _ => _.Pro_Email,
                        _ => _.Pro_Mobile,
                        _ => _.AdminEmail,
                        _ => _.AdminMobile,
                        _ => _.PrimaryContactName,
                        _ => _.ContactNumber,
                        _ => _.Designation,
                        _ => _.OfficialEmail,
                        _ => _.BackupContactName,
                        _ => _.BackupContactNumber,
                        _ => _.BackupDesignation,
                        _ => _.BackupOfficialEmail,
                        _ => _.AdminName,
                        _ => _.AdminSurname,
                        _ => _.NumberOfYearsInBusiness,
                        _ => _.Status,
                        _ => _.CityOrDisctrict,
                        _ => _.CountryName,
                         _ => _.RowName,
                          _ => _.Message
                        );



                });
        }
    }
}
