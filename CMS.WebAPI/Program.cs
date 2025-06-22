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


var builder = WebApplication.CreateBuilder(args);

MapsterConfig.RegisterMappings();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ DbContext Kaydı
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Servis Kayıtları
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IContentService, ContentService>();

// ✅ Repository Kayıtları
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IContentRepository, ContentRepository>();

// ✅ MemoryCache veya Redis
builder.Services.AddMemoryCache(); // İstersen RedisCache'e geçebilirsin

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapControllers();

app.Run();
