using System.Collections.Generic;

using OpenIddict;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace fb_webapi.Entities {
    public class User : OpenIddictUser {
        public User () {
            this.Meals = new List<Meal>();
        }

        public List<Meal> Meals { get; set; }
    }
}