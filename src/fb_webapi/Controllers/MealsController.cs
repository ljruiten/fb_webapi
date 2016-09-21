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
    [Route ("api/[controller]")]
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
        public IActionResult GetAll() {
            CurrentUser = this.userManager.GetUserAsync(HttpContext.User).Result;
            var meals = dbContext.Meals.Where(m => m.UserId == CurrentUser.Id);
            return new JsonResult(meals);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var meal = dbContext.Meals.Where(m => m.Id == id)
                .Select(m => new
                {
                    Id = m.Id,
                    UserId = m.UserId,
                    Title = m.Title,
                    Description = m.Description,
                    Pictures = m.Pictures.Select(p => p.Id)
                });
            return new JsonResult(meal);
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            CurrentUser = await userManager.GetUserAsync(HttpContext.User);

            var meal = new Meal();
            meal.UserId = CurrentUser.Id;
            dbContext.Meals.Add(meal);
            await dbContext.SaveChangesAsync();
            return new JsonResult(meal);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] Meal newMeal)
        {
            var oldMeal = dbContext.Meals
                .Where(m => m.Id == id)
                .First();

            if (oldMeal.Id != newMeal.Id)
            {
                return new BadRequestResult();
            }

            oldMeal.Title = newMeal.Title;
            oldMeal.Description = newMeal.Description;

            await dbContext.SaveChangesAsync();

            return new OkResult();
        }
    }
}