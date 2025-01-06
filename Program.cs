using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using dotnet_webapi_claude_wrapper.Configuration;
using dotnet_webapi_claude_wrapper.DataModel;
using dotnet_webapi_claude_wrapper.DataModel.Entities;
using dotnet_webapi_claude_wrapper.Repositories;
using dotnet_webapi_claude_wrapper.Services;
using Anthropic;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add JWT Configuration
var jwtConfig = builder.Configuration
    .GetSection("JwtConfig")
    .Get<JwtConfig>() ?? throw new InvalidOperationException("JwtConfig is missing in configuration");
builder.Services.AddSingleton(jwtConfig);

// Add SQLite Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig.Issuer,
        ValidAudience = jwtConfig.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(jwtConfig.Secret))
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger with JWT authentication
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "dotnet_webapi_claude_wrapper API", Version = "v1" });
    
    // Configure JWT authentication in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer 12345abcdef'",
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
            Array.Empty<string>()
        }
    });
});

// Register base services
builder.Services.AddScoped<IBaseRepository, BaseRepository>();
builder.Services.AddScoped<IBaseService, BaseService>();

// Register User services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// Add Claude configuration
builder.Services.Configure<ClaudeSettings>(
    builder.Configuration.GetSection("ClaudeSettings"));

// Register Anthropic client
builder.Services.AddSingleton<IAnthropicClient>(sp => 
{
    var settings = sp.GetRequiredService<IOptions<ClaudeSettings>>().Value;
    return new Anthropic.AnthropicClient(settings.ApiKey);
});

// Register Claude service
builder.Services.AddScoped<IClaudeService, ClaudeService>();

// Add repository
builder.Services.AddScoped<IClaudeRepository, ClaudeRepository>();

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Add HttpClient
builder.Services.AddHttpClient<ClaudeService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "dotnet_webapi_claude_wrapper API V1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();