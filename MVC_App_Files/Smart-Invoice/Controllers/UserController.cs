using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Invoice_web_app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartInvoice.Controllers
{
    public class UserController : Controller
    {
        // GET: /<controller>/
        private readonly UserDBContext _context;
        public UserController(UserDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            //var users = await _context.Tuser.ToListAsync();
            //ViewBag.faillogin = null;
            //return View(users);
            //var users = _context.Tuser.ToList();
            var users = await _context.Tuser.OrderBy(u => u.updated_at).ToListAsync();
            ViewBag.Users = users;
            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create()
        {
            // Retrieve form values using Request
            string firstName = Request.Form["first_name"];
            string lastName = Request.Form["last_name"];
            string userName = Request.Form["username"];
            string password = Request.Form["pwd"];
            string phone = Request.Form["phone"];
            string email = Request.Form["email"];
            string role = Request.Form["role"];

            // Validate or process the form data as needed
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(email))
            {
                ViewBag.ErrorMessage = "First Name and Email are required.";
                return View();
            }

            // Create a user and add it to the list
            var newUser = new User
            {
                first_name = firstName,
                last_name = lastName,
                username = userName,
                pwd = password,
                phone = phone,
                email = email,
                role = role,
                avatar=null,
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };

            _context.Tuser.Add(newUser);
            _context.SaveChanges();


            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var user = _context.Tuser.FirstOrDefault(u => u.user_id == id);

            if (user == null)
            {
                // User not found, handle accordingly (e.g., show an error message)
                return RedirectToAction("Index");
            }

            // Pass the user details to the view
            ViewBag.User = user;

            return View();
        }

        [HttpPost]
        public IActionResult Update()
        {
            // Retrieve data from the form or request parameters
            int userId = int.Parse(Request.Form["userId"]);
            string firstName = Request.Form["first_name"];
            string lastName = Request.Form["last_name"];
            string username = Request.Form["username"];
            string phone = Request.Form["phone"];
            string email = Request.Form["email"];
            string role = Request.Form["role"];

            // Find the user in the database
            var user = _context.Tuser.FirstOrDefault(u => u.user_id == userId);

            if (user == null)
            {
                // User not found, handle accordingly (e.g., show an error message)
                return RedirectToAction("Index");
            }

            // Update user details
            user.first_name = firstName;
            user.last_name = lastName;
            user.username = username;
            user.phone = phone;
            user.email = email;
            user.role = role;

            // Save changes to the database
            _context.SaveChanges();

            return RedirectToAction("Index");
        }



        public IActionResult Delete(int id)
        {
            // Fetch user details by id and pass them to the view for confirmation
            try
            {
                var user = _context.Tuser.FirstOrDefault(u => u.user_id == id);

                if (user != null)
                {
                    _context.Tuser.Remove(user);
                    _context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return RedirectToAction("Index");
            }
        }

        //[HttpPost]
        //public IActionResult ConfirmDelete(int id)
        //{
        //    // Delete user from the database
            
        //}
    }
}

