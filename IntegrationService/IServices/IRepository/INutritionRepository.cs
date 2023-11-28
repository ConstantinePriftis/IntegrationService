using IntegrationService.Models.Categories;
using IntegrationService.Models.Nutritions;
using IntegrationService.Services.Repository;

namespace IntegrationService.IServices.IRepository
{
    public interface INutritionRepository
        : IGenericRepository<Nutrition>
    {
    }
}
