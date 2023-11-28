using IntegrationService.Models.Product;
using IntegrationService.Models.Stores;
using IntegrationService.ViewModels;

namespace IntegrationService.IServices.ICommand
{
    public interface IProductCharacteristicCommand : ICommand<ProductCharacteristic>
    {
        int EvaluateDifferences();
    }
}
