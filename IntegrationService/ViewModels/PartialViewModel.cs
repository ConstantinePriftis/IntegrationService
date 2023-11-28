using IntegrationService.ViewModels.CategoryViewModels;
using IntegrationService.ViewModels.FieldProductStores;
using IntegrationService.ViewModels.FieldProductsViewModels;
using IntegrationService.ViewModels.Nutrition;
using IntegrationService.ViewModels.StoreViewModels;

namespace IntegrationService.ViewModels
{
	public class PartialViewModel
	{
        public List<FieldProductViewModel> FieldProductlList { get; set; }
        public List<FieldProductStoreViewModel> FieldProductStoreList { get; set; }
        public List<NutritionViewModel> NutritionList { get; set; }
        public List<CategoryViewModel> CategoryList { get; set; }
        public List<StoreViewModel> StoreList { get; set; }

    }
}
