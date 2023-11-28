using IntegrationService.Models.User;
using IntegrationService.ViewModels.FieldViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IntegrationService.Models.Fields
{
    public class DOMValues //: BaseEntity
    {
        public Guid Id { get; set; }
        public Guid DOMGroupsId { get; set; }
        public virtual DOMGroups DOMGroups { get; set; }
        public string Value { get; set; }

        public DOMValues(Guid dOMGroupsId, string value
            //,ApplicationUser applicationUser
            )
        {
            DOMGroupsId = dOMGroupsId;
            Value = value;
            //CreatedBy = applicationUser;
            //CreatedOn = DateTime.UtcNow;
            //UpdateModifier(applicationUser);
        }

        private void UpdateValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value?.Trim())) throw new ArgumentNullException(nameof(value));
            value = value.Trim();
            if (Value != value)
                Value = value;
        }
        
        private void UpdateModifier(ApplicationUser modifier)
        {
            //ModifiedBy = modifier;
            //ModifiedOn = DateTime.UtcNow;
        }
        public void Update(string name, ApplicationUser modifier)
        {
            UpdateValue(name);
            //UpdateModifier(modifier);
        }
        public static DOMValues Create(Guid dOMGroupsId, string name
            //, ApplicationUser applicationUser
            )
        {
            return new(dOMGroupsId, name
                //, applicationUser
                );
        }

        public DOMValuesViewModel ToDto()
        {
            return new DOMValuesViewModel()
            {
                Id = Id,
                Value = Value
            };
        }

        public SelectListItem ToSelectListItem()
        {
            return new SelectListItem()
            {
                Text = Value,
                Value = Value
            };
        }
    }
}
