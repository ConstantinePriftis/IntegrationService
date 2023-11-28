using IntegrationService.ConcreteRepository;
using IntegrationService.Data.DbContexts;
using IntegrationService.IServices.IQueries;
using IntegrationService.IServices.IQuery;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models;
using IntegrationService.Models.Categories;
using IntegrationService.Models.Fields;
using IntegrationService.ViewModels.FieldProductsViewModels;
using IntegrationService.ViewModels.FieldViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace IntegrationService.ConcreteServices.ConcreteRepository
{
    public class FieldPerProductViewModelRepository : GenericRepository<FieldPerProductViewModel>, IFieldPerProductViewModelRepository
    {
        public FieldPerProductViewModelRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
