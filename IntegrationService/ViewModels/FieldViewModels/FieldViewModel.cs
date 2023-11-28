using IntegrationService.ViewModels.ChannelViewModels;

namespace IntegrationService.ViewModels.FieldViewModels
{
    public class FieldViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsEnabled { get; set; }
        public Guid DomGroupId { get; set; }
        public List<string> PredefinedValues { get; set; }
    }
}
