using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.ControlRequirements;
using LockthreatCompliance.DataExporting.Excel.NPOI;
using LockthreatCompliance.DynamicEntityParameters;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.FindingReports.Dtos;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.FindingReports.Import
{
    public class ExternalFindingListExcelDataReader : NpoiExcelImporterBase<ImportExternalFindingDto>, IExternalFindingListExcelDataReader
    {
        private readonly IRepository<ControlRequirement> _controlRequirementsRepository;
        private readonly IRepository<ExternalAssessment> _externalAssessmentRepository;
        private readonly IRepository<AuditProject, long> _auditProjectRepository;
        private readonly IRepository<FindingReport> _findingReportRepository;
        private readonly ICustomDynamicAppService _customDynamicAppService;

        public ExternalFindingListExcelDataReader(ICustomDynamicAppService customDynamicAppService, IRepository<ControlRequirement> controlRequirementsRepository, IRepository<ExternalAssessment> externalAssessmentRepository, IRepository<AuditProject, long> auditProjectRepository,
            IRepository<FindingReport> findingReportRepository)
        {
            _customDynamicAppService = customDynamicAppService;
            _controlRequirementsRepository = controlRequirementsRepository;
            _externalAssessmentRepository = externalAssessmentRepository;
            _auditProjectRepository = auditProjectRepository;
            _findingReportRepository = findingReportRepository;
        }

        [UnitOfWork]
        public List<ImportExternalCapaDto> GetExternalCapaFromExcel(byte[] fileBytes, int? tenantId, int? auditId)
        {
            var importExternalCapaList = new List<ImportExternalCapaDto>();
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
                for (int i = (sheet.FirstRowNum + 1); i < t + 1; i++) //Read Excel File
                {
                    IRow row = sheet.GetRow(i);
                    if (row != null)
                    {
                        var getFinding = new FindingReport();
                        var cRId = "" + row.GetCell(0);
                        var correctiveAction = "" + row.GetCell(1);
                        var rootCause = "" + row.GetCell(2);
                        //   var isAccep =""+ row.GetCell(10);
                        var status = "" + row.GetCell(3);
                        var FindingStaus = "" + row.GetCell(4);
                        var actionResponseDate = "" + row.GetCell(5);
                        //var closedDate = ""+row.GetCell(11);
                        //var isStatus = row.GetCell(12);
                        var importExternalCapa = new ImportExternalCapaDto();
                        var checkCrid = (cRId.ToString() == null || cRId.ToString() == "") ? null : _controlRequirementsRepository.GetAll().Where(x => x.OriginalId.Trim().ToLower() == cRId.Trim().ToLower()).FirstOrDefault();
                        var getValidExternalAssessment = _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == auditId && x.HasQuestionaireGenerated == true).FirstOrDefault();
                        if (checkCrid != null)
                        {
                            if (getValidExternalAssessment != null)
                            {
                                getFinding = _findingReportRepository.GetAll().Where(x => x.ControlRequirementId == checkCrid.Id && x.AssessmentId == getValidExternalAssessment.Id && x.BusinessEntityId == getValidExternalAssessment.BusinessEntityId).FirstOrDefault();
                            }
                            else
                            {
                                getFinding = null;
                            }
                        }
                        else
                        {
                            getFinding = null;
                        }
                        if (getFinding != null)
                        {
                            if (cRId.ToString() != null)
                            {
                                if (checkCrid != null)
                                {
                                    if (getValidExternalAssessment != null)
                                    {

                                        importExternalCapa.ControlRequirementId = cRId;
                                        importExternalCapa.IsControlReqIdValid = true;
                                    }
                                    else
                                    {
                                        importExternalCapa.ControlRequirementId = "Generate Questionnary";
                                        importExternalCapa.IsControlReqIdValid = false;
                                    }
                                }
                                else
                                {
                                    importExternalCapa.ControlRequirementId = "Invalid Control Ref.";
                                    importExternalCapa.IsControlReqIdValid = false;
                                }
                            }
                            else
                            {
                                importExternalCapa.ControlRequirementId = "Control Ref. Is Empty";
                                importExternalCapa.IsControlReqIdValid = false;

                            }
                            importExternalCapa.ExpectedClosedDate = actionResponseDate;
                            if (importExternalCapa.ExpectedClosedDate != "" && importExternalCapa.ExpectedClosedDate!=null)
                            {
                                var stageEndDateCheck = (DateTime.TryParse(importExternalCapa.ExpectedClosedDate, out DateTime temp));
                                if (stageEndDateCheck == true)
                                {
                                    var startDate = Convert.ToDateTime(importExternalCapa.ExpectedClosedDate).ToString("yyyy-MM-dd");
                                    importExternalCapa.ExpectedClosedDate = Convert.ToDateTime(startDate).ToString(); ;
                                    importExternalCapa.IsExpectedClosedDate = true;
                                }
                                else
                                {
                                    importExternalCapa.ExpectedClosedDate = "Invalid date format";
                                    importExternalCapa.IsExpectedClosedDate = false;
                                }
                            
                            }
                            else
                            {
                                importExternalCapa.ExpectedClosedDate = "Invalid date format";
                                importExternalCapa.IsExpectedClosedDate = false;
                            }
                            importExternalCapa.RootCause = rootCause == "" ? (getFinding.Details.Split("`")[1]).ToString() : rootCause;
                            importExternalCapa.CorrectiveAction = correctiveAction == "" ? (getFinding.Details.Split("`")[0]).ToString() : correctiveAction;
                            // importExternalCapa.IsAccepted = isAccep;
                            //  importExternalCapa.Status = isStatus;

                            if (FindingStaus.ToString() != "" && FindingStaus.ToString() != null)
                            {
                                var checkFindingStaus = FindingStaus.Trim().ToLower();
                                switch (checkFindingStaus)
                                {
                                    case "capa open":
                                        {
                                            importExternalCapa.FindingStatus = Convert.ToInt32(FindingReportStatus.CapaOpen).ToString();
                                            importExternalCapa.IsFindingStatusValid = true;
                                            break;
                                        }

                                    case "capa closed":
                                        {
                                            importExternalCapa.FindingStatus = Convert.ToInt32(FindingReportStatus.CapaClosed).ToString();
                                            importExternalCapa.IsFindingStatusValid = true;
                                            break;
                                        }

                                    default:
                                        {
                                            importExternalCapa.FindingStatus = "Invalid Finding Status.";
                                            importExternalCapa.IsFindingStatusValid = false;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                importExternalCapa.FindingStatus = "Invalid Finding Status.";
                                importExternalCapa.IsFindingStatusValid = false;
                            }

                            if (importExternalCapa.CorrectiveAction.Trim().ToLower() != "null`null".Trim().ToLower() && importExternalCapa.CorrectiveAction.Trim().ToLower() != "`".Trim().ToLower() && importExternalCapa.CorrectiveAction.Trim().ToLower() != "null`" && importExternalCapa.CorrectiveAction.Trim().ToLower() != "`null")
                            {
                                importExternalCapa.IsCorrectiveActionvalid = true;
                            }
                            else
                            {
                                importExternalCapa.CorrectiveAction = "Invalid CorrectiveAction";
                                importExternalCapa.IsCorrectiveActionvalid = false;
                            }
                            if (status.ToString() != "" && status.ToString() != null)
                            {
                                var checkStaus = status.Trim().ToLower();
                                switch (checkStaus)
                                {
                                    case "new":
                                        {
                                            importExternalCapa.Status = Convert.ToInt32(FindingReportStatus.New).ToString();
                                            importExternalCapa.IsCapaStatusValid = true;
                                            break;
                                        }
                                    case "capa submitted":
                                        {
                                            importExternalCapa.Status = Convert.ToInt32(FindingReportStatus.CapaSubmitted).ToString();
                                            importExternalCapa.IsCapaStatusValid = true;
                                            break;
                                        }
                                    case "capa accepted":
                                        {
                                            importExternalCapa.Status = Convert.ToInt32(FindingReportStatus.CapaAccepted).ToString();
                                            importExternalCapa.IsCapaStatusValid = true;

                                            break;
                                        }
                                    case "capa open":
                                        {
                                            importExternalCapa.Status = Convert.ToInt32(FindingReportStatus.CapaOpen).ToString();
                                            importExternalCapa.IsCapaStatusValid = true;

                                            break;
                                        }


                                    case "capa approved":
                                        {
                                            importExternalCapa.Status = Convert.ToInt32(FindingReportStatus.CapaApproved).ToString();
                                            importExternalCapa.IsCapaStatusValid = true;
                                            break;
                                        }
                                    case "capa closed":
                                        {
                                            importExternalCapa.Status = Convert.ToInt32(FindingReportStatus.CapaClosed).ToString();
                                            importExternalCapa.IsCapaStatusValid = true;
                                            break;
                                        }
                                    default:
                                        {
                                            importExternalCapa.Status = "Invalid Status.";
                                            importExternalCapa.IsCapaStatusValid = false;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                importExternalCapa.Status = "Invalid Status.";
                                importExternalCapa.IsCapaStatusValid = false;
                            }

                        }
                        else
                        {
                            importExternalCapa.ControlRequirementId = "Raise Finding First";
                            importExternalCapa.IsControlReqIdValid = false;
                        }
                        if (importExternalCapa.IsControlReqIdValid == false || importExternalCapa.IsCapaStatusValid==false || importExternalCapa.IsFindingStatusValid==false || importExternalCapa.IsExpectedClosedDate==false || importExternalCapa.IsCorrectiveActionvalid==false)
                        {
                            importExternalCapa.Message = "Not Inserted or Updated";
                        }
                        else
                        {
                            importExternalCapa.Message = "Inserted or Updated";
                        }
                        int rowno = Convert.ToInt32(i) + 1;
                        importExternalCapa.RowNo = rowno.ToString();
                        importExternalCapaList.Add(importExternalCapa);
                      
                    }

                }
            }
            return importExternalCapaList;
        }

        public List<ImportExternalFindingDto> GetExternalFindingFromExcel(byte[] fileBytes, int? tenantId)
        {
            var importExternalFindingList = new List<ImportExternalFindingDto>();
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
                for (int i = (sheet.FirstRowNum + 1); i < t + 1; i++) //Read Excel File
                {
                    IRow row = sheet.GetRow(i);

                    if (row != null)
                    {

                        var stage = "" + row.GetCell(0);
                        var cRId = "" + row.GetCell(1);
                        var title = "" + row.GetCell(2);
                        var response = "" + row.GetCell(3);
                        var description = "" + row.GetCell(4);
                        var reference = "" + row.GetCell(5);
                        //   var FindingStaus = row.GetCell(8).ToString();
                        // var status = row.GetCell(9).ToString();
                        var DateFound = "" + row.GetCell(6);


                        var importExternalFinding = new ImportExternalFindingDto();
                        if (stage.ToString() != "" && stage.ToString() != null)
                        {
                            var checkStage = stage.Trim().ToLower().ToString();
                            switch (checkStage)
                            {
                                case "stage 1":
                                    {
                                        importExternalFinding.Stage = Convert.ToInt32(FindingReportCategory.Stage1).ToString();
                                        importExternalFinding.IsStageValid = true;
                                        break;
                                    }
                                case "stage 2":
                                    {
                                        importExternalFinding.Stage = Convert.ToInt32(FindingReportCategory.Stage2).ToString();
                                        importExternalFinding.IsStageValid = true;
                                        break;
                                    }
                                default:
                                    {
                                        importExternalFinding.Stage = "Stage name not valid";
                                        importExternalFinding.IsStageValid = false;
                                        break;
                                    }
                            }

                        }

                        else
                        {
                            importExternalFinding.Stage = "Stage Name is Empty";
                            importExternalFinding.IsStageValid = false;
                        }

                        if (response.ToString() != "" && response.ToString() != null)
                        {
                            var checkresponse = response.Trim().ToLower().ToString();
                            switch (checkresponse)
                            {
                                case "not selected":
                                    {
                                        importExternalFinding.Response = "Response cannot be Not Selected for the finding.";
                                        importExternalFinding.IsResponseValid = false;
                                        break;
                                    }
                                case "not applicable":
                                    {
                                        importExternalFinding.Response = "Response cannot be Not Applicable for the finding.";
                                        importExternalFinding.IsResponseValid = false;
                                        break;
                                    }
                                case "non compliant":
                                    {
                                        importExternalFinding.Response = Convert.ToInt32(ReviewDataResponseType.NonCompliant).ToString();
                                        importExternalFinding.IsResponseValid = true;
                                        break;
                                    }
                                case "partially compliant":
                                    {
                                        importExternalFinding.Response = Convert.ToInt32(ReviewDataResponseType.PartiallyCompliant).ToString();
                                        importExternalFinding.IsResponseValid = true;
                                        break;
                                    }
                                case "fully compliant":
                                    {
                                        importExternalFinding.Response = "Response cannot be Fully Compliant for the finding.";
                                        importExternalFinding.IsResponseValid = false;
                                        break;
                                    }
                                default:
                                    {
                                        importExternalFinding.Response = "Invalid Response Type";
                                        importExternalFinding.IsResponseValid = false;
                                        break;
                                    }
                            }
                        }

                        else
                        {

                            importExternalFinding.Response = "Invalid Response Type";
                            importExternalFinding.IsResponseValid = false;
                        }

                        if (DateFound.ToString() != "" && DateFound.ToString() != null)
                        {
                            var checkDateFound = (DateTime.TryParse(DateFound, out DateTime temp));
                            if (checkDateFound == true)
                            {
                                var startDate = Convert.ToDateTime(DateFound).ToString("yyyy-MM-dd");
                                importExternalFinding.DateFound = Convert.ToDateTime(startDate).ToString();

                                importExternalFinding.IsDateFoundvalid = true;

                            }
                            else
                            {
                                importExternalFinding.DateFound = "Invalid Date";
                                importExternalFinding.IsDateFoundvalid = false;
                            }

                        }

                        else
                        {
                            importExternalFinding.IsDateFoundvalid = true;
                            importExternalFinding.DateFound = "";
                        }

                        if (cRId.ToString() != "" && cRId.ToString() != null)
                        {
                            var checkCrid = _controlRequirementsRepository.GetAll().Where(x => x.OriginalId.Trim().ToLower() == cRId.Trim().ToLower()).FirstOrDefault();
                            if (checkCrid != null)
                            {
                                importExternalFinding.ControlRequirementId = cRId;
                                importExternalFinding.IsControlReqIdValid = true;
                            }
                            else
                            {
                                importExternalFinding.ControlRequirementId = "Invalid Control Ref.";
                                importExternalFinding.IsControlReqIdValid = false;
                            }
                        }

                        else
                        {
                            importExternalFinding.ControlRequirementId = "Control Ref. Is Empty";
                            importExternalFinding.IsControlReqIdValid = false;

                        }

                        if (title.ToString() != "" || title.ToString() != null)
                        {
                            importExternalFinding.Title = title;
                            importExternalFinding.IsTitleValid = true;
                        }

                        else
                        {
                            importExternalFinding.Title = "Title is Empty";
                            importExternalFinding.IsTitleValid = false;
                        }
                        
                        if(description.ToString()!="" && description.ToString()!=null)
                        {
                            importExternalFinding.Description = description;
                            importExternalFinding.IsDesciptionValid = true;

                        }
                        else
                        {
                            importExternalFinding.Description = "Description is Empty";
                            importExternalFinding.IsDesciptionValid = false;
                        }

                        importExternalFinding.Reference = reference;

                        if (importExternalFinding.IsControlReqIdValid == false || importExternalFinding.IsStageValid == false || importExternalFinding.IsTitleValid == false || importExternalFinding.IsResponseValid == false || importExternalFinding.IsDateFoundvalid == false || importExternalFinding.IsDesciptionValid==false)
                        {
                            importExternalFinding.Message = "Not Inserted or Updated";
                        }
                        else
                        {
                            importExternalFinding.Message = "Inserted or Updated";
                        }
                        int rowno = Convert.ToInt32(i) + 1;
                        importExternalFinding.RowNo = rowno.ToString();
                        importExternalFindingList.Add(importExternalFinding);
                    }
                   
                }

                return importExternalFindingList;
            }
        }
            
        
    }
}
