using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using SymptomLogger.src.Data;
using SymptomLogger.src.Repositories.Interfaces;
using SymptomLogger.src.Services;
using Serilog;
using SymptomLogger.src.Repositories;
using Microsoft.AspNetCore.Identity;
using SymptomLogger.src.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using SymptomLogger.src.Validators;
using FluentValidation.AspNetCore;
using SymptomLogger.src.Middleware;
using Microsoft.AspNetCore.Mvc;
using SymptomLogger.src.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SymptomLoggerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SymptomLoggerContext") ?? throw new InvalidOperationException("Connection string 'SymptomLoggerContext' not found.")));

// Serilog configuration
#region Serilog configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Host.UseSerilog();
#endregion 

// NewtonsoftJson for JSON Patch
builder.Services.AddControllers()
    .AddNewtonsoftJson();

// API versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;  // Header info for clients to see the supported versions
});

builder.Services.AddEndpointsApiExplorer();

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Dependency Injection 
#region DI
// DI Symptoms
builder.Services.AddScoped<ISymptomRepository, SymptomRepository>();
builder.Services.AddScoped<ISymptomService, SymptomService>();

// DI SymptomLogs
builder.Services.AddScoped<ISymptomLogRepository, SymptomLogRepository>();
builder.Services.AddScoped<ISymptomLogService, SymptomLogService>();

// DI Seeder
builder.Services.AddScoped<ISeedDatabaseService, SeedDatabaseService>();
#endregion 

// Register FluentValidation validators
builder.Services.AddValidatorsFromAssemblyContaining<CreatePatientDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateSymptomDTOValidator>();

// Enable automatic validation of DTOs using FluentValidation
builder.Services.AddFluentValidationAutoValidation();

// Global exception handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // JWT Auth
    #region JWT Auth
    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", document)] = []
    });
    #endregion 
});

// Identity and JWT configuration
#region Identity and JWT configuration
// Configure Identity
builder.Services.AddIdentity<Patient, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    // User settings
    //options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<SymptomLoggerContext>()
.AddDefaultTokenProviders();
// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true, 
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
#endregion 

var app = builder.Build();

app.UseSwagger();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

// Secure the API with authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
