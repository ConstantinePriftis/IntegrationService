using IntegrationService.Models.EntityBases;
using IntegrationService.Models.User;
using IntegrationService.ViewModels.ChannelViewModels;

namespace IntegrationService.Models.Channels
{
    public class Channel : BaseEntity
    {
        public Channel()
        {

        }
        private Channel(string name, string desc,ApplicationUser applicationUser)
        {
            UpdateName(name);
            Description = desc;
            CreatedBy = applicationUser;
            CreatedOn = DateTime.UtcNow;
            UpdateModifier(applicationUser);
        }
        public string ChannelName { get; set; }
        public string Description { get; set; }

        //private List<FieldChannel> _fieldChannels = new();
        //public virtual IReadOnlyList<FieldChannel> FieldChannels => _fieldChannels.ToList();
        private void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name?.Trim())) throw new ArgumentNullException(nameof(ChannelName));
            name = name.Trim();
            if (ChannelName != name)
                ChannelName = name;
        }
        private void UpdateModifier(ApplicationUser modifier)
        {
            ModifiedBy = modifier;
            ModifiedOn = DateTime.UtcNow;
        }
        public static Channel Create(string name, string description, ApplicationUser applicationUser)
        {
            return new( name, description, applicationUser);
        }
        public ChannelViewModel ToDto()
        {
            return new ChannelViewModel
            {
                Id = Id,
                Name = ChannelName,
                Description = Description
            };
        }
    }
}
