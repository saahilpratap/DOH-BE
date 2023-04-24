using Abp.Domain.Repositories;
using Abp.DynamicEntityParameters;
using Abp.Localization;
using Abp.Localization.Sources;
using LockthreatCompliance.DataExporting.Excel.NPOI;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LockthreatCompliance.DynamicEntityParameters.Importing
{
    public class DynamicParameterListExcelDataReader : NpoiExcelImporterBase<ImportDynamicParameterDto>,IDynamicParameterListExcelDataReader
    {
        private readonly IRepository<DynamicParameter> _dynamicParamRepository;
        private readonly IRepository<DynamicParameterValue> _dynamicParamValueRepository;

        public DynamicParameterListExcelDataReader(IRepository<DynamicParameter> dynamicParamRepository, IRepository<DynamicParameterValue> dynamicParamValueRepository)
        {
            _dynamicParamRepository = dynamicParamRepository;
            _dynamicParamValueRepository = dynamicParamValueRepository;
        }

        public List<DynamicParameter> GetDynamicParameterFromExcel(byte[] fileBytes, int? tenantId)
        {
            var dynamicParameters = new List<DynamicParameter>();
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
                    var dynamicParameter = new DynamicParameter();
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;


                    for(int j = 0; j < 3; j++)              //Read Data using Column Header
                    {
                        if(DynamicParameterConsts.ParameterName == sheet.GetRow(0).Cells[j]?.StringCellValue)
                        {
                            dynamicParameter.ParameterName = GetRequiredValueFromRowOrNull(row, j);
                        }
                        if (DynamicParameterConsts.InputType == sheet.GetRow(0).Cells[j]?.StringCellValue)
                        {
                            dynamicParameter.InputType = GetRequiredValueFromRowOrNull(row, j);
                        }
                        if (DynamicParameterConsts.Permission == sheet.GetRow(0).Cells[j]?.StringCellValue)
                        {
                            dynamicParameter.Permission = GetRequiredValueFromRowOrNull(row, j);
                        }
                    }

                    if (dynamicParameter.ParameterName != null)
                    {
                        bool duplicate = dynamicParameters.Any(p => p.ParameterName.ToLower().ToString() == dynamicParameter.ParameterName.ToString().ToLower());
                        if (!duplicate)
                        {
                            dynamicParameters.Add(dynamicParameter);
                        }                      
                    }
                    
                }
            }

            return dynamicParameters;
        }

        public List<ImportDynamicParameterValueDto> GetDynamicParameterValueFromExcel(byte[] fileBytes, int? tenantId)
        {
            var dynamicParameterValues = new List<ImportDynamicParameterValueDto>();
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
                int srno = 1;
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                {
                    IRow row = sheet.GetRow(i);
                    var dynamicParameterValue = new ImportDynamicParameterValueDto();
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;


                    for (int j = 0; j < 3; j++)              //Read Data using Column Header
                    {
                        if (DynamicParameterValueConsts.Value == sheet.GetRow(0).Cells[j]?.StringCellValue)
                        {
                            dynamicParameterValue.EntityFullName = GetRequiredValueFromRowOrNull(row, j);
                        }
                        if (DynamicParameterValueConsts.Name == sheet.GetRow(0).Cells[j]?.StringCellValue)
                        {
                            dynamicParameterValue.DynamicParameterName = GetRequiredValueFromRowOrNull(row, j);
                        }
                        if (DynamicParameterValueConsts.DynamicParameterId == sheet.GetRow(0).Cells[j]?.StringCellValue)
                        {
                            var tempDynamicParameterId = GetRequiredValueFromRowOrNull(row, j);
                            dynamicParameterValue.DynamicParameterId = Convert.ToInt32(tempDynamicParameterId);
                        }
                    }
                    if (dynamicParameterValue.DynamicParameterName != null && dynamicParameterValue.EntityFullName != null)
                    {
                        bool duplicate = dynamicParameterValues.Any(p => p.EntityFullName.ToString().ToLower() == dynamicParameterValue.EntityFullName.ToString().ToLower());
                        if (!duplicate)
                        {
                            dynamicParameterValue.SrNo = srno;
                            dynamicParameterValues.Add(dynamicParameterValue);
                            srno++;
                        }

                    }

                }
            }
            return dynamicParameterValues;
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
