using IntegrationService.IServices.IQuery;
using IntegrationService.Models;
using IntegrationService.Models.Nutritions;
using IntegrationService.ViewModels;
using IntegrationService.ViewModels.FieldProductsViewModels;
using IntegrationService.ViewModels.Nutrition;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IntegrationService.IServices.IQueries
{
    public interface INutritionQuery : IQuery<NutritionViewModel>
    {
        Task<NutritionViewModel?> GetByProductIdAsync(Guid? id);
        Task<IEnumerable<NutritionViewModel>?> GetBySku(string? sku);
        IQueryable<NutritionViewModel> GetIQueryableList();
        Task<IQueryable<NutritionViewModel>> GetQueryableNutritions();
        IQueryable<NutritionViewModel> GetQueryableExport(DateTime datetime);
        // Task<PagedResult<NutritionViewModel>> GetPagedListAsync(int page, List<FilterItem> filterItems);
        IQueryable<SelectListItem> GetSelectListProductsWithoutNutritionQueryable(string? term, List<FilterItem> filterItems);
        Task<IQueryable<NutritionViewModel>> ExportUpdates(DateTime filter);

		// Task<PagedResult<NutritionViewModel>> GetPagedListAsync(int page, List<FilterItem> filterItems);
	}
}
