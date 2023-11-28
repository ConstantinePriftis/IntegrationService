

namespace IntegrationService.ViewModels.FieldProductsViewModels
{
	public class FieldProductViewModel : object
	{
		public Guid Id { get; set; }
		public Guid ProductId { get; set; }
		public string? Sku { get; set; }
		public string? Title_Import { get; set; }
		public string? Description_Import { get; set; }
		public string? Status { get; set; }
		public string? Step { get; set; }
		public bool IsCompleted { get; set; }
		public DateTime ModifiedOn { get; set; }
		public IEnumerable<FieldPerProductViewModel> Fields { get; set; }
	}
}
