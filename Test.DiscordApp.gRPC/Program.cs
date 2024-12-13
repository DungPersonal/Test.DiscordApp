using Microsoft.OpenApi.Models;
using Serilog;
using Test.DiscordApp.Application.Services;
using Test.DiscordApp.Domain.Config;
using Test.DiscordApp.Infrastructure;

namespace Test.DiscordApp.Application;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("Program");
        logger.LogInformation("Starting TestDiscordApp.Application...");

        var builder = WebApplication.CreateBuilder(args);

        ConfigureBuilder(builder);

        var app = builder.Build();
        ConfigureApp(app);

        await app.RunAsync();
    }

    private static void ConfigureBuilder(WebApplicationBuilder builder)
    {
        #region Configure settings

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddUserSecrets(typeof(Program).Assembly, optional: false)
            .AddEnvironmentVariables();
        builder.Services
            .Configure<DiscordConfig>(builder.Configuration.GetSection("Discord"))
            .Configure<GithubConfig>(builder.Configuration.GetSection("Github"))
            .AddSingleton(TimeProvider.System);

        #endregion

        #region Configure project specific services

        builder
            .AddInfrastructureServices(builder.Configuration)
            .AddApplication();
        builder.Host.ConfigureDiscordClient(builder.Configuration);

        #endregion

        #region Configure gRPC

        builder.Services
            .AddCors(c => c.AddPolicy("AllowAllOrigins", b => b
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
            ))
            .AddGrpc()
            .AddJsonTranscoding();

        #endregion

        #region Configure Swagger

        builder.Services
            .AddGrpcSwagger()
            .AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "gRPC transcoding", Version = "v1" });
                // var filePath = Path.Combine(AppContext.BaseDirectory, "Server.xml");
                // c.IncludeXmlComments(filePath);
                // c.IncludeGrpcXmlComments(filePath, includeControllerXmlComments: true);
            });

        #endregion

        builder.Host
            .AddSerilog();
    }

    private static void ConfigureApp(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test.DiscordApp API");
                c.RoutePrefix = string.Empty; // Automatically open Swagger at the root
            });
        }

        app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
        app.UseCors("AllowAllOrigins"); 
        app.MapGrpcService<GreeterService>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");
    }

    private static void AddSerilog(this ConfigureHostBuilder host)
    {
        host.UseSerilog((context, configuration) => { configuration.ReadFrom.Configuration(context.Configuration); });
    }
}