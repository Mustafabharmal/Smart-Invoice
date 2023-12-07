using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Invoice_web_app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartInvoice.Controllers
{
    public class CategoryController : Controller
    {
        // GET: /<controller>/
        private readonly UserDBContext _context;
        public CategoryController(UserDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var role = HttpContext.Session.GetInt32("Role");
            var Storeid = HttpContext.Session.GetInt32("StoreId");

            var Category = await _context.Tcategory
           .Where(u => u.status == 1 && u.store_id== Storeid) // Add this line to filter by status
           .OrderBy(u => u.updated_at)
           .ToListAsync();

            var shop = await _context.Tstore
                .Where(u => u.status == 1) // Add this line to filter by status
                .OrderBy(u => u.updated_at)
                .ToListAsync();
            //ViewBag.Shop = shop;
            if (role == 0)
            {
                 Category = await _context.Tcategory
            .Where(u => u.status == 1) // Add this line to filter by status
            .OrderBy(u => u.updated_at)
            .ToListAsync();

                shop = await _context.Tstore
                .Where(u => u.status == 1) // Add this line to filter by status
                .OrderBy(u => u.updated_at)
                .ToListAsync();
            }


            ViewBag.Shop = shop;

            ViewBag.Category = Category;


            return View();
        }
        public async Task<IActionResult> Create()
        {

            var shop = _context.Tstore
               .Where(u => u.status == 1) // Add this line to filter by status
               .OrderBy(u => u.updated_at)
               .ToList();
            ViewBag.Shop = shop;
            //if (role == 0)
            //{
                
            //    shop = await _context.Tstore
            //    .Where(u => u.status == 1) // Add this line to filter by status
            //    .OrderBy(u => u.updated_at)
            //    .ToListAsync();
            //}
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add()
        {
            string categoryName = Request.Form["CategoryName"];
            string categoryCode = Request.Form["CategoryCode"];
            string description = Request.Form["Description"];

            int? storeIdNullable = HttpContext.Session.GetInt32("StoreId");
            var myrole = HttpContext.Session.GetInt32("Role");
            int storeId = storeIdNullable ?? 0;
            if (myrole == 0)
            {
                storeId = int.Parse(Request.Form["shopid"]);
            }
            // Validate or process the form data as needed
            if (string.IsNullOrEmpty(categoryName) || string.IsNullOrEmpty(categoryCode))
            {
                ViewBag.ErrorMessage = "Category Name and Code are required.";
                return View();
            }

            var category = new Category
            {
                product_name = categoryName,
                code = categoryCode,
                description = description,
                created_at = DateTime.Now,
                updated_at = DateTime.Now,
                store_id= storeId,
                status=1
                // Add other properties as needed
            };

            // Handle category image upload
            IFormFile categoryImage = Request.Form.Files["CategoryImage"];
            if (categoryImage != null && categoryImage.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    categoryImage.CopyTo(memoryStream);
                    category.image = memoryStream.ToArray();
                }
            }

            // Save the category to the database
            _context.Tcategory.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var shop = _context.Tstore
             .Where(u => u.status == 1) // Add this line to filter by status
             .OrderBy(u => u.updated_at)
             .ToList();
            ViewBag.Shop = shop;

            var user = _context.Tcategory.FirstOrDefault(u => u.category_id == id);

            if (user == null)
            {
                // User not found, handle accordingly (e.g., show an error message)
                return RedirectToAction("Index");
            }

            // Pass the user details to the view
            ViewBag.Category = user;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Update()
        {
            int Catid = int.Parse(Request.Form["category_id"]);
            string categoryName = Request.Form["CategoryName"];
            string categoryCode = Request.Form["CategoryCode"];
            string description = Request.Form["Description"];

            int? storeIdNullable = HttpContext.Session.GetInt32("StoreId");
            var myrole = HttpContext.Session.GetInt32("Role");
            int storeId = storeIdNullable ?? 0;
            if (myrole == 0)
            {
                storeId = int.Parse(Request.Form["shopid"]);
            }

            // Find the existing category
            var existingCategory = _context.Tcategory.FirstOrDefault(c => c.category_id == Catid);

            if (existingCategory != null)
            {
                // Check if a CategoryImage file was provided
                byte[] categoryImageData = null;
                IFormFile categoryImageFile = Request.Form.Files["image"]; // Assuming "image" is the correct form field name
                if (categoryImageFile != null && categoryImageFile.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        categoryImageFile.CopyTo(ms);
                        categoryImageData = ms.ToArray();
                    }
                }

                // Update the existing category properties
                existingCategory.product_name = categoryName;
                existingCategory.code = categoryCode;
                existingCategory.description = description;
                existingCategory.store_id = storeId; // You might not need to update the store_id, depending on your use case
                existingCategory.updated_at = DateTime.Now;

                if (categoryImageData != null)
                {
                    // Update image only if a new image is provided
                    existingCategory.image = categoryImageData;
                }

                // Save changes to the database
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
                }
            return RedirectToAction("Index");

        }
        public IActionResult Delete(int id)
        {
            // Fetch user details by id and pass them to the view for confirmation
            try
            {
                var user = _context.Tcategory.FirstOrDefault(u => u.category_id == id);
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

