using IntegrationService.ConcreteRepository;
using IntegrationService.Data.DbContexts;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Fields;

namespace IntegrationService.ConcreteServices.ConcreteRepository
{
    public class FieldProductStoreRepository :
    GenericRepository<FieldProductStore>, IFieldProductStoreRepository
    {
        public FieldProductStoreRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
