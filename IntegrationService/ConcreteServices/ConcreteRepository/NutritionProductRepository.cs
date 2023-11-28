using IntegrationService.ConcreteRepository;
using IntegrationService.Data.DbContexts;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Nutritions;
using IntegrationService.Models.Product;

namespace IntegrationService.ConcreteServices.ConcreteRepository
{
    public class NutritionProductRepository
        : GenericRepository<NutritionProduct>, INutritionProductRepository
    {
        public NutritionProductRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
