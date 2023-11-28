using IntegrationService.ConcreteRepository;
using IntegrationService.Models.Categories;
using IntegrationService.Models.Exports;
using IntegrationService.Services.Repository;

namespace IntegrationService.IServices.IRepository
{
    public interface IExportRequestRepository
        : IGenericRepository<ExportRequest>
    {
    }
}
