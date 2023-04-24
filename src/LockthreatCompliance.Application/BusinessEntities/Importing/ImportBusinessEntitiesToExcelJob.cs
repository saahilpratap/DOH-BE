using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.DynamicEntityParameters;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.ObjectMapping;
using Abp.Threading;
using LockthreatCompliance.Authorization.Users.Dto;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.BusinessEntities.Exporting;
using LockthreatCompliance.DynamicEntityParameters;
using LockthreatCompliance.FacilityTypes;
using LockthreatCompliance.Notifications;
using LockthreatCompliance.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.BusinessEntities.Importing
{
    public class ImportBusinessEntitiesToExcelJob : BackgroundJob<ImportUsersFromExcelJobArgs>, ITransientDependency
    {
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;
        private readonly IPreEntityListExcelDataReader _preEntityListExcelDataReader;
        private readonly IBusinessEntitiesExcelExporter _businessEntitiesExcelExporter;

        public ImportBusinessEntitiesToExcelJob(IRepository<BusinessEntity> businessEntityRepository,
            IAppNotifier appNotifier, IBinaryObjectManager binaryObjectManager, ILocalizationManager localizationManager, IObjectMapper objectMapper, IPreEntityListExcelDataReader preEntityListExcelDataReader, IBusinessEntitiesExcelExporter businessEntitiesExcelExporter)
        {
            _businessEntityRepository = businessEntityRepository;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _localizationSource = localizationManager.GetSource(LockthreatComplianceConsts.LocalizationSourceName);
            _objectMapper = objectMapper;
            _preEntityListExcelDataReader = preEntityListExcelDataReader;
            _businessEntitiesExcelExporter = businessEntitiesExcelExporter;
        }

        [UnitOfWork]
        public override void Execute(ImportUsersFromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var entities = GetBusinessEntitiesListFromExcelOrNull(args);
                if (entities == null || !entities.Any())
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                CreateBusinessEntities(args, entities);
            }
        }

        private List<ImportBusinessEntityUpdateDto> GetBusinessEntitiesListFromExcelOrNull(ImportUsersFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _preEntityListExcelDataReader.GetBusinessEntitiesFromExcel(file.Bytes, args.TenantId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateBusinessEntities(ImportUsersFromExcelJobArgs args, List<ImportBusinessEntityUpdateDto> entities)
        {
                var checkValidHeader = entities.FirstOrDefault();
                if (checkValidHeader.InvalidCount != false)
                {
                foreach (var item in entities)
                {
                    if (item.LicenseNumber != null)
                    {
                        var getLicenceData = _businessEntityRepository.GetAll();
                        var checkLicense = getLicenceData.Where(p => p.LicenseNumber.ToLower().Trim() == item.LicenseNumber.Trim().ToLower()).FirstOrDefault();
                        if (checkLicense != null)
                        {
                            var businessObj = _businessEntityRepository.GetAll().Where(p => p.LicenseNumber.ToLower().Trim() == item.LicenseNumber.ToLower().Trim()).ToList().FirstOrDefault();
                            businessObj.LicenseNumber = item.LicenseNumber;
                            businessObj.CompanyName = (item.CompanyName == null) ? businessObj.CompanyName : item.CompanyName;
                            businessObj.CompanyLegalName = (item.CompanyLegalName == null) ? businessObj.CompanyLegalName : item.CompanyLegalName;
                            businessObj.CompanyWebsite = (item.CompanyWebsite == null) ? businessObj.CompanyWebsite : item.CompanyWebsite;
                            businessObj.CompanyAddress = (item.CompanyAddress == null) ? businessObj.CompanyAddress : item.CompanyAddress;
                            if (item.IsCityValid != false)
                            {
                                businessObj.CityOrDisctrict = item.CityOrDisctrict;
                            }
                            else
                            {
                                businessObj.CityOrDisctrict = businessObj.CityOrDisctrict;
                            }
                            businessObj.PostalCode = (item.PostalCode == null) ? businessObj.PostalCode : item.PostalCode;
                            businessObj.Owner_EN = (item.Owner_EN == null) ? businessObj.Owner_EN : item.Owner_EN;
                            businessObj.Owner_Email = (item.Owner_Email == null) ? businessObj.Owner_Email : item.Owner_Email;
                            businessObj.Owner_Mobile = (item.Owner_Mobile == null) ? businessObj.Owner_Mobile : item.Owner_Mobile;
                            businessObj.Pro_EN = (item.Pro_EN == null) ? businessObj.Pro_EN : item.Pro_EN;
                            businessObj.Pro_Email = (item.Pro_Email == null) ? businessObj.Pro_Email : item.Pro_Email;
                            businessObj.Pro_Mobile = (item.Pro_Mobile == null) ? businessObj.Pro_Mobile : item.Pro_Mobile;
                            businessObj.CISO_EN = (item.CISO_EN == null) ? businessObj.CISO_EN : item.CISO_EN;
                            businessObj.CISO_Email = (item.CISO_Email == null) ? businessObj.CISO_Email : item.CISO_Email;
                            businessObj.CISO_Mobile = (item.CISO_Mobile == null) ? businessObj.CISO_Mobile : item.CISO_Mobile;
                            businessObj.PrimaryContactName = (item.PrimaryContactName == null) ? businessObj.PrimaryContactName : item.PrimaryContactName;
                            businessObj.Designation = (item.Designation == null) ? businessObj.Designation : item.Designation;
                            businessObj.ContactNumber = (item.ContactNumber == null) ? businessObj.ContactNumber : item.ContactNumber;
                            businessObj.OfficialEmail = (item.OfficialEmail == null) ? businessObj.OfficialEmail : item.OfficialEmail;
                            businessObj.BackupContactName = (item.BackupContactName == null) ? businessObj.BackupContactName : item.BackupContactName;
                            businessObj.BackupDesignation = (item.BackupDesignation == null) ? businessObj.BackupDesignation : item.BackupDesignation;
                            businessObj.BackupContactNumber = (item.BackupContactNumber == null) ? businessObj.BackupContactNumber : item.BackupContactNumber;
                            businessObj.BackupOfficialEmail = (item.BackupOfficialEmail == null) ? businessObj.BackupOfficialEmail : item.BackupOfficialEmail;
                            businessObj.Director_Incharge_EN = (item.Director_Incharge_EN == null) ? businessObj.Director_Incharge_EN : item.Director_Incharge_EN;
                            businessObj.Director_Incharge_Mobile = (item.Director_Incharge_Mobile == null) ? businessObj.Director_Incharge_Mobile : item.Director_Incharge_Mobile;
                            businessObj.Director_Incharge_Email = (item.Director_Incharge_Email == null) ? businessObj.Director_Incharge_Email : item.Director_Incharge_Email;
                            businessObj.Facility_Email = (item.Facility_Email == null) ? businessObj.Facility_Email : item.Facility_Email;
                            var id = _businessEntityRepository.UpdateAsync(businessObj);
                        }
                    }
                }
                AsyncHelper.RunSync(() => ProcessImportUsersResultAsync(args,entities));
                }
               else
                {
                SendInvalidColumnNotification(args, entities);
                return;
                }

        }

        private void SendInvalidColumnNotification(ImportUsersFromExcelJobArgs args, List<ImportBusinessEntityUpdateDto> entities)
        {
            var firstEntries = entities.ToList().FirstOrDefault();
            AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                args.User,
                "Import Failed.Column (" + firstEntries.InvalidName + "..) Does Not Match,Please check Column Name or use the import template provided",//_localizationSource.GetString("FileCantBeConvertedToUserList"),
                Abp.Notifications.NotificationSeverity.Warn));
        }

        private async Task ProcessImportUsersResultAsync(ImportUsersFromExcelJobArgs args, List<ImportBusinessEntityUpdateDto> entities)
        {
            if (args.User != null)
            {
                var InvalidData = entities.Where(x=>x.IsLicenseValid == false || x.IsCityValid == false).ToList();
                if (InvalidData.Count() != 0)
                {
                    var file = _businessEntitiesExcelExporter.ExportInvalidToFile(InvalidData);
                    var message = "Download File to check Invalid Business Entity Data- " + args.Code;
                    await _appNotifier.GlobleCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName, message, "BusinessEntity-" + args.User.UserId);
                }
                else
                {
                    await _appNotifier.GlobalMessageAsync(
                args.User,
                "Update Business Entities Import Done-" + args.Code,
                "BusinessEntity-" + args.User.UserId,//_localizationSource.GetString("AllUsersSuccessfullyImportedFromExcel"),
                Abp.Notifications.NotificationSeverity.Success);
                }
            }

        }

        private void SendInvalidExcelNotification(ImportUsersFromExcelJobArgs args)
        {
            if (args.User != null)
            {

                AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                args.User,
                "Entities import process has failed. File is invalid.",//_localizationSource.GetString("FileCantBeConvertedToUserList"),
                Abp.Notifications.NotificationSeverity.Warn));
            }

        }
        private void SendInvalidExcelRowNotification(ImportUsersFromExcelJobArgs args, List<ImportBusinessEntityUpdateDto> entities)
        {
            var getError = entities.Where(x => x.CanBeImported == false).ToList().FirstOrDefault();
            AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                args.User,
                "Entities import process has failed. Please Check Row No " + getError.RowName + " , Cell " + getError.Exception,//_localizationSource.GetString("FileCantBeConvertedToUserList"),
                Abp.Notifications.NotificationSeverity.Warn));
        }
    }
}
