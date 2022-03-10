using FluentMigrator.Runner;

namespace Biz.FullFodder4u.Restaurants.API;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder MigrateDb(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var runner = scope.ServiceProvider.GetService<IMigrationRunner>();
        runner.ListMigrations();
        runner.MigrateUp();
        return app;
    }
}
