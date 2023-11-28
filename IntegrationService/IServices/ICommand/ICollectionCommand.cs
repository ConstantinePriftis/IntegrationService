using IntegrationService.Models.Collection;
using IntegrationService.Models.Product;

namespace IntegrationService.IServices.ICommand
{
    public interface ICollectionCommand : ICommandAsync<Collection>, IFromImportCommand
    {
    }
}
