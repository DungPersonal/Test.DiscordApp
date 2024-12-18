using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Test.DiscordApp.Application.Behavior;

namespace Test.DiscordApp.Application;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddApplication(this IHostApplicationBuilder builder)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        builder.Services
            .AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(assembly);
                
                configuration.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
            })
            .AddValidatorsFromAssembly(assembly, includeInternalTypes: true)
            .AddMapster();

        return builder;
    }
}