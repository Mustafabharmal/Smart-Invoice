using Microsoft.EntityFrameworkCore;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Invoice_web_app.Models;
using Microsoft.Extensions.Configuration;
//using DBfirst.Models;
//using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession();
string connection = builder.Configuration.GetConnectionString("DefualtConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<UserDBContext>(options =>
{
    options.UseSqlServer(connection);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();  // Add this line

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");
//app.MapControllerRoute(
//    name: "Employees",
//    pattern: "{controller=Employees}/{action=Index}/{id?}");

app.Run();

