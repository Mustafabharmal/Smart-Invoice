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
            var Customer = await _context.Tcustomer
            .Where(u => u.status == 1) // Add this line to filter by status
            .OrderBy(u => u.updated_at)
            .ToListAsync();


            ViewBag.Customer = Customer;


            var Category = await _context.Tcategory
            .Where(u => u.status == 1) // Add this line to filter by status
            .OrderBy(u => u.updated_at)
            .ToListAsync();
            ViewBag.Category = Category;
            var Product = await _context.Tproduct
                 .Where(u => u.status == 1) // Add this line to filter by status
                 .OrderBy(u => u.updated_at)
                 .ToListAsync();
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
            Console.WriteLine(selectedcustomer);
            // var products = _context.Tproduct
            //.Where(p => selectedProductIds.Contains(p.product_id))
            //.GroupBy(p => p.product_id)
            //.Select(group => new
            //{
            //ProductId = group.Key,
            //Count = group.Count(),
            //Details = group.First() // You can choose any product details from the group as they should be the same
            // })
            // .ToList();
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
            return View();
        }
    }
}

