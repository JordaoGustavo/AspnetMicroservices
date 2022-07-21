using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddOcelot();


builder.Host.ConfigureLogging((hostingContex, logginBuilder) =>
{
    logginBuilder.AddConfiguration(hostingContex.Configuration.GetSection("Logging"));
    logginBuilder.AddConsole();
    logginBuilder.AddDebug();
});

builder.Host.ConfigureAppConfiguration((hostingContex, config) =>
{
    config.AddJsonFile($"ocelot.{hostingContex.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
});

var app = builder.Build();

await app.UseOcelot();

await app.RunAsync();
