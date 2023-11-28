using IntegrationService.IServices.IQuery;
using IntegrationService.ViewModels;
using IntegrationService.ViewModels.StoreViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IntegrationService.IServices.IQueries
{
    public interface IStoreQuery : IQuery<StoreViewModel>
    {
		IQueryable<SelectListItem> GetSelectListItemsQueryable(string term, List<FilterItem> filterItems);
    }
}
