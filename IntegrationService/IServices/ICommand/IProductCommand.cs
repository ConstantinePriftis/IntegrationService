

using IntegrationService.Models.Imports;
using IntegrationService.Models.Product;
using IntegrationService.Models.User;

namespace IntegrationService.IServices.ICommand
{
    public interface IProductCommand : ICommandAsync<Products>,IFromImportCommand
    {
        
    }
}
