using LockthreatCompliance.Assessments.Dto;
using LockthreatCompliance.DataExporting.Excel.NPOI;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LockthreatCompliance.Assessments.Importing
{
    public class SelfAssessmentResponseListExcelDataReader : NpoiExcelImporterBase<ImportSelfAssessmentDto>, ISelfAssessmentResponseListExcelDataReader
    {
        public List<ImportSelfAssessmentDto> GetAssessmentResponseFromExcel(byte[] fileBytes, int? tenantId)
        {
            var importSelfAssessments = new List<ImportSelfAssessmentDto>();
            ISheet sheet;
            using (var stream = new MemoryStream(fileBytes))
            {
                XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                sheet = hssfwb.GetSheetAt(4); //get first sheet from workbook   
                IRow headerRow = sheet.GetRow(0); //Get Header Row
                int cellCount = headerRow.LastCellNum;
                //for (int j = 0; j < cellCount; j++)
                //{
                //    ICell cell = headerRow.GetCell(j);
                //    if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                //}

                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                {
                    IRow row = sheet.GetRow(i);

                    //if (row == null) continue;
                    //if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    if (row.GetCell(4) != null)
                    {
                        var originalID = row.GetCell(1).ToString();
                        var description = row.GetCell(2).ToString();
                        var controlCategory = row.GetCell(3).ToString();
                        var controlMandate = row.GetCell(4).ToString();
                        var response = row.GetCell(5) == null ? "" : row.GetCell(5).ToString();
                        var comment = row.GetCell(6) == null ? "": row.GetCell(6).ToString();

                        if (row.GetCell(4).ToString() != "")
                        {
                            var importSelfAssessment = new ImportSelfAssessmentDto
                            {
                                OriginalID = originalID,
                                Description = description,
                                ControlCategory = controlCategory,
                                ControlMandate = controlMandate,
                                Response = response,
                                Comment = comment

                            };
                            importSelfAssessments.Add(importSelfAssessment);
                        }
                    }
                }

                return importSelfAssessments;
            }
        }
    }
}
