using System.Collections.Generic;
using fb_webapi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace fb_webapi {
    public class SeedController : Controller {
        private ApplicationDbContext dbContext;
        private UserManager<User> userManager;

        public SeedController (ApplicationDbContext dbContext,
                               UserManager<User> userManager) {
            this.userManager = userManager;
            this.dbContext = dbContext;
        }

        [Route ("api/seed/test")]
        [HttpGet]
        public IActionResult test () {
            var userId = dbContext.Users.First().Id;

            var meals = dbContext.Meals
                .Where(m => m.UserId == userId)
                .Include(m => m.Pictures)
                .First();

            //meals.User = null;

            return new JsonResult(meals);
        }
    

        [Route ("api/seed/start")]
        [HttpGet]
        public IActionResult start () {
            var user = new User();
            user.UserName = "superadmin";
            user.Email = "larsvanruiten@hotmail.com";
            var c = userManager.CreateAsync(user, "Password1!").Result;
            dbContext.SaveChanges();

            var meal1 = new Meal();
            meal1.Description = "very nice meal!";
            meal1.Title = "Baba ganoush";
            meal1.UserId = user.Id;

            var meal2 = new Meal();
            meal2.Description = "super nice meal!";
            meal2.Title = "Pizza Margherita";
            meal2.UserId = user.Id;

            var meal3 = new Meal();
            meal3.Description = "super nice pizza";
            meal3.Title = "Pizza Napoletana";
            meal3.UserId = user.Id;

            var meal4 = new Meal();
            meal4.Description = "This description is probably much too long to fit in the card and can only be shown in the detailed view.";
            meal4.Title = "This title is probably much too long to fit in the card and can only be shown in the detailed view.";
            meal4.UserId = user.Id;

            var meal5 = new Meal();
            meal5.Description = "very nice meal!";
            meal5.Title = "Baba ganoush";
            meal5.UserId = user.Id;

            var meal6 = new Meal();
            meal6.Description = "super nice meal!";
            meal6.Title = "Pizza Margherita";
            meal6.UserId = user.Id;

            var meal7 = new Meal();
            meal7.Description = "super nice pizza";
            meal7.Title = "Pizza Napoletana";
            meal7.UserId = user.Id;

            var meal8 = new Meal();
            meal8.Description = "This description is probably much too long to fit in the card and can only be shown in the detailed view.";
            meal8.Title = "This title is probably much too long to fit in the card and can only be shown in the detailed view.";
            meal8.UserId = user.Id;

            var meals = new List<Meal>();
            meals.Add(meal1);
            meals.Add(meal2);
            meals.Add(meal3);
            meals.Add(meal4);
            meals.Add(meal5);
            meals.Add(meal6);
            meals.Add(meal7);
            meals.Add(meal8);

            dbContext.Meals.AddRange(meals);
            user.Meals.AddRange(meals);

            dbContext.SaveChanges();

            return new StatusCodeResult(200);
        }
    }
}