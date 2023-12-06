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
    public class CustomerController : Controller
    {
        // GET: /<controller>/
        private readonly UserDBContext _context;
        public CustomerController(UserDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var Customer = await _context.Tcustomer
            .Where(u => u.status == 1) // Add this line to filter by status
            .OrderBy(u => u.updated_at)
            .ToListAsync();


            ViewBag.Customer = Customer;
            return View();
        }
        public async Task<IActionResult> Create()
        {
            var Cat = await _context.Tcategory
               .Where(u => u.status == 1) // Add this line to filter by status
               .OrderBy(u => u.updated_at)
               .ToListAsync();
            ViewBag.cat = Cat;
            var store = await _context.Tstore
                  .Where(u => u.status == 1) // Add this line to filter by status
                  .OrderBy(u => u.updated_at)
                  .ToListAsync();
            ViewBag.sto = store;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add()
        {
            string name = Request.Form["name"];
            string email = Request.Form["email"];
            string phone = Request.Form["phone"];
            //string country = Request.Form["country"];
            //string city = Request.Form["city"];
            //string address = Request.Form["address"];
            int store_id = int.Parse(Request.Form["Store"]);
            //int store_id = int.Parse(Request.Form["Store"]);
            string description = Request.Form["description"];

            // Create a new Customer instance and add it to the database
            var newCustomer = new Customer
            {
                name = name,
                email = email,
                phone = phone,
                store_id= store_id,
                user_id=1,
                status=1,
                description = description,
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };

            // Add any additional logic or validation as needed

            // Add the new customer to the database
            _context.Tcustomer.Add(newCustomer);
            _context.SaveChanges();

            //return RedirectToAction("Index");

            // Redirect to the store list or another appropriate action
            return RedirectToAction("Index", "Customer");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var Customer = await _context.Tcustomer.FirstOrDefaultAsync(u => u.customer_id == id);

            if (Customer == null)
            {
                // User not found, handle accordingly (e.g., show an error message)
                return RedirectToAction("Index");
            }
            
            var store = await _context.Tstore
                  .Where(u => u.status == 1) // Add this line to filter by status
                  .OrderBy(u => u.updated_at)
                  .ToListAsync();
            ViewBag.sto = store;
            // Pass the user details to the view
            ViewBag.Customer = Customer;
            return View();
        }
        [HttpPost]
        public IActionResult Edit()
        {
            // Retrieve data from the form using Request.Form
            int customerId = int.Parse(Request.Form["customer_id"]);
            string name = Request.Form["name"];
            string email = Request.Form["email"];
            string phone = Request.Form["phone"];
            string description = Request.Form["description"];
            int storeId = int.Parse(Request.Form["Store"]);

            // Fetch the existing customer from the database
            var existingCustomer = _context.Tcustomer.FirstOrDefault(c => c.customer_id == customerId);

            if (existingCustomer == null)
            {
                // Update the properties of the existing customer
                return RedirectToAction("Index");

            }
            existingCustomer.name = name;
            existingCustomer.email = email;
            existingCustomer.phone = phone;
            existingCustomer.description = description;
            existingCustomer.store_id = storeId;
            existingCustomer.updated_at = DateTime.Now;

            // Save changes to the database
            _context.SaveChanges();

            // Redirect to the customer list or another appropriate action
            return RedirectToAction("Index");
            // Handle the case where the customer with the specified ID is not found
        }
        public IActionResult Delete(int id)
        {
            // Fetch user details by id and pass them to the view for confirmation
            try
            {
                var user = _context.Tcustomer.FirstOrDefault(u => u.customer_id == id);
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

