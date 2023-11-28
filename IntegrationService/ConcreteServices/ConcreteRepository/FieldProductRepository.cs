using IntegrationService.ConcreteRepository;
using IntegrationService.Data.DbContexts;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Fields;

namespace IntegrationService.ConcreteServices.ConcreteRepository
{
    public class FieldProductRepository :
    GenericRepository<FieldProducts>, IFieldProductRepository
    {
        public FieldProductRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
