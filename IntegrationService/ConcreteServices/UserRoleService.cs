using IntegrationService.IServices.IRoles;
using Microsoft.AspNetCore.Identity;

namespace IntegrationService.ConcreteServices
{
    public class UserRoleService : IUserRoleService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRoleService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<bool> AddUserToRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Handle user not found error
                return false;
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                // Handle role not found error
                return false;
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                // User added to role successfully
                return true;
            }
            else
            {
                // Failed to add user to role
                // Handle the appropriate response and errors
                return false;
            }
        }
    }
}
