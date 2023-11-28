using IntegrationService.ConcreteRepository;
using IntegrationService.Data.DbContexts;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Fields;
using IntegrationService.Services.Repository;

namespace IntegrationService.ConcreteServices.ConcreteRepository
{
    public class DOMGroupsRepository : GenericRepository<DOMGroups>, IDOMGroupsRepository
    {
        public DOMGroupsRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
