using CsvHelper.Configuration.Attributes;
using IntegrationService.Models.Stores;

namespace IntegrationService.ViewModels.ImportViewModels
{
    public class ImportViewModel
    {
        [Name("STORE")]
        public string StoreCode { get; set; }
        [Name("SKU")]
        public string Sku { get; set; }
        [Name("TITLE")]
        public string Title { get; set; }

        [Name("DESCRIPTION")]
        public string Description { get; set; }
        [Name("STATUS")]
        public string Status { get; set; }
        [Name("TYPE")]
        public string Type { get; set; }
        [Name("STEP")]
        public string Step { get; set; }
        [Name("EFOOD")]
        public string Efood { get; set; }
        [Name("DIGITAL")]
        public string Digital { get; set; }

    }
}
