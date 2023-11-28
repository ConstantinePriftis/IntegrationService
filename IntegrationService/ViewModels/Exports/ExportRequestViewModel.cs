using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace IntegrationService.ViewModels.Exports
{
    public class ExportRequestViewModel
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Enabled { get; set; }
    }
}
