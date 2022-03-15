using FluentMigrator;

namespace Biz.FullFodder4u.Restaurants.API.Infrastructure.DbMigrations;

[Migration(202203111900)]
public class _202203111900_AlterTable_Restaurant_Unique_Name : Migration
{
    public override void Up()
    {
        Alter.Column("name").OnTable("restaurants")
            .AsString(250).Unique().NotNullable();
    }

    public override void Down()
    {
    }
}
