using Jam.API.Application.IntegrationEvents.Consumers;
using Jam.API.Application.Queries;
using Jam.API.Infrastructure.Services;
using Jam.Domain.AggregatesModel.JamAggregate;
using Jam.Infrastructure.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Jam.API.Infrastructure.Modules
{
    public static class AppModule
    {
        private const string _issuer = "dummy.com.tr";
        private const string _audience = "dummy.com.tr";
        private const string _secret = "a-secret-sentence";
        public static void Register(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddScoped<IJamRepository, JamRepository>();
            services.AddScoped<IJamQueries, JamQueries>();
        }
        public static void MassTransitConfiguration(this IServiceCollection services)
        {
            services.AddMassTransit(configurator =>
            {
                configurator.AddConsumer<UserValidationRegisterJamFailedEventConsumer>();
                configurator.AddConsumer<UserValidationRegisterJamSuccessEventConsumer>();
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
        public static void AddAuthenticationAndCORS(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateActor = true,
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidAudience = _audience,
                        ValidIssuer = _issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret))
                    };
                });

            services.AddCors(option =>
            {
                option.AddPolicy("allow", builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    // two different sub domains are two different origins
                    builder.AllowAnyOrigin();
                });
            });
        }
    }
}
