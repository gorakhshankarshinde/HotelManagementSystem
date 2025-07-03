using DotNetEnv;
using HotelManagement.WebApi;
using HotelManagement.WebApi.Data;
using HotelManagement.WebApi.Models; // for CsvSettings
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

DotNetEnv.Env.Load(); // Load environment variables from .env if present

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register IConfiguration
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Setup PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Bind CsvSettings from appsettings.json
var csvSettings = builder.Configuration.GetSection("CsvSettings").Get<CsvSettings>();

// Register CsvMenuItemService
builder.Services.AddSingleton<IMenuItemService>(sp =>
{
    var env = sp.GetRequiredService<IWebHostEnvironment>();
    string menuCsvPath = csvSettings.RunAsLocally
        ? Path.Combine(env.WebRootPath, csvSettings.LocalMenuItemCsvPath)
        : csvSettings.ProdMenuItemCsvPath;

    return new CsvMenuItemService(menuCsvPath);
});

// Register CsvOrderService
builder.Services.AddSingleton<IOrderService>(sp =>
{
    var env = sp.GetRequiredService<IWebHostEnvironment>();
    string orderCsvPath = csvSettings.RunAsLocally
        ? Path.Combine(env.WebRootPath, csvSettings.LocalOrderCsvPath)
        : csvSettings.ProdOrderCsvPath;

    return new CsvOrderService(orderCsvPath);
});




// CORS setup
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("https://gorakhshankarshinde.github.io")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Swagger setup
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();       // Enable Swagger in all environments
app.UseSwaggerUI();

// Redirect "/" to "/swagger"
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger", permanent: true);
    }
    else
    {
        await next.Invoke();
    }
});

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();
app.MapControllers();

// ✅ Render deployment support — bind to PORT environment variable
var port = Environment.GetEnvironmentVariable("PORT");

if (!string.IsNullOrEmpty(port))
{
    app.Run($"http://0.0.0.0:{port}");
}
else
{
    app.Run(); // Local development
}
