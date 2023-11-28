using IntegrationService.IServices.ICommand;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Imports;
using IntegrationService.Models.Stores;
using IntegrationService.Models.User;

namespace IntegrationService.ConcreteServices.Commands
{
    public class StoreCommand : IStoreCommand
    {
        private readonly IStoreRepository _storeRepository;
        public StoreCommand(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }
        public Task<int> Insert(Store item, ApplicationUser user, List<Guid>? items)
        {
            throw new NotImplementedException();
        }

        public void InsertFromImport(List<Import> collection)
        {
            var storeCodes = collection.GroupBy(x=>x.StoreCode).Select(g=>g.First()).Select(x=>x.StoreCode);
            foreach (var storeCode in storeCodes)
            {
                if (!_storeRepository.GetAll().Any(x => x.StoreCode == storeCode))
                {
                    var store = Store.Create(string.Empty, storeCode, string.Empty);
                    _storeRepository.Add(store);
                    _storeRepository.Save();
                }
            }
        }

        public Task<int> InsertRange(IEnumerable<Store> items, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(Guid id, Store item, ApplicationUser user, List<Guid>? items)
        {
            throw new NotImplementedException();
        }
    }
}
