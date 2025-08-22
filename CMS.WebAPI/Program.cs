using CMS.Application.Interfaces;
using CMS.Domain.Interfaces;
using CMS.Infrastructure.Data;
using CMS.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using CMS.Application.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using CMS.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CMS.Infrastructure.Data;


var builder = WebApplication.CreateBuilder(args);

MapsterConfig.RegisterMappings();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "CMS API",
		Version = "v1",
		Description = "Content Management System Web API"
	});

	// JWT Authentication for Swagger
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
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

// ✅ DbContext Kaydı
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Servis Kayıtları
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// ✅ Repository Kayıtları
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IContentRepository, ContentRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// ✅ MemoryCache 
builder.Services.AddMemoryCache();

// ✅ JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? "YourSuperSecretKeyHere12345678901234567890");

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.RequireHttpsMetadata = false;
	options.SaveToken = true;
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(key),
		ValidateIssuer = true,
		ValidIssuer = jwtSettings["Issuer"] ?? "CMS_API",
		ValidateAudience = true,
		ValidAudience = jwtSettings["Audience"] ?? "CMS_Users",
		ValidateLifetime = true,
		ClockSkew = TimeSpan.Zero
	};
});

// ✅ Authorization
builder.Services.AddAuthorization();

// ✅ CORS Ayarları
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowReactApp",
		policy => policy.WithOrigins("http://localhost:3000") // React çalıştığı port
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials());
});

var app = builder.Build();

app.UseCors("AllowReactApp");

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await SeedData.SeedDataAsync(context);
}

app.Run();
