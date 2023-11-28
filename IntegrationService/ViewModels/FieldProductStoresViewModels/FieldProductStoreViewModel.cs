using CsvHelper.Configuration.Attributes;
using IntegrationService.Models.Collection;
using IntegrationService.ViewModels.FieldProductsViewModels;

namespace IntegrationService.ViewModels.FieldProductStores
{
    public class FieldProductStoreViewModel : object
    {
        public Guid ProductStoreId { get; set; }
        public string Sku { get; set; }
        public string Title { get; set; }
		public string WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public DateTime ModifiedOn { get; set; }
        public IEnumerable<CollectionViewModel> Channels { get; set; }
        public bool IsPublished { get; set; }
        public IEnumerable<FieldPerProductViewModel> Fields { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
