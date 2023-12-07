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
        public SalesController(UserDBContext context)
        {
            _context = context;
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

            return View();
        }
    }
}

