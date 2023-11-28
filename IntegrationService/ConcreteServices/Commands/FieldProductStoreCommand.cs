using IntegrationService.IServices.ICommand;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Fields;
using IntegrationService.Models.User;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Microsoft.EntityFrameworkCore;

namespace IntegrationService.ConcreteServices.Commands
{
    public class FieldProductStoreCommand : IFieldProductStoreCommand
    {
        private readonly IFieldProductStoreRepository _fieldProductStoreRepository;
        private readonly IProductStoreRepository _productStoreRepository;
        public FieldProductStoreCommand(IFieldProductStoreRepository fieldProductStoreRepository, IProductStoreRepository productStoreRepository)
        {
            _fieldProductStoreRepository = fieldProductStoreRepository;
            _productStoreRepository = productStoreRepository;

        }
        
        public async Task<int> InsertRange(IEnumerable<FieldProductStore> items,bool isPublished, ApplicationUser user)
        {
            var fieldProductStores = new List<FieldProductStore>();
            foreach (var item in items)
            {
                var fieldProduct = await _fieldProductStoreRepository.FindBy(x => x.ProductStoresId == item.ProductStoresId && x.FieldId == item.FieldId).FirstOrDefaultAsync();
                var productStore = await _productStoreRepository.FindBy(x => x.Id == item.ProductStoresId).FirstOrDefaultAsync();
                if (fieldProduct != null)
                {
                    fieldProduct.UpdateValue(item.Value);
                }
                else
                {
                    fieldProductStores.Add(FieldProductStore.Create(item.FieldId, item.ProductStoresId, item.Value));
                }

                if (productStore != null)
                {
                    productStore.UpdateModified();
                    if (isPublished)
                    {
                        productStore.EnableIsPublished();
                    }
                    else
                    {
                        productStore.DisableIsPublished();
                    }
                }
            }
            _fieldProductStoreRepository.AddRange(fieldProductStores);
            return _fieldProductStoreRepository.Save();
        }

        public Task<int> Insert(FieldProductStore item, ApplicationUser user, List<Guid>? items)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertRange(IEnumerable<FieldProductStore> items, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(Guid id, FieldProductStore item, ApplicationUser user, List<Guid>? items)
        {
            throw new NotImplementedException();
        }
    }
}
