    using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.BusinessEntities.Exporting
{
    public class PreEntityExcelExporter : EpPlusExcelExporterBase, IPreEntityExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PreEntityExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }
        public FileDto ExportToFile(List<PreRegistrationImportDto> preRegistrationDto)
        {
            return CreateExcelPackage(
                "PreRegistartionErrorSheet.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("PreRegistartionErrorSheet"));
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
                        ("CountryName"),
                        ("Row No"),
                        ("Result")
                        );

                    AddObjects(
                        sheet, 2, preRegistrationDto,
                        _ => _.EntityGroupName,
                        _ => _.EntityType,
                        _ => _.IsPrimaryEntity,
                        _ => _.LicenseNumber,
                        _ => _.Facility_EN,
                        _ => _.IsPublic,
                        _ => _.DistrictName,
                        _ => _.FacilityTypeName,
                        _ => _.FacilitySubTypeName,
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
                         _ => _.CountryId,
                         _ => _.RowName,
                         _ => _.Message
                        );



                });
        }
    }
}
