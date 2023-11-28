using IntegrationService.ViewModels.FieldProductsViewModels;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

namespace IntegrationService.ViewModels
{
    public class Test
    {
        public List<FieldsPerProductEditViewModel> Fields { get; set; }


        public static void ExportToExcel<T>(List<T> data, string filePath)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Headers
                var properties = typeof(T).GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    //if (properties[i].)
                    worksheet.Cells[1, i + 1].Value = properties[i].Name;
                }

                // Data
                for (int i = 0; i < data.Count; i++)
                {
                    var item = data[i];
                    //var itemType = item.GetType();
                    //if(itemType.IsArray == true)
                    //{
                    //    var nestedProperties = 
                    //}
                    for (int j = 0; j < properties.Length; j++)
                    {
                        worksheet.Cells[i + 2, j + 1].Value = properties[j].GetValue(item);
                    }
                }

                package.Save();
            }
        }

    }
}
