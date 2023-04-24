using LockthreatCompliance.Assessments.Dto;
using LockthreatCompliance.DataExporting.Excel.NPOI;
using System.Collections.Generic;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System;
using LockthreatCompliance.BusinessEntities;
using Abp.Domain.Repositories;
using LockthreatCompliance.ControlRequirements;
using System.Linq.Dynamic.Core;
using System.Linq;

namespace LockthreatCompliance.ExternalAssessments.Importing
{
    public class ExternalAssessmentResponseListExcelDataReader : NpoiExcelImporterBase<ImportSelfAssessmentDto>, IExternalAssessmentResponseListExcelDataReader
    {
        private readonly IRepository<ControlRequirement> _controlRequirementsRepository;

        public ExternalAssessmentResponseListExcelDataReader(IRepository<ControlRequirement> controlRequirementsRepository)
        {
            _controlRequirementsRepository= controlRequirementsRepository;  
        }
        public List<ImportSelfAssessmentDto> GetAssessmentResponseFromExcel(byte[] fileBytes, int? tenantId)
        {
            var importSelfAssessments = new List<ImportSelfAssessmentDto>();
            ISheet sheet;
            using (var stream = new MemoryStream(fileBytes))
            {
                XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                IRow headerRow = sheet.GetRow(0); //Get Header Row
                int cellCount = headerRow.LastCellNum;
              

                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                {
                    IRow row = sheet.GetRow(i);
                   
                    if (i<=201) 
                    {

                        if (row != null)
                        {
                            var DomainName=""+ "" + row.GetCell(0);
                            var originalID =""+ row.GetCell(1);
                            var description =""+ row.GetCell(2);
                            var controlCategory =""+ row.GetCell(3);
                            var updatedResponse =""+ row.GetCell(5);
                            var response =""+ row.GetCell(4);
                            var comment =""+ row.GetCell(6);

                            var importSelfAssessment = new ImportSelfAssessmentDto();

                            if (originalID.ToString() != "" && originalID.ToString() != null)
                            {
                                var checkCrid =  _controlRequirementsRepository.GetAll().Where(x => x.OriginalId.Trim().ToLower() == originalID.Trim().ToLower()).FirstOrDefault();
                                if (checkCrid != null)
                                {
                                    importSelfAssessment.OriginalID = checkCrid.OriginalId;
                                    importSelfAssessment.IsvalidOriginalId = true;
                                }
                                else
                                {
                                    importSelfAssessment.OriginalID = "Control referance Name is invalid";
                                    importSelfAssessment.IsvalidOriginalId = false;
                                }
                            }

                            else
                            {
                                importSelfAssessment.OriginalID = "Control referance Name is Empty";
                                importSelfAssessment.IsvalidOriginalId = false;
                            }

                            if (response.ToString() != "" && response.ToString() != null)
                            {
                                var checkresponse = response.Trim().ToLower().ToString();

                                switch (checkresponse)
                                {
                                    case "notselected":
                                        {
                                            importSelfAssessment.Response = Convert.ToInt32(ReviewDataResponseType.NotSelected).ToString(); ;
                                            importSelfAssessment.IsvalidResponse = true;
                                            break;
                                        }
                                    case "notapplicable":
                                        {
                                            importSelfAssessment.Response = Convert.ToInt32(ReviewDataResponseType.NotApplicable).ToString(); 
                                            importSelfAssessment.IsvalidResponse = true;
                                            break;
                                        }
                                    case "noncompliant":
                                        {
                                            importSelfAssessment.Response = Convert.ToInt32(ReviewDataResponseType.NonCompliant).ToString();
                                            importSelfAssessment.IsvalidResponse = true;
                                            break;
                                        }
                                    case "partiallycompliant":
                                        {
                                            importSelfAssessment.Response = Convert.ToInt32(ReviewDataResponseType.PartiallyCompliant).ToString();
                                            importSelfAssessment.IsvalidResponse = true;
                                            break;
                                        }
                                    case "fullycompliant":
                                        {
                                            importSelfAssessment.Response = Convert.ToInt32(ReviewDataResponseType.FullyCompliant).ToString();
                                            importSelfAssessment.IsvalidResponse = true;
                                            break;
                                        }
                                    default:
                                        {
                                            importSelfAssessment.Response = "Invalid Response Type.";
                                            importSelfAssessment.IsvalidResponse = false;                                          
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                importSelfAssessment.Response = "Invalid Response Type.";
                                importSelfAssessment.IsvalidResponse = false;
                            }

                            if (updatedResponse.ToString() != "" && updatedResponse.ToString() != null)
                            {
                                var checkresponse = updatedResponse.Trim().ToLower().ToString();

                                switch (checkresponse)
                                {
                                    case "notselected":
                                        {
                                            importSelfAssessment.UpdatedResponse= Convert.ToInt32(ReviewDataResponseType.NotSelected).ToString(); 
                                            importSelfAssessment.IsValidUpdateResponse = true;
                                            break;
                                        }
                                    case "notapplicable":
                                        {

                                            importSelfAssessment.UpdatedResponse ="Invalid update response type";
                                            importSelfAssessment.IsValidUpdateResponse = false;
                                           
                                            break;
                                        }
                                    case "noncompliant":
                                        {

                                            importSelfAssessment.UpdatedResponse = "Invalid update response type";
                                            importSelfAssessment.IsValidUpdateResponse = false;
                                            break;
                                        }
                                    case "partiallycompliant":
                                        {
                                            importSelfAssessment.UpdatedResponse = Convert.ToInt32(ReviewDataResponseType.PartiallyCompliant).ToString();
                                            importSelfAssessment.IsValidUpdateResponse = true;
                                            break;
                                        }
                                    case "fullycompliant":
                                        {
                                            importSelfAssessment.UpdatedResponse = Convert.ToInt32(ReviewDataResponseType.FullyCompliant).ToString();
                                            importSelfAssessment.IsValidUpdateResponse = true;
                                            break;
                                        }
                                    default:
                                        {
                                            importSelfAssessment.UpdatedResponse = "Invalid update response type.";
                                            importSelfAssessment.IsValidUpdateResponse = false;
                                            break;
                                        }
                                }

                            }

                            else
                            {
                                importSelfAssessment.UpdatedResponse = "Invalid update response type.";
                                importSelfAssessment.IsValidUpdateResponse = false;
                            }

                            if(description!="" && description!=null)
                            {
                                importSelfAssessment.Description = description;
                            }
                            else
                            {
                                importSelfAssessment.Description = "";
                            }
                            if(controlCategory!=null && controlCategory!="")
                            {
                                importSelfAssessment.ControlCategory = controlCategory;
                            }
                            else
                            {
                                importSelfAssessment.ControlCategory = "";
                            }
                            if(DomainName!=null && DomainName!="")
                            {
                                importSelfAssessment.DomainName = DomainName;
                            }
                            else
                            {
                                importSelfAssessment.DomainName = "";
                            }

                            if(comment.ToString()!="" && comment.ToString()!=null)
                            {
                                importSelfAssessment.Comment = comment.ToString();                             
                            }
                            else
                            {
                                importSelfAssessment.Comment = "";
                            }

                            if (importSelfAssessment.IsvalidOriginalId == false || importSelfAssessment.IsvalidResponse == false || importSelfAssessment.IsValidUpdateResponse == false)
                            {
                                importSelfAssessment.Message = "Not Inserted or Updated";
                            }
                            else
                            {
                                importSelfAssessment.Message = "Inserted or Updated";
                            }
                            int rowno = Convert.ToInt32(i) + 1;
                            importSelfAssessment.RowNo = rowno.ToString();
                            importSelfAssessments.Add(importSelfAssessment);
                        }
                    }
                }

                return importSelfAssessments;
            }
        }
    }
}
