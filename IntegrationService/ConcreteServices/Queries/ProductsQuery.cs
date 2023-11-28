using IntegrationService.ConcreteServices.Helper;
using IntegrationService.IServices.IQueries;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models;
using IntegrationService.ViewModels;
using IntegrationService.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IntegrationService.ConcreteServices.Queries
{
    public class ProductsQuery : IProductsQuery
    {
        private readonly IProductsRepository _productsRepository;
        public ProductsQuery(IProductsRepository productsRepository)
        {
            _productsRepository= productsRepository;
        }

        private IQueryable<ProductsViewModel> GetProductsListQueryable()
        {
            return (from product in _productsRepository.GetAll().Include(x => x.FieldProducts).Include(x => x.ProductStore)
                    select new ProductsViewModel()
                    {
                        Id = product.Id,
                        Sku = product.Sku,
                        Title = product.Title,
                        //Fields = product.FieldProducts.Select(x => x.Field).Select(x => x.ToDto()).ToList(),
                        //Stores = product.ProductStore.Select(x => x.Store).Select(x => x.ToDto()).ToList()
                    }
                   );
        }

        public async Task<List<ProductsViewModel>> GetListAsync(Expression<Func<ProductsViewModel, bool>> predicate)
        {
            return await GetProductsListQueryable()
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<List<ProductsViewModel>> GetListAsync()
        {
            return await GetProductsListQueryable().ToListAsync();
        }

        public async Task<List<SelectListItem>> GetSelectListItems()
        {
            var skata = await GetProductsListQueryable().ToListAsync();
            return await GetProductsListQueryable().Select(x => x.ToSelectListItem()).ToListAsync();
        }

        public IQueryable<SelectListItem> GetSelectListItemsQueryable(string? term, List<FilterItem> filterItems)
        {
            return GetProductsListQueryable().ApplyTermFilter(FilterHelper.GetFiltersByKey(nameof(ProductsViewModel.Sku), filterItems))
                .Where(x => x.Sku.Contains(term)).Select(x => x.ToSelectListItem());
        }

        public Task<PagedResult<ProductsViewModel>> GetPagedListAsync(int page)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<ProductsViewModel>> GetPagedListAsync(int page, List<FilterItem> filterItems)
        {
            throw new NotImplementedException();
        } 
    }
}
