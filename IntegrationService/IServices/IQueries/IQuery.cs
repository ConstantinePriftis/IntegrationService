using IntegrationService.Models;
using IntegrationService.Models.EntityBases;
using IntegrationService.Models.User;
using IntegrationService.ViewModels;
using IntegrationService.ViewModels.FieldProductsViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace IntegrationService.IServices.IQuery
{
    public interface IQuery<T> where T : class, new()
    {
        Task<PagedResult<T>> GetPagedListAsync(int page, List<FilterItem> filterItems);
        Task<PagedResult<T>> GetPagedListAsync(int page);
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetListAsync();
        Task<List<SelectListItem>> GetSelectListItems();
    }
}
