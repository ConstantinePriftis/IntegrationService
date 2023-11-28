using IntegrationService.Models.Exports;
using IntegrationService.Models.User;

namespace IntegrationService.IServices.ICommand
{
    public interface IExportRequestCommand : ICommandAsync<ExportRequest>
    {
        void Add(ApplicationUser applicationUser);
    }
}
