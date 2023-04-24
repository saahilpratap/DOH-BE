using LockthreatCompliance.DataExporting.Excel.NPOI;
using LockthreatCompliance.FacilitySubtypes.Dto;
using LockthreatCompliance.FacilitySubTypes;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LockthreatCompliance.FacilitySubtypes.Importing
{
    public class FacilitySubTypesListExcelDataReader : NpoiExcelImporterBase<ImportFacilitySubType> ,IFacilitySubTypesListExcelDataReader
    {
        public List<ImportFacilitySubType> GetFacilitySubTypesFromExcel(byte[] fileBytes, int? tenantId)
        {
            var importFacilitySubTypes = new List<ImportFacilitySubType>();
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
                    var importFacilitySubType = new ImportFacilitySubType();
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;


                    for (int j = 0; j < 3; j++)              //Read Data using Column Header
                    {
                        if (FacilitySubTypesConsts.Name == sheet.GetRow(0).Cells[j]?.StringCellValue)
                        {
                            importFacilitySubType.FacilitySubTypeName = GetRequiredValueFromRowOrNull(row, j);
                        }
                        if (FacilitySubTypesConsts.ControlType == sheet.GetRow(0).Cells[j]?.StringCellValue)
                        {
                            importFacilitySubType.ControlType = GetRequiredValueFromRowOrNull(row, j);
                        }
                        if (FacilitySubTypesConsts.FacilityTypeName == sheet.GetRow(0).Cells[j]?.StringCellValue)
                        {
                            importFacilitySubType.FacilityTypeName = GetRequiredValueFromRowOrNull(row, j);
                        }
                    }

                    if (importFacilitySubType.FacilityTypeName != null  && importFacilitySubType.FacilitySubTypeName != null)
                    {
                        bool checkDublicate = importFacilitySubTypes.Any(x => x.FacilitySubTypeName.ToString().ToLower() == importFacilitySubType.FacilitySubTypeName.ToString().ToLower() && x.FacilityTypeName.ToString().ToLower()==importFacilitySubType.FacilityTypeName.ToString().ToLower());
                        if (!checkDublicate)
                        {
                            importFacilitySubTypes.Add(importFacilitySubType);
                        }
                    }
                }

                return importFacilitySubTypes;
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
