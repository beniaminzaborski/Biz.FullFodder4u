using Biz.FullFodder4u.Restaurants.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddCustomControllers()
    .AddJwtAuthentication(builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddCustomSwagger()
    .AddDataAccess(builder.Configuration)
    .AddOpenTelemetry("Biz.FullFodder4u.Restaurants");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection()
    .UseAuthentication()
    .UseAuthorization()
    .MigrateDb();

app.MapControllers()
    .RequireAuthorization();

app.Run();
