using IntegrationService.IServices.IQuery;
using IntegrationService.ViewModels;
using IntegrationService.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IntegrationService.IServices.IQueries
{
    public interface IProductsQuery : IQuery<ProductsViewModel>
    {
		IQueryable<SelectListItem> GetSelectListItemsQueryable(string? term, List<FilterItem> filterItems);
    }
}
