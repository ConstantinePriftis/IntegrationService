using IntegrationService.ConcreteRepository;
using IntegrationService.Data.DbContexts;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Product;
using IntegrationService.Models.Stores;

namespace IntegrationService.ConcreteServices.ConcreteRepository
{
    public class ProductsRepository :
    GenericRepository<Products>, IProductsRepository
    {
        public ProductsRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
