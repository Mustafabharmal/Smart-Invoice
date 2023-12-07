using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Invoice_web_app.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
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
            var role = HttpContext.Session.GetInt32("Role");
            var Storeid= HttpContext.Session.GetInt32("StoreId");
            var users = await _context.Tuser
                  .Where(u => u.status == 1 && u.store_id == Storeid) // Add this line to filter by status
                  .OrderBy(u => u.updated_at)
                  .ToListAsync();
            if (role==0)
            {
                 users = await _context.Tuser
               .Where(u => u.status == 1) // Add this line to filter by status
               .OrderBy(u => u.updated_at)
               .ToListAsync();
            }

            var shop = await _context.Tstore
                  .Where(u => u.status == 1) // Add this line to filter by status
                  .OrderBy(u => u.updated_at)
                  .ToListAsync();
            ViewBag.Shop = shop;


            ViewBag.Users = users;
            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult AddUser()
        {
            var shop =  _context.Tstore
                .Where(u => u.status == 1) // Add this line to filter by status
                .OrderBy(u => u.updated_at)
                .ToList();
            ViewBag.Shop = shop;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormFile avatar)
        {
            // Retrieve form values using Request
            string firstName = Request.Form["first_name"];
            string lastName = Request.Form["last_name"];
            string userName = Request.Form["username"];
            string password = Request.Form["pwd"];
            string phone = Request.Form["phone"];
            string email = Request.Form["email"]; 
            int role = int.Parse(Request.Form["role"]);
            int? storeIdNullable = HttpContext.Session.GetInt32("StoreId");
            int? Userid = HttpContext.Session.GetInt32("UserId");
            // Handle the nullable case
            var myrole = HttpContext.Session.GetInt32("Role");
            int storeId = storeIdNullable ?? 0;
            if (role==0)
            {
                storeId = 0;
            }
            if(myrole==0)
            {
                storeId = int.Parse(Request.Form["shopid"]);
            }
            var passwordHasher = new PasswordHasher<object>();
            var hashedPassword = passwordHasher.HashPassword(null, password);

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(email))
            {
                ViewBag.ErrorMessage = "First Name and Email are required.";
                return View();
            }

            // Check if an avatar file was provided
            byte[] avatarData = null;
            if (avatar != null && avatar.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    avatar.CopyTo(ms);
                    avatarData = ms.ToArray();
                }
            }
            // Create a user and add it to the list
            var newUser = new User
            {
                first_name = firstName,
                last_name = lastName,
                username = userName,
                pwd = hashedPassword,
                phone = phone,
                email = email,
                store_id= storeId,
                role = role,
                status = 1,
                avatar = avatarData,
              
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };

            _context.Tuser.Add(newUser);
            _context.SaveChanges();
            var shop =  _context.Tstore
                .Where(u => u.status == 1) // Add this line to filter by status
                .OrderBy(u => u.updated_at)
                .ToList();
            ViewBag.Shop = shop;

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
            var shop =  _context.Tstore
                .Where(u => u.status == 1) // Add this line to filter by status
                .OrderBy(u => u.updated_at)
                .ToList();
            ViewBag.Shop = shop;
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
            int role = int.Parse(Request.Form["role"]);
            var myrole = HttpContext.Session.GetInt32("Role");
            int? storeIdNullable = HttpContext.Session.GetInt32("StoreId");
            int storeId = storeIdNullable ?? 0;
            if (myrole == 0)
            {
                storeId = int.Parse(Request.Form["shopid"]);
            }
            // Find the user in the database
            var user = _context.Tuser.FirstOrDefault(u => u.user_id == userId);

            if (user == null)
            {
                // User not found, handle accordingly (e.g., show an error message)
                return RedirectToAction("Index");
            }
            IFormFile imageFile = Request.Form.Files["avatar"];
            if (imageFile != null && imageFile.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    imageFile.CopyTo(ms);
                    user.avatar = ms.ToArray();
                }
            }
            // Update user details
            user.first_name = firstName;
            user.last_name = lastName;
            user.username = username;
            user.phone = phone;
            user.email = email;
            user.role = role;
            user.store_id = storeId;
            user.status = 1;
            user.updated_at = DateTime.Now;
            // Save changes to the database
            _context.SaveChanges();
            var shop =  _context.Tstore
                .Where(u => u.status == 1) // Add this line to filter by status
                .OrderBy(u => u.updated_at)
                .ToList();
            ViewBag.Shop = shop;
            return RedirectToAction("Index");
        }



        public IActionResult Delete(int id)
        {
            // Fetch user details by id and pass them to the view for confirmation
            try
            {
                var user = _context.Tuser.FirstOrDefault(u => u.user_id == id);
                user.status = 0;
                if (user != null)
                {
                    //_context.Tuser.Remove(user);
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

