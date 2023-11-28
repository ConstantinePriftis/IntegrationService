namespace IntegrationService.Models.EntityBases
{
    public class SimpleBaseEntity: BaseEntityDateTime
    {
        public Guid Id { get; set; } = Guid.NewGuid();
       
    }
}
