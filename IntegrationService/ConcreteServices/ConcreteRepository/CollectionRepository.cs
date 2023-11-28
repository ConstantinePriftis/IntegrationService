using IntegrationService.ConcreteRepository;
using IntegrationService.Data.DbContexts;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Collection;

namespace IntegrationService.ConcreteServices.ConcreteRepository
{
    public class CollectionRepository : GenericRepository<Collection>, ICollectionRepository
    {
        public CollectionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
