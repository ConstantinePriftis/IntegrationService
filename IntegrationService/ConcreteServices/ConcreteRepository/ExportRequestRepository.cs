using IntegrationService.ConcreteRepository;
using IntegrationService.Data.DbContexts;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Categories;
using IntegrationService.Services.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IntegrationService.ConcreteServices.ConcreteRepository
{
    public class ExportRequestRepository
         : GenericRepository<Models.Exports.ExportRequest>, IExportRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public ExportRequestRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

      
    }
}
