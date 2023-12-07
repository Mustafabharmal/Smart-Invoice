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
            var storeid = HttpContext.Session.GetInt32("StoreId");
            var Role = HttpContext.Session.GetInt32("Role");
            if (Role == 0)
            {
                var Customer = await _context.Tcustomer
          .Where(u => u.status == 1 && u.store_id == storeid) // Add this line to filter by status
          .OrderBy(u => u.updated_at)
          .CountAsync();
            }
            else {
                var Customer = await _context.Tcustomer
             .Where(u => u.status == 1 && u.store_id == storeid) // Add this line to filter by status
             .OrderBy(u => u.updated_at)
             .CountAsync();
            }
           
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