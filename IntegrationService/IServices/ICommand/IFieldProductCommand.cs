using IntegrationService.Models.Fields;
using IntegrationService.Models.Product;
using IntegrationService.Models.User;

namespace IntegrationService.IServices.ICommand
{
    public interface IFieldProductCommand: ICommandAsync<FieldProducts>
    {
        Task<int> InsertRange(IEnumerable<FieldProducts> items, Products product, ApplicationUser user);
    }
}
