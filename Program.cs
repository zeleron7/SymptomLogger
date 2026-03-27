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

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SymptomLoggerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SymptomLoggerContext") ?? throw new InvalidOperationException("Connection string 'SymptomLoggerContext' not found.")));

// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.

// NewtonsoftJson for JSON Patch
builder.Services.AddControllers()
    .AddNewtonsoftJson();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddEndpointsApiExplorer();

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// DI Symptoms
builder.Services.AddScoped<ISymptomRepository, SymptomRepository>();
builder.Services.AddScoped<ISymptomService, SymptomService>();

// DI Seeder
builder.Services.AddScoped<ISeedDatabaseService, SeedDatabaseService>();

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

// Global exception handling middleware
app.UseMiddleware<SymptomLogger.src.Middleware.ExceptionHandlingMiddleware>();

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
