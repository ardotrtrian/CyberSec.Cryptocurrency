using CyberSec.Cryptocurrency.API.Middlewares;
using CyberSec.Cryptocurrency.Service.Interfaces;
using CyberSec.Cryptocurrency.Service.Models.Settings;
using CyberSec.Cryptocurrency.Service.Persistence;
using CyberSec.Cryptocurrency.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSettings"));

// configure DI for application services
builder.Services.AddScoped<IDate, Date>();
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddDbContext<CryptocurrencyContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

// global cors policy
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();

// custom jwt auth middleware
app.UseMiddleware<JwtMiddleware>();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
