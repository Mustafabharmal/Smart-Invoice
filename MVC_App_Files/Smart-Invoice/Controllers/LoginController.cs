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

        public LoginController(UserDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            //string connectionString = "data source = localhost;initial catalog = SmartInvoice; user id = sa; password=reallyStrongPwd123";
            //var products = _context.Tuser.ToList();
            //return View(products);
            //var users = _context.Tuser.ToList();
            //ViewBag.Users = users;
            return View();
        }
        public IActionResult OnPostSignIn(String email, String password)
        {
            //return RedirectToAction("Index", "Product");
            var employee =  _context.Tuser.First(m => m.email == email && m.pwd == password);

            if (employee == null)
            {
                ViewBag.faillogin = "Password or email id is wrong";
                return View(); // Return the login page with an error message
            }

            // Check other conditions if needed (e.g., IsActive)

            // Set session variables
            //HttpContext.Session.SetString("Email", employee.email);
            //HttpContext.Session.SetString("Role", employee.role);
            //HttpContext.Session.SetInt32("Id", employee.user_id);

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

