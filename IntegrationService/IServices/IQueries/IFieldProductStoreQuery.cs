using IntegrationService.IServices.IQuery;
using IntegrationService.ViewModels.FieldProductStores;
using IntegrationService.ViewModels.FieldProductsViewModels;
using IntegrationService.ViewModels.FieldViewModels;

namespace IntegrationService.IServices.IQueries
{
    public interface IFieldProductStoreQuery : IQuery<FieldProductStoreViewModel>
    {
        Task<FieldProductStoreViewModel?> GetByProductStoreIdAsync(Guid? id);
        IQueryable<FieldProductStoreViewModel> GetIQueryableList(DateTime filter);
    }
}
