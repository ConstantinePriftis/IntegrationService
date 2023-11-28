using IntegrationService.ViewModels.FieldViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IntegrationService.ViewModels.ChannelViewModels
{
    public class ChannelViewModel
    {
        //public ChannelViewModel(Guid id,string name, string description,List<FieldViewModel> fields)
        //{
        //    Id = id;    
        //    Name = name;
        //    Description = description;
        //    Fields = fields;
        //}
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<FieldViewModel> Fields { get; set; }
        public SelectListItem ToSelectListItem()
        {
            return new SelectListItem(Name, Id.ToString());
        }
    }
}
