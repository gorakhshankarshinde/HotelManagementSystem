using HotelManagement.WebApi.Data;
using Microsoft.EntityFrameworkCore;

using DotNetEnv;

DotNetEnv.Env.Load(); // ✅ Load before builder is created

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the configuration so it can be injected
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();               // Enable Swagger middleware
    app.UseSwaggerUI();             // Enable Swagger UI
}

// Enable Swagger UI in all environments
app.UseSwagger();               // Enable Swagger middleware
app.UseSwaggerUI();             // Enable Swagger UI

// Redirect the root URL (/) to Swagger UI
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

// Enable CORS policy here:
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();
app.MapControllers();

//app.Run();

// Ensure the app listens on the correct port defined by the environment (Render usually provides the PORT variable)
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");
