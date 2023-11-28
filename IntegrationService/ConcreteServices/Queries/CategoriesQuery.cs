using IntegrationService.ConcreteServices.Helper;
using IntegrationService.IServices.IQueries;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models;
using IntegrationService.ViewModels;
using IntegrationService.ViewModels.CategoryViewModels;
using IntegrationService.ViewModels.ChannelViewModels;
using IntegrationService.ViewModels.FieldProductsViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace IntegrationService.ConcreteServices.Queries
{
    public class CategoriesQuery : ICategoriesQuery
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoriesQuery(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        private IQueryable<CategoryViewModel> GetCategoryQueryable()
        {
            return (from category in _categoryRepository.GetAll()
                    select new CategoryViewModel()
                    {
                        Id = category.Id,
                        Code = category.Code,
                        Level = category.Level.ToString(),
                        Description = category.Description
                    }
                   );
        }
        private async Task<PagedResult<CategoryViewModel>> GetCategoryPagedList(int page)
        {

            var pagedResult = await GetCategoryQueryable().ApplyPagedListAsync(page, 10);

            return new PagedResult<CategoryViewModel>
            {
                Results = pagedResult.Results.AsNoTracking().ToList(),
                PageCount = pagedResult.PageCount,
                PageSize = pagedResult.PageSize,
                CurrentPage = pagedResult.CurrentPage,
                EndPage = pagedResult.EndPage,
                StartPage = pagedResult.StartPage,
                RowCount = pagedResult.RowCount

            };
        }

        public async Task<PagedResult<CategoryViewModel>> GetPagedListAsync(int page)
        {
            var result = await GetCategoryPagedList(page);
            return result;
        }

        public Task<List<CategoryViewModel>> GetListAsync(Expression<Func<CategoryViewModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CategoryViewModel>> GetListAsync()
        {
            return await GetCategoryQueryable().ToListAsync();
        }

        public async Task<PagedResult<CategoryViewModel>> GetPagedListAsync(int page, List<FilterItem> filterItems)
        {
            var pagedResult = await GetCategoryQueryable().FilterByValues(FilterHelper.GetStaticFilters(filterItems))
                                        .Sort(FilterHelper.GetSortFilters(filterItems))
                                            .ApplyPagedListAsync(page, 13);

            return new PagedResult<CategoryViewModel>
            {
                Results =
                pagedResult.Results.AsNoTracking()
                .ToList(),
                PageCount = pagedResult.PageCount,
                PageSize = pagedResult.PageSize,
                CurrentPage = pagedResult.CurrentPage,
                EndPage = pagedResult.EndPage,
                StartPage = pagedResult.StartPage,
                RowCount = pagedResult.RowCount

            };
        }

        public async Task<List<SelectListItem>> GetSelectListItems()
        {
            return await GetCategoryQueryable().Select(x=>x.ToSelectListItem()).ToListAsync();
        }
        public IQueryable<SelectListItem> GetSelectListItemsQueryable(string term)
        {
            return GetCategoryQueryable().Where(x => x.Code.Contains(term) || x.Description.Contains(term)).Select(x=>x.ToSelectListItem());
        }
        
    }
}
