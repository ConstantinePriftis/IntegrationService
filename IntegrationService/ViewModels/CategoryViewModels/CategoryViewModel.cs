using Microsoft.AspNetCore.Mvc.Rendering;

namespace IntegrationService.ViewModels.CategoryViewModels
{

    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Level { get; set; }
        public string Description { get; set; }

        public SelectListItem ToSelectListItem()
        {
            return new SelectListItem()
            {
                Text = Description,
                Value = Code
            };
        }
    }
}
