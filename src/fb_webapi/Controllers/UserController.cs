using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using fb_webapi.Entities;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace fb_webapi.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext dbContext;

        public UserController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(string id)
        //{
        //
        //}

        [HttpGet]
        [Route("/api/users/{id}/meals")]
        public IActionResult GetMeals(string id)
        {
            var meals = dbContext.Meals
                .Where(m => m.UserId == id)
                .ToList();

            return new JsonResult(meals);
        }
    }
}
