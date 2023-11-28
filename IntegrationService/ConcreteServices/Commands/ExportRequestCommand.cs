using IntegrationService.IServices.ICommand;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Exports;
using IntegrationService.Models.User;

namespace IntegrationService.ConcreteServices.Commands
{
    public class ExportRequestCommand : IExportRequestCommand
    {
        private readonly IExportRequestRepository _exportRequestRepository;

        public ExportRequestCommand(IExportRequestRepository exportRequestRepository)
        {
            _exportRequestRepository = exportRequestRepository;
        }

        public void Add(ApplicationUser applicationUser)
        {
            _exportRequestRepository.Add(ExportRequest.Create(applicationUser));
            _exportRequestRepository.Save();
        }

        public Task<int> Insert(ExportRequest item, ApplicationUser user, List<Guid>? items)
        {
            _exportRequestRepository.Add(item);
            return Task.FromResult(_exportRequestRepository.Save());
        }

        public Task<int> InsertRange(IEnumerable<ExportRequest> items, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(Guid id, ExportRequest item, ApplicationUser user, List<Guid>? items)
        {
            throw new NotImplementedException();
        }
    }
}
