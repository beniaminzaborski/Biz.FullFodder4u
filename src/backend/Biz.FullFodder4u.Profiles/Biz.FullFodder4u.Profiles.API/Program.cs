using Biz.FullFodder4u.Profiles.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddCustomControllers()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddOpenTelemetry("Biz.FullFodder4u.Profiles", builder.Configuration)
    .AddCustomMassTransit(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
