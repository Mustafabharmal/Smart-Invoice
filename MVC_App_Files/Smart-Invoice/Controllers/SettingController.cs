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
    public class SettingController : Controller
    {
        // GET: /<controller>/
        private readonly UserDBContext _context;
        public SettingController(UserDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var id = HttpContext.Session.GetInt32("UserId");
            var user = _context.Tuser.FirstOrDefault(u => u.user_id == id);

            if (user == null)
            {
                // User not found, handle accordingly (e.g., show an error message)
                return RedirectToAction("Index");
            }

            // Pass the user details to the view
            ViewBag.User = user;
            var shop = _context.Tstore
                .Where(u => u.status == 1) // Add this line to filter by status
                .OrderBy(u => u.updated_at)
                .ToList();
            ViewBag.Shop = shop;
            return View();
        }
        public IActionResult Forget()
        {
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
            var shop = _context.Tstore
                .Where(u => u.status == 1) // Add this line to filter by status
                .OrderBy(u => u.updated_at)
                .ToList();
            ViewBag.Shop = shop;
            return RedirectToAction("Index");
        }

    }
}

