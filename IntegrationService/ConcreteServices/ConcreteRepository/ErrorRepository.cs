using IntegrationService.ConcreteRepository;
using IntegrationService.Data.DbContexts;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Errors;

namespace IntegrationService.ConcreteServices.ConcreteRepository
{
    public class ErrorRepository : GenericRepository<Error>, IErrorRepository
    {
        public ErrorRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
