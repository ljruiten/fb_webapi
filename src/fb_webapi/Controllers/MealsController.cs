using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using fb_webapi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AspNet.Security.OAuth.Validation;

namespace fb_webapi.Controllers {
    [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    //[Route ("api/[controller]")]
    public class MealsController : Controller {
        private ApplicationDbContext dbContext;
        private UserManager<User> userManager;
        private User CurrentUser;

        public MealsController(ApplicationDbContext dbContext,
                               UserManager<User> userManager) {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }


        [HttpGet]
        [Route("/api/meals/test")]
        public IActionResult TestAll() {
            var meals = dbContext.Meals
                .Include(meal => meal.Pictures)
                .ToList();

            return new JsonResult(meals);
        }

        [HttpGet]
        [Route("/api/users/test")]
        public IActionResult Test() {
            var user = dbContext.Users
                .Include(u => u.Meals)
                .ToList()
                .FirstOrDefault();

            return new JsonResult(user);
        }


        [HttpGet]
        [Route("/api/meals/all")]
        public IActionResult GetAll() {
            CurrentUser = this.userManager.GetUserAsync(HttpContext.User).Result;
            var meals = dbContext.Meals.Where(m => m.UserId == CurrentUser.Id);
            return new JsonResult(meals);
        }

        [HttpPost]
        [Route("api/meals/new")]
        public async Task<IActionResult> Create()
        {
            //CurrentUser = await userManager.GetUserAsync(HttpContext.User);

            var meal = new Meal();
            meal.UserId = CurrentUser.Id;
            dbContext.Meals.Add(meal);
            await dbContext.SaveChangesAsync();
            return new JsonResult(meal);
        }
    }
}