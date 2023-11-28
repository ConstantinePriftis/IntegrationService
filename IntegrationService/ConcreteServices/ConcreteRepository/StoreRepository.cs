using IntegrationService.ConcreteRepository;
using IntegrationService.Data.DbContexts;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Imports;
using IntegrationService.Models.Stores;
using IntegrationService.Services.Repository;

namespace IntegrationService.ConcreteServices.ConcreteRepository
{
    public class StoreRepository :
    GenericRepository<Store>, IStoreRepository
    {
        public StoreRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
