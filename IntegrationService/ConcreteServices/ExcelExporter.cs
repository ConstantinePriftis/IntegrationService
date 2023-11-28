using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using IntegrationService.ViewModels.FieldProductsViewModels;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using OfficeOpenXml;


namespace IntegrationService.ConcreteServices
{
	public class ExcelExporter
	{
		public static ExcelPackage ExportToExcel(Dictionary<string, List<Dictionary<string, List<object>>>> dataPerSheet)
		{
			if (dataPerSheet == null || dataPerSheet.Count == 0)
			{
				throw new ArgumentException("Data cannot be null or empty.");
			}
			ExcelPackage.LicenseContext = LicenseContext.Commercial;

			var dictionaryData = dataPerSheet.Values;
			int headerRowIndex = 1;
			var package = new ExcelPackage();
				foreach (var dictionaryItem in dictionaryData)
				{
					foreach (var sheetData in dictionaryItem)
					{
						foreach (var item in sheetData)
						{
							var sheetName = item.Key;
							var data = item.Value;

							var worksheet = package.Workbook.Worksheets.Add(sheetName);
							int columnIndex = 1;
							foreach (var dataItem in data)
							{
								columnIndex = 1;
								headerRowIndex = 1;
								var dataItemType = dataItem.GetType(); 
								/*Json Deserialize logic
								//var serializedDataItem = JsonConvert.SerializeObject(dataItem, Formatting.Indented);
								//var dynamicDeserializedObject = JsonConvert.DeserializeObject<dynamic>(serializedDataItem);

								//Json Deserialize logic
								// Write headers

								//foreach (JToken key in dynamicDeserializedObject)
								//{
								//	var header = key.Path;
								//	JToken headerValue = key.First;

								//	if (header.Equals("Fields"))
								//	{
								//		//if (headerValue.Type is IEnumerable)
								//		//{
								//		var children = headerValue.Children();
								//		foreach (JToken field in children)
								//		{
								//			var dynamicHeader = field.Path;
								//			foreach (var prop in field.Children())
								//			{
								//				var path = prop.Path.ToString().Split('.').Last();
								//				ExcelRangeBase collumnIndex = FindColumnByName(worksheet, path.ToString());

								//				if(collumnIndex == null)
								//				{
								//					worksheet.Cells[headerRowIndex, columnIndex].Value = path;
								//					columnIndex++;
								//				}												

								//			}											
								//		}
								//		//}
								//	}
								//	else
								//	{
								//		worksheet.Cells[headerRowIndex, columnIndex].Value = header;
								//	}
								//	columnIndex++;
								//}*/

								foreach (var header in dataItemType.GetProperties())
								{

									if (!IEnumerableDoesMatch(header.PropertyType.Name))
										worksheet.Cells[headerRowIndex, columnIndex++].Value = header.Name;
									else
									{
										var methodToInvoke = header.DeclaringType
											.GetMethods()
											.FirstOrDefault(x => x.Name == "get_Fields");
										var invokedMethod = methodToInvoke.Invoke(dataItem, null);
										if (invokedMethod is IEnumerable listOfObjects)
										{
											if (header.CanRead)
											{

												foreach (var obj in listOfObjects)
												{

													var fieldPropertyInfo = obj.GetType().GetProperty("FieldName");
													var val = fieldPropertyInfo.GetValue(obj);
													worksheet.Cells[headerRowIndex, columnIndex++].Value = val;
												}

											}
										}
									}
								}
							}
							foreach (var d in data)
							{
								var dataItemType = d.GetType();
								columnIndex = 1;
								headerRowIndex++;
								foreach (var entity in dataItemType.GetProperties())
								{

									if (!IEnumerableDoesMatch(entity.PropertyType.Name))
									{
										var headerValue = entity.GetValue(d);
										worksheet.Cells[headerRowIndex, columnIndex++].Value = headerValue;
									}
									else
									{
										var methodToInvoke = entity.DeclaringType
											.GetMethods()
											.FirstOrDefault(x => x.Name == "get_Fields");
										var invokedMethod = methodToInvoke.Invoke(d, null);
										if (invokedMethod is IEnumerable listOfObjects)
										{
											if (entity.CanRead)
											{
												foreach (var obj in listOfObjects)
												{
													var fieldPropertyInfo = obj.GetType().GetProperty("Value");
													var val = fieldPropertyInfo.GetValue(obj);
													worksheet.Cells[headerRowIndex, columnIndex++].Value = val;
												}

											}
										}
									}

								}
							}
						}
					}
				}
				return package;
		}
		private static bool IEnumerableDoesMatch(String input)
		{
			Regex regularExpression = new Regex("IEnumerable`[A-Za-z0-9]+", RegexOptions.IgnoreCase);
			return regularExpression.IsMatch(input);
		}
	}
}
