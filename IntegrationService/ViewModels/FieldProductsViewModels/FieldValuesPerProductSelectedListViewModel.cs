using Microsoft.AspNetCore.Mvc.Rendering;

namespace IntegrationService.ViewModels.FieldProductsViewModels
{
    public class FieldValuesPerProductSelectedListViewModel
    {
        public Guid FieldId { get; set; }
        public IEnumerable<SelectListItem> PredefinedValues { get; set; }
        public string SelectedValue { get; set; }

    }
}
