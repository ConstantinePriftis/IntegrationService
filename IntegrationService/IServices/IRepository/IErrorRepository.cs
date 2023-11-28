using IntegrationService.Models.Categories;
using IntegrationService.Models.Errors;
using IntegrationService.Services.Repository;

namespace IntegrationService.IServices.IRepository
{
    public interface IErrorRepository
        : IGenericRepository<Error>
    {
    }
}
