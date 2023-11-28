using IntegrationService.Models.EntityBases;
using IntegrationService.Models.Product;
using IntegrationService.Models.Stores;

namespace IntegrationService.Models.Fields
{
    //Παραμετρικά πεδία ανά προιόν
    public class FieldProducts : SimpleBaseEntity
    {
        public FieldProducts()
        {
            
        }
        public FieldProducts(Guid fieldId, Guid productsId, string value)
        {
            FieldId = fieldId;
            ProductsId = productsId;
            Value = value;
            CreatedOn = DateTime.UtcNow;
            ModifiedOn = DateTime.UtcNow;
        }

        public Guid FieldId { get;  set; }
        public Guid ProductsId { get;  set; }

        public virtual Product.Products Products { get; set; }
        public virtual Field Field { get; set; }

        public string Value { get; set; }

        public static FieldProducts Create(Guid fieldId, Guid productsId, string value)
        {
            return new(fieldId, productsId, value);
        }

        public void UpdateValue(string value)
        {
            Value = value;
            ModifiedOn = DateTime.UtcNow;
        }
    }
}
