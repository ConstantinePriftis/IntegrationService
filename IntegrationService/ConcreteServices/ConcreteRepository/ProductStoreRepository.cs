using IntegrationService.ConcreteRepository;
using IntegrationService.Data.DbContexts;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Product;

namespace IntegrationService.ConcreteServices.ConcreteRepository
{
    public class ProductStoreRepository :
    GenericRepository<ProductStores>, IProductStoreRepository
    {
        public ProductStoreRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
