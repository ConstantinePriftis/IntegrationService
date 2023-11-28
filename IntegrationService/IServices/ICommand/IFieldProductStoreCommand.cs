using IntegrationService.Models.Fields;
using IntegrationService.Models.User;

namespace IntegrationService.IServices.ICommand
{
    public interface IFieldProductStoreCommand: ICommandAsync<FieldProductStore>
    {
        Task<int> InsertRange(IEnumerable<FieldProductStore> items, bool isPublished, ApplicationUser user);
    }
}
