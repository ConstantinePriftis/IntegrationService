using AutoMapper;
using FluentFTP;
using GridShared.Filtering;
using IntegrationService.ConcreteServices.Commands;
using IntegrationService.IServices.Excel;
using IntegrationService.IServices.Ftp;
using IntegrationService.IServices.ICommand;
using IntegrationService.IServices.IQueries;
using IntegrationService.Models.Categories;
using IntegrationService.Models.Errors;
using IntegrationService.Models.Imports;
using IntegrationService.Models.Nutritions;
using IntegrationService.Models.Stores;
using IntegrationService.Services;
using IntegrationService.ViewModels.CategoryViewModels;
using IntegrationService.ViewModels.ImportViewModels;
using IntegrationService.ViewModels.Nutrition;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using static System.Net.WebRequestMethods;

namespace IntegrationService.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class ImportController : Controller
    {
        private readonly INutritionCommand _nutritionCommand;
        private readonly ICSVService _csvService;
        private readonly IImportCommand _importCommand;
        private readonly IProductCharacteristicCommand _productCharacteristicCommand;
        private readonly IMapper _mapper;
        private readonly IProductCommand _productCommand;
        private readonly IStoreCommand _storeCommand;
        private readonly ICollectionCommand _collectionCommand;
        private readonly IFtpIntegrator _ftpIntegrator;
        private readonly IFtpIntegrator _ftp;
        private readonly ICategoryCommand _CategoryCommand;
        private readonly IExcelService _excelService;
        private readonly IFieldProductStoreQuery _fieldProductStoreQuery;
        private readonly IErrorCommand _errorCommand;

        public ImportController(
            ICSVService csvService,
            IImportCommand importCommand,
            IMapper mapper,
            IProductCharacteristicCommand productCharacteristicCommand,
            IProductCommand productCommand,
            IStoreCommand storeCommand,
            ICategoryCommand categoryCommand,
            IFtpIntegrator ftp,
            IExcelService excelService,
            ICollectionCommand collectionCommand,
            IFtpIntegrator ftpIntegrator,
            IFieldProductStoreQuery fieldProductStoreQuery,
            IErrorCommand errorCommand,
            INutritionCommand nutritionCommand)

        {
            _nutritionCommand = nutritionCommand;
            _csvService = csvService;
            _importCommand = importCommand;
            _productCharacteristicCommand = productCharacteristicCommand;
            _mapper = mapper;
            _productCommand = productCommand;
            _storeCommand = storeCommand;
            _ftp = ftp;
            _CategoryCommand = categoryCommand;
            _excelService = excelService;
            _collectionCommand = collectionCommand;
            _ftpIntegrator = ftpIntegrator;
            _fieldProductStoreQuery = fieldProductStoreQuery;
            _errorCommand = errorCommand;
        }
        [HttpGet]
        public IActionResult Index()
        {
            //var fileStream = _ftpIntegrator.GetFileStream();
            //var imports = _csvService.ReadCSV<ImportViewModel>(new MemoryStream(fileStream));

            //var MappedImports = imports.Select(x => _mapper.Map<ImportViewModel, Import>(x)).ToList();
            //_collectionCommand.InsertFromImport(MappedImports);
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(100000000)]
        public async Task<IActionResult> IndexAsync([FromForm] IFormFile file)
        {
            //IFtpIntegrator ftp = _ftp;

            //_ftp.UploadFileStream();
            //_ftp.GetFileStream("");

            //var fileStream = _ftpIntegrator.GetFileStream();
            try
            {
                //Nutrition Logic
                //var nutritions = _csvService.ReadCSV<NutritionViewModel>(file.OpenReadStream());
                //var MappedImports = nutritions.Select(x => _mapper.Map<NutritionViewModel, Nutrition>(x)).ToList();
                //_nutritionCommand.InsertFromImport(MappedImports);

                var imports = _csvService.ReadCSV<ImportViewModel>(file.OpenReadStream(), Encoding.GetEncoding(1253));
                var MappedImports = imports.Select(x => _mapper.Map<ImportViewModel, Import>(x)).ToList();
                //_collectionCommand.InsertFromImport(MappedImports);
               // _importCommand.Insert(MappedImports);
                //_storeCommand.InsertFromImport(MappedImports);
                var differences = _productCharacteristicCommand.EvaluateDifferences();
                _productCommand.InsertFromImport(MappedImports);

            }
            catch (Exception ex)
            {
                await _errorCommand.Insert(Error.Create(ex.Message), null, null);
                throw ex;
            }

            //var content = new MultipartFormDataContent();
            ////var fileStream = File.OpenRead("");
            //Stream str = new MemoryStream();
            //str.Read(bytes);
            //var streamContent = new StreamContent(str);
            //var imageContent = new ByteArrayContent(streamContent.ReadAsByteArrayAsync().Result);
            //imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            //content.Add(imageContent, "image", Path.GetFileName("/app.xlsx"));
            return View();
            //return File(ImportBytes, "application/ms-excel", "ReportFile.xlsx");            //return View(content);
        }
    }
}
