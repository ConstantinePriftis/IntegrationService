using IntegrationService.Models.Categories;
using IntegrationService.Models.Stores;
using IntegrationService.Services.Repository;

namespace IntegrationService.IServices.IRepository
{
    public interface ICategoryRepository
        : IGenericRepository<Category>
    {
    }
}
