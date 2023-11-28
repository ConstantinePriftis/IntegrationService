using IntegrationService.ConcreteRepository;
using IntegrationService.Data.DbContexts;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Categories;
using IntegrationService.Models.Nutritions;

namespace IntegrationService.ConcreteServices.ConcreteRepository
{
    public class NutritionRepository : GenericRepository<Nutrition>, INutritionRepository
    {
        public NutritionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
