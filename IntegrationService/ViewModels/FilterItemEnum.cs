using IntegrationService.Models.Stores;
using IntegrationService.Models;

namespace IntegrationService.ViewModels
{
	public class FilterItemEnum : Enumeration<FilterItemEnum, string>
	{
		public static readonly string Static = "10";
		public static readonly string Dynamic = "20";
		public static readonly string StaticSort = "30";
        public static readonly string DynamicCollections = "40";
        public static readonly string DynamicSort = "50";
	}
}
