using LockthreatCompliance.DataExporting.Excel.NPOI;
using LockthreatCompliance.FacilityTypes.Dtos;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LockthreatCompliance.FacilityTypes.Importing
{
    public class FacilityTypesListExcelDataReader : NpoiExcelImporterBase<ImportFacilityTypes>, IFacilityTypesListExcelDataReader
    {
        public List<ImportFacilityTypes> GetFacilityTypesFromExcel(byte[] fileBytes, int? tenantId)
        {
            var importFacilityTypes = new List<ImportFacilityTypes>();
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

                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                {
                    IRow row = sheet.GetRow(i);
                    var importFacilityType = new ImportFacilityTypes();
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;


                    for (int j = 0; j < 2; j++)              //Read Data using Column Header
                    {
                        if (FacilityTypesConsts.Name == sheet.GetRow(0).Cells[j]?.StringCellValue)
                        {
                            importFacilityType.Name = GetRequiredValueFromRowOrNull(row, j);
                        }
                        if (FacilityTypesConsts.ControlType == sheet.GetRow(0).Cells[j]?.StringCellValue)
                        {
                            importFacilityType.ControlType = GetRequiredValueFromRowOrNull(row, j);
                        }
                    }

                    if (importFacilityType.Name != null && importFacilityType.ControlType != null)
                    {
                        bool checkDublicate = importFacilityTypes.Any(x => x.Name.ToString().ToLower() == importFacilityType.Name.ToString().ToLower());
                        if (!checkDublicate)
                        {
                            importFacilityTypes.Add(importFacilityType);
                        }
                    }
                }

                return importFacilityTypes;
            }
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
