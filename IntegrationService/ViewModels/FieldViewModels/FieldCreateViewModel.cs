using IntegrationService.Models.Channels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IntegrationService.ViewModels.FieldViewModels
{
    public class FieldCreateViewModel
    {
        //public FieldCreateViewModel(List<SelectListItem> channels)
        //{
        //    Channels = channels;
        //}

        public string Name { get; set; }
        public string Type { get; set; }
        public List<SelectListItem> Channels { get; set; }
        public Guid[] SelectedChannels { get; set; }
    }
}
