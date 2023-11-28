using IntegrationService.IServices.IQuery;
using IntegrationService.ViewModels.ChannelViewModels;
using IntegrationService.ViewModels.FieldProductsViewModels;
using IntegrationService.ViewModels.FieldViewModels;

namespace IntegrationService.IServices.IQueries
{
    public interface ΙFieldPerProductViewModelQuery : IQuery<FieldPerProductViewModel>
    {
        Task<FieldPerProductViewModel?> GetByProductIdAsync(Guid? id);
        Task<IQueryable<FieldPerProductViewModel>> GetIQueryableList();
    }
}
