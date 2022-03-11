using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

//public class Program
//{
//    public static void Main(string[] args)
//    {
//        //new WebHostBuilder()
//        WebApplication.CreateBuilder(args).WebHost
//            .UseKestrel()
//            .UseContentRoot(Directory.GetCurrentDirectory())
//            .ConfigureAppConfiguration((hostingContext, config) =>
//            {
//                config
//                    .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
//                    .AddJsonFile("appsettings.json", true, true)
//                    .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
//                    .AddJsonFile("ocelot.json")
//                    .AddEnvironmentVariables();
//            })
//            .ConfigureServices(s =>
//            {
//                s.AddControllers();

//                s.AddEndpointsApiExplorer()
//                    .AddSwaggerGen();

//                s.AddOcelot()
//                    .AddPolly();
//            })
//            .ConfigureLogging((hostingContext, logging) =>
//            {
//                //add your logging
//            })
//            .UseIISIntegration()
//            .Configure(app =>
//            {
//                if (app.Environment.IsDevelopment())
//                {
//                    app.UseSwagger();
//                    app.UseSwaggerUI();
//                }

//                app.UseHttpsRedirection()
//                    .UseAuthorization();

//                //app.MapControllers();

//                app.UseOcelot()
//                    .Wait();
//            })
//            .Build()
//            .Run();
//    }
//}

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureAppConfiguration((hostingContext, config) =>
{
    config
        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
        .AddJsonFile("appsettings.json", true, true)
        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
        .AddJsonFile("ocelot.json")
        .AddEnvironmentVariables();
});

// Add services to the container.
//builder.Services.AddControllers();

//builder.Services
//    .AddEndpointsApiExplorer()
//    .AddSwaggerGen();

builder.Services
    .AddOcelot()
    .AddPolly();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection()
//    .UseAuthorization();

//app.MapControllers();

app.UseOcelot();

app.Run();
