using IntegrationService.ConcreteRepository;
using IntegrationService.Data.DbContexts;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Imports;
using IntegrationService.Models.Product;
using IntegrationService.Services.Repository;

namespace IntegrationService.ConcreteServices.ConcreteRepository
{
    public class ProductCharacteristicRepository :
    GenericRepository<ProductCharacteristic>, IProductCharacteristicRepository
    {
        public ProductCharacteristicRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}

