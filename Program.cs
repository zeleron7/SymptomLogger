using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
//using SymptomLogger.src.AutoMapper;
using SymptomLogger.src.Data;
using SymptomLogger.src.Entity;
using SymptomLogger.src.Middleware;
using SymptomLogger.src.Repositories;
using SymptomLogger.src.Repositories.Interfaces;
using SymptomLogger.src.Services;
using SymptomLogger.src.Services.Interfaces;
using SymptomLogger.src.Validators;
using System.Text;

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
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); < older way, which can lead to issues if there are multiple profiles or assemblies. 
//builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

// Dependency Injection 
#region DI
// DI Symptoms
builder.Services.AddScoped<ISymptomRepository, SymptomRepository>();
builder.Services.AddScoped<ISymptomService, SymptomService>();

// DI SymptomLogs
builder.Services.AddScoped<ISymptomLogRepository, SymptomLogRepository>();
builder.Services.AddScoped<ISymptomLogService, SymptomLogService>();

// DI Conditions
builder.Services.AddScoped<IConditionRepository, ConditionRepository>();
builder.Services.AddScoped<IConditionService, ConditionService>();

// DI Seeder
builder.Services.AddScoped<ISeedDatabaseService, SeedDatabaseService>();
builder.Services.AddScoped<ConditionSeeder>();
#endregion 

// Register FluentValidation validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateSymptomDTOValidator>();

// Enable automatic validation of DTOs using FluentValidation
builder.Services.AddFluentValidationAutoValidation();

// Global exception handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Configure routing to use lowercase URLs
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

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
builder.Services.AddIdentity<User, IdentityRole>(options =>
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

// Cors configuration to allow Streamlit frontend to access the API
#region CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowStreamlit",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:8501") // Streamlit default port
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
#endregion 

var app = builder.Build();

// Seed conditions on startup
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<ConditionSeeder>();
    seeder.SeedConditions();
}

app.UseExceptionHandler();

// Use CORS policy
app.UseCors("AllowStreamlit");

app.UseSwagger();

app.UseSerilogRequestLogging(options
  => options.EnrichDiagnosticContext = RequestEnricher.LogRequestDetails); // Enrich logs with additional request info (e.g., IP, User-Agent)

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

Log.CloseAndFlush();
