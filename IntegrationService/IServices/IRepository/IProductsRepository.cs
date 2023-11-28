using IntegrationService.Models.Product;
using IntegrationService.Models.Stores;
using IntegrationService.Services.Repository;

namespace IntegrationService.IServices.IRepository
{
    public interface IProductsRepository
        : IGenericRepository<Products>
    {
    }
}
