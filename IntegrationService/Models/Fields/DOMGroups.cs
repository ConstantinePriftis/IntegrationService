namespace IntegrationService.Models.Fields
{
    public class DOMGroups
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        private List<Field> _fields = new();
        public virtual IReadOnlyList<Field> Fields => _fields.ToList();
        private List<DOMValues> _domValues = new();
        public virtual IReadOnlyList<DOMValues> DOMValues => _domValues.ToList();
    }
}
