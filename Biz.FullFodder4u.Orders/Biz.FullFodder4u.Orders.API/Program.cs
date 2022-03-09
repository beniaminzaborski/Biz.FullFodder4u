using Biz.FullFodder4u.Orders.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddCustomControllers()
    .AddCustomAuthentication(builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddCustomSwagger()
    .AddOpenTelemetry("Biz.FullFodder4u.Orders", builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection()
    .UseAuthentication()
    .UseAuthorization();

app.MapControllers()
    .RequireAuthorization();

app.Run();
