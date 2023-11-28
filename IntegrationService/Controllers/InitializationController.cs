using AutoMapper;
using IntegrationService.ConcreteServices.Commands;
using IntegrationService.ConcreteServices.ConcreteRepository;
using IntegrationService.ConcreteServices.Queries;
using IntegrationService.ConcreteServices.Query;
using IntegrationService.IServices.Excel;
using IntegrationService.IServices.Ftp;
using IntegrationService.IServices.ICommand;
using IntegrationService.IServices.IQueries;
using IntegrationService.IServices.IQuery;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Errors;
using IntegrationService.Models.Fields;
using IntegrationService.Models.Imports;
using IntegrationService.Models.ProductCharacteristicImports;
using IntegrationService.Services;
using IntegrationService.ViewModels.ImportViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Net.WebRequestMethods;

namespace IntegrationService.Controllers
{
    public class InitializationController : Controller
    {
        private readonly INutritionCommand _nutritionCommand;
        private readonly ICSVService _csvService;
        private readonly IImportCommand _importCommand;
        private readonly IProductCharacteristicCommand _productCharacteristicCommand;
        private readonly IMapper _mapper;
        private readonly IProductCommand _productCommand;
        private readonly IStoreCommand _storeCommand;
        private readonly IFtpIntegrator _ftp;
        private readonly ICategoryCommand _CategoryCommand;
        private readonly IExcelService _excelService;
        private readonly ICollectionCommand _collectionCommand;
        private readonly IFtpIntegrator _ftpIntegrator;
        private readonly IFieldProductStoreQuery _fieldProductStoreQuery;
        private readonly IErrorCommand _errorCommand;
        private readonly IFieldsQuery _fieldsQuery;
        private readonly IProductsQuery _productsQuery;
        private readonly IProductsRepository _productsRepository;
        private readonly IFieldRepository _fieldRepository;
        private readonly IFieldProductRepository _fieldProductRepository;

        public InitializationController(
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
            INutritionCommand nutritionCommand,
            IFieldsQuery fieldsQuery,
            IProductsQuery productsQuery,
            IProductsRepository productsRepository,
            IFieldRepository fieldRepository,
            IFieldProductRepository fieldProductRepository)

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
            _fieldsQuery = fieldsQuery;
            _productsQuery = productsQuery;
            _productsRepository = productsRepository;
            _fieldRepository = fieldRepository;
            _fieldProductRepository = fieldProductRepository;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(100000000)]
        public async Task<IActionResult> IndexAsync([FromForm] IFormFile file)
        {
            try
            {
                var imports = _csvService.ReadCSV<ProductCharacteristicImport>(file.OpenReadStream(), Encoding.UTF8);
                var products = _productsRepository.GetAll();
                var productDictionary = products.ToDictionary(kvp => kvp.Sku, kvp => kvp.Id);
                var fields = _fieldRepository.GetAll().Where(x=>!x.IsStore).ToList();
                var psc = imports.Where(x=> products.Select(y=>y.Sku).Contains(x.SKU)).Select(x => new
                {
                    Sku = x.SKU,
                    Fields = GetChannelsWithValues(x, fields.Select(x => x.Name).ToList())
                });
                //Guid idToInsert = Guid.Empty;
                var FieldsPerProduct = fields.SelectMany(y => psc.Select(x => new FieldProducts
                {
                    ProductsId = productDictionary[x.Sku],
                    FieldId = y.Id,
                    ModifiedOn = DateTime.UtcNow,
                    Value = x.Fields[y.Name]
                }));
                int batchSize = 1000;
                int totalEntities = FieldsPerProduct.Count();
                int totalBatches = (int)Math.Ceiling((double)totalEntities / batchSize);



                for (int i = 0; i < totalBatches; i++)
                {
                    var currentBatch = FieldsPerProduct.Skip(i * batchSize).Take(batchSize).ToList();
                    _fieldProductRepository.AddRange(FieldsPerProduct as IEnumerable<FieldProducts>);
                    _fieldProductRepository.Save();
                }
                //_fieldProductRepository.AddRange(FieldsPerProduct as IEnumerable<FieldProducts>);
                //_fieldProductRepository.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                await _errorCommand.Insert(Error.Create(ex.Message), null, null);
                throw ex;
            }
        }
        private static Dictionary<string, string> GetChannelsWithValues(object obj, List<string> channels)
        {
            var dict = new Dictionary<string, string>();
            var properties = obj.GetType().GetProperties();

            foreach (var property in properties.Where(x => channels.Contains(x.Name)))
            {
                var value = property.GetValue(obj)?.ToString();
                dict.Add(property.Name, value);
            }

            return dict;
        }
    }
}
