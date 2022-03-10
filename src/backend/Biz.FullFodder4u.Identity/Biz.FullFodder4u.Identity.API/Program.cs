using Biz.FullFodder4u.Identity.API;
using Biz.FullFodder4u.Identity.API.ExceptionHandling;
using Biz.FullFodder4u.Identity.API.Services;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddScoped<IUserService, UserService>()
    .AddCustomControllers()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddOpenTelemetry("Biz.FullFodder4u.Identity", builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection()
    .UseAuthorization();

app.MapControllers();

app.Run();
