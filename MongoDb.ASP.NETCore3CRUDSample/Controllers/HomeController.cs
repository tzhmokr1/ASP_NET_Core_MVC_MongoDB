using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using MongoDb.ASP.NETCore3CRUDSample.DataAccess.Models;
using MongoDb.ASP.NETCore3CRUDSample.DataAccess.Repositories;
using MongoDb.ASP.NETCore3CRUDSample.DataAccess;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace MongoDb.ASP.NETCore3CRUDSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        public IConfiguration Configuration { get; }
        
        public HomeController(ICustomerRepository customerRepository)
        {
            this._customerRepository = customerRepository;      
        }
      
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var model = await _customerRepository.GetAllCustomers();
            return View(model);
        }
       [HttpGet]
       [ActionName("Get")]
        public async Task<ActionResult> GetCustomerById(int id)
        {
            var customer = await _customerRepository.GetCustomer(id);

            if (customer == null)
                return new NotFoundResult();
            return View("GetCustomer", customer);
        } 
        [HttpGet]
        public  ActionResult Insert()
        {
            return View("Insert", new Customer());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert([Bind(include: "CustomerID,Name,Age,Salary")]Customer customer)
        {
            if (ModelState.IsValid)
            {
                await _customerRepository.Create(customer);

                TempData["Message"] = "Customer Inserted Successfully";
                //return RedirectToAction("Index");    
            }
            //return View("Error");
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult> Update(int id)
        {
            Customer customer = await _customerRepository.GetCustomer(id);
            return View("Update", customer);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update( [Bind(include: "CustomerID,Name,Age,Salary")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                var customerFromDb = await _customerRepository.GetCustomer(customer.CustomerID);

                if (customerFromDb == null)
                    return new NotFoundResult();

                customer.Id = customerFromDb.Id;
                await _customerRepository.Update(customer);
                TempData["Message"] = "Customer Updated Successfully";
                return RedirectToAction("Index");
            }

            return View("Error");
        }

        public async Task<ActionResult> ConfirmDelete(int id)
        {
            var customerFromDb = await _customerRepository.GetCustomer(id);
            return View("ConfirmDelete", customerFromDb);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var customer = await _customerRepository.GetCustomer(id);
            if (customer == null)
                return new NotFoundResult();

            var result =  await _customerRepository.Delete(customer.CustomerID);
            if (result)
            {
                TempData["Message"] = "Customer deleted successfully";
            }
            else
            {
                TempData["Message"] = "Error while deleting customer";
            }
            return RedirectToAction("Index");
        }    

    }
}
