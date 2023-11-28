using CsvHelper.Configuration.Attributes;
using IntegrationService.Models.Product;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IntegrationService.ViewModels.Nutrition
{
	public class NutritionViewModel : object
	{
		[Name("Id")]
		public string Id { get; set; }
		[Name("Sku")]
		public string Sku { get; set; }
		[Name("Title")]
		public string Title { get; set; }

		[Name("Cell1")]
		public string Cell1 { get; set; }
		[Name("Cell2")]
		public string Cell2 { get; set; }
		[Name("Cell3")]
		public string Cell3 { get; set; }
		[Name("Cell4")]
		public string Cell4 { get; set; }
		[Name("IsHighlight")]
		public bool? IsHighlight { get; set; }
		[Name("Order")]

		public int Order { get; set; }
		[Name("IsBold")]

		public bool? IsBold { get; set; }
		[Name("Note")]

		public string Note { get; set; }
		public Guid ProductId { get; set; }
		public DateTime ModifiedOn { get; set; }
		public DateTime CreatedOn { get; set; }

		public List<SelectListItem> ListItems { get; set; }
		public SelectListItem ToSelectListItem()
		{
			return new SelectListItem()
			{
				Text = Sku,
				Value = Sku
			};
		}
	}
}

