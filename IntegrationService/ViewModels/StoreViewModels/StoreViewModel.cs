using Microsoft.AspNetCore.Mvc.Rendering;

namespace IntegrationService.ViewModels.StoreViewModels
{
    public class StoreViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string WarehouseID { get; set; }
        public string Type { get; set; }

        public SelectListItem ToSelectListItem()
        {
            return new SelectListItem(Id.ToString(), WarehouseID);
        }
    }
}
