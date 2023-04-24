using Abp.Domain.Repositories;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.DataExporting.Excel.NPOI;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LockthreatCompliance.AuditProjects.Importing
{
    public class AuditProjectListExcelDataReader : NpoiExcelImporterBase<ExportAuditProject>, IAuditProjectListExcelDataReader
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<AuditProject, long> _auditProjectRepository;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IRepository<CertificateImport> _certificateImportRepository;
        public AuditProjectListExcelDataReader(IRepository<User, long> userRepository, IRepository<AuditProject, long> auditProjectRepository, IRepository<BusinessEntity> businessEntityRepository, IRepository<CertificateImport> certificateImportRepository)
        {
            _userRepository = userRepository;
            _auditProjectRepository = auditProjectRepository;
            _businessEntityRepository = businessEntityRepository;
            _certificateImportRepository = certificateImportRepository;
        }
        public List<ExportAuditProject> GetAuditProjectFromExcel(byte[] fileBytes, int? tenantId)
        {
            var importAuditProjects = new List<ExportAuditProject>();
            ISheet sheet;
            using (var stream = new MemoryStream(fileBytes))
            {
                XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format
                sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                IRow headerRow = sheet.GetRow(0); //Get Header Row
                int cellCount = headerRow.LastCellNum;
                for (int j = 0; j < cellCount; j++)
                {
                    ICell cell = headerRow.GetCell(j);
                    if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                }

                int t = 0;
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    t++;
                }

                for (int i = (sheet.FirstRowNum + 1); i <= t + 1; i++) //Read Excel File
                {

                    IRow row = sheet.GetRow(i);

                    var importAuditProject = new ExportAuditProject();

                    if (row != null)
                    {
                        int rowno = Convert.ToInt32(i) + 1;
                        importAuditProject.RowName = rowno.ToString();
                        importAuditProject.TenantId = tenantId;
                        importAuditProject.FiscalYear = "" + row.GetCell(0);
                        importAuditProject.status = "" + row.GetCell(1);
                        importAuditProject.AuditId = "" + row.GetCell(2);
                        importAuditProject.PrimaryLicenseNumber = "" + row.GetCell(3);
                        importAuditProject.SecondaryLicenseNumber = "" + row.GetCell(4);
                        importAuditProject.EntityGroupName = "" + row.GetCell(5);
                        importAuditProject.PrimaryEntityName = "" + row.GetCell(6);
                        importAuditProject.SecondaryEntityName = "" + row.GetCell(7);
                        importAuditProject.FacilityName = "" + row.GetCell(8);
                        importAuditProject.FacilitySubTypeName = "" + row.GetCell(9);
                        importAuditProject.City = "" + row.GetCell(10);
                        importAuditProject.AuditTypeName = "" + row.GetCell(11);
                        importAuditProject.AuditTitle = "" + row.GetCell(12);
                        importAuditProject.StartDate = "" + row.GetCell(13);
                        importAuditProject.EndDate = "" + row.GetCell(14);
                        importAuditProject.AuditDuration = "" + row.GetCell(15);
                        importAuditProject.LeadAuditorEmail = "" + row.GetCell(16);
                        importAuditProject.StageStartDate = "" + row.GetCell(17);
                        importAuditProject.StageEndDate = "" + row.GetCell(18);
                        importAuditProject.StageAuditDuration = "" + row.GetCell(19);
                        importAuditProject.AuditeeName = "" + row.GetCell(21);
                        importAuditProject.AuditeeEmail = "" + row.GetCell(22);
                        importAuditProject.AuditeeContact = "" + row.GetCell(23);
                        importAuditProject.AuditCoordinatorName = "" + row.GetCell(24);
                        importAuditProject.AuditCoordinatorEmail = "" + row.GetCell(25);
                        importAuditProject.EntityDirectorName = "" + row.GetCell(26);
                        importAuditProject.EntityDirectorEmail = "" + row.GetCell(27);
                        importAuditProject.AuditStatus=""+ row.GetCell(29);
                        importAuditProject.CAPAStatus = "" + row.GetCell(30);
                        importAuditProject.AuditOutcomeReport = "" + row.GetCell(31);
                        importAuditProject.ActualAuditReport = "" + row.GetCell(32);
                        importAuditProject.CAPAsubmissiondate = "" + row.GetCell(33);
                        importAuditProject.DateofReleasing1stRevised = "" + row.GetCell(34);
                        importAuditProject.DateofReleasing2ndRevised = "" + row.GetCell(35);
                        importAuditProject.Comments = "" + row.GetCell(36);
                        importAuditProject.IdBeImported = false;
                        importAuditProject.LicenseBeImported = false;
                        importAuditProject.StartDateBeImported = false;
                        importAuditProject.EndDateBeImported = false;
                        importAuditProject.IsAuditDurationCheck = false;
                        importAuditProject.IsLeadEmailCheck = false;
                        importAuditProject.IsStageStartDateCheck = false;
                        importAuditProject.IsStageEndDateCheck = false;
                        importAuditProject.IsStageAuditDurationCheck = false;
                        

                        if (importAuditProject.AuditId != "")
                        {

                            var checkId = int.TryParse(importAuditProject.AuditId, out int n);
                            if (checkId == true)
                            {
                                var IsExist = _auditProjectRepository.GetAll().IgnoreQueryFilters().Where(x => x.Id == Convert.ToInt32(importAuditProject.AuditId)).FirstOrDefault();
                                if (IsExist == null)
                                {
                                    importAuditProject.AuditId = "Audit Id does not exist";
                                    importAuditProject.IdBeImported = false;
                                }
                                else
                                {
                                    importAuditProject.AuditId = importAuditProject.AuditId;
                                    importAuditProject.IdBeImported = true;
                                }
                            }
                            else
                            {
                                importAuditProject.AuditId = "Invalid audit Id";
                                importAuditProject.IdBeImported = false;
                            }
                        }
                        else
                        {
                            importAuditProject.AuditId = "Audit Id is Empty";
                            importAuditProject.IdBeImported = false;
                        }
                        if (importAuditProject.PrimaryLicenseNumber != "")
                        {
                            var IsLicenseExist = _businessEntityRepository.GetAll().IgnoreQueryFilters().Where(x => x.LicenseNumber.ToLower() == importAuditProject.PrimaryLicenseNumber.Trim().ToString().ToLower()).FirstOrDefault();
                            if (IsLicenseExist == null)
                            {
                                importAuditProject.PrimaryLicenseNumber = "License number does not exist";
                                importAuditProject.LicenseBeImported = false;
                            }
                            else
                            {
                                importAuditProject.PrimaryLicenseNumber = importAuditProject.PrimaryLicenseNumber;
                                importAuditProject.LicenseBeImported = true;
                            }
                        }
                        else
                        {
                            importAuditProject.PrimaryLicenseNumber = "License number is empty";
                            importAuditProject.LicenseBeImported = false;
                        }
                        if (importAuditProject.StartDate != "")
                        {
                            var startDateCheck = (DateTime.TryParse(importAuditProject.StartDate, out DateTime temp));
                            if (startDateCheck == true)
                            {
                                importAuditProject.StartDate = importAuditProject.StartDate;
                                importAuditProject.StartDateBeImported = true;
                            }
                            else
                            {
                                importAuditProject.StartDate = "Invalid date format";
                                importAuditProject.StartDateBeImported = false;
                            }
                        }
                        else
                        {
                            importAuditProject.StartDate = "Start date is empty";
                            importAuditProject.StartDateBeImported = false;
                        }
                        if (importAuditProject.EndDate != "")
                        {
                            var endDateCheck = (DateTime.TryParse(importAuditProject.EndDate, out DateTime temp));
                            if (endDateCheck == true)
                            {
                                importAuditProject.EndDate = importAuditProject.EndDate;
                                importAuditProject.EndDateBeImported = true;
                            }
                            else
                            {
                                importAuditProject.EndDate = "Invalid date format";
                                importAuditProject.EndDateBeImported = false;
                            }
                        }
                        else
                        {
                            importAuditProject.EndDate = "End date is empty";
                            importAuditProject.EndDateBeImported = false;
                        }

                        if (importAuditProject.AuditDuration != "")
                        {
                            var IsValid = decimal.TryParse(importAuditProject.AuditDuration, out decimal n);
                            if (IsValid == true)
                            {
                                importAuditProject.AuditDuration = importAuditProject.AuditDuration;
                                importAuditProject.IsAuditDurationCheck = true;
                            }
                            else
                            {
                                importAuditProject.AuditDuration = "Invalid data - Number is Required";
                                importAuditProject.IsAuditDurationCheck = false;
                            }
                        }
                        else
                        {
                            importAuditProject.AuditDuration = importAuditProject.AuditDuration;
                            importAuditProject.IsAuditDurationCheck = true;
                        }
                        if (importAuditProject.StageAuditDuration != "")
                        {
                            var IsValid = decimal.TryParse(importAuditProject.StageAuditDuration, out decimal n);
                            if (IsValid == true)
                            {
                                importAuditProject.StageAuditDuration = importAuditProject.StageAuditDuration;
                                importAuditProject.IsStageAuditDurationCheck = true;
                            }
                            else
                            {
                                importAuditProject.StageAuditDuration = "Invalid data - Number is Required";
                                importAuditProject.IsStageAuditDurationCheck = false;
                            }
                        }
                        else
                        {
                            importAuditProject.StageAuditDuration = importAuditProject.StageAuditDuration;
                            importAuditProject.IsStageAuditDurationCheck = true;
                        }

                        if (importAuditProject.LeadAuditorEmail != "")
                        {
                            string email = importAuditProject.LeadAuditorEmail;
                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                            if (isEmail == true)
                            {
                                var checkLeadAudit = _userRepository.GetAll().Where(x => x.EmailAddress.Trim().ToLower() == importAuditProject.LeadAuditorEmail.Trim().ToLower()).FirstOrDefault();
                                if (checkLeadAudit != null)
                                {
                                    importAuditProject.LeadAuditorEmail = importAuditProject.LeadAuditorEmail;
                                    importAuditProject.IsLeadEmailCheck = true;
                                }
                                else
                                {
                                    importAuditProject.LeadAuditorEmail = "Email Id does not exist";
                                    importAuditProject.IsLeadEmailCheck = false;
                                }
                            }
                            else
                            {
                                importAuditProject.LeadAuditorEmail = "Invalid email Id formate";
                                importAuditProject.IsLeadEmailCheck = false;
                            }
                        }
                        else
                        {
                            importAuditProject.LeadAuditorEmail = importAuditProject.LeadAuditorEmail;
                            importAuditProject.IsLeadEmailCheck = true;
                        }

                        if (importAuditProject.StageStartDate != "")
                        {
                            var stageStartDateCheck = (DateTime.TryParse(importAuditProject.StageStartDate, out DateTime temp));
                            if (stageStartDateCheck == true)
                            {
                                importAuditProject.StageStartDate = importAuditProject.StageStartDate;
                                importAuditProject.IsStageStartDateCheck = true;
                            }
                            else
                            {
                                importAuditProject.StageStartDate = "Invalid date format";
                                importAuditProject.IsStageStartDateCheck = false;
                            }
                        }
                        else
                        {
                            importAuditProject.StageStartDate = importAuditProject.StageStartDate;
                            importAuditProject.IsStageStartDateCheck = true;
                        }
                        if (importAuditProject.StageEndDate != "")
                        {
                            var stageEndDateCheck = (DateTime.TryParse(importAuditProject.StageEndDate, out DateTime temp));
                            if (stageEndDateCheck == true)
                            {
                                importAuditProject.StageEndDate = importAuditProject.StageEndDate;
                                importAuditProject.IsStageEndDateCheck = true;
                            }
                            else
                            {
                                importAuditProject.StageEndDate = "Invalid date format";
                                importAuditProject.IsStageEndDateCheck = false;
                            }
                        }
                        else
                        {
                            importAuditProject.StageEndDate = importAuditProject.StageEndDate;
                            importAuditProject.IsStageEndDateCheck = true;
                        }

                        if (importAuditProject.IdBeImported == false || importAuditProject.LicenseBeImported == false || importAuditProject.StartDateBeImported == false || importAuditProject.EndDateBeImported == false)
                        {
                            importAuditProject.Result = "Not Inserted";
                        }
                        else
                        {
                            importAuditProject.Result = "Inserted";
                        }

                        if(importAuditProject.ActualAuditReport!="")
                        {
                            var stageEndDateCheck = (DateTime.TryParse(importAuditProject.ActualAuditReport, out DateTime temp));
                            if (stageEndDateCheck == true)
                            {
                                importAuditProject.ActualAuditReport = importAuditProject.ActualAuditReport;
                                
                            }
                            else
                            {
                                importAuditProject.ActualAuditReport = "Invalid date format";
                               
                            }
                        }
                        else
                        {
                            importAuditProject.ActualAuditReport = importAuditProject.ActualAuditReport;
                           
                        }

                        if (importAuditProject.CAPAsubmissiondate != "")
                        {
                            var stageEndDateCheck = (DateTime.TryParse(importAuditProject.CAPAsubmissiondate, out DateTime temp));
                            if (stageEndDateCheck == true)
                            {
                                importAuditProject.CAPAsubmissiondate = importAuditProject.CAPAsubmissiondate;

                            }
                            else
                            {
                                importAuditProject.CAPAsubmissiondate = "Invalid date format";

                            }
                        }
                        else
                        {
                            importAuditProject.CAPAsubmissiondate = importAuditProject.CAPAsubmissiondate;

                        }

                        if (importAuditProject.DateofReleasing1stRevised != "")
                        {
                            var stageEndDateCheck = (DateTime.TryParse(importAuditProject.DateofReleasing1stRevised, out DateTime temp));
                            if (stageEndDateCheck == true)
                            {
                                importAuditProject.DateofReleasing1stRevised = importAuditProject.DateofReleasing1stRevised;

                            }
                            else
                            {
                                importAuditProject.DateofReleasing1stRevised = "Invalid date format";

                            }
                        }
                        else
                        {
                            importAuditProject.DateofReleasing1stRevised = importAuditProject.DateofReleasing1stRevised;

                        }

                        if (importAuditProject.DateofReleasing2ndRevised != "")
                        {
                            var stageEndDateCheck = (DateTime.TryParse(importAuditProject.DateofReleasing2ndRevised, out DateTime temp));
                            if (stageEndDateCheck == true)
                            {
                                importAuditProject.DateofReleasing2ndRevised = importAuditProject.DateofReleasing2ndRevised;

                            }
                            else
                            {
                                importAuditProject.DateofReleasing2ndRevised = "Invalid date format";

                            }
                        }
                        else
                        {
                            importAuditProject.DateofReleasing2ndRevised = importAuditProject.DateofReleasing2ndRevised;

                        }


                        importAuditProjects.Add(importAuditProject);

                    }

                }

            }

            return importAuditProjects;
        }

        public List<CertificateImportExcelDto> GetCertificateFromExcel(byte[] fileBytes, int? tenantId)
        {
            var importcertificateList = new List<CertificateImportExcelDto>();
            ISheet sheet;
            using (var stream = new MemoryStream(fileBytes))
            {
                XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format
                sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                IRow headerRow = sheet.GetRow(0); //Get Header Row
                int cellCount = headerRow.LastCellNum;
                for (int j = 0; j < cellCount; j++)
                {
                    ICell cell = headerRow.GetCell(j);
                    if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                }

                int t = 0;
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    t++;
                }
                for (int i = (sheet.FirstRowNum + 1); i <= t + 1; i++) //Read Excel File
                {

                    IRow row = sheet.GetRow(i);

                    var certificateImport = new CertificateImportExcelDto();

                    if (row != null)
                    {
                        int rowno = Convert.ToInt32(i) + 1;
                        certificateImport.RowName = rowno.ToString();
                        certificateImport.LicenseNumber = "" + row.GetCell(0);
                        certificateImport.EntityName = "" + row.GetCell(1);
                        certificateImport.IssueDate = "" + row.GetCell(2);
                        certificateImport.ExpireDate = "" + row.GetCell(3);
                        certificateImport.IdBeImported = false;
                        certificateImport.IsLicenseNumber = false;
                        certificateImport.IsExpireDate = false;
                        certificateImport.IsIssueDate = false;
                        bool duplicate = false;
                        if (certificateImport.LicenseNumber != "")
                        {
                            var getLicenceNumber = importcertificateList.Where(x => x.LicenseNumber != null);
                            duplicate = getLicenceNumber.Any(p => p.LicenseNumber.Trim().ToString().ToUpper() == certificateImport.LicenseNumber.Trim().ToString().ToUpper());
                            if (!duplicate)
                            {
                                var checkLicenseExist = _businessEntityRepository.GetAll().Where(x => x.LicenseNumber.Trim().ToLower() == certificateImport.LicenseNumber.Trim().ToLower()).ToList().FirstOrDefault();
                                if (checkLicenseExist != null)
                                {
                                    certificateImport.LicenseNumber = certificateImport.LicenseNumber;
                                    certificateImport.IsLicenseNumber = true;
                                }
                                else
                                {
                                    certificateImport.LicenseNumber = "License Number Does Not Exist";
                                    certificateImport.IsLicenseNumber = false;
                                }
                            }
                            else
                            {
                                certificateImport.LicenseNumber = "Duplicate License Number";
                                certificateImport.IsLicenseNumber = false;
                            }
                        }
                        else
                        {
                            certificateImport.LicenseNumber = "License Number is Empty";
                            certificateImport.IsLicenseNumber = false;
                        }
                        if (certificateImport.IssueDate != "")
                        {
                            var stageEndDateCheck = (DateTime.TryParse(certificateImport.IssueDate, out DateTime temp));
                            if (stageEndDateCheck == true && certificateImport.IsLicenseNumber == true)
                            {
                                var getOldData = _certificateImportRepository.GetAll().Where(x => x.LicenseNumber.Trim().ToLower() == certificateImport.LicenseNumber.Trim().ToLower()).OrderBy(x => x.Id).LastOrDefault();
                                var tempIssueDate = Convert.ToDateTime(certificateImport.IssueDate).ToString("yyyy-MM-dd hh:ss");
                                var issueDate = Convert.ToDateTime(tempIssueDate);
                                if (getOldData != null)
                                {
                                    if (getOldData.IssueDate < issueDate)
                                    {
                                        certificateImport.IssueDate = certificateImport.IssueDate;
                                        certificateImport.IsIssueDate = true;
                                    }
                                    else
                                    {
                                        certificateImport.IssueDate = "Date is Not Greater";
                                        certificateImport.IsIssueDate = false;
                                    }
                                }
                                else
                                {
                                    certificateImport.IssueDate = certificateImport.IssueDate;
                                    certificateImport.IsIssueDate = true;
                                }
                            }
                            else
                            {
                                certificateImport.IssueDate = "Invalid date format";
                                certificateImport.IsIssueDate = false;
                            }
                        }
                        else
                        {
                            certificateImport.IssueDate = "Issue Date Is Empty";
                            certificateImport.IsIssueDate = false;
                        }
                        if (certificateImport.ExpireDate != "")
                        {
                            var stageEndDateCheck = (DateTime.TryParse(certificateImport.ExpireDate, out DateTime temp));
                            if (stageEndDateCheck == true && certificateImport.IsLicenseNumber == true)
                            {
                                var getOldData = _certificateImportRepository.GetAll().Where(x => x.LicenseNumber.Trim().ToLower() == certificateImport.LicenseNumber.Trim().ToLower()).OrderBy(x => x.Id).LastOrDefault();
                                var tempExpireDate = Convert.ToDateTime(certificateImport.ExpireDate).ToString("yyyy-MM-dd hh:ss");
                                var expireDate = Convert.ToDateTime(tempExpireDate);
                                if (getOldData != null)
                                {
                                    if (getOldData.ExpireDate < expireDate)
                                    {
                                        certificateImport.ExpireDate = certificateImport.ExpireDate;
                                        certificateImport.IsExpireDate = true;
                                    }
                                    else
                                    {
                                        certificateImport.ExpireDate = "Date is Not Greater";
                                        certificateImport.IsExpireDate = false;
                                    }
                                }
                                else
                                {
                                    certificateImport.ExpireDate = certificateImport.ExpireDate;
                                    certificateImport.IsExpireDate = true;
                                }
                            }
                            else
                            {
                                certificateImport.ExpireDate = "Invalid date format";
                                certificateImport.IsExpireDate = false;
                            }
                        }
                        else
                        {
                            certificateImport.ExpireDate = "Issue Date Is Empty";
                            certificateImport.IsExpireDate = false;
                        }
                        if (certificateImport.IsLicenseNumber == false || certificateImport.IsIssueDate == false || certificateImport.IsExpireDate == false)
                        {
                            certificateImport.Result = "Not Inserted";
                        }
                        else
                        {
                            certificateImport.Result = "Inserted";
                        }
                        importcertificateList.Add(certificateImport);
                    }
                }
            }
            return importcertificateList;
        }
        private string GetRequiredValueFromRowOrNull(IRow row, int j)
        {
            ICell cell = row.GetCell(j, MissingCellPolicy.RETURN_BLANK_AS_NULL);
            if (cell != null)
            {
                return cell.ToString();
            }
            else
            {
                return null;
            }
        }
    }
}
