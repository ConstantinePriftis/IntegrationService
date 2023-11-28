using IntegrationService.ConcreteServices.Helper;
using IntegrationService.IServices.IQueries;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models;
using IntegrationService.ViewModels;
using IntegrationService.ViewModels.FieldProductStores;
using IntegrationService.ViewModels.FieldProductsViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IntegrationService.ConcreteServices.Queries
{

    public class FieldProductQuery : IFieldProductQuery
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IFieldRepository _fieldRepository;

        public FieldProductQuery(IProductsRepository productsRepository,
            IFieldRepository fieldRepository)
        {
            _fieldRepository = fieldRepository;
            _productsRepository = productsRepository;
        }
        
        private IQueryable<FieldProductViewModel> GetFieldProductListQueryable(List<FilterItem> filters)
        {
            Func<Guid?, IQueryable<FieldPerProductViewModel>> getFieldsPerProduct = (id) => (GetFieldsPerProduct(id));
            var sortableProdFields = GetProdWithSortDynamicFields(filters).Sort(FilterHelper.GetSortDynamicFilters(filters)).Select(x=>x.ProductId);
            var result = (from prodFields in sortableProdFields
                          join prod in _productsRepository.GetAll() on prodFields equals prod.Id
                          where !FilterHelper.GetDynamicFilters(filters).Any() || GetProdIdsWithDynamicFieldsFilters(filters).Contains(prod.Id)
                          select new FieldProductViewModel
                          {
                              Id = prod.Id,
                              ProductId = prod.Id,
                              Sku = prod.Sku,
                              Title_Import = prod.Title,
                              Description_Import = prod.Description,
                              //Description_Category = prod.Description_Category,
                              Status = prod.Status,
                              Step = prod.Step,
                              IsCompleted = prod.Enabled,
                              ModifiedOn = prod.ModifiedOn,
                              Fields = getFieldsPerProduct(prod.Id).AsEnumerable()
                          });

            return result.FilterByValues(FilterHelper.GetStaticFilters(filters));
        }

        private IQueryable<Guid> GetProdIdsWithDynamicFieldsFilters(List<FilterItem> filters)
        {
            var fieldsParamWithFilters = GetFieldsPerProduct()
                .DynamicFilterByValues(FilterHelper.GetDynamicFilters(filters))
                .Select(x => x.ProductId ?? Guid.Empty).Distinct();
            return fieldsParamWithFilters;
        }

        private IQueryable<FieldPerProductViewModel> GetProdWithSortDynamicFields(List<FilterItem> filters)
        {
            var fieldName = filters.Where(x => x.Type == FilterItemEnum.DynamicSort).Select(x => x.Field).FirstOrDefault();
            var prod = fieldName == null ? from pr in _productsRepository.GetAll()
                                           select
                                           new FieldPerProductViewModel
                                           {
                                               ProductId = pr.Id
                                           } :from pr in _productsRepository.GetAll()
                       join fields in GetFieldsPerProduct().Where(x => fieldName == null || x.FieldName == fieldName) on pr.Id equals fields.ProductId
                       into data
                       from m in data.DefaultIfEmpty()
                       select
                       new FieldPerProductViewModel
                       {
                           FieldId = m.FieldId,
                           FieldName = m.FieldName,
                           ProductId = pr.Id,
                           Value = m.Value
                       };
            
            return prod;
        }

        private IQueryable<FieldPerProductViewModel> GetFieldsPerProduct(Guid? id = null)
        {

            var allFields = _fieldRepository.FindBy(x => !x.IsStore);
            return from fields in allFields
                   join fieldProducts in _fieldRepository.GetAll()
                       .SelectMany(x => x.FieldProducts)
                       .Where(x => id == null || x.ProductsId == id)
                       on fields.Id equals fieldProducts.FieldId into data
                   from m in data.DefaultIfEmpty()
                   select new FieldPerProductViewModel
                   {
                       FieldId = fields.Id,
                       FieldName = fields.Name,
                       ProductId = m.ProductsId,
                       Value = m.Value,
                       Order = fields.Order,
                       DomGroupDesc = fields.DOMGroups.Description,
                       DomValues = fields.DOMGroups.DOMValues.Select(x => x.ToSelectListItem()).ToList()
                   };
        }
        private async Task<PagedResult<FieldProductViewModel>> GetFieldProductPagedList(int page, List<FilterItem> filters)
        {
            var pagedResult = await GetFieldProductListQueryable(filters)
                                        .Sort(FilterHelper.GetSortFilters(filters))
                                            .ApplyPagedListAsync(page, 13);

            return new PagedResult<FieldProductViewModel>
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

        public async Task<PagedResult<FieldProductViewModel>> GetPagedListAsync(int page, List<FilterItem> filterItems)
        {
            var result = await GetFieldProductPagedList(page, filterItems);
            return result;
        }

        public async Task<FieldProductViewModel?> GetByProductIdAsync(Guid? id)
        {
            var fieldProduct = await GetFieldProductListQueryable(new List<FilterItem>() { }).Where(x => x.ProductId == id).FirstOrDefaultAsync();
            return fieldProduct;
        }

        public async Task<FieldProductViewModel?> GetBySku(string sku)
        {
                var fieldProduct = await GetFieldProductListQueryable(new List<FilterItem>() { }).Where(x => x.Sku == sku).FirstOrDefaultAsync();
                return fieldProduct;
        }

        public async Task<List<FieldProductViewModel>> GetListAsync(Expression<Func<FieldProductViewModel, bool>> predicate)
        {
            var skata = await GetFieldProductListQueryable(new List<FilterItem>() { }).Where(predicate).ToListAsync();
            return skata;
        }

        public async Task<List<FieldProductViewModel>> GetListAsync()
        {
            return (await GetFieldProductPagedList(1, null)).Results;
        }

        public async Task<PagedResult<FieldProductViewModel>> GetPagedListAsync(int page)
        {
            var result = await GetFieldProductPagedList(page, new List<FilterItem>() { });
            return result;
        }

        public Task<List<SelectListItem>> GetSelectListItems()
        {
            throw new NotImplementedException();
        }

        public IQueryable<FieldProductViewModel> GetIQueryableList(DateTime filter)
        {
            var filters = new List<FilterItem>() { new FilterItem() { Field = nameof(FieldProductStoreViewModel.ModifiedOn), Type = FilterItemEnum.Static, Value = new string[] { filter.ToString("yyyy-MM-dd") } } };
            var listOfProducts = GetFieldProductListQueryable(filters);
            return listOfProducts;
        }
    }
}
