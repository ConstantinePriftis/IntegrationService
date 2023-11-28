using IntegrationService.ConcreteServices;
using IntegrationService.IServices.IRoles;
using IntegrationService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationService.Controllers
{

    public class MyController : Controller
    {
        //private readonly List<IndexModel.TableRow> _rows;
        //private readonly UserManager<IdentityUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserRoleService _userRoleService;
		private readonly List<IndexModel.TableRow> _rows;

		public MyController(UserRoleService userRoleService)
        {
            _userRoleService = userRoleService;

        }
        public MyController(List<IndexModel.TableRow> rows)
        {
            _rows = rows;
            string x = string.Empty;
        }

        // Your other controller actions...
        public async Task<IActionResult> AddUserToRole(string userId, string roleName)
        {
            bool result = await _userRoleService.AddUserToRole(userId, roleName);
            if (result)
            {
                // User added to role successfully
                // Handle the appropriate response
            }
            else
            {
                // Failed to add user to role
                // Handle the appropriate response and errors
            }
            return Ok();
        }

            [HttpGet]
        public void Index()
        {
            // Use the rows as needed
            //foreach (var row in _rows)
            //{
            //    // Do something with each row
            //}
        }
    }

}
