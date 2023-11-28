using IntegrationService.Models.EntityBases;
using IntegrationService.Models.Stores;

namespace IntegrationService.Models.Product
{
    public class ProductCharacteristicStores : SimpleBaseEntity
    {
        public Guid ProductCharacteristicId { get; set; }
        public virtual ProductCharacteristic ProductChars { get; set; }
        public Guid StoreId { get; set; }
        public virtual Store Store { get; set; }

        private ProductCharacteristicStores(Guid productCharacteristicId, Guid storeId)
        {
            ProductCharacteristicId = productCharacteristicId;
            StoreId = storeId;
            CreatedOn = DateTime.UtcNow;
            ModifiedOn = DateTime.UtcNow;
        }
        public ProductCharacteristicStores()
        {

        }
        public static ProductCharacteristicStores Create(Guid productCharacteristicId, Store store)
        {
            var productStores = new ProductCharacteristicStores(productCharacteristicId, store.Id);
            return productStores;
        }
        public void Update(ProductCharacteristic productCharacteristic, Store store)
        {
            Store = store;
            ProductChars = productCharacteristic;
            ModifiedOn = DateTime.UtcNow;
            ProductCharacteristicId = productCharacteristic.Id;
            StoreId = store.Id;
        }

    }
}
