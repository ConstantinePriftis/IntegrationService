using IntegrationService.Models.CustomFieldCollections;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;

namespace IntegrationService.ViewModels.FieldProductsViewModels
{
	public class FieldPerProductViewModel 
	{
        public Guid? FieldId { get; set; }
        public Guid? ProductId { get; set; }
        public string Sku { get; set; }
        public string FieldName { get; set; }
        public string ProductName { get; set; }
        public string Value { get; set; }
        public int Order { get; set; }
        public string DomGroupDesc  { get; set; }
        public List<SelectListItem> DomValues { get; set; }
        public DateTime ModifiedDate { get; set; }
	}
}
