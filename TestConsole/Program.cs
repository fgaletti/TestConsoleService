// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Reflection;
using TestConsole;
using TestConsole.Configuration;


try
{
    Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
    Log.Information("Building host");
    var host = CreateHostBuilder(args).Build(); ;
    Log.Information("Starting host");
    host.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}


IHostBuilder  CreateHostBuilder(string[] args)
{
    {
        return Host.CreateDefaultBuilder(args)
                  .ConfigureServices((context, services) => ConfigureServices(context.Configuration, services))
                 .UseSerilog((hostingContext, loggerConfiguration) =>
                     loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
                  )
                  .ConfigureAppConfiguration((_, configurationBuilder) => SetupConfiguration(configurationBuilder));
    }
}

 void ConfigureServices(IConfiguration configuration, IServiceCollection services)
{
    services.AddLogging();
    services.AddOptions();
   // services.AddHostedService<Orchestrator>();
    services.AddOptions<QueryOptions>().Bind(configuration.GetSection(QueryOptions.SectionName)).PostConfigure(
        options =>
        {
            var executingLocation = AppDomain.CurrentDomain.BaseDirectory;          
            options.OrderNumbersQuery = $"{executingLocation}{options.OrderNumbersQuery}";
            options.OrderQuery = $"{executingLocation}{options.OrderQuery}";
        });

    services.AddHostedService<Orchestrator>();
}

 void SetupConfiguration(IConfigurationBuilder configurationBuilder)
{
    var dotNetCoreEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
    Console.WriteLine(dotNetCoreEnvironmentVariable != null
        ? $"NETCORE_ENVIRONMENT = {dotNetCoreEnvironmentVariable}"
        : "NETCORE_ENVIRONMENT was not set.  Will use appsettings.json file as the default");

    configurationBuilder
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT")}.json", true, true)
        .AddEnvironmentVariables()
        .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
}



