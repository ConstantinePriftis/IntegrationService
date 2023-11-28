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
    public class FieldProductStoreQuery : IFieldProductStoreQuery
    {
        private readonly IProductStoreRepository _productStoreRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IFieldRepository _fieldRepository;
        private readonly ICollectionRepository _collectionRepository;
        private readonly IChannelRepository _channelRepository;

        public FieldProductStoreQuery(
         IProductStoreRepository productStoreRepository,
         IStoreRepository storeRepository,
            IProductsRepository productsRepository,
            IFieldRepository fieldRepository,
            ICollectionRepository collectionRepository,
            IChannelRepository channelRepository)
        {
            _productStoreRepository = productStoreRepository;
            _storeRepository = storeRepository;
            _productsRepository = productsRepository;
            _fieldRepository = fieldRepository;
            _collectionRepository = collectionRepository;
            _channelRepository = channelRepository;
        }

        private IQueryable<FieldProductStoreViewModel> GetFieldProductListQueryable(List<FilterItem> filters)
        {
            Func<Guid?, IQueryable<FieldPerProductViewModel>> getFieldsPerProduct = (id) => (GetFieldsPerProductStore(id));

            Func<string?, string?, IQueryable<CollectionViewModel>> getCollections = (sku, storeCode) => (from collection in _collectionRepository.GetAll()
                                                                                                          join channel in _channelRepository.GetAll() on collection.ChannelId equals channel.Id
                                                                                                          where (sku == null || collection.Sku == sku) && (storeCode == null || collection.StoreCode == storeCode)
                                                                                                          select
                                                                                                          new CollectionViewModel
                                                                                                          {
                                                                                                              ChannelId = channel.Id,
                                                                                                              Sku = collection.Sku,
                                                                                                              StoreCode = collection.StoreCode,
                                                                                                              ChannelName = channel.ChannelName,
                                                                                                              Value = collection.GetCollectionStatusByValue()
                                                                                                          });
            
			var sortableProdFields = GetProdWithSortDynamicFields(filters).Sort(FilterHelper.GetSortDynamicFilters(filters)).Select(x => x.ProductId);
            var prodStoresWithFilters = GetProdStoreIdsWithDynamicFieldsFilters(filters);
            var collectionsWithFilters = GetCollectionsWithFilters(filters);
            var filterableProductStores = (from ps in _productStoreRepository.GetAll()
                                           where 
                                           (!FilterHelper.GetDynamicFilters(filters).Any() ||
                                           prodStoresWithFilters.Contains(ps.Id))
                                                   &&
                                                    (!FilterHelper.GetCollectionsFilters(filters).Any()
                                                    ||
                                                    collectionsWithFilters.Contains(ps.Id)
                                                    )
                                           select ps);

            var result = from prodFields in sortableProdFields
                         join ps in filterableProductStores on prodFields equals ps.Id
						 join prod in _productsRepository.GetAll().Include(x => x.ProductStore) on ps.ProductId equals prod.Id
						 join s in _storeRepository.GetAll() on ps.StoreId equals s.Id
                         select new FieldProductStoreViewModel
                         {
                             ProductStoreId = ps.Id,
                             Sku = prod.Sku,
                             Title = prod.Title,
                             WarehouseID = s.StoreCode,
                             WarehouseName = s.Name,
                             ModifiedOn = ps.ModifiedOn,
                             IsPublished = ps.Enabled,
                             Channels = getCollections(prod.Sku, s.StoreCode).AsEnumerable(),
                             Fields = getFieldsPerProduct(ps.Id).AsEnumerable(),
                         };

            return result.FilterByValues(FilterHelper.GetStaticFilters(filters));
        }
        private IQueryable<Guid> GetProdStoreIdsWithDynamicFieldsFilters(List<FilterItem> filters)
        {
            var fieldsParamWithFilters = GetFieldsPerProductStore()
				.DynamicFilterByValues(FilterHelper.GetDynamicFilters(filters))
                .Select(x => x.ProductId ?? Guid.Empty).Distinct();
            return fieldsParamWithFilters;
        }
		private IQueryable<FieldPerProductViewModel> GetProdWithSortDynamicFields(List<FilterItem> filters)
		{
			var fieldName = filters.Where(x => x.Type == FilterItemEnum.DynamicSort).Select(x => x.Field).FirstOrDefault();
			var prod = fieldName == null ? from product in _productsRepository.GetAll().Include(x => x.ProductStore)
										   join ps in _productStoreRepository.GetAll() on product.Id equals ps.ProductId
										   select
										   new FieldPerProductViewModel
										   {
											   ProductId = ps.Id
										   } : from product in _productsRepository.GetAll().Include(x => x.ProductStore)
											   join ps in _productStoreRepository.GetAll() on product.Id equals ps.ProductId
											   join fields in GetFieldsPerProductStore().Where(x => fieldName == null || x.FieldName == fieldName) on ps.Id equals fields.ProductId
											   into data
											   from m in data.DefaultIfEmpty()
											   select
											   new FieldPerProductViewModel
											   {
												   FieldId = m.FieldId,
												   FieldName = m.FieldName,
												   ProductId = ps.Id,
												   Value = m.Value
											   };

			return prod;
		}
		private IQueryable<FieldPerProductViewModel> GetFieldsPerProductStore(Guid? id = null)
		{

			var allFields = _fieldRepository.FindBy(x => x.IsStore);
			return from fields in allFields
				   join fieldProductStores in allFields.SelectMany(x => x.FieldProductStore)
						 .Where(x => id == null || x.ProductStoresId == id) on fields.Id equals fieldProductStores.FieldId
				   into data
				   from m in data.DefaultIfEmpty()
				   select
				   new FieldPerProductViewModel
				   {
					   FieldId = fields.Id,
					   FieldName = fields.Name,
                       ProductId = m.ProductStoresId,
					   Value = m.Value,
					   Order = fields.Order,
					   DomGroupDesc = fields.DOMGroups.Description,
					   DomValues = fields.DOMGroups.DOMValues.Select(x => x.ToSelectListItem()).ToList()

				   };
		}
		private IQueryable<Guid> GetCollectionsWithFilters(List<FilterItem> filters)
		{
            return (from collection in _collectionRepository.GetAll()
                    join ps in _productStoreRepository.GetAll() on new { Sku = collection.Sku, StoreCode = collection.StoreCode } equals new { Sku = ps.Product.Sku, StoreCode = ps.Store.StoreCode }
                    join channel in _channelRepository.GetAll() on collection.ChannelId equals channel.Id
                    select
                    new CollectionViewModel
                    {
                        ProductStoreId = ps.Id,
                        ChannelId = channel.Id,
                        Sku = collection.Sku,
                        StoreCode = collection.StoreCode,
                        ChannelName = channel.ChannelName,
                        Value = collection.Value
                    }).DynamicFilterByValues(FilterHelper.GetCollectionsFilters(filters)).Select(x=>x.ProductStoreId);
		}
		private async Task<PagedResult<FieldProductStoreViewModel>> GetFieldProductStoresPagedList(int page, List<FilterItem> filters)
        {
            var pagedResult = await GetFieldProductListQueryable(filters)
                                     .Sort(FilterHelper.GetSortFilters(filters))
                                         .ApplyPagedListAsync(page, 13);

            return new PagedResult<FieldProductStoreViewModel>
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

        public async Task<PagedResult<FieldProductStoreViewModel>> GetPagedListAsync(int page, List<FilterItem> filterItems)
        {
            var result = await GetFieldProductStoresPagedList(page, filterItems);
            return result;
        }

        public async Task<FieldProductStoreViewModel?> GetByProductStoreIdAsync(Guid? id)
        {
            var fieldProduct = await GetFieldProductListQueryable(new List<FilterItem>() { }).Where(x => x.ProductStoreId == id).FirstOrDefaultAsync();
            return fieldProduct;
        }

        public Task<List<FieldProductStoreViewModel>> GetListAsync(Expression<Func<FieldProductStoreViewModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<List<FieldProductStoreViewModel>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<FieldProductStoreViewModel>> GetPagedListAsync(int page)
        {
            //var pagedResult = await GetFieldProductListQueryable(DateTime.MinValue).ApplyPagedListAsync(page, 35);

            //return new PagedResult<FieldProductStoreViewModel>
            //{
            //    Results =
            //    pagedResult.Results.AsNoTracking()
            //    .ToList(),
            //    PageCount = pagedResult.PageCount,
            //    PageSize = pagedResult.PageSize,
            //    CurrentPage = pagedResult.CurrentPage,
            //    EndPage = pagedResult.EndPage,
            //    StartPage = pagedResult.StartPage,
            //    RowCount = pagedResult.RowCount

            //};
            throw new NotImplementedException();
        }

        public Task<List<SelectListItem>> GetSelectListItems()
        {
            throw new NotImplementedException();
        }

        public IQueryable<FieldProductStoreViewModel> GetIQueryableList(DateTime filter)
        {
            var filters = new List<FilterItem>() { new FilterItem() { Field = nameof(FieldProductStoreViewModel.ModifiedOn), Type = FilterItemEnum.Static, Value = new string[] { filter.ToString("yyyy-MM-dd") } } };
            var listOfProducts = GetFieldProductListQueryable(filters);
            return listOfProducts;
        }
    }
}
