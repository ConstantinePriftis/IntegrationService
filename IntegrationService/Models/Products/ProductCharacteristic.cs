using IntegrationService.Models.EntityBases;
using IntegrationService.Models.Stores;

namespace IntegrationService.Models.Product
{
    public class ProductCharacteristic : SimpleBaseEntity
    {
        public string Sku { get; set; }
        public string ProductDescription { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Step { get; set; }
        public string Status { get; set; }
        public bool IsChanged { get; set; }

        private List<ProductCharacteristicStores> _productCharacteristicStore = new();
        public virtual IReadOnlyList<ProductCharacteristicStores> ProductCharacteristicStore => _productCharacteristicStore.ToList() ?? new List<ProductCharacteristicStores>();
        private ProductCharacteristic(string sku, string productDescription, string description, string type, string step, string status, bool isChanged, List<Store> stores
            )
        {
            Description = description;
            UpdateSku(sku);
            UpdateProductDescription(productDescription);
            UpdateType(type);
            UpdateStep(step);
            UpdateStatus(status);
            UpdateChangedStatus(isChanged);
            UpdateProductCharacteristicStores(stores);
        }
        public ProductCharacteristic()
        {

        }
        public void UpdateProductCharacteristicStores(List<Store> stores)
        {
            if (stores != null)
            {
                _productCharacteristicStore = stores.Select(x => ProductCharacteristicStores.Create(Id, x)).ToList() ?? new();
            }
        }
        private void UpdateSku(string sku)
        {
            Sku = sku;
        }
        private void UpdateProductDescription(string prodDescription)
        {
            ProductDescription = prodDescription;
        }
        private void UpdateType(string type)
        {
            Type = type;
        }
        private void UpdateStep(string step)
        {
            Step = step;
        }
        private void UpdateStatus(string status)
        {
            Status = status;
        }
        private void UpdateChangedStatus(bool isChanged)
        {
            IsChanged = isChanged;
        }
        public static ProductCharacteristic Create(
            string sku,
            string productDescription,
            string description,
            string type,
            string step,
            string status,
            bool isChanged,
            List<Store> stores)
        {
            return new ProductCharacteristic(sku, productDescription, description, type, step, status, isChanged, stores);
        }

        public void Update(string sku, string productDescription, string description, string type, string step, string status, List<Store> stores)
        {
            Description = description;
            UpdateSku(sku);
            UpdateProductDescription(productDescription);
            UpdateType(type);
            UpdateStep(step);
            UpdateStatus(status);
            UpdateChangedStatus(false);
            UpdateProductCharacteristicStores(stores);
            ModifiedOn = DateTime.Now;
        }
    }
}
