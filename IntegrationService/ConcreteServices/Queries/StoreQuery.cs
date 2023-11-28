using IntegrationService.ConcreteServices.ConcreteRepository;
using IntegrationService.ConcreteServices.Helper;
using IntegrationService.IServices.IQueries;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models;
using IntegrationService.ViewModels;
using IntegrationService.ViewModels.CategoryViewModels;
using IntegrationService.ViewModels.ProductViewModels;
using IntegrationService.ViewModels.StoreViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IntegrationService.ConcreteServices.Queries
{
    public class StoreQuery : IStoreQuery
    {
        private readonly IStoreRepository _storeRepository;
        public StoreQuery(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }
        private IQueryable<StoreViewModel> GetStoreListQueryable()
        {
            return (from store in _storeRepository.GetAll()
                    select new StoreViewModel()
                    {
                        Id = store.Id,
                        WarehouseID = store.StoreCode,
                        Name = store.Name,
                        Type = store.Type
                    }
                   );
        }

        public async Task<List<StoreViewModel>> GetListAsync(Expression<Func<StoreViewModel, bool>> predicate)
        {
            return await GetStoreListQueryable()
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<List<StoreViewModel>> GetListAsync()
        {
            return await GetStoreListQueryable().ToListAsync();
        }
        public async Task<List<SelectListItem>> GetSelectListItems()
        {
            var skata = await GetStoreListQueryable().ToListAsync();
            return await GetStoreListQueryable().Select(x => x.ToSelectListItem()).ToListAsync();
        }

        public IQueryable<SelectListItem> GetSelectListItemsQueryable(string term, List<FilterItem> filterItems)
        {
            return GetStoreListQueryable().ApplyTermFilter(FilterHelper.GetFiltersByKey(nameof(StoreViewModel.WarehouseID), filterItems))
                .Where(x => x.WarehouseID.Contains(term)).Select(x => x.ToSelectListItem());
        }

        public async Task<PagedResult<StoreViewModel>> GetPagedListAsync(int page, List<FilterItem> filterItems)
        {
            var pagedResult = await GetStoreListQueryable().FilterByValues(FilterHelper.GetStaticFilters(filterItems))
                                        .Sort(FilterHelper.GetSortFilters(filterItems))
                                            .ApplyPagedListAsync(page, 13);

            return new PagedResult<StoreViewModel>
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

        public Task<PagedResult<StoreViewModel>> GetPagedListAsync(int page)
        {
            throw new NotImplementedException();
        }
    }
}
