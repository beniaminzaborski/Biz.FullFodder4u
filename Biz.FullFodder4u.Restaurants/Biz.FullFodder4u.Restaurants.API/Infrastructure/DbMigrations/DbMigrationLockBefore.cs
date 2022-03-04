using FluentMigrator;

namespace Biz.FullFodder4u.Restaurants.API.Infrastructure.DbMigrations;

//[Maintenance(MigrationStage.BeforeAll, TransactionBehavior.Default)]
public class DbMigrationLockBefore : Migration
{
    private int LockTimeout { get; set; }

    public DbMigrationLockBefore(IConfiguration configuration)
    {
        LockTimeout = configuration.GetSection("FluentMigration").GetValue("LockTimeout", 300) * 1000;
    }

    public override void Down()
    {
        throw new NotImplementedException("Down migrations are not supported for sp_getapplock");
    }

    public override void Up()
    {
        Execute.Sql(@$"
            DECLARE @result INT
            EXEC @result = sp_getapplock 'restaurants', 'Exclusive', 'Session', {LockTimeout}

            IF @result < 0
            BEGIN
                DECLARE @msg NVARCHAR(1000) = 'Received error code ' + CAST(@result AS VARCHAR(10)) + ' from sp_getapplock during migrations of restaurants';
                THROW 99999, @msg, 1;
            END
        ");
    }
}
