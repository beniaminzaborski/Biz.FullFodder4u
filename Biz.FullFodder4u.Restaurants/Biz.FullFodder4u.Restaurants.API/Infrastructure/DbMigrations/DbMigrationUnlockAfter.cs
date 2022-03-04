using FluentMigrator;

namespace Biz.FullFodder4u.Restaurants.API.Infrastructure.DbMigrations;

//[Maintenance(MigrationStage.AfterAll, TransactionBehavior.Default)]
public class DbMigrationUnlockAfter : Migration
{
    public override void Down()
    {
        throw new NotImplementedException("Down migrations are not supported for sp_releaseapplock");
    }

    public override void Up()
    {
        Execute.Sql("EXEC sp_releaseapplock 'restaurants', 'Session'");
    }
}
