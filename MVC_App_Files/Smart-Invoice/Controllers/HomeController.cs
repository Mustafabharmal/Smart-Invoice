using Invoice_web_app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace Invoice_web_app.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserDBContext _context;
        public HomeController(ILogger<HomeController> logger, UserDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
        
            var role = HttpContext.Session.GetInt32("Role");
            var Storeid = HttpContext.Session.GetInt32("StoreId");

            var ProductCount = await _context.Tproduct
                 .Where(u => u.status == 1 && u.store_id == Storeid) // Add this line to filter by status
                 .OrderBy(u => u.updated_at)
                 .CountAsync();
          
            var UserCount = await _context.Tstore
                  .Where(u => u.status == 1) // Add this line to filter by status
                  .OrderBy(u => u.updated_at)
                  .CountAsync();

            var SalesCount = await _context.Tsale
                .Where(u => u.store_id == Storeid) // Add this line to filter by status
                .OrderBy(u => u.updated_at)
                .CountAsync();

            var CustomerCount = await _context.Tcustomer
           .Where(u => u.status == 1 && u.store_id == Storeid) // Add this line to filter by status
           .OrderBy(u => u.updated_at)
           .CountAsync();
           
            if (role == 0)
            {
                ProductCount = await _context.Tproduct
               .Where(u => u.status == 1) // Add this line to filter by status
               .OrderBy(u => u.updated_at)
               .CountAsync();

                UserCount = await _context.Tstore
                     .Where(u => u.status == 1) // Add this line to filter by status
                     .OrderBy(u => u.updated_at)
                     .CountAsync();
                SalesCount = await _context.Tsale
                .OrderBy(u => u.updated_at)
                .CountAsync();
                CustomerCount = await _context.Tcustomer
               .Where(u => u.status == 1) // Add this line to filter by status
               .OrderBy(u => u.updated_at)
               .CountAsync();
            }

            ViewBag.CustomerCount = CustomerCount;

            ViewBag.SalesCount = SalesCount;
            ViewBag.ProductCount = ProductCount;
            ViewBag.UserCount = UserCount;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Product()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}