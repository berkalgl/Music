using Jam.API.Infrastructure.Filters;
using Jam.API.Infrastructure.Modules;
using Jam.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers(options =>
    {
        options.Filters.Add(typeof(HttpGlobalExceptionFilter));
        options.Filters.Add(typeof(ValidatorActionFilter));
    })
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Fluent Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.SwaggerGenOptions();
//adding mediatR to project
builder.Services.AddMediatR(typeof(Program));
builder.Services.Register();

//MassTransit configuration
builder.Services.MassTransitConfiguration();

builder.Services.AddDbContext<JamDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "JamDb"));

builder.Services.AddAuthenticationAndCORS();
var app = builder.Build();

var dbInstance = app.Services.CreateScope().ServiceProvider.GetRequiredService<JamDbContext>();
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