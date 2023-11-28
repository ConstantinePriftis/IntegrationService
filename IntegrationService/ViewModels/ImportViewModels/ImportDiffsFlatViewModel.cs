namespace IntegrationService.ViewModels.ImportViewModels
{
    public class ImportDiffsFlatViewModel
    {
        public string StoreCode { get; set; }
        public string Sku { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Step { get; set; }
        public string Status { get; set; }
        public List<string> StoreCodes { get; set; }
    }
}
