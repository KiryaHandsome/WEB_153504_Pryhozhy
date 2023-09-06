using Microsoft.EntityFrameworkCore;
using WEB_153504_Pryhozhy.Domain.Entities;

namespace WEB_153504_Pryhozhy.API.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Category> Categories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {

        }
    }
}
