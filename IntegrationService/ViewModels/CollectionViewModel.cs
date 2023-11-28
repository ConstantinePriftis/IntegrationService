namespace IntegrationService.ViewModels
{
    public class CollectionViewModel
    {
        public Guid ProductStoreId { get; set; }
        public Guid ProductId { get; set; }
        public Guid StoreId { get; set; }
        public string Sku { get; set; }
        public string StoreCode { get; set; }
        public Guid? ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string Value { get; set; }
    }
}
