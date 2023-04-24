using iTextSharp.text;
using LockthreatCompliance.Contacts.Dtos;
using LockthreatCompliance.DataExporting.Excel.NPOI;
using MimeKit.Cryptography;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LockthreatCompliance.Contacts.Importing
{
    public class ContactListExcelDataReader : NpoiExcelImporterBase<ImportContactDto>, IContactListExcelDataReader
    {
        public List<ImportContactDto> GetContactFromExcel(byte[] fileBytes, int? tenantId)
        {
            var importContactList = new List<ImportContactDto>();
            ISheet sheet;
            using (var stream = new MemoryStream(fileBytes))
            {
                XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format
                hssfwb.MissingCellPolicy = MissingCellPolicy.CREATE_NULL_AS_BLANK;
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

                    var importContact = new ImportContactDto();
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;


                    for (int j = 0; j < 10; j++)              //Read Data using Column Header
                    {
                        if (ContactConsts.FirstName == sheet.GetRow(0).Cells[j]?.StringCellValue)
                        {
                            importContact.FirstName = GetRequiredValueFromRowOrNull(row, j);
                        }
                        if (ContactConsts.LastName == sheet.GetRow(0).Cells[j]?.StringCellValue)
                        {
                            importContact.LastName = GetRequiredValueFromRowOrNull(row, j);
                        }
                        if (ContactConsts.JobTitle == sheet.GetRow(0).Cells[j]?.StringCellValue)
                        {
                            importContact.JobTitle = GetRequiredValueFromRowOrNull(row, j);
                        }
                        if (ContactConsts.Mobile == sheet.GetRow(0).Cells[j]?.StringCellValue)
                        {
                            importContact.Mobile = GetRequiredValueFromRowOrNull(row, j);
                        }
                        if (ContactConsts.DirectPhone == sheet.GetRow(0).Cells[j]?.StringCellValue)
                        {
                          
                          importContact.DirectPhone = GetRequiredValueFromRowOrNull(row, j);

                        }
                        if (ContactConsts.CompanyName == sheet.GetRow(0).Cells[j]?.StringCellValue)
                        {
                           
                                importContact.CompanyName = GetRequiredValueFromRowOrNull(row, j);
                        }
                        if (ContactConsts.BusinessEntityId == sheet.GetRow(0).Cells[j]?.StringCellValue)
                        {
                           var temp = GetRequiredValueFromRowOrNull(row, j);
                            importContact.BusinessEntityId = Convert.ToInt32(temp);
                        }
                        if (ContactConsts.ContactTypeId == sheet.GetRow(0).Cells[j]?.StringCellValue)
                        {
                            var temp = GetRequiredValueFromRowOrNull(row, j);
                            importContact.ContactTypeId = Convert.ToInt32(temp);
                        }
                        if (ContactConsts.ContactOwnerType == sheet.GetRow(0).Cells[j]?.StringCellValue)
                        {
                            var temp = GetRequiredValueFromRowOrNull(row, j);
                            importContact.ContactOwnerType = Convert.ToInt32(temp);
                        }
                        if (ContactConsts.Email == sheet.GetRow(0).Cells[j]?.StringCellValue)
                        {
                            importContact.Email = GetRequiredValueFromRowOrNull(row, j);
                        }
                    }

                    if (importContact.Email != null && importContact.ContactTypeId != 0  && importContact.BusinessEntityId != 0 && importContact.Mobile !=null && importContact.CompanyName !=null)
                    {
                        string email = importContact.Email;
                        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                        Match match = regex.Match(email);
                        if (match.Success)
                        {
                            bool duplicate = importContactList.Any(p => p.Email.ToString().ToLower() == importContact.Email.ToString().ToLower());
                            if (!duplicate)
                            {
                                importContactList.Add(importContact);
                            }
                        }

                    }
                }

                return importContactList;
            }
        }

        private string GetRequiredValueFromRowOrNull(IRow row,int j)
        {
            ICell cell = row.GetCell(j, MissingCellPolicy.RETURN_BLANK_AS_NULL);
            if(cell != null)
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
