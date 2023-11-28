using IntegrationService.Data.DbContexts;
using IntegrationService.Models.Imports;
using IntegrationService.Services.Repository;
using Microsoft.EntityFrameworkCore;

namespace IntegrationService.ConcreteRepository
{
    public class ImportRepository :
    GenericRepository<Import>, IImportRepository
    {
        public ImportRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
