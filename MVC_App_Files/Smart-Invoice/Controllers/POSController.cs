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
   
    public class POSController : Controller
    {
        // GET: /<controller>/
        private readonly UserDBContext _context;
        //public List<int> prodid = null;
        private readonly ProductSelection _productSelection;
        public POSController(UserDBContext context, ProductSelection productSelection)
        {
            _context = context;
            _productSelection = productSelection;
        }
        public async Task<IActionResult> Index()
        {
            var role = HttpContext.Session.GetInt32("Role");
            var Storeid = HttpContext.Session.GetInt32("StoreId");

            var Customer = await _context.Tcustomer
            .Where(u => u.status == 1 && u.store_id==Storeid) // Add this line to filter by status
            .OrderBy(u => u.updated_at)
            .ToListAsync();
            var Category = await _context.Tcategory
         .Where(u => u.status == 1 && u.store_id == Storeid) // Add this line to filter by status
         .OrderBy(u => u.updated_at)
         .ToListAsync();
            var Product = await _context.Tproduct
                 .Where(u => u.status == 1 && u.store_id == Storeid) // Add this line to filter by status
                 .OrderBy(u => u.updated_at)
                 .ToListAsync();

            if(role==0)
            {
                 Customer = await _context.Tcustomer
            .Where(u => u.status == 1) // Add this line to filter by status
            .OrderBy(u => u.updated_at)
            .ToListAsync();
                 Category = await _context.Tcategory
             .Where(u => u.status == 1) // Add this line to filter by status
             .OrderBy(u => u.updated_at)
             .ToListAsync();
                 Product = await _context.Tproduct
                     .Where(u => u.status == 1) // Add this line to filter by status
                     .OrderBy(u => u.updated_at)
                     .ToListAsync();
            }
            ViewBag.Customer = Customer;
            ViewBag.Category = Category;
            ViewBag.Product = Product;


            //ViewBag.prodid = prodid;
            var selectedProductIds = _productSelection.ProductIds;
            ViewBag.prodid = selectedProductIds;

            var selectedcust= _productSelection.custid;
            var selectedcustomer = await _context.Tcustomer
            .Where(u => u.customer_id == selectedcust) // Add this line to filter by status
            .OrderBy(u => u.updated_at)
            .FirstOrDefaultAsync();


            ViewBag.selectedcustomer = selectedcustomer;
            var productsliis = selectedProductIds
            .GroupBy(id => id)
            .Select(groupId => new
            {
                ProductId = groupId.Key,
                Count = groupId.Count(),
                Details = _context.Tproduct.FirstOrDefault(p => p.product_id == groupId.Key)
            })
            .ToList();
            var subtotal = productsliis.Sum(item => item.Details != null ?
                                       decimal.TryParse(item.Details.price, out decimal price) ?
                                       price * item.Count : 0 : 0);

            ViewBag.Subtotal = subtotal;
            ViewBag.Tax = subtotal * (decimal)0.18;
            ViewBag.Total = subtotal + subtotal * (decimal)0.18;

            // Store the product details in ViewBag
            ViewBag.SelectedProducts = productsliis;
            ViewBag.TotalIteCount = selectedProductIds.Count();
            return View();
        }
        public IActionResult ProductDetails(int productId)
        {
            _productSelection.ProductIds.Add(productId);
            return RedirectToAction("Index");
        }
        public IActionResult ProductDecre(int productId)
        {
            //if (_productSelection.ProductIds.Count > 0)
            //{
            //    // Remove the last product ID
            //    _productSelection.ProductIds.RemoveAt(_productSelection.ProductIds.Count - 1);
            //}
            _productSelection.ProductIds.Remove(productId);
            return RedirectToAction("Index");
        }
        public IActionResult ProductDele(int productId)
        {
            _productSelection.ProductIds.RemoveAll(id => id == productId);
            //return RedirectToAction("Index");
            return RedirectToAction("Index");
        }
        public IActionResult Clearall()
        {
            _productSelection.ProductIds.Clear();
            return RedirectToAction("Index");
        }
        public IActionResult UseCust(int id)
        {
            _productSelection.custid = id;
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Innvoice()
        {
            var selectedcust = _productSelection.custid;
            var selectedcustomer = await _context.Tcustomer
            .Where(u => u.customer_id == selectedcust) // Add this line to filter by status
            .OrderBy(u => u.updated_at)
            .FirstOrDefaultAsync();

            ViewBag.selectedcustomer = selectedcustomer;
            Console.WriteLine(selectedcustomer);
            var selectedProductIds = _productSelection.ProductIds;
            ViewBag.prodid = selectedProductIds;

            var productsliis = selectedProductIds
           .GroupBy(id => id)
           .Select(groupId => new
           {
               ProductId = groupId.Key,
               Count = groupId.Count(),
               Details = _context.Tproduct.FirstOrDefault(p => p.product_id == groupId.Key)
           })
           .ToList();
            var subtotal = productsliis.Sum(item => item.Details != null ?
                                       decimal.TryParse(item.Details.price, out decimal price) ?
                                       price * item.Count : 0 : 0);

            ViewBag.Subtotal = subtotal;
            ViewBag.Tax = subtotal * (decimal)0.18;
            ViewBag.Total = subtotal + subtotal * (decimal)0.18;
            ViewBag.Currenttime = DateTime.Now;
            // Store the product details in ViewBag
            ViewBag.SelectedProducts = productsliis;
            ViewBag.TotalIteCount = selectedProductIds.Count();

            var Innno = await _context.Tsale.CountAsync();
            ViewBag.Innno = Innno;
            return View();
        }
        public async Task<IActionResult> Savecust()
        {
          

            // Get customer details
            var selectedcust = _productSelection.custid;
            var selectedcustomer = await _context.Tcustomer.FirstOrDefaultAsync(u => u.customer_id == selectedcust);

            // Get selected product details
            var selectedProductIds = _productSelection.ProductIds;
            var productsliis = selectedProductIds.GroupBy(id => id)
                .Select(groupId => new
                {
                    ProductId = groupId.Key,
                    Count = groupId.Count(),
                    Details = _context.Tproduct.FirstOrDefault(p => p.product_id == groupId.Key)
                })
                .ToList();

            // Calculate total and tax
            var subtotal = productsliis.Sum(item => item.Details != null ?
                decimal.TryParse(item.Details.price, out decimal price) ?
                price * item.Count : 0 : 0);
            var tax = subtotal * (decimal)0.18;
            var total = subtotal + tax;

            //var Storeid = HttpContext.Session.GetInt32("StoreId");
            int? storeIdNullable = HttpContext.Session.GetInt32("StoreId");
            int store_id = storeIdNullable ?? 0;
            int? Userid = HttpContext.Session.GetInt32("UserId");
            int User_id = Userid ?? 0;
            var saleData = new Sale
            {
                sale_date = DateTime.Now,
                customer_name = (_productSelection.custid).ToString(),
                product_name = string.Join(",", productsliis.Select(p => p.Details.product_id)),
                quantity = string.Join(",", productsliis.Select(p => p.Count.ToString())),
                price = string.Join(",", productsliis.Select(p => p.Details.price)),
                discount = "0", // Modify this value based on your logic
                tax = tax.ToString(),
                subtotal = subtotal.ToString(),
                paid_with = 1, // Set appropriate value based on payment method
                store_id = store_id, // Modify this value if needed
                user_id = User_id, // Modify this value if needed
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };

            _context.Tsale.Add(saleData);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}

