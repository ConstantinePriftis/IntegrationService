using IntegrationService.Models.EntityBases;
using IntegrationService.Models.Product;
using IntegrationService.ViewModels.StoreViewModels;

namespace IntegrationService.Models.Stores
{
    public class Store : SimpleBaseEntity
    {
        public Store()
        {
            
        }
        private Store(string name, string storeCode, string type)
        {
            UpdateName(name);
            UpdateSalesSystem(type);
            StoreCode = storeCode;
            CreatedOn = DateTime.UtcNow;
            UpdateModifiedOn();
        }
        public string? Type { get; set; }
        public string Name { get; set; }
        public string StoreCode { get; set; }
        public bool ComesFromReflexes { get; set; }
        private List<ProductCharacteristicStores> _productCharacteristicStores = new();
        public virtual IReadOnlyList<ProductCharacteristicStores> ProductCharacteristicStores => _productCharacteristicStores.ToList() ?? new List<ProductCharacteristicStores>();
        private List<ProductStores> _productStores = new();
        public virtual IReadOnlyList<ProductStores> ProductStores => _productStores.ToList() ?? new List<ProductStores>();
        private void UpdateName(string name)
        {
            //if (string.IsNullOrWhiteSpace(name?.Trim())) throw new ArgumentNullException(nameof(name));
            //name = name.Trim();
            if (Name != name)
                Name = name ?? string.Empty;
        }
        private void UpdateComesFromReflexes(bool comesFromReflexes)
        {
            ComesFromReflexes = comesFromReflexes;
        }

        private void UpdateSalesSystem(string type)
        {
            //Type = StoreSaleSystem.IsDefinedValue(type)?type:throw new ArgumentException($"{nameof(StoreSaleSystem)} undefined sale system");
            var saleSystem = StoreSaleSystem.ByValue(type??string.Empty)?.Value ?? string.Empty;
            if (!saleSystem.Equals(StoreSaleSystem.R2))
            {
                UpdateComesFromReflexes(true);
            }
            else 
            {
                UpdateComesFromReflexes(false);
            }
        }
        private void UpdateModifiedOn()
        {
            ModifiedOn = DateTime.UtcNow;
        }
        public static Store Create(string name, string storeCode,string type)
        {
            return new(name, storeCode, type);
        }
        public StoreViewModel ToDto()
        {
            return new StoreViewModel()
            {
                Id = Id,
                Name = Name,
                WarehouseID = StoreCode
            };
        }
    }
}
