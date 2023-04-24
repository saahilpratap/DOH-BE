using System;
using System.Net;
using System.Threading.Tasks;
using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Linq;
using System.Collections.Generic;
using LockthreatCompliance.AuthoritativeDocuments;
using Abp.Domain.Repositories;
using LockthreatCompliance.EntityFrameworkCore.RepositoryExtensions;
using LockthreatCompliance.Domains;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using LockthreatCompliance.Web.Models.File;
using LockthreatCompliance.ControlStandards;
using LockthreatCompliance.ControlRequirements;
using Microsoft.AspNetCore.StaticFiles;
using LockthreatCompliance.CustomExceptions;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Countries;
using LockthreatCompliance.BusinessTypes;
using LockthreatCompliance.FacilityTypes;
using LockthreatCompliance.Common;
using LockthreatCompliance.Enums;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.RemediationPlans;
using LockthreatCompliance.WrokFlows;
using LockthreatCompliance.FindingReports.Dtos;
using LockthreatCompliance.AuditQuestResponses;
using LockthreatCompliance.QuestResponses;
using LockthreatCompliance.TableTopExercises;
using LockthreatCompliance.PatientAuthenticationPlatform;
using LockthreatCompliance.PatientAuthenticationPlatform.Dtos;
using DevExpress.Office.Utils;

namespace LockthreatCompliance.Web.Controllers
{
    public class FileController : LockthreatComplianceControllerBase
    {
        private readonly IRepository<TableTopExerciseSectionAttachement, long> _tableTopExerciseSectionAttchemntRepository;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IRepository<AuthoritativeDocument> _authoritativeDocumentRepository;
        private readonly IRepository<Domain> _domainRepository;
        private readonly IRepository<ControlStandard> _controlStandardRepository;
        private readonly IRepository<ControlRequirement> _controlRequirementRepository;
        private readonly IRepository<DocumentPath> _documentPathRepository;
        private readonly IRepository<Country> _countryRepository;
        //private readonly IRepository<BusinessType> _businessTypeRepository;
        private readonly IRepository<FacilityType> _facilityRepository;
        private readonly IEntityUserCreator _entityUserCreator;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IRepository<AuditDocumentPath, long> _auditDocumentPathRepository;
        private readonly IRepository<AuditDocSubModelPath, long> _auditDocSubModelPathRepository;
        private readonly IRepository<AuditQuestionResponseDocumentPath, long> _auditQuestionResponseDocumentPath;
        private readonly IRepository<RemediationDocument> _remediationDocumentRepository;
        private readonly IRepository<TemplateChecklistAttachment, long> _templatechecklistRepository;
        private readonly IRepository<Template, long> _templateServiceRepository;
        private readonly IRepository<EmailNotificationTemplate, long> _emailNotificationTemplateServiceRepository;
        private readonly IRepository<EmailReminderTemplate, long> _emailReminderTemplateServiceRepository;
        private readonly IRepository<EntityApplicationSetting> _entityApplicationSettingRepository;
        private readonly IRepository<AuditQuestResponse> _auditQuestResponseRepository;
        private readonly IRepository<PatientAuthenticationPlatformAttachment> _patientAuthenticationPlatformAttachmentRepository;
        private readonly IRepository<QuestResponse> _questResponseRepository;
        private readonly IRepository<CertificateImport> _certificateImportRepository;
        private readonly IRepository<CertificateQRCode.CertificateQRCode> _certificateQRCodeRepository;
        private readonly IRepository<TableTopExerciseEntityAttachment, long> _tableTopExerciseEntityAttachmentRepository;
        private readonly IRepository<PatientAuthenticationPlatformGlobalAttachment, long> _patientAuthenticationPlatformGlobalAttachmentRepository;

        private readonly IPatientAuthenticationPlatformAppService _PatientAuthenticationPlatformAppService;
        public FileController(IPatientAuthenticationPlatformAppService PatientAuthenticationPlatformAppService,
            IRepository<PatientAuthenticationPlatformGlobalAttachment, long> patientAuthenticationPlatformGlobalAttachmentRepository,
            IRepository<TableTopExerciseSectionAttachement, long> tableTopExerciseSectionAttchemntRepository,
            IRepository<CertificateImport> certificateImportRepository,
            IRepository<RemediationDocument> remediationDocumentRepository,
            ITempFileCacheManager tempFileCacheManager,
            IBinaryObjectManager binaryObjectManager,
            IHostingEnvironment hostingEnvironment,
            IEntityUserCreator entityUserCreator,
            IRepository<TemplateChecklistAttachment, long> templatechecklistRepository,
            IRepository<AuthoritativeDocument> authoritativeDocumentRepository,
            IRepository<Domain> domainRepository,
            IRepository<EntityApplicationSetting> entityApplicationSettingRepository,
            IRepository<ControlStandard> controlStandardRepository,
            IRepository<ControlRequirement> controlRequirementRepository,
            IRepository<DocumentPath> documentPathRepository,
            IRepository<Country> countryRepository,
            //IRepository<BusinessType> businessTypeRepository,
            IRepository<FacilityType> facilityRepository, IRepository<AuditDocSubModelPath, long> auditDocSubModelPathRepository,
            IRepository<Template, long> templateServiceRepository, IRepository<EmailNotificationTemplate, long> emailNotificationTemplateServiceRepository,
            IRepository<EmailReminderTemplate, long> emailReminderTemplateServiceRepository,
            IRepository<BusinessEntity> businessEntityRepository, IRepository<AuditDocumentPath, long> auditDocumentPathRepository, IRepository<AuditQuestResponse> auditQuestResponseRepository,
            IRepository<PatientAuthenticationPlatformAttachment> patientAuthenticationPlatformAttachmentRepository, IRepository<AuditQuestionResponseDocumentPath, long> auditQuestionResponseDocumentPath, IRepository<QuestResponse> questResponseRepository,
            IRepository<CertificateQRCode.CertificateQRCode> certificateQRCodeRepository,
            IRepository<TableTopExerciseEntityAttachment, long> tableTopExerciseEntityAttachmentRepository)
        {
            _PatientAuthenticationPlatformAppService = PatientAuthenticationPlatformAppService;
            _patientAuthenticationPlatformGlobalAttachmentRepository = patientAuthenticationPlatformGlobalAttachmentRepository;
            _tableTopExerciseSectionAttchemntRepository = tableTopExerciseSectionAttchemntRepository;
            _certificateImportRepository = certificateImportRepository;
            _templatechecklistRepository = templatechecklistRepository;
            _entityApplicationSettingRepository = entityApplicationSettingRepository;
            _remediationDocumentRepository = remediationDocumentRepository;
            _businessEntityRepository = businessEntityRepository;
            _entityUserCreator = entityUserCreator;
            //_businessTypeRepository = businessTypeRepository;
            _facilityRepository = facilityRepository;
            _countryRepository = countryRepository;
            _documentPathRepository = documentPathRepository;
            _hostingEnvironment = hostingEnvironment;
            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;
            _authoritativeDocumentRepository = authoritativeDocumentRepository;
            _domainRepository = domainRepository;
            _controlStandardRepository = controlStandardRepository;
            _controlRequirementRepository = controlRequirementRepository;
            _auditDocumentPathRepository = auditDocumentPathRepository;
            _auditDocSubModelPathRepository = auditDocSubModelPathRepository;
            _templateServiceRepository = templateServiceRepository;
            _emailNotificationTemplateServiceRepository = emailNotificationTemplateServiceRepository;
            _emailReminderTemplateServiceRepository = emailReminderTemplateServiceRepository;
            _auditQuestResponseRepository = auditQuestResponseRepository;
            _patientAuthenticationPlatformAttachmentRepository = patientAuthenticationPlatformAttachmentRepository;
            _auditQuestionResponseDocumentPath = auditQuestionResponseDocumentPath;
            _questResponseRepository = questResponseRepository;
            _certificateQRCodeRepository = certificateQRCodeRepository;
            _tableTopExerciseEntityAttachmentRepository = tableTopExerciseEntityAttachmentRepository;
        }

