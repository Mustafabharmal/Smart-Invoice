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
    public class StoreController : Controller
    {
        // GET: /<controller>/
        private readonly UserDBContext _context;
        public StoreController(UserDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var shop = await _context.Tstore
                  .Where(u => u.status == 1) // Add this line to filter by status
                  .OrderBy(u => u.updated_at)
                  .ToListAsync();
            ViewBag.Shop = shop;
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add()
        {
            // Retrieve form values using Request
            string storeName = Request.Form["store_name"];
            string phone = Request.Form["phone"];
            string email = Request.Form["email"];
            string address = Request.Form["Address"];
            string taxId = Request.Form["Tax_ID"];

            // Validate or process the form data as needed
            if (string.IsNullOrEmpty(storeName) || string.IsNullOrEmpty(email))
            {
                ViewBag.ErrorMessage = "Store Name and Email are required.";
                return View();
            }

            // Check if an image file was provided
            byte[] logoData = null;
            if (Request.Form.Files.Count > 0)
            {
                var logoFile = Request.Form.Files[0];
                using (MemoryStream ms = new MemoryStream())
                {
                    logoFile.CopyTo(ms);
                    logoData = ms.ToArray();
                }
            }

            // Create a store and add it to the database
            var newStore = new Store
            {
                store_name = storeName,
                phone = phone,
                email = email,
                Address = address,
                Tax_ID = taxId,
                logo = logoData,
                status=1,
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };

            _context.Tstore.Add(newStore);
            await _context.SaveChangesAsync();

            // Redirect to the store list or another appropriate action
            return RedirectToAction("Index", "Store");
        }

        public IActionResult Edit(int id)
        {
            var user = _context.Tstore.FirstOrDefault(u => u.store_id == id);

            if (user == null)
            {
                // User not found, handle accordingly (e.g., show an error message)
                return RedirectToAction("Index");
            }

            // Pass the user details to the view
            ViewBag.store = user;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Update()
        {
            int storeId = int.Parse(Request.Form["store_id"]);
            string storeName = Request.Form["store_name"];
            string phone = Request.Form["phone"];
            string email = Request.Form["email"];
            string address = Request.Form["Address"];
            string taxId = Request.Form["Tax_ID"];

            // Find the store in the database
            var store = _context.Tstore.FirstOrDefault(s => s.store_id == storeId);

            if (store == null)
            {
                // Store not found, handle accordingly (e.g., show an error message)
                return RedirectToAction("Index");
            }

            // Check if a new image file was provided
            byte[] newLogoData = null;
            if (Request.Form.Files.Count > 0)
            {
                var newLogoFile = Request.Form.Files[0];
                using (MemoryStream ms = new MemoryStream())
                {
                    newLogoFile.CopyTo(ms);
                    newLogoData = ms.ToArray();
                }
            }

            // Update store details
            store.store_name = storeName;
            store.phone = phone;
            store.email = email;
            store.Address = address;
            store.Tax_ID = taxId;

            if (newLogoData != null)
            {
                // Update logo only if a new image is provided
                store.logo = newLogoData;
            }

            // Save changes to the database
            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            // Fetch user details by id and pass them to the view for confirmation
            try
            {
                var user = _context.Tstore.FirstOrDefault(u => u.store_id == id);
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
    }
}

