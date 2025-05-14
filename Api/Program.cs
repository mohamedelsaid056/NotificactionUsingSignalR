using System.Globalization;
using System.Reflection;
using System.Text.Json.Serialization;

using Application.MappingProfiles;
using Application.Services;
using Domain.Interfaces.Services;
using FluentValidation;
using Infrastructure;
using Infrastructure.RealTime;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Notification.API.Middleware;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();


// Add Infrastructure Layer
builder.Services.AddInfrastructure(builder.Configuration);


// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add Application Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<INotificationService, NotificationService>();



// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Notification API", Version = "v1" });

    // Configure Swagger to use JWT Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization ",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null);



builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
    options.SerializerOptions.PropertyNamingPolicy = null;
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.WriteIndented = true;
    //
});
//builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
    options.SuppressInferBindingSourcesForParameters = true;
    options.SuppressConsumesConstraintForFormFileParameters = true;
});
//
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


//
var app = builder.Build();




app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();


// Use custom error handler middleware
app.UseMiddleware<ErrorHandlerMiddleware>();


// Use authentication and authorization
app.UseAuthentication();
app.UseAuthorization();


app.UseStaticFiles();




app.MapControllers();



// Use SignalR endpoints 
app.MapHub<NotificationHub>("/notificationHub");

app.Run();

