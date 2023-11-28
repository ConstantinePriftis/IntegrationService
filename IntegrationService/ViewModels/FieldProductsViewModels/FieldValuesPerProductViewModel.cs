namespace IntegrationService.ViewModels.FieldProductsViewModels
{
    public class FieldValuesPerProductViewModel
    {
        public Guid ProductId { get; set; }
        public string Sku { get; set; }
        public string Title { get; set; }
        public IEnumerable<FieldValuesPerProductSelectedListViewModel> FieldsSelectLists { get; set; }
    }
}
