using IntegrationService.ViewModels.FieldViewModels;
using IntegrationService.ViewModels.StoreViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Xml.Linq;

namespace IntegrationService.ViewModels.ProductViewModels
{
    public class ProductsViewModel
    {
        public Guid Id { get; set; }
        public string Sku { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Description_Category { get; set; }
        public string Status { get; set; }
        public string Step { get; set; }
        public bool IsCompleted { get; set; }
        public List<FieldViewModel> Fields { get; set; }
        public List<StoreViewModel> Stores { get; set; }

        public SelectListItem ToSelectListItem()
        {
            return new SelectListItem(Id.ToString(), Sku);
        }
    }
}
