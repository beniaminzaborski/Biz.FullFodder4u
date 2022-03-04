using Biz.FullFodder4u.Restaurants.API.Infrastructure.EntityMappings;
using Biz.FullFodder4u.Restaurants.API.Model;
using Microsoft.EntityFrameworkCore;

namespace Biz.FullFodder4u.Restaurants.API.Infrastructure;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

    public DbSet<Restaurant> Restaurants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new RestaurantMapping());

        base.OnModelCreating(modelBuilder);
    }
}
