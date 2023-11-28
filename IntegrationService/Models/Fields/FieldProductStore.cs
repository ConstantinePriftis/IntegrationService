using IntegrationService.Models.EntityBases;
using IntegrationService.Models.Product;

namespace IntegrationService.Models.Fields
{
    public class FieldProductStore : SimpleBaseEntity
    {
        public FieldProductStore(Guid fieldId, Guid productStoreId, string value)
        {
            FieldId = fieldId;
            ProductStoresId = productStoreId;
            Value = value;
        }
        public FieldProductStore()
        {
            
        }

        public Guid FieldId { get; private set; }
        public Guid ProductStoresId { get; set; }
        public virtual ProductStores ProductStores { get; set; }
        public virtual Field Field { get; set; }

        public string Value { get; set; }

        public static FieldProductStore Create(Guid fieldId, Guid productStoresId, string value)
        {
            return new(fieldId, productStoresId, value);
        }
        public void UpdateValue(string value)
        {
            Value = value;
            ModifiedOn = DateTime.UtcNow;
        }
    }
}
