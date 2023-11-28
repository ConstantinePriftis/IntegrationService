using IntegrationService.ConcreteServices.Helper;
using IntegrationService.IServices.IQueries;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models;
using IntegrationService.ViewModels;
using IntegrationService.ViewModels.Nutrition;
using IntegrationService.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IntegrationService.ConcreteServices.Queries
{
	public class NutritionQuery : INutritionQuery
	{
		private readonly INutritionRepository _nutritionRepository;
		private readonly IProductsRepository _productsRepository;
		private readonly IFieldRepository _fieldRepository;
		private readonly IExportRequestRepository _exportRequestRepository;

        public NutritionQuery(
            INutritionRepository nutritionRepository,
            IProductsRepository productsRepository,
            IFieldRepository fieldRepository,
			IExportRequestRepository exportRequestRepository)
        {
            _nutritionRepository = nutritionRepository;
            _productsRepository = productsRepository;
            _fieldRepository = fieldRepository;
            _exportRequestRepository = exportRequestRepository;
        }
        private IQueryable<NutritionViewModel> GetNutritionProductQueryable(List<FilterItem> filters)
        {
            var allFields = _fieldRepository.FindBy(x => !x.IsStore);

            var result = (from prod in _productsRepository.GetAll()
                          join nut in _nutritionRepository.GetAll() on prod.Id equals nut.ProductId
                          select new NutritionViewModel
                          {
                              Id = prod.Id.ToString(),
                              ProductId = prod.Id,
                              Sku = prod.Sku,
                              Title = (from fields in allFields
                                       join fieldProducts in _fieldRepository.GetAll().SelectMany(x => x.FieldProducts).Where(x => x.ProductsId == prod.Id) on fields.Id equals fieldProducts.FieldId
                                       where fields.Name == nameof(NutritionViewModel.Title)
                                       select fieldProducts.Value
                                      ).FirstOrDefault() ?? string.Empty
                          }).Distinct();

            return result.FilterByValues(FilterHelper.GetStaticFilters(filters));
        }
        private IQueryable<NutritionViewModel> GetExportQueryable(DateTime datetime)
        {
			var dateFilter = (datetime.Date == default
				? (_exportRequestRepository.GetAll().Select(x => x.ModifiedOn).DefaultIfEmpty().Max())
				: datetime.Date);
			var filters = new List<FilterItem>() { new FilterItem() { Field = nameof(NutritionViewModel.ModifiedOn), Type = FilterItemEnum.Static, Value = new string[] { dateFilter.ToString("yyyy-MM-dd") } } };
            
            var result = (from nut in _nutritionRepository.GetAll()
                          join nutSku in (from sk in (from nutrition in _nutritionRepository.GetAll()
                                                      select new { Sku = nutrition.Sku, ModifiedOn = nutrition.ModifiedOn })
                                                      .FilterByValues(FilterHelper.GetStaticFilters(filters)
                                                      )
                                          select sk.Sku).Distinct() on nut.Sku equals nutSku
                          select new NutritionViewModel
                          {
                              Sku = nut.Sku,
                              Title = nut.Title ?? string.Empty,
                              Cell1 = nut.FirstCell ?? string.Empty,
                              Cell2 = nut.SecondCell ?? string.Empty,
                              Cell3 = nut.ThirdCell ?? string.Empty,
                              Cell4 = nut.FourthCell ?? string.Empty,
                              IsHighlight = nut.IsHighlight,
                              Order = nut.Order,
                              IsBold = nut.IsBold,
                              Note = nut.Note ?? string.Empty,
                          });
            
            return result;
        }
        private Task<IQueryable<NutritionViewModel>> GetNutritionListQueryable(Expression<Func<NutritionViewModel, bool>> predicate)
        {

            var query = (from nut in _nutritionRepository.GetAll()
                         join prod in _productsRepository.GetAll() on nut.ProductId equals prod.Id
                         into prodNuts
                         from prod in prodNuts
                             //group nut.Sku by prod.Sku into SkuGroup
                         select new NutritionViewModel()
                         {
                             Id = nut.Id.ToString(),
                             Sku = nut.Sku,
                             Title = nut.Title ?? string.Empty,
                             Cell1 = nut.FirstCell ?? string.Empty,
                             Cell2 = nut.SecondCell ?? string.Empty,
                             Cell3 = nut.ThirdCell ?? string.Empty,
                             Cell4 = nut.FourthCell ?? string.Empty,
                             IsHighlight = nut.IsHighlight,
                             Order = nut.Order,
                             IsBold = nut.IsBold,
                             Note = nut.Note ?? string.Empty,
                             ProductId = prod.Id
                         }); ;
            return Task.FromResult(query.Where(predicate).OrderBy(x => x.Order).AsQueryable());
        }
        private Task<IQueryable<NutritionViewModel>> GetNutritionListQueryable()
        {

			var query = (from nut in _nutritionRepository.GetAll()
						 join prod in _productsRepository.GetAll() on nut.ProductId equals prod.Id
						 into prodNuts
						 from prod in prodNuts
							 //group nut.Sku by prod.Sku into SkuGroup
						 select new NutritionViewModel()
						 {
							 Id = nut.Id.ToString(),
							 Sku = nut.Sku,
							 Title = nut.Title ?? string.Empty,
							 Cell1 = nut.FirstCell ?? string.Empty,
							 Cell2 = nut.SecondCell ?? string.Empty,
							 Cell3 = nut.ThirdCell ?? string.Empty,
							 Cell4 = nut.FourthCell ?? string.Empty,
							 IsHighlight = nut.IsHighlight,
							 Order = nut.Order,
							 IsBold = nut.IsBold,
							 Note = nut.Note ?? string.Empty,
							 ProductId = prod.Id
						 });
			return Task.FromResult(query.AsQueryable());
		}

        public async Task<PagedResult<NutritionViewModel>> GetPagedListAsync(int page, List<FilterItem> filterItems)
        {
            var pagedResult = await GetNutritionProductQueryable(filterItems)
                                    .Sort(FilterHelper.GetSortFilters(filterItems))
                                         .ApplyPagedListAsync(page, 13);

			return new PagedResult<NutritionViewModel>
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
        public IQueryable<NutritionViewModel> GetQueryableExport(DateTime datetime)
        {
            return GetExportQueryable(datetime);
        }
        public async Task<List<NutritionViewModel>> GetListAsync(Expression<Func<NutritionViewModel, bool>> predicate)
        {
            var listOfResults = await GetNutritionListQueryable(predicate);
            return listOfResults.ToList();
        }

        public async Task<List<NutritionViewModel>> GetListAsync()
        {
            var listOfResults = await GetNutritionListQueryable();
            return listOfResults.ToList();
        }

        public async Task<IEnumerable<NutritionViewModel>?> GetBySku(string? sku)
        {
            var listOfResults = await GetNutritionListQueryable(x => x.Sku == sku);
            return listOfResults.AsEnumerable();
        }
        public async Task<IQueryable<NutritionViewModel>> GetQueryableNutritions()
        {
            var result = await GetNutritionListQueryable();
            return result;
        }
        public Task<NutritionViewModel?> GetByProductIdAsync(Guid? id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<NutritionViewModel> GetIQueryableList()
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<NutritionViewModel>> GetPagedListAsync(int page)
        {
            throw new NotImplementedException();
        }
        public IQueryable<SelectListItem> GetSelectListProductsWithoutNutritionQueryable(string? term, List<FilterItem> filterItems)
        {
            return (from prod in _productsRepository.GetAll()
                    where !(from nutrition in _nutritionRepository.GetAll()
                                  select nutrition.ProductId).Contains(prod.Id)
                    select new NutritionViewModel
                    {
                        Id = prod.Id.ToString(),
                        ProductId = prod.Id,
                        Sku = prod.Sku
                    }).ApplyTermFilter(FilterHelper.GetFiltersByKey(nameof(ProductsViewModel.Sku), filterItems))
                .Where(x => x.Sku.Contains(term)).Select(x => x.ToSelectListItem());
        }
        public Task<List<SelectListItem>> GetSelectListItems()
        {
            throw new NotImplementedException();
        }

		public Task<IQueryable<NutritionViewModel>> ExportUpdates(DateTime filter)
		{
			throw new NotImplementedException();
		}
	}
}
