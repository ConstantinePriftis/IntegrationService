using IntegrationService.Models.User;

namespace IntegrationService.Models.EntityBases
{
    public class BaseEntity : SimpleBaseEntity
    {
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser ModifiedBy { get; set; }

    }
}
