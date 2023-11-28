using IntegrationService.Models.EntityBases;
using IntegrationService.Models.Stores;

namespace IntegrationService.Models.Product
{
    public class ProductStores : SimpleBaseEntity
    {
        public Guid ProductId { get; set; }
        public virtual Products Product { get; set; }
        public Guid StoreId { get; set; }
        public virtual Store Store { get; set; }

        private ProductStores(Guid productId, Guid storeId, string storeSaleSystem)
        {
            ProductId = productId;
            StoreId = storeId;
            CreatedOn = DateTime.UtcNow;
            UpdateModified();
            Enabled = !storeSaleSystem.Equals(StoreSaleSystem.R2) ?true:false;
        }
        public ProductStores()
        {

        }
        public static ProductStores Create(Guid productId, Store store)
        {
            var productStores = new ProductStores(productId, store.Id, StoreSaleSystem.ByValue(store.Type ?? string.Empty)?.Value ?? string.Empty);
            return productStores;
        }
        public void EnableIsPublished()
        {
            Enabled = true;
            UpdateModified();
        }

        public void DisableIsPublished()
        {
            Enabled = false;
            UpdateModified();
        }
        public void UpdateIsPublished()
        {
            var storeType = Store.Type;
            Enabled = !(StoreSaleSystem.ByValue(Store.Type ?? string.Empty)?.Value ?? string.Empty).Equals(StoreSaleSystem.R2) ? true : false;
        }
        public void UpdateModified()
        {
            ModifiedOn = DateTime.UtcNow;
        }
        public void Update(Store store)
        {
            Enabled = !(StoreSaleSystem.ByValue(store.Type ?? string.Empty)?.Value ?? string.Empty).Equals(StoreSaleSystem.R2) ? true : false;
        }

    }
}
