using IntegrationService.Models.EntityBases;
using IntegrationService.Models.Fields;
using IntegrationService.Models.Stores;
using IntegrationService.ViewModels.ChannelViewModels;

namespace IntegrationService.Models.Product
{
    public class Products : SimpleBaseEntity
    {
        public Products()
        {

        }
        private Products(string sku, string title, string description, string description_category, string status, string step, List<Store> stores)
        {
            Sku = sku;
            Status = status;    
            Step = step;
            Title = title;
            Description = description;
            Description_Category = description_category;
            UpdateProductStores(stores);
            UpdateEnabled();
            CreatedOn = DateTime.UtcNow;
            ModifiedOn = DateTime.UtcNow;
        }
        public string Sku { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Description_Category { get; set; }
        public string Status { get; set; }
        public string Step { get; set; }

        private List<FieldProducts> _fieldProducts = new();
        public virtual IReadOnlyList<FieldProducts> FieldProducts => _fieldProducts.ToList();
        private List<ProductStores> _productStores = new();
        public virtual IReadOnlyList<ProductStores> ProductStore => _productStores ?? new List<ProductStores>();
        public void UpdateProductStores(List<Store> stores)
        {
            //_productStores.Where(x => stores.Any(y => y.Id.Equals(x.StoreId))).ToList().ForEach(x => x.Update(stores.FirstOrDefault(y => y.Id == x.StoreId)));
            if (stores != null && stores.Count > 0 )
            {
                var newStores = stores.Where(x => !_productStores.Any(y => y.StoreId.Equals(x.Id))).Select(x => ProductStores.Create(Id, x)).ToList();
                _productStores.AddRange(newStores);
            }
        }
        
        public void Update(string sku, string title, string description, string status, string step, List<Store> stores)
        {
            Sku = sku;
            Title = title;
            Description = description;
            Status = status;
            Step = step;
            UpdateProductStores(stores);
            UpdateModified();
            UpdateEnabled();
        }
        public void UpdateModified()
        {
            ModifiedOn = DateTime.UtcNow;
        }
        public void UpdateEnabled()
        {
            Enabled = false;
        }
        public void Completed()
        {
            Enabled = true;
        }
        public void UpdateDescriptionCategory(string? description_category)
        {
            Description_Category = description_category;
        }
        public static Products Create(string sku, string title, string description, string description_category, string status, string step, List<Store> stores)
        {
            return new(sku, title, description, description_category, status, step, stores);
        }
        public ChannelViewModel ToDto()
        {
            return new ChannelViewModel
            {
                Id = Id,
                Name = Sku,
                Description = Title
            };
        }
    }
}
