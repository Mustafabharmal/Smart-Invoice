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
    public class SalesController : Controller
    {
        private readonly UserDBContext _context;
        private readonly ProductSelection _productSelection;
        public SalesController(UserDBContext context, ProductSelection productSelection)
        {
            _context = context;
            _productSelection = productSelection;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var role = HttpContext.Session.GetInt32("Role");
            var Storeid = HttpContext.Session.GetInt32("StoreId");
            var Sales = await _context.Tsale
                 .Where(u => u.store_id == Storeid) // Add this line to filter by status
                 .OrderBy(u => u.updated_at)
                 .ToListAsync();
            if (role == 0)
            {
                Sales = await _context.Tsale
                 .OrderBy(u => u.updated_at)
                 .ToListAsync();
            }
            ViewBag.Sales = Sales;
            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {

            var Sales = await _context.Tsale
                 .Where(u => u.sale_id == id) // Add this line to filter by status
                 .OrderBy(u => u.updated_at)
                 .FirstOrDefaultAsync();
            ViewBag.Sales = Sales;
            _productSelection.ProductIds.Clear();
            if (Sales != null)
            {
                // Split the product_id string into an array of strings
                var productIdsStringArray = Sales.product_name.Split(',');
                var quantityStringArray = Sales.quantity.Split(',');
                foreach (var sale in productIdsStringArray)
                {
                    // Assuming 'sale' represents a product ID in string format
                    if (int.TryParse(sale, out int productId) && int.TryParse(sale, out int Count))
                    {
                        for (int j = 0; j < Count-1; j++)
                        {
                            _productSelection.ProductIds.Add(productId);
                        }

                    }
                    else
                    {
                        // Handle the case where parsing fails (optional)
                    }
                }
                // Now, 'productIds' is a List<int> containing the parsed product IDs
            }
            var selectedProductIds = _productSelection.ProductIds;
            ViewBag.prodid = selectedProductIds;
            var productsliis = selectedProductIds.GroupBy(id => id)
                .Select(groupId => new
             {
                 ProductId = groupId.Key,
                 Count = groupId.Count(),
                 Details = _context.Tproduct.FirstOrDefault(p => p.product_id == groupId.Key)
             })
             .ToList();
            ViewBag.SelectedProducts = productsliis;
            return View();
        }
    }
}

