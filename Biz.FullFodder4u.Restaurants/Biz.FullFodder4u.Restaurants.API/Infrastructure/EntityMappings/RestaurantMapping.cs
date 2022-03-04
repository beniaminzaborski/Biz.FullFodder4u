using Biz.FullFodder4u.Restaurants.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biz.FullFodder4u.Restaurants.API.Infrastructure.EntityMappings;

internal class RestaurantMapping : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.ToTable(name: "restaurants");

        builder.HasKey(p => p.Id);
        builder.Property<Guid>(nameof(Restaurant.Id)).HasColumnName("id").ValueGeneratedNever();

        builder.Property<string>(nameof(Restaurant.Name)).HasColumnName("name").HasMaxLength(250);
    }
}
