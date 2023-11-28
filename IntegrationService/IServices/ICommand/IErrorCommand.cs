using IntegrationService.Models.Errors;

namespace IntegrationService.IServices.ICommand
{
    public interface IErrorCommand : ICommandAsync<Error>
    {
    }
}
