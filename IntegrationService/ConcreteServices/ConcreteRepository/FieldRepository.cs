using IntegrationService.ConcreteRepository;
using IntegrationService.Data.DbContexts;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Fields;
using IntegrationService.Models.Stores;

namespace IntegrationService.ConcreteServices.ConcreteRepository
{
    public class FieldRepository :
    GenericRepository<Field>, IFieldRepository
    {
        public FieldRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
