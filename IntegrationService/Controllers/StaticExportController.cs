using IntegrationService.ConcreteServices;
using IntegrationService.Data.DbContexts;
using IntegrationService.IServices.Excel;
using IntegrationService.IServices.ICommand;
using IntegrationService.IServices.IQueries;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Errors;
using IntegrationService.Models.User;
using IntegrationService.ViewModels.Exports;
using IntegrationService.ViewModels.FieldProductStores;
using IntegrationService.ViewModels.FieldProductsViewModels;
using IntegrationService.ViewModels.Nutrition;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Text.RegularExpressions;

namespace IntegrationService.Controllers
{
	public class StaticExportController : Controller
	{
		private readonly IFieldProductQuery _fieldProductQuery;
		private readonly ApplicationDbContext _ctx;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IExportRequestRepository _exportRepository;
		private readonly IExportRequestCommand _exportCommand;
		private readonly IErrorCommand _errorCommand;
		private readonly IFieldProductStoreQuery _fieldProductStoreQuery;
		private readonly IExcelService _excelService;
		private readonly INutritionQuery _nutritionQuery;

		public StaticExportController(
			IFieldProductQuery fieldProductQuery,
			ApplicationDbContext ctx,
			UserManager<ApplicationUser> userManager,
			IExportRequestRepository exportRequestRepository,
			IExportRequestCommand exportRequestCommand,
			IErrorCommand errorCommand,
			IFieldProductStoreQuery fieldProductStoreQuery,
			IExcelService excelService,
			INutritionQuery nutritionQuery)
		{
			_fieldProductQuery = fieldProductQuery;
			_ctx = ctx;
			_userManager = userManager;
			_exportRepository = exportRequestRepository;
			_exportCommand = exportRequestCommand;
			_errorCommand = errorCommand;
			_fieldProductStoreQuery = fieldProductStoreQuery;
			_excelService = excelService;
			_nutritionQuery = nutritionQuery;

		}
		[HttpGet(Name = "Index")]
		public IActionResult Index()
		{
			var exports = _exportRepository.GetAll();
			var users = _userManager.Users.AsQueryable();

			var list = (from export in exports
						join uc in users on export.CreatedBy.Id equals uc.Id
						join um in users on export.ModifiedBy.Id equals um.Id
						where export.Enabled == true
						select new ExportRequestViewModel
						{
							CreatedBy = uc.Name + uc.LastName,
							CreatedOn = export.CreatedOn,
							ModifiedBy = um.Name + um.LastName,
							ModifiedOn = export.ModifiedOn,

						}).AsQueryable();
			return View(list);
		}

		[HttpPost]
		public async Task<IActionResult> Index(DateTime filter)
		{
			try
			{
				filter = (filter.Date == default
				? (_exportRepository.GetAll().Select(x => x.ModifiedOn).DefaultIfEmpty().Max())
				: filter.Date);
				byte[] bytes = new byte[0];

				var user = await _userManager.GetUserAsync(User);
				var fieldPerProductQuery = _fieldProductStoreQuery.GetIQueryableList(filter).ToList();
				var selectedValues = fieldPerProductQuery;
				var productData = _fieldProductQuery.GetIQueryableList(filter).ToList();
				var nutritionsBySku = _nutritionQuery.GetQueryableExport(filter).ToList();


				//Instantiating data dictionaries
				Dictionary<string, List<FieldProductViewModel>> Data = new Dictionary<string, List<FieldProductViewModel>>();
				Dictionary<string, List<FieldProductStoreViewModel>> CollectionData = new Dictionary<string, List<FieldProductStoreViewModel>>();
				Dictionary<string, List<NutritionViewModel>> UpdatedNutritions = new Dictionary<string, List<NutritionViewModel>>();

				//Adding data from queries into dictionaries
				Data.Add("Μητρώο ειδών", productData);
				CollectionData.Add("Είδη ανα κατάστημα", selectedValues);
				UpdatedNutritions.Add("Διατροφικα Χαρακτηρηστικά", nutritionsBySku);


				List<List<FieldPerProductViewModel>> ProductFieldData = new List<List<FieldPerProductViewModel>>();
				List<List<FieldProductStoreViewModel>> CollectionFieldData = new List<List<FieldProductStoreViewModel>>();

				//Passing data from queries into dictionaries
				var dictionaries = new List<Dictionary<string, List<object>>>();
				dictionaries.Add(Data.ToDictionary(k => k.Key, v => v.Value.ToList<object>()));
				dictionaries.Add(CollectionData.ToDictionary(k => k.Key, v => v.Value.ToList<object>()));
				dictionaries.Add(UpdatedNutritions.ToDictionary(k => k.Key, v => v.Value.ToList<object>()));

				//Addind Data Dictionaries to single grouped one
				var dictionaryForExport = new Dictionary<string, List<Dictionary<string, List<object>>>>();
				dictionaryForExport.Add("export", dictionaries);
				var package = ExcelExporter.ExportToExcel(dictionaryForExport);

				package = ModifyFirstCollumn(package, "Μητρώο ειδών", "IsActive");

				//Deleting non needed Collumns
				DeleteCollumns(package, "Είδη ανα κατάστημα", new string[] { "ProductStoreId", "Title", "WarehouseName", "ModifiedOn", "IsPublished", "CreatedOn", "MinimumQuantity", "MaximumQuantity", "DisplayDiscountAsPercentage" });
				DeleteCollumns(package, "Μητρώο ειδών", new string[] { "Id", "ProductId","ModifiedOn" });
				DeleteCollumns(package, "Διατροφικα Χαρακτηρηστικά", new string[] { "Id", "ProductId", "ListItems", "CreatedOn", "ModifiedOn" });

				bytes = package.GetAsByteArray();
				package.Dispose();
				const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

				var fileContentResult = new FileContentResult(bytes, contentType)
				{
					FileDownloadName = "export.xlsx"
				};
				_exportCommand.Add(user);
				return fileContentResult;
			}
			catch (Exception ex)
			{
				var result = await _errorCommand.Insert(Error.Create(ex.Message), null, null);
				throw ex;
			}
		}

