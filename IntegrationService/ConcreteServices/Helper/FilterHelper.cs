using IntegrationService.Models.Collection;
using IntegrationService.ViewModels;
using IntegrationService.ViewModels.FieldProductsViewModels;

namespace IntegrationService.ConcreteServices.Helper
{
    public static class FilterHelper
    {
        public static Dictionary<string, object> GetStaticFilters(List<FilterItem> filters)
        {
            var staticFilters = filters.Where(x => x.Type.Equals(FilterItemEnum.Static)).ToDictionary(x => x.Field, x => (object)x.Value);
            return staticFilters;
        }
        public static Dictionary<string, object> GetDynamicFilters(List<FilterItem> filters)
        {
            var dynamicFilters = filters.Where(x => x.Type.Equals(FilterItemEnum.Dynamic)).ToDictionary(x => x.Field, x => (object)new Dictionary<string, object> { { nameof(FieldPerProductViewModel.FieldId), new Guid(x.Id) }, { nameof(FieldPerProductViewModel.Value), x.Value } });
            return dynamicFilters;
        }
		public static Dictionary<string, object> GetCollectionsFilters(List<FilterItem> filters)
		{
            var dynamicFilters = filters.Where(x => x.Type.Equals(FilterItemEnum.DynamicCollections)).ToDictionary(x => x.Field, x => (object)new Dictionary<string, object> { { nameof(CollectionViewModel.ChannelName), x.Field }, { nameof(CollectionViewModel.Value), x.Value.FirstOrDefault() == nameof(CollectionValueEnumeration.IN) ? new string[] { CollectionValueEnumeration.IN } : x.Value.FirstOrDefault() == nameof(CollectionValueEnumeration.OUT) ? new string[] { CollectionValueEnumeration.OUT } : new string[] { " " } } });
			return dynamicFilters;
		}
		public static Dictionary<string, object> GetSortFilters(List<FilterItem> filters)
        {
            var sortFilters = filters.Where(x => x.Type.Equals(FilterItemEnum.StaticSort)).ToDictionary(x => x.Field, x => (object)x.Value);
            return sortFilters;
        }
        public static Dictionary<string, object> GetSortDynamicFilters(List<FilterItem> filters)
        {
            var sortDynamicFiltes = filters.Where(x => x.Type.Equals(FilterItemEnum.DynamicSort)).ToDictionary(x => nameof(x.Value), x => (object)x.Value);
            return sortDynamicFiltes;
        }
        public static Dictionary<string, object> GetFiltersByKey(string key, List<FilterItem> filters)
		{
			var filtersWithKey = filters.Where(x => x.Field.Equals(key) && x.Type.Equals(FilterItemEnum.Static)).ToDictionary(x => x.Field, x => (object)x.Value);
			return filtersWithKey;
		}
	}
}
