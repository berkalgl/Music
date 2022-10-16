using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using User.API.Application.IntegrationEvents.Consumers;
using User.API.Infrastructure.Filters;
using User.API.Infrastructure.Modules;
using User.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
})
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.SwaggerGenOptions();

//MassTransit configuration
builder.Services.MassTransitConfiguration();

//adding mediatR to project
builder.Services.AddMediatR(typeof(Program));
builder.Services.Register();

builder.Services.AddDbContext<UserDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "UserDb"));

var app = builder.Build();

var dbInstance = app.Services.CreateScope().ServiceProvider.GetRequiredService<UserDbContext>();
SeedData.Initialize(dbInstance);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("allow");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
    public static string AppName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
}