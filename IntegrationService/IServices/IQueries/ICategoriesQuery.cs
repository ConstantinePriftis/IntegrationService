using IntegrationService.IServices.IQuery;
using IntegrationService.ViewModels.CategoryViewModels;
using IntegrationService.ViewModels.ChannelViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IntegrationService.IServices.IQueries
{
    public interface ICategoriesQuery : IQuery<CategoryViewModel>
    {
        IQueryable<SelectListItem> GetSelectListItemsQueryable(string term);
    }
}
