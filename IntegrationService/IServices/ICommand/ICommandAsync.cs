using IntegrationService.Models.Fields;
using IntegrationService.Models.User;
using System.Security.Claims;

namespace IntegrationService.IServices.ICommand
{
    public interface ICommandAsync<T1> where T1 : class, new()
    {
        Task<int> Insert(T1 item, ApplicationUser user, List<Guid>? items);
        Task<int> InsertRange(IEnumerable<T1> items, ApplicationUser user);
        Task<int> Update(Guid id, T1 item, ApplicationUser user, List<Guid>? items);
    }
}
