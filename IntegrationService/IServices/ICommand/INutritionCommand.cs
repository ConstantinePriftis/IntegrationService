using IntegrationService.Models.Categories;
using IntegrationService.Models.Nutritions;
using IntegrationService.Models.Product;
using IntegrationService.Models.User;

namespace IntegrationService.IServices.ICommand
{
    public interface INutritionCommand : ICommandAsync<Nutrition>
    {
        void InsertRange(string sku, List<Nutrition> nutritions, out string validationMessage);
        void InsertRange(string sku, List<Nutrition> nutritions);
        void InsertFromImport(List<Nutrition> collection);
        void Insert(Nutrition item, ApplicationUser user, List<Guid>? items);
        Task<int> UpdateNutrition(Guid nutritionId, Nutrition nutrition, Products product);
        Task<int> UpdateRange(List<Nutrition> nutritions, ApplicationUser user, List<Guid>? items);
	}
}
