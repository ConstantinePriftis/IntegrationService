using IntegrationService.IServices.IQuery;
using IntegrationService.Models;
using IntegrationService.ViewModels;
using IntegrationService.ViewModels.FieldProductsViewModels;

namespace IntegrationService.IServices.IQueries
{
    public interface IFieldProductQuery : IQuery<FieldProductViewModel>
    {
        Task<FieldProductViewModel?> GetByProductIdAsync(Guid? id);
        IQueryable<FieldProductViewModel> GetIQueryableList(DateTime filter);
        Task<FieldProductViewModel?> GetBySku(string sku);
    }
}
