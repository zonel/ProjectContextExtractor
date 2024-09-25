using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ProjectContextExtractor
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";

                    var basePath = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin", StringComparison.Ordinal));
                    config.SetBasePath(basePath);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{environment}.json", optional: true)
                        .AddJsonFile("prompts.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    var config = new Configuration();
                    context.Configuration.Bind(config);
                    services.AddSingleton(config);

                    services.AddTransient<ProjectFilesSchema>();
                    services.AddTransient<ProjectContextExtractor>();
                    services.AddTransient<ForewordPromptGenerator>();
                    services.AddTransient<EmbeddingManager>();
                })
                .Build();

            // var embeddingManager = host.Services.GetRequiredService<EmbeddingManager>();
            // embeddingManager.GenerateAndStoreEmbeddingsAsync().Wait();

            var extractor = host.Services.GetRequiredService<ProjectContextExtractor>();
            extractor.Execute();
        }
    }
}