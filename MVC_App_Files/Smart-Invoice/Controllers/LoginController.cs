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
using Microsoft.AspNetCore.Identity;
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
            
            //var employee =  _context.Tuser.FirstOrDefault(m => m.email == email && m.pwd == password && m.status==1);

            //if (employee == null)
            //{
            //    ViewBag.faillogin = "Password or Email is not registered";
            //    return View("Index");// Return the login page with an error message
            //}

            var employee = _context.Tuser.FirstOrDefault(m => m.email == email && m.status == 1);

            if (employee == null)
            {
                ViewBag.faillogin = "Email is not registered or You are waiting for approval";
                return View("Index"); // Return the login page with an error message
            }

            var passwordHasher = new PasswordHasher<object>();

            // Verify the hashed password
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(null, employee.pwd, password);

            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                // Password is correct
                // Proceed with login logic
                ViewBag.faillogin = "Invalid password";
                return View("Index");
            }
    
            // Check other conditions if needed (e.g., IsActive)

            // Set session variables
            HttpContext.Session.SetString("Email", employee.email);
            HttpContext.Session.SetInt32("Role", employee.role);
            HttpContext.Session.SetInt32("UserId", employee.user_id);
            HttpContext.Session.SetString("FirstName", employee.first_name);
            HttpContext.Session.SetString("LastName", employee.last_name);
            HttpContext.Session.SetInt32("StoreId", employee.store_id);
            HttpContext.Session.SetString("Username", employee.username);
            HttpContext.Session.SetString("Phone", employee.phone);
            
            if (employee.avatar != null)
            {
                string avatarBase64 = Convert.ToBase64String(employee.avatar);
                // Store the Base64 string in the session
                HttpContext.Session.SetString("Avtar", avatarBase64);
            }
            else
            {
                HttpContext.Session.SetString("Avtar", "");
            }
            // Redirect to the desired page after successful login
            if(employee.role==2)
            {
                return RedirectToAction("Index", "POS");

            }
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

