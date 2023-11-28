using IntegrationService.Models.EntityBases;
using Microsoft.AspNetCore.Identity;

namespace IntegrationService.Models.User
{
    public partial class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
    }
    public partial class UserLogin : IdentityUserLogin<Guid>
    {
    }
    public partial class UserRole : IdentityUserRole<Guid>
    {
    }
    public partial class UserClaim : IdentityUserClaim<Guid>
    {
    }
    public partial class Role : IdentityRole<Guid>
    {
        public Role() : base()
        {
        }

        public Role(string roleName)
        {
            Name = roleName;
        }
    }
    public partial class RoleClaim : IdentityRoleClaim<Guid>
    {
    }
    public partial class UserToken : IdentityUserToken<Guid>
    {
    }
}
