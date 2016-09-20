using OpenIddict;
using Microsoft.EntityFrameworkCore;

namespace fb_webapi.Entities {
    public class ApplicationDbContext : OpenIddictDbContext<User> {
        public DbSet<Meal> Meals { get; set; }
        //public DbSet<Group> Groups { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    }
}