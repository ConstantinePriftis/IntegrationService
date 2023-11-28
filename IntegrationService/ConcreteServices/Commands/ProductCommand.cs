using IntegrationService.IServices.ICommand;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Imports;
using IntegrationService.Models.Product;
using IntegrationService.Models.User;
using LinqToDB;
using Microsoft.EntityFrameworkCore;

namespace IntegrationService.ConcreteServices.Commands
{
    public class ProductCommand : IProductCommand
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IProductCharacteristicRepository _productCharacteristicRepository;
        private readonly IProductStoreRepository _productStoreRepository;
        public ProductCommand(IProductsRepository productsRepository, IStoreRepository storeRepository,
            IProductCharacteristicRepository productCharacteristicRepository, IProductStoreRepository productStoreRepository)
        {
            _storeRepository = storeRepository;
            _productsRepository = productsRepository;
            _productCharacteristicRepository = productCharacteristicRepository;
            _productStoreRepository = productStoreRepository;
        }

        public void InsertFromImport(List<Import> collection)
        {
            var stores = _storeRepository.GetAll();
            if (_productsRepository.GetAll().Count() <= 0)
            {
                try
                {
                    var products = collection.GroupBy(x => x.Sku)
                                     .Select(x =>
                                     Products.Create(
                                        x.Key,
                                        x.FirstOrDefault()?.Title ?? string.Empty,
                                        x.FirstOrDefault()?.Description ?? string.Empty,
                                        string.Empty,
                                         x.FirstOrDefault()?.Status ?? string.Empty,
                                          x.FirstOrDefault()?.Step ?? string.Empty,
                                        stores.Where(y => x.Select(s => s.StoreCode)
                                        .ToArray()
                                        .Contains(y.StoreCode))
                                        .ToList()))
                                        .ToList();
                    _productsRepository.AddRange(products);
                }
                catch (Exception ex)
                {
                    var exc = ex;
                }

            }
            else
            {
                List<Products> products = new List<Products>();
                var prodChars = _productCharacteristicRepository.FindBy(x => x.IsChanged == false).Include(x => x.ProductCharacteristicStore);
                foreach (var item in prodChars)
                {
                    var storesPerSku = stores.Where(x => item.ProductCharacteristicStore.Select(s => s.StoreId).Any(s => s.Equals(x.Id))).ToList();
                    var product = _productsRepository.FindBy(p => p.Sku == item.Sku).Include(x=>x.ProductStore).Include(x=>x.FieldProducts).FirstOrDefault();
                    
                    if (product !=null)
                    {
                        //var existedStores = stores.Where(x => product.ProductStore.Select(s => s.StoreId).Any(s => s.Equals(x.Id))).ToList();
                        //var totalStores = storesPerSku.Union(existedStores).ToList();
                        product.ProductStore.Where(x => stores.Any(y => y.Id.Equals(x.StoreId))).ToList().ForEach(x => x.Update(stores.FirstOrDefault(y => y.Id == x.StoreId)));
                        var prodStores = storesPerSku.Select(x => ProductStores.Create(product.Id, x));
                        _productStoreRepository.AddRange(prodStores);
                        _productStoreRepository.Save();
                        product.Update(item.Sku, item.ProductDescription ?? string.Empty,item.Description ?? string.Empty, item.Status ?? string.Empty, item.Step ?? string.Empty, null);
                    }
                    else
                    {
                        var newProduct = Products.Create(item.Sku, item.ProductDescription ?? string.Empty,item.Description ?? string.Empty, string.Empty, item.Status ?? string.Empty, item.Step ?? string.Empty, storesPerSku);
                        products.Add(newProduct);
                    }
                }
                _productsRepository.AddRange(products);
            }
            _productsRepository.Save();
        }

        public async Task<int> Insert(Products item, ApplicationUser user, List<Guid>? items)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertRange(IEnumerable<Products> items, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(Guid id, Products item, ApplicationUser user, List<Guid>? items)
        {
            throw new NotImplementedException();
        }
    }
}
