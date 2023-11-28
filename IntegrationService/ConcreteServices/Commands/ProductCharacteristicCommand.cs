using IntegrationService.IServices.ICommand;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Product;
using IntegrationService.ViewModels.ImportViewModels;
using Microsoft.EntityFrameworkCore;

namespace IntegrationService.ConcreteServices.Commands
{
    public class ProductCharacteristicCommand : IProductCharacteristicCommand
    {
        private readonly IProductStoreRepository _productStoreRepo;
        private readonly IProductCharacteristicRepository _prodCharRepo;
        private readonly IStoreRepository _storeRepo;
        private readonly IProductsRepository _productsRepository;

        public ProductCharacteristicCommand(
            IProductCharacteristicRepository prodCharRepo,
            IStoreRepository storeRepo, IProductStoreRepository productStoreRepository, IProductsRepository productsRepository)
        {
            _productStoreRepo = productStoreRepository;
            _prodCharRepo = prodCharRepo;
            _storeRepo = storeRepo;
            _productsRepository = productsRepository;
        }

        public int EvaluateDifferences()
        {
            var dt2 = _prodCharRepo.GetContext().Set<ImportDiffsViewModel>().FromSqlRaw($"select * from [dbo].[GetDiffs]()").ToList()
                .GroupBy(x=>x.Sku)
                .Select(x=>new ImportDiffsFlatViewModel()
                {
                    Sku = x.Key,
                    Title = x?.FirstOrDefault()?.Title ?? string.Empty,
                    Type = x?.FirstOrDefault()?.Type ?? string.Empty,
                    Step = x?.FirstOrDefault()?.Step ?? string.Empty,
                    Status = x?.FirstOrDefault()?.Status ?? string.Empty,
                    StoreCodes = x?.Select(s=>s.StoreCode).ToList() ?? new()
                });
            if (dt2.Count() > 0)
            {
                List<ProductCharacteristic> productCharacteristics = new List<ProductCharacteristic>();
                foreach (var item in dt2)
                {
                    //var product = _productsRepository.FindBy(x=>x.Sku == item.Sku);
                    var prodChar = _prodCharRepo.FindBy(x => x.Sku.Equals(item.Sku)).Include(x=>x.ProductCharacteristicStore).FirstOrDefault();
                    var stores = _storeRepo?.FindBy(x => item.StoreCodes.Any(s=>s.Equals(x.StoreCode))).ToList();
                    //var notInListStores = stores.Where(x => !prodChar.ProductCharacteristicStore.Select(s => s.StoreId).Any(s => s.Equals(x.Id))).ToList();

                    if (prodChar != null)
                    {
                        prodChar.Update(item.Sku, item.Title,item.Description, item.Type, item.Step, item.Status, stores);
                        //var state = _prodCharRepo.GetEntityState(prodChar);
                         
                       // _productStoreRepo.AddOrUpdateRange(notInListStores.Select(x=> ProductCharacteristicStores.Create(prodChar.Id,x)));
                    }
                    else
                    {
                        productCharacteristics.Add(ProductCharacteristic.Create(
                            item.Sku,
                            item.Title,
                            item.Description,
                            item.Type,
                            item.Step,
                            item.Status,
                            false,
                            stores));
                    }
                }
                _prodCharRepo.AddRange(productCharacteristics);
            }
            
            return SaveChanges();
        }

        public int SaveChanges()
        {
            return _prodCharRepo.Save();
        }

        public void Insert(ProductCharacteristic item)
        {
            throw new NotImplementedException();
        }

        public void Insert(List<ProductCharacteristic> collection)
        {
            throw new NotImplementedException();
        }

        public void InsertOrUpdate(ProductCharacteristic item)
        {
            throw new NotImplementedException();
        }

        public void InsertOrUpdate(List<ProductCharacteristic> collection)
        {
            throw new NotImplementedException();
        }
    }
}
