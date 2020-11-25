using Library.Repository_Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.WebApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _repository;

        public CustomerController(ICustomerRepository repository)
        {
            _repository = repository;
        }
        // GET: Customer
        public ActionResult Index()
        {
            var customers = _repository.GetAll();
            return View(customers);
        }

        // GET: Customer/Details/5
        public ActionResult Details(int id)
        {
            var customer = _repository.Get(id);
            return View(customer);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("FirstName,LastName")] CustomerViewModel viewCustomer, IFormCollection collection)
        {
            // check the modelstat
            if (!ModelState.IsValid)
            {
                return View(viewCustomer);
            }

            try
            {
                // give the id as 1 just to get it into the system
                // the create repo does not use that id to form a new DB entry
                var customer = new Library.Customer(viewCustomer.FirstName, viewCustomer.LastName, 1);
                _repository.Create(customer);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "There was a problem Creating the Customer");
                return View(viewCustomer);
            }
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            // add which customer we are currently adding to tempdata to be able to check with after the fact
            TempData["Customer"] = id;
            var customer = _repository.Get(id);
            var viewCustomer = new CustomerViewModel()
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Id = customer.Id
            };
            return View(viewCustomer);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("FirstName,LastName,Id")] CustomerViewModel viewCustomer, IFormCollection collection)
        {
            // get the current id from tempdata to ensure the user hasn't modified the id that the page put out
            int tdId = (int)TempData.Peek("Customer");
            if(tdId != id && tdId != viewCustomer.Id)
            {
                // *ER add error message
                return RedirectToAction(nameof(Index));
            }

            // check the modelstat
            if(!ModelState.IsValid)
            {
                return View(viewCustomer);
            }

            try
            {
                var customer = new Library.Customer(viewCustomer.FirstName, viewCustomer.LastName, viewCustomer.Id);
                _repository.Update(customer);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "There was a problem updating the Customer");
                return View(viewCustomer);
            }
        }
    }
}
