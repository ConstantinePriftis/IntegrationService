using AutoMapper.Internal;
using IntegrationService.IServices.Excel;
using IntegrationService.Models.Fields;
using IntegrationService.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OfficeOpenXml;
using OfficeOpenXml.Table.PivotTable;
using System.Net;
using System.Reflection;
//using System.Reflection;
//using System.Reflection.PortableExecutable;
//using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace IntegrationService.ConcreteServices
{
    public class ExcelService : IExcelService
    {
        public byte[] ProcessExcelData<T>(ExcelPackage package, Dictionary<string, IQueryable<T>> Data, string[] sheets, List<string> headers, Func<List<T>, object[]> formatterFunc = null)
        {
            using (package)
            {
                var row = 1;
                var col = 1;
                var currentSheet = 1;

                foreach (var item in Data)
                {
                    //productData = item.Value.AsEnumerable().ToList();
                    if (package.Workbook.Worksheets.Any(x => x.Name == item.Key))
                        return null;
                    var sheet = package.Workbook.Worksheets.Add(item.Key);


                    foreach (var val in item.Value)
                    {
                        var itemType = val.GetType();
                        //var fields = itemType.ReflectedType;
                        //fields.SetMemberValue(item.Key, val);
                        foreach (var propertyInfo in itemType.GetProperties())
                        {

                            if (!headers.Contains(propertyInfo.Name) && !(IEnumerableDoesMatch(propertyInfo.PropertyType.Name)))
                            {
                                headers.Add(propertyInfo.Name);
                            }
                            if (IEnumerableDoesMatch(propertyInfo.PropertyType.Name))
                            {
                                //var fieldList =  new List<Field>();
                                //var valType = val.GetType();
                                //var memberValue = itemType.GetMemberValue(propertyInfo.GetMemberValue);
                                var propertyInfoValue = (object)val.GetType().GetProperty(propertyInfo.Name).GetValue(val);
                                //var innerProperties = typeof(propertyInfoValue).GetProperties();
                                //object propertyValue = propertyInfoValue.GetValue(val, null);
                                //FieldInfo fieldInfo = itemType.GetField(propertyInfo.Name);
                                List<Field> fieldList = propertyInfoValue as List<Field> ?? new List<Field>();

                                foreach (var field in fieldList)
                                {
                                    if (!headers.Contains(field.Name))
                                    {
                                        headers.Add(field.Name.ToString());
                                    }
                                }
                            }
                        }
                        //var itemtype = val.GetType();
                        //itemType.SetMemberValue((string)item.Key, val);



                    }
                    foreach (var h in headers)
                    {
                        sheet.Cells[row, col++].Value = h;
                    }
                    foreach (var d in item.Value)
                    {
                        var propInfo = d.GetType();

                        foreach (var propertyInfo in propInfo.GetProperties())
                        {
                            row++;
                            col = 1;
                            sheet.Cells[row, col++].Value = propInfo.GetField(d.GetType().Name);
                            //sheet.Cells[row, col++].Value = propInfo.GetField(d.GetType().Name);
                            //sheet.Cells[row, col++].Value = propInfo.GetField(d.GetType().Name);
                            //sheet.Cells[row, col++].Value = propInfo.GetField(d.GetType().Name);

                            if (IEnumerableDoesMatch(d.GetType().Name))
                            {
                                PropertyInfo propInfoType = propInfo.GetProperty(propertyInfo.Name);
                                FieldInfo fieldInfo = propInfoType.GetValue(d.GetType().Name) as FieldInfo;
                                //FieldInfo fieldInfo = propInfo.GetField(d.GetType().Name);
                                List<Field> fieldList = fieldInfo.GetValue(d) as List<Field>;

                                foreach (var f in fieldList)
                                {
                                    FieldInfo field = f.GetType().GetField(f.GetType().Name);
                                    sheet.Cells[row, col++].Value = field.GetValue(f) ?? string.Empty;
                                }
                            }
                        }
                        //propInfo.SetMemberValue(d,item.Value);


                    }
                    row = 1;
                    col = 1;
                    currentSheet++;
                    headers.Clear();

                }

            }
            return package.GetAsByteArray();
        }
        private bool IEnumerableDoesMatch(String input)
        {
            Regex regularExpression = new Regex("IEnumerable`[A-Za-z0-9]+", RegexOptions.IgnoreCase);
            return regularExpression.IsMatch(input);
        }

    }
}