		[HttpPost]
		[Produces("multipart/form-data")]
		public async Task<IActionResult> GenerateExport()
		{
			return View();
		}
		private bool IEnumerableDoesMatch(String input)
		{
			Regex regularExpression = new Regex("IEnumerable`[A-Za-z0-9]+", RegexOptions.IgnoreCase);
			return regularExpression.IsMatch(input);
		}
		private string[] GetProductStaticHeaders()
		{
			return new[] { "Sku", "Step", "Status", "Description" };
		}
		private string[] GeCollectionStaticHeaders()
		{
			return new[] { "Sku", "WarehouseID" };
		}
		private ExcelPackage ModifyFirstCollumn(ExcelPackage package, string worksheetName, string CollumnName)
		{
			//string columnName = "IsActive";
			var worksheet = package.Workbook.Worksheets.FirstOrDefault(x => x.Name == worksheetName);
			// Find the column index of the desired column
			int columnIndex = 0;
			for (int col = 1; col <= worksheet.Dimension?.Columns; col++)
			{
				string currentColumnName = worksheet.Cells[1, col].Text;
				if (currentColumnName.Equals(CollumnName, StringComparison.OrdinalIgnoreCase))
				{
					columnIndex = col;
					break;
				}
			}
			if (columnIndex > 0)
			{
				// Create a new worksheet
				int lastUsedColumn = worksheet.Dimension.End.Column;
				// Copy the column to the front
				for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
				{
					// Get the value from the original column
					var cellValue = worksheet.Cells[row, columnIndex].Value;
					// Shift the values to the right
					for (int column = lastUsedColumn; column >= 1; column--)
					{
						worksheet.Cells[row, column + 1].Value = worksheet.Cells[row, column].Value;
					}
					// Copy the value to the front of the row
					worksheet.Cells[row, 1].Value = cellValue;
				}
				var columnRange = worksheet.Cells[1, columnIndex, worksheet.Dimension.End.Row, columnIndex];
				worksheet.DeleteColumn(columnIndex + 1);
			}
			// Save the modified Excel file
			package.Save();
			return package;
		}

		private void DeleteCollumns(ExcelPackage package, string workSheetName, string[] collumnNames)
		{
			var workSheet = package.Workbook.Worksheets.FirstOrDefault(x => x.Name == workSheetName);
			foreach (var name in collumnNames)
			{
				int columnIndex = workSheet?
				.Cells["1:1"]
				.FirstOrDefault(c => c.Value?.ToString()?.Equals(name, StringComparison.OrdinalIgnoreCase) ?? false)
				?.Start.Column ?? -1;
				if (columnIndex != -1)
				{
					workSheet?.DeleteColumn(columnIndex);
				}
			}
			package.Save();
		}
	}
}
