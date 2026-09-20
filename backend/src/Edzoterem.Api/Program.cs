using System.Text;
using Edzoterem.Api.Middleware;
using Edzoterem.Application.Interfaces;
using Edzoterem.Application.Security;
using Edzoterem.Application.Services;
using Edzoterem.Infrastructure.Data;
using Edzoterem.Infrastructure.Data.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Edzőterem API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT token: Bearer {token}"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Hiányzik a 'Default' connection string az appsettings.json-ból.");

// Fix verzió megadva AutoDetect helyett, hogy migráció/build ne igényeljen élő adatbázis-kapcsolatot.
var mySqlServerVersion = new MySqlServerVersion(new Version(8, 0, 35));
builder.Services.AddDbContext<EdzoteremDbContext>(options =>
    options.UseMySql(connectionString, mySqlServerVersion));

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));
var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
    ?? throw new InvalidOperationException("Hiányzik a 'Jwt' konfigurációs szekció az appsettings.json-ból.");

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
        ValidIssuer = jwtSettings.Kiado,
        ValidAudience = jwtSettings.Kozonseg,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Kulcs))
    };
});

builder.Services.AddAuthorization();

const string CorsPolicyNev = "FrontendPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyNev, policy =>
    {
        policy.WithOrigins(
                builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                ?? new[] { "http://localhost:5173" })
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDolgozokService, DolgozokService>();
builder.Services.AddScoped<ITagokService, TagokService>();
builder.Services.AddScoped<IBerletekService, BerletekService>();
builder.Services.AddScoped<ICsoportosFoglalkozasokService, CsoportosFoglalkozasokService>();
builder.Services.AddScoped<IEgyeniFoglalkozasokService, EgyeniFoglalkozasokService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<EdzoteremDbContext>();
    db.Database.Migrate();
    DbSeeder.Seed(db);
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors(CorsPolicyNev);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
