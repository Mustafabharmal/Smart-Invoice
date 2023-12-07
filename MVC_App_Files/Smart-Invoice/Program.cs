using Microsoft.EntityFrameworkCore;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Invoice_web_app.Models;
using Microsoft.Extensions.Configuration;
//using DBfirst.Models;
//using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession();
string connection = builder.Configuration.GetConnectionString("DefualtConnection");
builder.Services.AddSingleton<ProductSelection>();
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
    pattern: "{controller=Home}/{action=Index}/{id?}",
    defaults: new { controller = "Home", action = "Index" }
);

app.MapControllerRoute(
    name: "login",
    pattern: "{controller=Login}/{action=Index}/{id?}",
    defaults: new { controller = "Login", action = "Index" }
);
//app.MapControllerRoute(
//    name: "Employees",
//    pattern: "{controller=Employees}/{action=Index}/{id?}");


//app.Use(async (context, next) =>
//{
//    // Check if user is authenticated based on the presence of 'user_id' in session
//    var userId = context.Session.GetInt32("UserId");
//    var path = context.Request.Path;

//    if (userId == null && !path.StartsWithSegments("/Login", StringComparison.OrdinalIgnoreCase))
//    {
//        // User is not authenticated and not already on the login page, redirect to the login page
//        context.Response.Redirect("/Login/Index");
//        return;
//    }

//    // User is authenticated or already on the login page, continue to the next middleware
//    await next();
//});
app.Run();

