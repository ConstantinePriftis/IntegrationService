using IntegrationService.IServices.ICommand;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Errors;
using IntegrationService.Models.User;

namespace IntegrationService.ConcreteServices.Commands
{
    public class ErrorCommand : IErrorCommand
    {
        private readonly IErrorRepository _errorRepository;

        public ErrorCommand(IErrorRepository errorRepository)
        {
            _errorRepository = errorRepository;
        }

        public Task<int> Insert(Error error, ApplicationUser user, List<Guid>? items)
        {
            try
            {
                _errorRepository.Add(error);
                return Task.FromResult(_errorRepository.Save());
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public Task<int> InsertRange(IEnumerable<Error> items, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(Guid id, Error item, ApplicationUser user, List<Guid>? items)
        {
            throw new NotImplementedException();
        }
    }
}
