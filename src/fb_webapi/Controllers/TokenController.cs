using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNetCore.Http.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using fb_webapi.Entities;
using Microsoft.AspNetCore.Builder;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace fb_webapi.Controllers
{

    public class TokenController : Controller
    {
        private UserManager<User> userManager;

        public TokenController (UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost("~/connect/token")]
        [Produces("application/json")]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIdConnectRequest();

            if (request.IsPasswordGrantType())
            {
                var user = await userManager.FindByNameAsync(request.Username);
                if (user == null)
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The username/password couple is invalid."
                    });
                }

                // Ensure the password is valid.
                if (!await userManager.CheckPasswordAsync(user, request.Password))
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The username/password couple is invalid."
                    });
                }

                // Note: for a more complete sample including account lockout support, visit
                // https://github.com/openiddict/openiddict-core/blob/dev/samples/Mvc.Server/Controllers/AuthorizationController.cs

                //var identity = await userManager.CreateIdentityAsync(user, request.GetScopes());
                //var identity = new ClaimsIdentity(await userManager.GetClaimsAsync(user));
                var claims = await userManager.GetClaimsAsync(user);
                var identity = new ClaimsIdentity(claims);

                //var principal = new ClaimsPrincipal()

                // Create a new authentication ticket holding the user identity.
                var ticket = new AuthenticationTicket(
                    new ClaimsPrincipal(identity),
                    new AuthenticationProperties(),
                    OpenIdConnectServerDefaults.AuthenticationScheme);

                ticket.SetResources(request.GetResources());
                ticket.SetScopes(request.GetScopes());
                
                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }

            return BadRequest(new OpenIdConnectResponse
            {
                Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                ErrorDescription = "The specified grant type is not supported."
            });
        }
    }
}
