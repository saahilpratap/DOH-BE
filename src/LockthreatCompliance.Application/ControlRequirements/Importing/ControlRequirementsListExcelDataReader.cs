using Abp.Domain.Repositories;
using LockthreatCompliance.ControlRequirements.Dtos;
using LockthreatCompliance.DataExporting.Excel.NPOI;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LockthreatCompliance.ControlRequirements.Importing
{
    public class ControlRequirementsListExcelDataReader : NpoiExcelImporterBase<ImportControlRequirementDto>, IControlRequirementsListExcelDataReader
    {
        private readonly IRepository<ControlRequirement> _ControlRequirement;

        public ControlRequirementsListExcelDataReader(IRepository<ControlRequirement> ControlRequirement)
        {
            _ControlRequirement = ControlRequirement;
        }
        public List<ImportControlRequirementDto> GetControlRequirementFromExcel(byte[] fileBytes, int? tenantId)
        {
            var importControlRequirements = new List<ImportControlRequirementDto>();
            var InvalidMessage = new ImportControlRequirementDto();
            InvalidMessage.InvalidCount = true;
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

                List<String> headerList = new List<string>();
                for (int i = 0; i <= 0; i++)
                {
                    string header = "";
                    for (int j = 0; j < cellCount; j++)
                    {
                        header = sheet.GetRow(0).Cells[j]?.StringCellValue;
                        headerList.Add(header);
                    }


                }
                var list = headerList;
                int checkHeaderCount = 0;
                int checkInvalidCount = 0;
                List<string> staticHeader = new List<string>();
                staticHeader.Add("OriginalId"); staticHeader.Add("Description"); staticHeader.Add("ControlStandardName"); staticHeader.Add("DomainName");
                staticHeader.Add("ControlType"); staticHeader.Add("ControlStandardId"); staticHeader.Add("AuthoritativeDocumentId"); staticHeader.Add("IndustryMandated");
                staticHeader.Add("Isscored");
                var list2 = list.Except(staticHeader).ToList();
                foreach (var item in staticHeader)
                {
                    foreach (var item2 in list)
                    {
                        if (item == item2)
                        {
                            checkHeaderCount++;
                        }
                        else
                        {
                            checkInvalidCount++;
                        }
                    }
                }
                var count = checkHeaderCount;
                if (count == 9)
                {
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        var importControlRequirement = new ImportControlRequirementDto();
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;


                        for (int j = 0; j < 9; j++)              //Read Data using Column Header
                        {
                            if (ControlRequirementConsts.OriginalId == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var temp = GetRequiredValueFromRowOrNull(row, j);
                                if (temp != null)
                                {
                                    importControlRequirement.OriginalId = temp;
                                }
                                else
                                {
                                    importControlRequirement.Exception += "OriginalId is Empty, ";
                                }
                            }
                            if (ControlRequirementConsts.Description == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                importControlRequirement.Description = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (ControlRequirementConsts.ControlStandardName == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var temp = GetRequiredValueFromRowOrNull(row, j);
                                if (temp != null)
                                {
                                    importControlRequirement.ControlStandardName = temp;
                                }
                                else
                                {
                                    importControlRequirement.Exception += "Control Standard Name is Empty, ";
                                }
                            }
                            if (ControlRequirementConsts.DomainName == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var temp = GetRequiredValueFromRowOrNull(row, j);
                                if (temp != null)
                                {
                                    importControlRequirement.DomainName = temp;
                                }
                                else
                                {
                                    importControlRequirement.Exception += "Domain Name is Empty, ";
                                }
                            }
                            if (ControlRequirementConsts.ControlType == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {

                                var temp = GetRequiredValueFromRowOrNull(row, j);
                                if (temp != null)
                                {
                                    importControlRequirement.ControlType = Convert.ToInt32(temp);
                                }
                                else
                                {
                                    importControlRequirement.ControlType = 0;
                                }


                            }
                            if (ControlRequirementConsts.ControlStandardId == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var tempcontrolStandardId = GetRequiredValueFromRowOrNull(row, j);
                                importControlRequirement.ControlStandardId = Convert.ToInt32(tempcontrolStandardId);
                            }
                            if (ControlRequirementConsts.AuthoritativeDocumentId == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var temp2 = GetRequiredValueFromRowOrNull(row, j);
                                if (temp2 != null)
                                {
                                    var tempAuthoritativeDocumentId = GetRequiredValueFromRowOrNull(row, j);
                                    importControlRequirement.AuthoritativeDocumentId = Convert.ToInt32(tempAuthoritativeDocumentId);
                                }
                                else
                                {
                                    importControlRequirement.Exception += "Authoritative DocumentId is Empty, ";
                                }
                            }
                            if (ControlRequirementConsts.IndustryMandated == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var tempindustryMandated = GetRequiredValueFromRowOrNull(row, j);
                                importControlRequirement.IndustryMandated = tempindustryMandated == "0" ? false : true;
                            }
                            if (ControlRequirementConsts.Isscored == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var tempIsscored = GetRequiredValueFromRowOrNull(row, j);
                                importControlRequirement.Iscored = tempIsscored == "0" ? false : true;
                            }

                        }
                        importControlRequirement.CanBeImported = false;
                        int rowno = Convert.ToInt32(i) + 1;
                        importControlRequirement.RowName = rowno.ToString();
                        if (importControlRequirement.AuthoritativeDocumentId != 0 && importControlRequirement.DomainName != null && importControlRequirement.ControlStandardName != null && importControlRequirement.OriginalId != null)
                        {
                            importControlRequirement.CanBeImported = true;
                        }
                        else
                        {
                            importControlRequirement.Exception += "Authoritative DocumentId is Invalid, ";
                        }
                        importControlRequirement.InvalidCount = true;
                        importControlRequirements.Add(importControlRequirement);


                    }
                }
                else
                {
                    InvalidMessage.InvalidCount = false;
                    foreach (var item in list2.Take(3))
                    {
                        InvalidMessage.InvalidName += item + ", ";
                    }
                    importControlRequirements.Add(InvalidMessage);
                }
            }

            return importControlRequirements;
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
