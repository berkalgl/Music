using MassTransit;
using Microsoft.OpenApi.Models;
using User.API.Application.IntegrationEvents.Consumers;
using User.API.Application.Queries;
using User.Domain.AggregatesModel.UserProfileAggregate;
using User.Infrastructure.Repositories;

namespace User.API.Infrastructure.Modules
{
    public static class AppModule
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<IUserProfileQueries, UserProfileQueries>();
        }
        public static void MassTransitConfiguration(this IServiceCollection services)
        {
            services.AddMassTransit(configurator =>
            {
                configurator.AddConsumer<UserRegisteredToJamEventConsumer>();
                configurator.AddConsumer<JamStartedEventConsumer>();
                configurator.UsingRabbitMq((context, config) =>
                {
                    config.Host("rabbitmq", "/", host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });

                    //Handshake with RabbitMQ
                    config.ConfigureEndpoints(context);
                });
            });
        }
        public static void SwaggerGenOptions(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }
    }
}
