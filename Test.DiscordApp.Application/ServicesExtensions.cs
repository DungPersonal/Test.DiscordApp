using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Test.DiscordApp.Application;

public static class ServicesExtensions
{
    public static IHostApplicationBuilder AddApplication(this IHostApplicationBuilder builder)
    {
        var assembly = typeof(ServicesExtensions).Assembly;
        builder.Services
            .AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly))
            .AddValidatorsFromAssembly(assembly);

        return builder;
    }
}