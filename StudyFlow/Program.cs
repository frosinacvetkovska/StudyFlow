using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Data;
using StudyFlow.Models.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
    Console.WriteLine($"Connection string: {connectionString}");

builder.Services.AddDefaultIdentity<StudyFlowUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Add a health check endpoint
app.Map("/health", healthApp =>
{
    healthApp.Run(async context =>
    {
        context.Response.StatusCode = 200;
        await context.Response.WriteAsync("Healthy");
    });
});



app.Run();

public partial class Program { }
