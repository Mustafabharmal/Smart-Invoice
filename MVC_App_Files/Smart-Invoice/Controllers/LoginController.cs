using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Invoice_web_app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Invoice_web_app.Controllers
{
    public class LoginController : Controller
    {
        //UserDBContext db = new UserDBContext();
        private readonly UserDBContext _context;
        //[BindProperty]
        //public string Email { get; set; }
        //public string Password { get; set; }

        public LoginController(UserDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            //string connectionString = "data source = localhost;initial catalog = SmartInvoice; user id = sa; password=reallyStrongPwd123";
            var users = await _context.Tuser.ToListAsync();
            //ViewBag.faillogin = null;
            //return View(users);
            //var users = _context.Tuser.ToList();
            //ViewBag.Users = users;
            return View();
        }
        public IActionResult SignIn()
        {
            //return RedirectToAction("Index", "Product");
            string email = Request.Form["Email"];
            string password = Request.Form["Password"];
            var employee =  _context.Tuser.FirstOrDefault(m => m.email == email && m.pwd == password);

            if (employee == null)
            {
                ViewBag.faillogin = "Password or email id is not registered";
                return View("Index");// Return the login page with an error message
            }

            // Check other conditions if needed (e.g., IsActive)

            // Set session variables
            HttpContext.Session.SetString("Email", employee.email);
            HttpContext.Session.SetString("Role", employee.role);
            HttpContext.Session.SetInt32("UserId", employee.user_id);
            HttpContext.Session.SetString("FirstName", employee.first_name);
            HttpContext.Session.SetString("LastName", employee.last_name);
            HttpContext.Session.SetString("Username", employee.username);
            HttpContext.Session.SetString("Phone", employee.phone);

            // Redirect to the desired page after successful login
            return RedirectToAction("Index", "Home");
   
        }

        public IActionResult Forget()
        {
            return View();
        }
        
        public IActionResult SignUp()
        {
            return View();
        }
    }
}

