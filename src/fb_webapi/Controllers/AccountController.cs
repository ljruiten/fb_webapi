using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using fb_webapi.Entities;
using fb_webapi.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace fb_webapi.Controllers {
    [Route("api/[controller]")]
    public class AccountController : Controller {
        private UserManager<User> userManager;
        private RoleManager<IdentityRole> roleManager;

        public AccountController (UserManager<User> userManager, RoleManager<IdentityRole> roleManager) {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpPost]
        [Route("/api/account/create")]
        public IActionResult Create([FromForm] CreateAccountModel model) {
            if (!ModelState.IsValid) {
                return new BadRequestResult();
            }

            var user = new User ();
            user.UserName = model.Username;
            user.Email = model.Email;

            var role = new IdentityRole("user");
            var d = roleManager.CreateAsync(role).Result;

            var result = userManager.CreateAsync(user, model.Password).Result;

            if (result.Succeeded) {
                var a = userManager.AddClaimAsync(user, new Claim(ClaimTypes.NameIdentifier, user.Id, ClaimValueTypes.String)).Result;
                var b = userManager.AddToRoleAsync(user, "user").Result;


                var response = new OperationResult {
                    Succeeded = true,
                    Message = "account-created"
                };

                return new JsonResult(response);
            } else {
                var response = new OperationResult {
                    Succeeded = false,
                    Message = result.ToString()
                };

                return new JsonResult(response);
            }
        }
    }
}