        [DisableAuditing]
        public ActionResult DownloadTempFile(FileDto file)
        {
            var fileBytes = _tempFileCacheManager.GetFile(file.FileToken);
            if (fileBytes == null)
            {
                return NotFound(L("RequestedFileDoesNotExists"));
            }

            return File(fileBytes, file.FileType, file.FileName);
        }

        [DisableAuditing]
        public async Task<ActionResult> DownloadBinaryFile(Guid id, string contentType, string fileName)
        {
            var fileObject = await _binaryObjectManager.GetOrNullAsync(id);
            if (fileObject == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            return File(fileObject.Bytes, contentType, fileName);
        }

        public async Task ImportAuthoritativeDocuments()
        {
            var file = Request.Form.Files[0];
            string folderName = "Upload";
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();

            //  string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string fileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (fileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }

                    var authoritativeDocuments = new List<AuthoritativeDocument>();
                    IRow headerRow = sheet.GetRow(0); //Get Header Row
                    int cellCount = headerRow.LastCellNum;
                    for (int j = 0; j < cellCount; j++)
                    {
                        ICell cell = headerRow.GetCell(j);
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                    }
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                        var authoritativeDocumentId = row.GetCell(0).ToString(); // authoritative document Id
                        var authoritativeDocumentName = row.GetCell(1).ToString(); // authoritative document name
                        authoritativeDocuments.Add(new AuthoritativeDocument
                        {
                            //  Code = authoritativeDocumentId,
                            Name = authoritativeDocumentName,
                            TenantId = AbpSession.TenantId
                        });
                    }
                    await _authoritativeDocumentRepository.InsertAllAsync(authoritativeDocuments);
                }
            }
        }
        public async Task ImportAuthoritativeDocumentDomains()
        {
            try
            {
                var file = Request.Form.Files[0];
                string folderName = "Upload";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string fileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet sheet;
                    string fullPath = Path.Combine(newPath, file.FileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;
                        if (fileExtension == ".xls")
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                        }
                        else
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                        }

                        var authoritativeDocumentDomains = new List<DomainDtoForExcel>();
                        var adIds = new HashSet<string>();
                        IRow headerRow = sheet.GetRow(0); //Get Header Row
                        int cellCount = headerRow.LastCellNum;
                        for (int j = 0; j < cellCount; j++)
                        {
                            ICell cell = headerRow.GetCell(j);
                            if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                        }
                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null) continue;
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                            var AD_Domain_ID = row.GetCell(0).ToString(); // authoritative document  domain Id
                            var AD_DOMAIN_NAME = row.GetCell(1).ToString(); // authoritative document domain name
                            var AD_ID = row.GetCell(2).ToString(); // authorirative document code

                            authoritativeDocumentDomains.Add(new DomainDtoForExcel
                            {
                                Code = AD_Domain_ID,
                                Name = AD_DOMAIN_NAME,
                                AD_ID = AD_ID
                            });
                            adIds.Add(AD_ID);
                        }

                        var authoritativeDocuments = await _authoritativeDocumentRepository.GetAll()
                           .Where(ad => adIds.ToList().Contains(ad.Code))
                            .ToListAsync();
                        var dataToInsert = new List<Domain>();
                        foreach (var authoritativeDocumentDomain in authoritativeDocumentDomains.ToList())
                        {
                            var ad = authoritativeDocuments.FirstOrDefault(e => e.Code == authoritativeDocumentDomain.AD_ID);
                            if (ad != null)
                            {
                                dataToInsert.Add(new Domain
                                {
                                    //  Code = authoritativeDocumentDomain.Code,
                                    Name = authoritativeDocumentDomain.Name,
                                    AuthoritativeDocumentId = ad.Id,
                                    TenantId = AbpSession.TenantId,
                                    AuthoritativeDocumentName = ad.Name
                                });
                            }
                        }
                        await _domainRepository.InsertAllAsync(dataToInsert);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task ImportControlStandards()
        {
            var file = Request.Form.Files[0];
            string folderName = "Upload";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string fileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (fileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }

                    var controlStandards = new List<ControlStandard>();
                    var domainNames = new HashSet<string>();
                    IRow headerRow = sheet.GetRow(0); //Get Header Row
                    int cellCount = headerRow.LastCellNum;
                    for (int j = 0; j < cellCount; j++)
                    {
                        ICell cell = headerRow.GetCell(j);
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                    }
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                        var controlId = row.GetCell(0).ToString(); // control Id
                        var originalControlId = row.GetCell(1).ToString(); // original control id
                        var controlDescription = row.GetCell(2).ToString(); // control description
                        var controlName = row.GetCell(3).ToString(); //control name
                        var domainName = row.GetCell(4).ToString(); // domain name

                        controlStandards.Add(new ControlStandard
                        {
                            //  Code = controlId,
                            OriginalControlId = originalControlId,
                            Description = controlDescription,
                            Name = controlName,
                            DomainName = domainName,
                            TenantId = AbpSession.TenantId
                        });
                        domainNames.Add(domainName);
                    }
                    var allDomains = await _domainRepository.GetAll()
                        .Where(e => domainNames.ToList().Contains(e.Name))
                        .ToListAsync();
                    controlStandards.ToList().ForEach(controlStandard =>
                    {
                        var domain = allDomains.FirstOrDefault(e => e.Name == controlStandard.DomainName);
                        if (domain == null)
                        {
                            controlStandards.Remove(controlStandard);
                        }
                        else
                        {
                            controlStandard.DomainId = domain.Id;
                            controlStandard.AuthoritativeDocumentId = domain.AuthoritativeDocumentId;
                        }
                    });
                    await _controlStandardRepository.InsertAllAsync(controlStandards);
                }
            }
        }
        public async Task ImportControlRequirements()
        {
            var file = Request.Form.Files[0];
            string folderName = "Upload";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string fileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (fileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }

                    var controlRequirements = new List<ControlRequirement>();
                    var controlStandardNames = new HashSet<string>();
                    IRow headerRow = sheet.GetRow(0); //Get Header Row
                    int cellCount = headerRow.LastCellNum;
                    for (int j = 0; j < cellCount; j++)
                    {
                        ICell cell = headerRow.GetCell(j);
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                    }
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                        var originalControlId = row.GetCell(0).ToString(); // original control Id
                        var controlStandardName = row.GetCell(1).ToString(); // control Standard name
                        var controlRequirement = row.GetCell(2).ToString(); // control  requirement description
                        Enum.TryParse(row.GetCell(3).ToString(), out ControlType controlType);
                        var domainName = row.GetCell(4).ToString(); // domain name

                        controlRequirements.Add(new ControlRequirement
                        {
                            ControlStandardName = controlStandardName,
                            ControlType = controlType,
                            Description = controlRequirement,
                            OriginalId = originalControlId,
                            DomainName = domainName,
                            TenantId = AbpSession.TenantId
                        });
                        controlStandardNames.Add(controlStandardName);
                    }
                    var allControlStandards = await _controlStandardRepository.GetAll()
                        .Where(e => controlStandardNames.ToList().Contains(e.Name))
                        .ToListAsync();

                    foreach (var controlRequirement in controlRequirements.ToList())
                    {
                        var controlStandard = allControlStandards.FirstOrDefault(e => e.Name == controlRequirement.ControlStandardName);
                        if (controlStandard == null)
                        {
                            controlRequirements.Remove(controlRequirement);
                        }
                        else
                        {
                            controlRequirement.ControlStandardId = controlStandard.Id;
                            controlRequirement.AuthoritativeDocumentId = controlStandard.AuthoritativeDocumentId;

                        }
                    }
                    await _controlRequirementRepository.InsertAllAsync(controlRequirements);
                }
            }
        }
        public async Task ImportBusinessEntities()
        {
            var file = Request.Form.Files.First();
            string folderName = "Upload";

            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string fileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (fileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }
                    var countries = await _countryRepository.GetAll().ToListAsync();
                    //  var businessTypes = await _businessTypeRepository.GetAll().ToListAsync();
                    var facilityTypes = await _facilityRepository.GetAll().ToListAsync();

                    var businessEntities = new List<BusinessEntity>();
                    IRow headerRow = sheet.GetRow(0); //Get Header Row
                    int cellCount = headerRow.LastCellNum;
                    for (int j = 0; j < cellCount; j++)
                    {
                        ICell cell = headerRow.GetCell(j);
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                    }
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                        //  var businessTypeId = businessTypes.FirstOrDefault(bt => bt.Name == row.GetCell(6).ToString())?.Id;
                        var facilityTypeId = facilityTypes.FirstOrDefault(bt => bt.Name == row.GetCell(7).ToString())?.Id;
                        var parentCompanyName = row.GetCell(18) == null ? "" : row.GetCell(18).ToString();
                        Enum.TryParse(row.GetCell(5).ToString(), out ControlType controlType);
                        var hasParent = row.GetCell(17).ToString().ToUpper() == "TRUE";
                        var countryId = countries.FirstOrDefault(c => c.Name == row.GetCell(13).ToString())?.Id;

                        var entityId = row.GetCell(0) == null ? "" : row.GetCell(0).ToString();
                        var companyName = row.GetCell(1) == null ? "" : row.GetCell(1).ToString();
                        var companyLegalName = row.GetCell(2) == null ? "" : row.GetCell(2).ToString();
                        var companyWebsite = row.GetCell(3) == null ? "" : row.GetCell(3).ToString();
                        var yearsInBusiness = int.Parse(row.GetCell(4).ToString());
                        var isSuspended = row.GetCell(8).ToString().ToUpper() == "TRUE";
                        var isGovernmentOwned = row.GetCell(9).ToString().ToUpper() == "TRUE";
                        var isCompanyLicensed = row.GetCell(10).ToString().ToUpper() == "TRUE";
                        var licenseNumber = row.GetCell(11) == null ? "" : row.GetCell(11).ToString();
                        var companyAddress = row.GetCell(12) == null ? "" : row.GetCell(12).ToString();
                        var cityOrDistrict = row.GetCell(14) == null ? "" : row.GetCell(14).ToString();
                        var postalCode = row.GetCell(15) == null ? "" : row.GetCell(15).ToString();
                        var isAuditable = row.GetCell(16).ToString().ToUpper() == "TRUE";
                        var adminName = row.GetCell(19) == null ? "" : row.GetCell(19).ToString();
                        var adminPosition = row.GetCell(20) == null ? "" : row.GetCell(20).ToString();
                        var adminEmail = row.GetCell(21) == null ? "" : row.GetCell(21).ToString();
                        var adminMobile = row.GetCell(22) == null ? "" : row.GetCell(22).ToString();

                        var businessEntity = new BusinessEntity
                        {
                            AdminEmail = adminEmail,
                            AdminMobile = adminMobile,
                            AdminPosition = adminPosition,
                            AdminName = adminName,
                            //   BusinessTypeId = businessTypeId.Value,
                            CityOrDisctrict = cityOrDistrict,
                            CountryId = countryId.Value,
                            FacilityTypeId = facilityTypeId.Value,
                            CompanyAddress = companyAddress,
                            PostalCode = postalCode,
                            NumberOfYearsInBusiness = yearsInBusiness,
                            LicenseNumber = licenseNumber,
                            IsAuditableEntity = isAuditable,
                            IsCompanyLicensed = isCompanyLicensed,
                            IsSuspended = isSuspended,
                            IsGovernmentOwned = isGovernmentOwned,
                            ComplianceType = controlType,
                            IsParentReportingEnabled = hasParent,
                            //  ParentCompanyName = parentCompanyName,
                            TenantId = AbpSession.TenantId
                        };
                        businessEntities.Add(businessEntity);
                    }
                    foreach (var businessEntity in businessEntities)
                    {
                        //var organizationUser = await _entityUserCreator.CreateAsync
                        //                        (
                        //                        businessEntity.AdminEmail,
                        //                        businessEntity.AdminEmail,
                        //                        AbpSession.TenantId,
                        //                        EntityType.BusinessEntity,
                        //                        businessEntity.CompanyName,
                        //                        null,
                        //                        false
                        //                        );
                        //businessEntity.OrganizationUnit = organizationUser.OrganizationUnit;
                        //businessEntity.OrganizationUnitId = organizationUser.OrganizationUnit.Id;
                        //await _businessEntityRepository.InsertAsync(businessEntity);
                    }
                }
            }
        }
        public async Task<ActionResult> UploadReviewAttachment([FromForm] UploadDocumentInput input)
        {
            var file = input.File;
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();

            var uploads = Path.Combine(webRootPath, "DocumentStorage");
            var documentPath = new DocumentPath(file.FileName, input.ReviewId);
            await _documentPathRepository.InsertAsync(documentPath);

            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }
            if (file.Length > 0)
            {
                var filePath = Path.Combine(uploads, documentPath.Code);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

            }
            var uploadResult = new UploadedFileOutput
            {
                Code = documentPath.Code,
                FileName = documentPath.FileName
            };
            return Ok(uploadResult);
        }
        public async Task<ActionResult> UploadQuestionAttachment([FromForm] UploadQuestionAttachmentInput input)
        {
            var file = input.File;
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();

            var uploads = Path.Combine(webRootPath, "DocumentStorage");
            var documentPath = new DocumentPath(file.FileName, null, input.ReviewQuestionId);
            await _documentPathRepository.InsertAsync(documentPath);

            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }
            if (file.Length > 0)
            {
                var filePath = Path.Combine(uploads, documentPath.Code);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            var uploadResult = new UploadedFileOutput
            {
                Code = documentPath.Code,
                FileName = documentPath.FileName
            };
            return Ok(uploadResult);
        }
        public async Task<ActionResult> UploadAttachments([FromForm] AttachmentInput input)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            var   uploadResult = new UploadedFileOutput();
            var file = input.File;

            int FindingId =int.Parse(input.Title);

            var uploads = Path.Combine(webRootPath, "DocumentStorage");
            var documentPath = new DocumentPath(file.FileName, null, null, file.FileName);

            var getcheck = await _documentPathRepository.GetAll().Where(x => x.FindingReportId == FindingId && x.FileName.Trim().ToLower()== file.FileName.Trim().ToLower()).FirstOrDefaultAsync();
            if (getcheck == null)
            {
                var getId = await _documentPathRepository.InsertOrUpdateAndGetIdAsync(documentPath);

                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(uploads, documentPath.Code);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                }

               uploadResult = new UploadedFileOutput
                {
                    Code = documentPath.Code,
                    FileName = documentPath.FileName

                };
            }
            else
            {
                 uploadResult = new UploadedFileOutput
                {
                    Code = getcheck.Code,
                    FileName = getcheck.FileName

                };
            }
            return Ok(uploadResult);
        }

        [HttpGet]
        public async Task<ActionResult> Download([FromQuery] string file)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            var uploads = Path.Combine(webRootPath, "DocumentStorage");
            var documentPath = await _documentPathRepository.FirstOrDefaultAsync(e => e.Code == file);
            if (documentPath == null)
            {
                throw new NotFoundException($"Couldn't find document with code {file}");
            }
            var filePath = Path.Combine(uploads, file);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), documentPath.FileName);
        }

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        public async Task DeleteAttachment(string code)
        {
            if (code != "undefined" && code != null)
            {
                string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();

                var filePath = Path.Combine(webRootPath, "DocumentStorage");
                var removefile = _documentPathRepository.FirstOrDefault(c => c.Code == code);
                if (removefile != null)
                {
                    await _documentPathRepository.DeleteAsync(removefile);
                    if (Directory.Exists(filePath))
                    {
                        var file = Directory.GetFiles(filePath, code).FirstOrDefault();
                        if (file != null)
                        {
                            System.IO.File.Delete(file);
                        }

                    }
                }
            }
        }

        public async Task<ActionResult> UploadAuditProjectAttachments([FromForm] AttachmentInput input)
        {
            var file = input.File;
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();

            var uploads = Path.Combine(webRootPath, "AuditProjectStorage");
            var documentPath = new AuditDocumentPath(file.FileName, null, input.Title);
            await _auditDocumentPathRepository.InsertAsync(documentPath);

            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }
            if (file.Length > 0)
            {
                var filePath = Path.Combine(uploads, documentPath.Code);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

            }
            var uploadResult = new UploadedFileOutput
            {
                Code = documentPath.Code,
                FileName = documentPath.FileName
            };
            return Ok(uploadResult);
        }

        public async Task<ActionResult> UploadAuditSubDocProjAttachments([FromForm] AttachmentInput input)
        {
            var file = input.File;
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();

            var uploads = Path.Combine(webRootPath, "AuditProjectStorage");
            var documentPath = new AuditDocSubModelPath(file.FileName, input.Title, null, null, null, null);
            await _auditDocSubModelPathRepository.InsertAsync(documentPath);

            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }
            if (file.Length > 0)
            {
                var filePath = Path.Combine(uploads, documentPath.Code);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

            }
            var uploadResult = new UploadedFileOutput
            {
                Code = documentPath.Code,
                FileName = documentPath.FileName
            };
            return Ok(uploadResult);
        }

        public async Task DeleteAuditAttachment(string code)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();

            var filePath = Path.Combine(webRootPath, "AuditProjectStorage");
            var removefile = _auditDocumentPathRepository.FirstOrDefault(c => c.Code == code);
            if (removefile != null)
            {
                await _auditDocumentPathRepository.HardDeleteAsync(removefile);
                if (Directory.Exists(filePath))
                {
                    var file = Directory.GetFiles(filePath, code).FirstOrDefault();
                    if (file != null)
                    {
                        System.IO.File.Delete(file);
                    }

                }
            }
        }

        public async Task DeleteAuditSubAttachment(string code)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            var filePath = Path.Combine(webRootPath, "AuditProjectStorage");
            var removefile = _auditDocSubModelPathRepository.FirstOrDefault(c => c.Code == code);
            if (removefile != null)
            {
                await _auditDocSubModelPathRepository.HardDeleteAsync(removefile);
                if (Directory.Exists(filePath))
                {
                    var file = Directory.GetFiles(filePath, code).FirstOrDefault();
                    if (file != null)
                    {
                        System.IO.File.Delete(file);
                    }


                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> DownloadAuditAttachment([FromQuery] string file)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            var uploads = Path.Combine(webRootPath, "AuditProjectStorage");
            var documentPath = await _auditDocumentPathRepository.FirstOrDefaultAsync(e => e.Code == file);
            if (documentPath == null)
            {
                throw new NotFoundException($"Couldn't find document with code {file}");
            }
            var filePath = Path.Combine(uploads, file);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), documentPath.FileName);
        }

        [HttpGet]
        public async Task<ActionResult> DownloadAuditSubAttachment([FromQuery] string file)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();

            var uploads = Path.Combine(webRootPath, "AuditProjectStorage");
            var documentPath = await _auditDocSubModelPathRepository.FirstOrDefaultAsync(e => e.Code == file);
            if (documentPath == null)
            {
                throw new NotFoundException($"Couldn't find document with code {file}");
            }
            var filePath = Path.Combine(uploads, file);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), documentPath.FileName);
        }

        public async Task<ActionResult> UploadRemediationAttachments([FromForm] AttachmentInput input)
        {
            var uploadResult = new UploadedFileOutput();
            try
            {
                var file = input.File;
                string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
                if (webRootPath != "")
                {
                    //string webRootPaths=@+:+webRootPath.ToString();
                    var uploads = Path.Combine(webRootPath, "DocumentStorage");
                    var documentPath = new RemediationDocument(file.FileName, input.Title);
                    await _remediationDocumentRepository.InsertAsync(documentPath);

                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }
                    if (file.Length > 0)
                    {
                        var filePath = Path.Combine(uploads, documentPath.Code);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                    }
                    uploadResult = new UploadedFileOutput
                    {
                        Code = documentPath.Code,
                        FileName = documentPath.FileName
                    };

                }
                else
                {
                    throw new UserFriendlyException("Please Insert Appsetting Document Path");
                }
                return Ok(uploadResult);
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public async Task DeleteRemediationAttachment(string code)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();

            var filePath = Path.Combine(webRootPath, "DocumentStorage");
            var removefile = _remediationDocumentRepository.FirstOrDefault(c => c.Code == code);
            if (removefile != null)
            {
                await _remediationDocumentRepository.HardDeleteAsync(removefile);
                if (Directory.Exists(filePath))
                {
                    var file = Directory.GetFiles(filePath, code).FirstOrDefault();
                    if (file != null)
                    {
                        System.IO.File.Delete(file);
                    }

                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> DownloadRemediationAttachment([FromQuery] string file)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            var uploads = Path.Combine(webRootPath, "DocumentStorage");
            var documentPath = await _remediationDocumentRepository.FirstOrDefaultAsync(e => e.Code == file);
            if (documentPath == null)
            {
                throw new NotFoundException($"Couldn't find document with code {file}");
            }
            var filePath = Path.Combine(uploads, file);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), documentPath.FileName);

        }

        public async Task<ActionResult> UploadTeamplateAttachments([FromForm] AttachmentInput input)
        {
            var uploadResult = new UploadedFileOutput();
            try
            {
                var file = input.File;
                string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
                if (webRootPath != "")
                {
                    //string webRootPaths=@+:+webRootPath.ToString();
                    var uploads = Path.Combine(webRootPath, "TemplateCheckList");
                    var documentPath = new TemplateChecklistAttachment(file.FileName, input.Title);
                    await _templatechecklistRepository.InsertAsync(documentPath);

                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }
                    if (file.Length > 0)
                    {
                        var filePath = Path.Combine(uploads, documentPath.Code);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                    }
                    uploadResult = new UploadedFileOutput
                    {
                        Code = documentPath.Code,
                        FileName = documentPath.FileName
                    };

                }
                else
                {
                    throw new UserFriendlyException("Please Insert Appsetting Document Path");
                }
                return Ok(uploadResult);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteTeamplateAttachment(string code)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();

            var filePath = Path.Combine(webRootPath, "TemplateCheckList");
            var removefile = _templatechecklistRepository.FirstOrDefault(c => c.Code == code);
            if (removefile != null)
            {
                await _templatechecklistRepository.HardDeleteAsync(removefile);
                if (Directory.Exists(filePath))
                {
                    var file = Directory.GetFiles(filePath, code).FirstOrDefault();
                    if (file != null)
                    {
                        System.IO.File.Delete(file);
                    }

                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> DownloadTeamplateAttachment([FromQuery] string file)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            var uploads = Path.Combine(webRootPath, "TemplateCheckList");
            var documentPath = await _templatechecklistRepository.FirstOrDefaultAsync(e => e.Code == file);
            if (documentPath == null)
            {
                throw new NotFoundException($"Couldn't find document with code {file}");
            }
            var filePath = Path.Combine(uploads, file);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), documentPath.FileName);

        }

        public async Task<ActionResult> UploadWorkflowTeamplateAttachments([FromForm] AttachmentInput input)
        {
            var uploadResult = new UploadedFileOutput();
            try
            {
                var file = input.File;
                string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
                if (webRootPath != "")
                {
                    //string webRootPaths=@+:+webRootPath.ToString();
                    var uploads = Path.Combine(webRootPath, "WorkflowTeamplate");
                    var documentPath = new TemplateChecklistAttachment(file.FileName, input.Title);
                    var tempOjb = await _templateServiceRepository.GetAll().FirstOrDefaultAsync(x => x.TemplateTitle == input.Title);


                    var attachmentList = new List<AttachmentWithTitleDto>();
                    var obj = new AttachmentWithTitleDto();

                    if (("" + tempOjb.TemplateDescription).Length == 0)
                    {
                        obj.Code = documentPath.Code;
                        obj.Title = documentPath.FileName;
                        attachmentList.Add(obj);
                        tempOjb.TemplateDescription = Newtonsoft.Json.JsonConvert.SerializeObject(attachmentList);
                    }
                    else
                    {
                        attachmentList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AttachmentWithTitleDto>>(tempOjb.TemplateDescription);
                        obj.Code = documentPath.Code;
                        obj.Title = documentPath.FileName;
                        attachmentList.Add(obj);
                        tempOjb.TemplateDescription = Newtonsoft.Json.JsonConvert.SerializeObject(attachmentList);
                    }

                    var id = _templateServiceRepository.Update(tempOjb);


                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }
                    if (file.Length > 0)
                    {
                        var filePath = Path.Combine(uploads, documentPath.Code);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                    }
                    uploadResult = new UploadedFileOutput
                    {
                        Code = documentPath.Code,
                        FileName = documentPath.FileName
                    };

                }
                else
                {
                    throw new UserFriendlyException("Please Insert Appsetting Document Path");
                }
                return Ok(uploadResult);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteWorkflowTeamplateAttachment(string code, long Id)
        {

            try
            {
                string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();

                var filePath = Path.Combine(webRootPath, "WorkflowTeamplate");


                var documentPath = await _templateServiceRepository.GetAll().Where(x => x.Id == Id).FirstOrDefaultAsync();

                var fileAttachList = new List<AttachmentWithTitleDto>();

                var listofFileNames = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AttachmentWithTitleDto>>(documentPath.TemplateDescription);
                if (listofFileNames != null)
                {

                    fileAttachList = listofFileNames;

                }

                if (documentPath.Id != 0)
                {
                    var updatedList = fileAttachList.Where(x => x.Code != code);
                    documentPath.TemplateDescription = (updatedList.Count() != 0) ? Newtonsoft.Json.JsonConvert.SerializeObject(updatedList) : "";
                    await _templateServiceRepository.UpdateAsync(documentPath);
                }


                if (Directory.Exists(filePath))
                {
                    var file = Directory.GetFiles(filePath, code).FirstOrDefault();
                    if (file != null)
                    {
                        System.IO.File.Delete(file);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<ActionResult> DownloadWorkflowTeamplateAttachment(string file, long Id)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            var uploads = Path.Combine(webRootPath, "WorkflowTeamplate");
            var documentPath = await _templateServiceRepository.GetAll().Where(x => x.Id == Id).FirstOrDefaultAsync();

            var fileName = "";
            if (documentPath == null)
            {
                throw new NotFoundException($"Couldn't find document with code {file}");
            }


            var listofFileNames = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AttachmentWithTitleDto>>(documentPath.TemplateDescription);
            foreach (var item1 in listofFileNames)
            {
                if (item1.Code == file)
                {
                    fileName = item1.Title;
                }
            }


            if (fileName == "")
            {
                throw new NotFoundException($"Couldn't find document with code {file}");
            }

            var filePath = Path.Combine(uploads, file);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), fileName);

        }

        public async Task<ActionResult> UploadEmailNotificationAttachments([FromForm] AttachmentInput input)
        {
            var uploadResult = new UploadedFileOutput();
            try
            {
                var file = input.File;
                string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
                if (webRootPath != "")
                {
                    //string webRootPaths=@+:+webRootPath.ToString();
                    var uploads = Path.Combine(webRootPath, "EmailNotification");
                    var documentPath = new TemplateChecklistAttachment(file.FileName, input.Title);

                    var tempOjb = await _emailNotificationTemplateServiceRepository.GetAll().FirstOrDefaultAsync(x => x.Id == int.Parse(input.Title));

                    var attachmentList = new List<AttachmentWithTitleDto>();
                    var obj = new AttachmentWithTitleDto();

                    if (("" + tempOjb.AttachmentJson).Length == 0)
                    {
                        obj.Code = documentPath.Code;
                        obj.Title = documentPath.FileName;
                        attachmentList.Add(obj);
                        tempOjb.AttachmentJson = Newtonsoft.Json.JsonConvert.SerializeObject(attachmentList);
                    }
                    else
                    {
                        attachmentList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AttachmentWithTitleDto>>(tempOjb.AttachmentJson);
                        obj.Code = documentPath.Code;
                        obj.Title = documentPath.FileName;
                        attachmentList.Add(obj);
                        tempOjb.AttachmentJson = Newtonsoft.Json.JsonConvert.SerializeObject(attachmentList);
                    }

                    var id = _emailNotificationTemplateServiceRepository.Update(tempOjb);

                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }
                    if (file.Length > 0)
                    {
                        var filePath = Path.Combine(uploads, documentPath.Code);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                    uploadResult = new UploadedFileOutput
                    {
                        Code = documentPath.Code,
                        FileName = documentPath.FileName
                    };

                }
                else
                {
                    throw new UserFriendlyException("Please Insert Appsetting Document Path");
                }
                return Ok(uploadResult);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<ActionResult> DownloadEmailNotificationAttachment(string file, long Id)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            var uploads = Path.Combine(webRootPath, "EmailNotification");
            var documentPath = await _emailNotificationTemplateServiceRepository.GetAll().Where(x => x.Id == Id).FirstOrDefaultAsync();

            var fileName = "";
            if (documentPath == null)
            {
                throw new NotFoundException($"Couldn't find document with code {file}");
            }


            if (documentPath != null)
            {
                var listofFileNames = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AttachmentWithTitleDto>>(documentPath.AttachmentJson);
                if (listofFileNames != null)
                {
                    foreach (var item1 in listofFileNames)
                    {
                        if (item1.Code == file)
                        {
                            fileName = item1.Title;
                        }
                    }
                }
            }


            if (fileName == "")
            {
                throw new NotFoundException($"Couldn't find document with code {file}");
            }

            var filePath = Path.Combine(uploads, file);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), fileName);

        }

        public async Task<ActionResult> UploadEmailReminderAttachments([FromForm] AttachmentInput input)
        {
            var uploadResult = new UploadedFileOutput();
            try
            {
                var file = input.File;
                string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
                if (webRootPath != "")
                {
                    //string webRootPaths=@+:+webRootPath.ToString();
                    var uploads = Path.Combine(webRootPath, "EmailReminder");
                    var documentPath = new TemplateChecklistAttachment(file.FileName, input.Title);

                    var tempOjb = await _emailReminderTemplateServiceRepository.GetAll().FirstOrDefaultAsync(x => x.Id == int.Parse(input.Title));

                    var attachmentList = new List<AttachmentWithTitleDto>();
                    var obj = new AttachmentWithTitleDto();

                    if (("" + tempOjb.AttachmentJson).Length == 0)
                    {
                        obj.Code = documentPath.Code;
                        obj.Title = documentPath.FileName;
                        attachmentList.Add(obj);
                        tempOjb.AttachmentJson = Newtonsoft.Json.JsonConvert.SerializeObject(attachmentList);
                    }
                    else
                    {
                        attachmentList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AttachmentWithTitleDto>>(tempOjb.AttachmentJson);
                        obj.Code = documentPath.Code;
                        obj.Title = documentPath.FileName;
                        attachmentList.Add(obj);
                        tempOjb.AttachmentJson = Newtonsoft.Json.JsonConvert.SerializeObject(attachmentList);
                    }

                    var id = _emailReminderTemplateServiceRepository.Update(tempOjb);

                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }
                    if (file.Length > 0)
                    {
                        var filePath = Path.Combine(uploads, documentPath.Code);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                    uploadResult = new UploadedFileOutput
                    {
                        Code = documentPath.Code,
                        FileName = documentPath.FileName
                    };

                }
                else
                {
                    throw new UserFriendlyException("Please Insert Appsetting Document Path");
                }
                return Ok(uploadResult);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<ActionResult> DownloadEmailReminderAttachment(string file, long Id)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            var uploads = Path.Combine(webRootPath, "EmailReminder");
            var documentPath = await _emailReminderTemplateServiceRepository.GetAll().Where(x => x.Id == Id).FirstOrDefaultAsync();

            var fileName = "";
            if (documentPath == null)
            {
                throw new NotFoundException($"Couldn't find document with code {file}");
            }



            var listofFileNames = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AttachmentWithTitleDto>>(documentPath.AttachmentJson);
            if (listofFileNames != null)
            {
                foreach (var item1 in listofFileNames)
                {
                    if (item1.Code == file)
                    {
                        fileName = item1.Title;
                    }
                }
            }



            if (fileName == "")
            {
                throw new NotFoundException($"Couldn't find document with code {file}");
            }

            var filePath = Path.Combine(uploads, file);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), fileName);

        }


        [HttpGet]
        public async Task<ActionResult> Downloadreports([FromQuery] string file)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            var uploads = Path.Combine(webRootPath, "AuditReports");
            var documentPath = await _auditDocumentPathRepository.GetAll().ToListAsync();

            var fileName = "";
            if (documentPath == null)
            {
                throw new NotFoundException($"Couldn't find document with code {file}");
            }

            foreach (var item in documentPath)
            {
                
                if (item.FileName == file)
                {
                    fileName = item.FileName;
                }
            }

            if (fileName == "")
            {
                throw new NotFoundException($"Couldn't find document with code {file}");
            }

            var filePath = Path.Combine(uploads, file);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), fileName);

        }

        [HttpGet]
        public async Task<ActionResult> DownloadEntityCertificate([FromQuery] string file)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            var uploads = Path.Combine(webRootPath, "Certificate");
            var documentPath = await _certificateImportRepository.GetAll().ToListAsync();

            var fileName = "";
            if (documentPath == null)
            {
                throw new NotFoundException($"Couldn't find document with code {file}");
            }

            foreach (var item in documentPath)
            {
                if (item.FileName == file)
                {
                    fileName = item.FileName;
                }
            }

            if (fileName == "")
            {
                throw new NotFoundException($"Couldn't find document with code {file}");
            }

            var filePath = Path.Combine(uploads, file);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), fileName);

        }


        [HttpGet]
        public async Task<ActionResult> DownloadCertificates([FromQuery] string file)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            var uploads = Path.Combine(webRootPath, "Certificate");
            var documentPath = await _certificateImportRepository.GetAll().ToListAsync();

            var fileName = "";
            if (documentPath == null)
            {
                throw new NotFoundException($"Couldn't find document with code {file}");
            }

            foreach (var item in documentPath)
            {

                if (item.FileName == file)
                {
                    fileName = item.FileName;
                }
            }

            if (fileName == "")
            {
                throw new NotFoundException($"Couldn't find document with code {file}");
            }

            var filePath = Path.Combine(uploads, file);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), fileName);

        }


        public async Task DeleteEmailNotificationAttachment(string code, long id)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();

            var filePath = Path.Combine(webRootPath, "EmailNotification");


            var documentPath = await _emailNotificationTemplateServiceRepository.GetAll().Where(x => x.Id == id).FirstOrDefaultAsync();

            var fileAttachList = new List<AttachmentWithTitleDto>();



            if (documentPath.AttachmentJson != null)
            {
                var listofFileNames = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AttachmentWithTitleDto>>(documentPath.AttachmentJson);
                if (listofFileNames != null)
                {
                    fileAttachList = listofFileNames;
                }
            }


            if (documentPath.Id != 0)
            {
                var updatedList = fileAttachList.Where(x => x.Code != code);
                documentPath.AttachmentJson = (updatedList.Count() != 0) ? Newtonsoft.Json.JsonConvert.SerializeObject(updatedList) : "";
                await _emailNotificationTemplateServiceRepository.UpdateAsync(documentPath);
            }


            if (Directory.Exists(filePath))
            {
                var file = Directory.GetFiles(filePath, code).FirstOrDefault();
                System.IO.File.Delete(file);
            }
        }

        public async Task DeleteEmailReminderAttachment(string code, long id)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();

            var filePath = Path.Combine(webRootPath, "EmailReminder");


            var documentPath = await _emailReminderTemplateServiceRepository.GetAll().Where(x => x.Id == id).FirstOrDefaultAsync();

            var fileAttachList = new List<AttachmentWithTitleDto>();

            var listofFileNames = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AttachmentWithTitleDto>>(documentPath.AttachmentJson);

            if (listofFileNames != null)
            {
                fileAttachList = listofFileNames;
            }

            if (documentPath.Id != 0)
            {
                var updatedList = fileAttachList.Where(x => x.Code != code);
                documentPath.AttachmentJson = (updatedList.Count() != 0) ? Newtonsoft.Json.JsonConvert.SerializeObject(updatedList) : "";
                await _emailReminderTemplateServiceRepository.UpdateAsync(documentPath);
            }


            if (Directory.Exists(filePath))
            {
                var file = Directory.GetFiles(filePath, code).FirstOrDefault();
                if (file != null)
                {
                    System.IO.File.Delete(file);
                }

            }
        }


        public async Task<ActionResult> UploadAuditQuestResponseAttachments([FromForm] AttachmentInput input)
        {
            try
            {
                var file = input.File;
                string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();

                var uploads = Path.Combine(webRootPath, "AuditProjectStorage");
                var letId = input.Title.Split(",");
                var documentPath = new AuditQuestionResponseDocumentPath(file.FileName, input.Title, int.Parse(letId[0]), int.Parse(letId[1]), null);
                //await _auditQuestionResponseDocumentPath.InsertAsync(documentPath);

                var obj = await _auditQuestResponseRepository.GetAll().Include(x => x.AuditProject).Include(x => x.Question)
                    .Where(x => x.AuditProjectId == long.Parse(letId[0]) && x.QuestionId == long.Parse(letId[1]) && x.QuestionGroupId == long.Parse(letId[2])).FirstOrDefaultAsync();

                if (obj == null)
                {
                    AuditQuestResponse tempOjb = new AuditQuestResponse()
                    {
                        QuestionId = int.Parse(letId[1]),
                        AuditProjectId = long.Parse(letId[0]),
                        QuestionGroupId = long.Parse(letId[2]),
                        Attachment = documentPath.Code,
                        FileName = documentPath.FileName,
                        Id = 0,
                    };
                    var id = await _auditQuestResponseRepository.InsertAndGetIdAsync(tempOjb);
                }
                else
                {
                    obj.Attachment = documentPath.Code;
                    obj.FileName = documentPath.FileName;
                    obj.QuestionGroupId = long.Parse(letId[2]);
                    var id = await _auditQuestResponseRepository.UpdateAsync(obj);

                }

                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(uploads, documentPath.Code);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                }
                var uploadResult = new UploadAuditQuesResFileOutput
                {
                    Code = documentPath.Code,
                    FileName = documentPath.FileName,
                    AuditProjectId = long.Parse(letId[0]),
                    QuestionId = long.Parse(letId[1]),
                };
                return Ok(uploadResult);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteAuditQuestionResponse(string code)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            var filePath = Path.Combine(webRootPath, "AuditProjectStorage");
            var removefile = _auditQuestResponseRepository.FirstOrDefault(c => c.Attachment == code);
            if (removefile != null)
            {
                removefile.Attachment = null;
                removefile.FileName = null;
                var update = _auditQuestResponseRepository.Update(removefile);
            }
            //await _auditDocSubModelPathRepository.HardDeleteAsync(removefile);
            if (Directory.Exists(filePath))
            {
                var file = Directory.GetFiles(filePath, code).FirstOrDefault();
                if (file != null)
                {
                    System.IO.File.Delete(file);
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> DownloadAuditQuestionResponse([FromQuery] string file)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            var uploads = Path.Combine(webRootPath, "AuditProjectStorage");
            var documentPath = await _auditQuestResponseRepository.FirstOrDefaultAsync(e => e.Attachment == file);
            if (documentPath == null)
            {
                throw new NotFoundException($"Couldn't find document with code {file}");
            }
            var filePath = Path.Combine(uploads, file);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), documentPath.FileName);
        }

        public async Task<ActionResult> UploadAssessmentQuestionResponse([FromForm] AttachmentInput input)
        {
            try
            {
                var file = input.File;
                string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();

                var uploads = Path.Combine(webRootPath, "SelfAssessmentStorage");
                var letId = input.Title.Split(",");
                var documentPath = new AuditQuestionResponseDocumentPath(file.FileName, input.Title, int.Parse(letId[0]), int.Parse(letId[1]), null);
                //await _auditQuestionResponseDocumentPath.InsertAsync(documentPath);

                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(uploads, documentPath.Code);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                }
                var uploadResult = new UploadAuditQuesResFileOutput
                {
                    Code = documentPath.Code,
                    FileName = documentPath.FileName,
                    AuditProjectId = long.Parse(letId[0]),
                    QuestionId = long.Parse(letId[1]),
                };
                return Ok(uploadResult);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteAssessmentQuestionResponse(string code)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            var filePath = Path.Combine(webRootPath, "SelfAssessmentStorage");
            var removefile = _questResponseRepository.FirstOrDefault(c => c.Attachment == code);
            if (removefile != null)
            {
                removefile.Attachment = null;
                removefile.FileName = null;
                var update = _questResponseRepository.Update(removefile);
            }
            //await _auditDocSubModelPathRepository.HardDeleteAsync(removefile);
            if (Directory.Exists(filePath))
            {
                var file = Directory.GetFiles(filePath, code).FirstOrDefault();
                if (file != null)
                {
                    System.IO.File.Delete(file);
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> DownloadAssessmentQuestionResponse([FromQuery] string file)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            var uploads = Path.Combine(webRootPath, "SelfAssessmentStorage");
            var documentPath = await _questResponseRepository.FirstOrDefaultAsync(e => e.Attachment == file);
            if (documentPath == null)
            {
                throw new NotFoundException($"Couldn't find document with code {file}");
            }
            var filePath = Path.Combine(uploads, file);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), documentPath.FileName);
        }

        public async Task<ActionResult> UploadPAPAttachments([FromForm] AttachmentInput input)
        {
            try
            {
                var file = input.File;
                string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();

                var uploads = Path.Combine(webRootPath, "PAPStorage");
                var letId = input.Title.Split(",");
                var documentPath = new AuditQuestionResponseDocumentPath(file.FileName, input.Title, int.Parse(letId[0]), int.Parse(letId[1]), null);
               
               
                    PatientAuthenticationPlatformAttachment tempOjb = new PatientAuthenticationPlatformAttachment()
                    {
                        PAPAttachmentType = (int.Parse(letId[1]) == 0) ? PAPAttachmentType.BusinessEntity : PAPAttachmentType.Authority,
                        PAPId = int.Parse(letId[0]),
                        Code = documentPath.Code,
                        FileName = documentPath.FileName,
                        Title = documentPath.FileName,
                        Id = 0,
                    };

                    var id = await _patientAuthenticationPlatformAttachmentRepository.InsertOrUpdateAndGetIdAsync(tempOjb);
              

                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(uploads, documentPath.Code);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                }
                var uploadResult = new UploadAuditQuesResFileOutput
                {
                    Code = documentPath.Code,
                    FileName = documentPath.FileName,                  
                };
                return Ok(uploadResult);
            }
            catch (Exception ex)
            {
                throw;
            }
        }




        //TTE-Attachement_upload_Delete

        [HttpGet]
        public async Task<ActionResult> DownloadPAPAttachment([FromQuery] string file)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            var uploads = Path.Combine(webRootPath, "PAPStorage");
            var documentPath = await _patientAuthenticationPlatformAttachmentRepository.FirstOrDefaultAsync(e => e.Code == file);
            if (documentPath == null)
            {
                throw new NotFoundException($"Couldn't find document with code {file}");
            }
            var filePath = Path.Combine(uploads, file);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), documentPath.FileName);
        }


        public async Task DeletePAPAttachment(string code)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            var filePath = Path.Combine(webRootPath, "PAPStorage");
            var removefile = _patientAuthenticationPlatformAttachmentRepository.FirstOrDefault(c => c.Code == code);
            if (removefile != null)
            {
                await _patientAuthenticationPlatformAttachmentRepository.DeleteAsync(removefile);
            }
            //await _auditDocSubModelPathRepository.HardDeleteAsync(removefile);
            if (Directory.Exists(filePath))
            {
                var file = Directory.GetFiles(filePath, code).FirstOrDefault();
                if (file != null)
                {
                    System.IO.File.Delete(file);
                }
            }
        }


        #region TTEENtityResponse

        public async Task<ActionResult> UploadTTEENtityAttachment([FromForm] AttachmentInput input)
        {
            try
            {
                var file = input.File;
                string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();

                var uploads = Path.Combine(webRootPath, "TTEENtity");
                var letId = input.Title.Split(",");
                var documentPath = new AuditQuestionResponseDocumentPath(file.FileName, input.Title, int.Parse(letId[1]), int.Parse(letId[1]), null);

                var obj = await _tableTopExerciseEntityAttachmentRepository.GetAll()
                    .Where(x => x.TableTopExerciseEntityId == long.Parse(letId[1]) && x.FileName.ToLower()== letId[1].ToLower()).FirstOrDefaultAsync();

                if (obj == null)
                {
                    TableTopExerciseEntityAttachment tempOjb = new TableTopExerciseEntityAttachment()
                    {
                        TableTopExerciseEntityId = int.Parse(letId[1]),
                        Code = documentPath.Code,
                        FileName = documentPath.FileName,
                        Title = documentPath.FileName,
                        Id = 0,
                    };
                    var id = await _tableTopExerciseEntityAttachmentRepository.InsertAndGetIdAsync(tempOjb);
                }
                else
                {
                    obj.Code = documentPath.Code;
                    obj.FileName = documentPath.FileName;
                    var id = await _tableTopExerciseEntityAttachmentRepository.UpdateAsync(obj);

                }

                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(uploads, documentPath.Code);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                }
                var uploadResult = new UploadAuditQuesResFileOutput
                {
                    Code = documentPath.Code,
                    FileName = documentPath.FileName,
                };
                return Ok(uploadResult);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
       
        [HttpGet]
        public async Task<ActionResult> DownloadTTEENtityAttachment([FromQuery] string file)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            var uploads = Path.Combine(webRootPath, "TTEENtity");
            var documentPath = await _tableTopExerciseEntityAttachmentRepository.FirstOrDefaultAsync(e => e.Code == file);
            if (documentPath == null)
            {
                throw new NotFoundException($"Couldn't find document with code {file}");
            }
            var filePath = Path.Combine(uploads, file);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), documentPath.FileName);
        }

        public async Task DeleteTTEENtityAttachment(string code)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            var filePath = Path.Combine(webRootPath, "TTEENtity");
            var removefile = _tableTopExerciseEntityAttachmentRepository.FirstOrDefault(c => c.Code == code);
            if (removefile != null)
            {
                await _tableTopExerciseEntityAttachmentRepository.DeleteAsync(removefile);
            }
            if (Directory.Exists(filePath))
            {
                var file = Directory.GetFiles(filePath, code).FirstOrDefault();
                if (file != null)
                {
                    System.IO.File.Delete(file);
                }
            }
        }

        #endregion


        public async Task<ActionResult> DownloadTTXAttachment([FromQuery] string file)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            var uploads = Path.Combine(webRootPath, "TTXFiles");
            var documentPath = await _tableTopExerciseSectionAttchemntRepository.FirstOrDefaultAsync(e => e.Code == file);
            if (documentPath == null)
            {
                throw new NotFoundException($"Couldn't find document with code {file}");
            }
            var filePath = Path.Combine(uploads, file);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), documentPath.FileName);
        }



        public async Task DeleteTTXfile (string code)
        {
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            var filePath = Path.Combine(webRootPath, "TTXFiles");
            var removefile = _tableTopExerciseSectionAttchemntRepository.FirstOrDefault(c => c.Code == code);
            if (removefile != null)
            {
                await _tableTopExerciseSectionAttchemntRepository.HardDeleteAsync(removefile);
            }
            //await _auditDocSubModelPathRepository.HardDeleteAsync(removefile);
            if (Directory.Exists(filePath))
            {
                var file = Directory.GetFiles(filePath, code).FirstOrDefault();
                if (file != null)
                {
                    System.IO.File.Delete(file);
                }
            }
        }

        public async Task<IActionResult> UploadTTXFileSystem([FromForm] AttachmentInput input)
        {
            var result = new List<UploadedFileOutput>();
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            if (webRootPath != null)
            {
                
                    var file = input.File;
                    var uploads = Path.Combine(webRootPath, "TTXFiles");
                    var documentPath = new TableTopExerciseSectionAttachement(file.FileName, input.File.FileName);
                 //   await _tableTopExerciseSectionRepository.InsertAsync(documentPath);

                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }
                    if (file.Length > 0)
                    {
                        var filePath = Path.Combine(uploads, documentPath.Code);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                    }

                    var uploadResult = new UploadedFileOutput
                    {
                        Code = documentPath.Code,
                        FileName = documentPath.FileName
                    };

                    result.Add(uploadResult);
              
            }

            return Ok(result);

        }

        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        public async Task<IActionResult> UploadPAPGlobalFileSystem([FromForm] AttachmentInput input)
        {
            var result = new List<UploadedFileOutput>();
            string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
            if (webRootPath != null)
            {

                var file = input.File;
                var uploads = Path.Combine(webRootPath, "PAPGlobal");
                var documentPath = new PatientAuthenticationPlatformGlobalAttachment(file.FileName, input.File.FileName);               
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(uploads, documentPath.Code);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
                var uploadResult = new UploadedFileOutput
                {
                    Code = documentPath.Code,
                    FileName = documentPath.FileName                   
                };

                result.Add(uploadResult);
            }
            return Ok(result);
        }


        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        public async Task<IActionResult> UploadPAPGlobalFileSystems ()
        {
            var files = Request.Form.Files;
            var result = new CreateorEditPatientAuthenticationPlatformGlobalAttachmentDto();
            if (files.Count() > 0)
            {               
                string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
                if (webRootPath != null)
                {
                    foreach (var obj in files)
                    {
                        var file = obj.FileName;
                        var uploads = Path.Combine(webRootPath, "PAPGlobal");
                        var documentPath = new PatientAuthenticationPlatformGlobalAttachment(obj.FileName, obj.FileName);
                        if (!Directory.Exists(uploads))
                        {
                            Directory.CreateDirectory(uploads);
                        }
                        if (file.Length > 0)
                        {
                            var filePath = Path.Combine(uploads, documentPath.Code);
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await obj.CopyToAsync(fileStream);
                            }
                        }
                        var uploadResult = new PatientAuthenticationPlatformGlobalAttachmentDto()
                        {
                            Code = documentPath.Code,
                            FileName = documentPath.FileName,
                            Id = 0,
                            Static = true,
                            Title = documentPath.FileName,
                        };
                        result.PatientAuthenticationPlatformGlobalAttachmentDto.Add(uploadResult);
                    }

                    await _PatientAuthenticationPlatformAppService.CreateOrEditPAPGlobalAttachment(result);
                }
               
            }
            return Ok(result);
        }

        public async Task<ActionResult> DownloadPAPGlobalAttachment ([FromQuery] string file)
        {
            try
            {
                string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
                var uploads = Path.Combine(webRootPath, "PAPGlobal");
                var documentPath = await _patientAuthenticationPlatformGlobalAttachmentRepository.FirstOrDefaultAsync(e => e.Code == file);
                if (documentPath == null)
                {
                    throw new NotFoundException($"Couldn't find document with code {file}");
                }
                var filePath = Path.Combine(uploads, file);
                if (!System.IO.File.Exists(filePath))
                    return NotFound();

                var memory = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                return File(memory, GetContentType(filePath), documentPath.FileName);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task DeletePAPGlobalAttachmentfile(string code)
        {
            try
            {
                string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
                var filePath = Path.Combine(webRootPath, "PAPGlobal");
                var removefile = _patientAuthenticationPlatformGlobalAttachmentRepository.FirstOrDefault(c => c.Code == code);
                if (removefile != null)
                {
                    removefile.DeleterUserId = AbpSession.UserId;
                    removefile.LastModifierUserId = AbpSession.UserId;
                    removefile.LastModificationTime = DateTime.Now;
                    await _patientAuthenticationPlatformGlobalAttachmentRepository.DeleteAsync(removefile);
                }
                if (Directory.Exists(filePath))
                {
                    var file = Directory.GetFiles(filePath, code).FirstOrDefault();
                    if (file != null)
                    {
                        System.IO.File.Delete(file);
                    }
                }
            }
            catch(Exception)
            {
                throw;  
            }
        }
        public async Task<List<PatientAuthenticationPlatformAttachmentDto>> GetAllPAPGlobalAttachment()
        {
            try
            {
                var result = new List<PatientAuthenticationPlatformAttachmentDto>();
                result = await _patientAuthenticationPlatformGlobalAttachmentRepository.GetAll().Select(res => new PatientAuthenticationPlatformAttachmentDto()
                {
                     FileName= res.FileName,
                     Code=res.Code,
                     Static=res.Static, 
                }).ToListAsync();

                return result;
            }
            catch(Exception)
            {
                throw;
            }
             
        }


    }
}