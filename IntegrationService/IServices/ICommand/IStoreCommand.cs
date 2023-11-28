using IntegrationService.Models.Product;
using IntegrationService.Models.Stores;

namespace IntegrationService.IServices.ICommand
{
    public interface IStoreCommand : ICommandAsync<Store>, IFromImportCommand
    {
    }
}
