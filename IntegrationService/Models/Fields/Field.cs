using IntegrationService.Models.Channels;
using IntegrationService.Models.EntityBases;
using IntegrationService.Models.Product;
using IntegrationService.Models.User;
using IntegrationService.ViewModels.FieldViewModels;
using System.Runtime.InteropServices;

namespace IntegrationService.Models.Fields
{
    public class Field : BaseEntity
    {
        public Field()
        {

        }
        private Field(string name, string type, int order, List<Guid> channels, ApplicationUser applicationUser)
        {
            UpdateName(name);
            UpdateType(type);
            UpdateOrder(order);
			//UpdateFieldChannels(channels);
			CreatedBy = applicationUser;
            CreatedOn = DateTime.UtcNow;
            UpdateModifier(applicationUser);
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsStore { get; set; }
        public int Order { get; set; }
        public Guid DOMGroupsId { get; set; }
        public virtual DOMGroups DOMGroups { get; set; }

        //private List<FieldChannel> _fieldChannels = new();
        //public virtual IReadOnlyList<FieldChannel> FieldChannels => _fieldChannels.ToList();

        private List<FieldProducts> _fieldProducts = new();
        private List<FieldProductStore> _fieldProductStores = new();
        public virtual IReadOnlyList<FieldProducts> FieldProducts => _fieldProducts.ToList();
        public virtual IReadOnlyList<FieldProductStore> FieldProductStore => _fieldProductStores.ToList();
        //public void UpdateFieldChannels(List<Guid> channels)
        //{
        //    _fieldChannels = channels.Select(x=> FieldChannel.Create(x,Id)).ToList();
        //}
        public void UpdateFieldProduct(Field field, string value)
        {
            //var item = FieldProducts.Create(field, Id, value);
            //_fieldProducts = new List<FieldProducts>() { item };
        }
        private void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name?.Trim())) throw new ArgumentNullException(nameof(Name));
            name = name.Trim();
            if (Name != name)
                Name = name;
        }
        private void UpdateType(string type)
        {
            if (string.IsNullOrWhiteSpace(type?.Trim())) throw new ArgumentNullException(nameof(Type));
            type = type.Trim();
            if (Type != type)
                Type = type;
        }
		private void UpdateOrder(int order)
		{
			Order = order;
		}
		private void UpdateModifier(ApplicationUser modifier)
        {
            ModifiedBy = modifier;
            ModifiedOn = DateTime.UtcNow;
        }
        public void Update(string name, string type, List<Guid> channels, ApplicationUser modifier)
        {
            UpdateName(name);
            UpdateType(type);
			//UpdateFieldChannels(channels);
			UpdateModifier(modifier);
        }
        public static Field Create(string name, string type, int order, List<Guid> channels, ApplicationUser applicationUser)
        {
            return new(name,type,order,channels,applicationUser);
        }

        public FieldViewModel ToDto()
        {
            return new FieldViewModel()
            {
                Id= Id,
                Name= Name,
                Type = Type
            };
        }
    }
}
