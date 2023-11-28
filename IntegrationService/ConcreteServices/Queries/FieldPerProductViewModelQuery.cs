using IntegrationService.ConcreteServices.ConcreteRepository;
using IntegrationService.IServices.IQueries;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models;
using IntegrationService.ViewModels;
using IntegrationService.ViewModels.FieldProductStores;
using IntegrationService.ViewModels.FieldProductsViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace IntegrationService.ConcreteServices.Queries
{
    public class FieldPerProductViewModelQuery : ΙFieldPerProductViewModelQuery
    {
        private readonly IFieldPerProductViewModelRepository _fieldPerProductViewModelRepository;
        private readonly IFieldRepository _fieldRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IProductCharacteristicRepository _productCharacteristicRepository;
        private readonly IFieldProductStoreQuery _fieldProductStoreQuery;

        public FieldPerProductViewModelQuery(
            IFieldPerProductViewModelRepository fieldPerProductViewModelRepository,
            IFieldRepository fieldRepository,
            IProductsRepository productsRepository,
            IProductCharacteristicRepository productCharacteristicRepository,
            IFieldProductStoreQuery fieldProductStoreQuery)
        {
            _fieldPerProductViewModelRepository = fieldPerProductViewModelRepository;
            _fieldRepository = fieldRepository;
            _productsRepository = productsRepository;
            _productCharacteristicRepository = productCharacteristicRepository;
            _fieldProductStoreQuery = fieldProductStoreQuery;
        }
        public Task<FieldPerProductViewModel?> GetByProductIdAsync(Guid? id)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<FieldPerProductViewModel>> GetIQueryableList()
        {
             return  _fieldPerProductViewModelRepository.GetAll() as IQueryable<FieldPerProductViewModel>;
        }

        public Task<List<FieldPerProductViewModel>> GetListAsync(Expression<Func<FieldPerProductViewModel, bool>> predicate)
        {
            return Task.FromResult(_fieldPerProductViewModelRepository.GetAll().ToList());
        }

        public Task<List<FieldPerProductViewModel>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<FieldPerProductViewModel>> GetPagedListAsync(int page)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<FieldPerProductViewModel>> GetPagedListAsync(int page, List<FilterItem> filterItems)
        {
            throw new NotImplementedException();
        }

        public Task<List<SelectListItem>> GetSelectListItems()
        {
            throw new NotImplementedException();
        }
       
    }
}
