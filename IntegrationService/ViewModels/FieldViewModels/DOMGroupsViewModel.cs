namespace IntegrationService.ViewModels.FieldViewModels
{
    public class DOMGroupsViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public List<FieldViewModel> Fields { get; set; }
        public List<DOMValuesViewModel> DOMValues { get; set; }
    }
}
