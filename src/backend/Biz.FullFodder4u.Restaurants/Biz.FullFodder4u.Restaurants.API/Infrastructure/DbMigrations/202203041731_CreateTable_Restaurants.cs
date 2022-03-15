using FluentMigrator;

namespace Biz.FullFodder4u.Restaurants.API.Infrastructure.DbMigrations;

[Migration(202203041731)]
public class _202203041731_CreateTable_Restaurants : Migration
{
    public override void Up()
    {
        Create.Table("restaurants")
            .WithColumn("id").AsGuid().NotNullable()
            .WithColumn("name").AsString(250).NotNullable();

        Create.PrimaryKey($"PK__restaurants__id").OnTable("restaurants")
                        .Column("id");
    }

    public override void Down()
    {
        
    }
}
