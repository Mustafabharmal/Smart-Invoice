using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Invoice_web_app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Invoice_web_app.Controllers
{
    public class ProductController : Controller
    {
        // GET: /<controller>/
        private readonly UserDBContext _context;
        public ProductController(UserDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var role = HttpContext.Session.GetInt32("Role");
            var Storeid = HttpContext.Session.GetInt32("StoreId");
            var Product = await _context.Tproduct
                 .Where(u => u.status == 1 && u.store_id == Storeid) // Add this line to filter by status
                 .OrderBy(u => u.updated_at )
                 .ToListAsync();
            var Cat = await _context.Tcategory
                  .Where(u => u.status == 1 && u.store_id == Storeid) // Add this line to filter by status
                  .OrderBy(u => u.updated_at)
                  .ToListAsync();
            var User = await _context.Tstore
                  .Where(u => u.status == 1) // Add this line to filter by status
                  .OrderBy(u => u.updated_at)
                  .ToListAsync();
            if (role==0)
            {
                 Product = await _context.Tproduct
                .Where(u => u.status == 1 ) // Add this line to filter by status
                .OrderBy(u => u.updated_at)
                .ToListAsync();
                 Cat = await _context.Tcategory
                      .Where(u => u.status == 1) // Add this line to filter by status
                      .OrderBy(u => u.updated_at)
                      .ToListAsync();
                 User = await _context.Tstore
                      .Where(u => u.status == 1) // Add this line to filter by status
                      .OrderBy(u => u.updated_at)
                      .ToListAsync();
            }
           
            ViewBag.Product = Product;
            ViewBag.cat = Cat;
            ViewBag.Use = User;
            return View();
        }
        public async Task<IActionResult> Create()
        {
            var role = HttpContext.Session.GetInt32("Role");
            var Storeid = HttpContext.Session.GetInt32("StoreId");
            var Cat = await _context.Tcategory
                .Where(u => u.status == 1 && u.store_id == Storeid) // Add this line to filter by status
                .OrderBy(u => u.updated_at)
                .ToListAsync();
            //ViewBag.cat = Cat;
            var store = await _context.Tstore
                  .Where(u => u.status == 1 && u.store_id == Storeid) // Add this line to filter by status
                  .OrderBy(u => u.updated_at)
                  .ToListAsync();
            //ViewBag.sto = store;
            if (role == 0)
            {
                Cat = await _context.Tcategory
                     .Where(u => u.status == 1) // Add this line to filter by status
                     .OrderBy(u => u.updated_at)
                     .ToListAsync();
                store = await _context.Tstore
                     .Where(u => u.status == 1) // Add this line to filter by status
                     .OrderBy(u => u.updated_at)
                     .ToListAsync();
            }

            ViewBag.cat = Cat;
            ViewBag.sto = store;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add()
        {
            // Retrieve form values using Request
            string productName = Request.Form["product_name"];
            int category = int.Parse(Request.Form["category"]);
            int minQuantity = int.Parse(Request.Form["min_quantity"]);
            int quantity = int.Parse(Request.Form["quantity"]);
            string description = Request.Form["description"];
            string price = Request.Form["price"];
            //int store_id = int.Parse(Request.Form["Store"]);
            //int user_id = Request.Form["price"];
            //int status = int.Parse(Request.Form["status"]);
            int? storeIdNullable = HttpContext.Session.GetInt32("StoreId");
            var myrole = HttpContext.Session.GetInt32("Role");
            int? Userid = HttpContext.Session.GetInt32("UserId");
            int User_id = Userid ?? 0;
            int store_id = storeIdNullable ?? 0;
            if (myrole == 0)
            {
                store_id = int.Parse(Request.Form["Store"]);
            }

            // Check if a product image file was provided
            byte[] productImageData = null;
            IFormFile productImageFile = Request.Form.Files["image"];
            if (productImageFile != null && productImageFile.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    productImageFile.CopyTo(ms);
                    productImageData = ms.ToArray();
                }
            }

            // Create a Product and add it to the database
            var newProduct = new Product
            {
                product_name = productName,
                category_id = category,
                min_quantity = minQuantity,
                quantity = quantity,
                description = description,
                price = price,
                status = 1,
                image = productImageData,
                store_id = store_id, // You need to set the appropriate store_id here
                user_id = User_id, // You need to set the appropriate user_id here
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };

            _context.Tproduct.Add(newProduct);
            _context.SaveChanges();

            //return RedirectToAction("Index");

            // Redirect to the store list or another appropriate action
            return RedirectToAction("Index", "Product");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var Product = await _context.Tproduct.FirstOrDefaultAsync(u => u.product_id == id);

            if (Product == null)
            {
                // User not found, handle accordingly (e.g., show an error message)
                return RedirectToAction("Index");
            }
            var role = HttpContext.Session.GetInt32("Role");
            var Storeid = HttpContext.Session.GetInt32("StoreId");
            var Cat = await _context.Tcategory
                .Where(u => u.status == 1 && u.store_id == Storeid) // Add this line to filter by status
                .OrderBy(u => u.updated_at)
                .ToListAsync();
            //ViewBag.cat = Cat;
            var store = await _context.Tstore
                  .Where(u => u.status == 1 && u.store_id == Storeid) // Add this line to filter by status
                  .OrderBy(u => u.updated_at)
                  .ToListAsync();
            //ViewBag.sto = store;
            if (role == 0)
            {
                Cat = await _context.Tcategory
                     .Where(u => u.status == 1) // Add this line to filter by status
                     .OrderBy(u => u.updated_at)
                     .ToListAsync();
                store = await _context.Tstore
                     .Where(u => u.status == 1) // Add this line to filter by status
                     .OrderBy(u => u.updated_at)
                     .ToListAsync();
            }

            ViewBag.cat = Cat;
            ViewBag.sto = store;
            // Pass the user details to the view
            ViewBag.Product = Product;
            return View();
        }
        [HttpPost]
        public IActionResult Update()
        {
            // Retrieve data from the form or request parameters
            int productId = int.Parse(Request.Form["product_id"]);
            string productName = Request.Form["product_name"];
            int category = int.Parse(Request.Form["category"]);
            int minQuantity = int.Parse(Request.Form["min_quantity"]);
            int quantity = int.Parse(Request.Form["quantity"]);
            string description = Request.Form["description"];
            string price = Request.Form["price"];
            int storeId = int.Parse(Request.Form["Store"]);

            // Find the product in the database
            var product = _context.Tproduct.FirstOrDefault(p => p.product_id == productId);

            if (product == null)
            {
                // Product not found, handle accordingly (e.g., show an error message)
                return RedirectToAction("Index");
            }

            // Update product details
            product.product_name = productName;
            product.category_id = category;
            product.min_quantity = minQuantity;
            product.quantity = quantity;
            product.description = description;
            product.price = price;
            product.store_id = storeId;
            product.updated_at = DateTime.Now;

            // Check if a new image is provided
            IFormFile imageFile = Request.Form.Files["image"];
            if (imageFile != null && imageFile.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    imageFile.CopyTo(ms);
                    product.image = ms.ToArray();
                }
            }

            // Save changes to the database
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            // Fetch user details by id and pass them to the view for confirmation
            try
            {
                var user = _context.Tproduct.FirstOrDefault(u => u.product_id == id);
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

