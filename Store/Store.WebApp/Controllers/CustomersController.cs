using Library.Repository_Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Library.Repository_Interfaces;
using Store.WebApp.ViewModels;
using Store.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.WebApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;

        public CustomersController(ICustomerRepository customerRepository, IOrderRepository orderRepository)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
        }
        // GET: Customers
        public ActionResult Index(string firstNameSearch, string lastNameSearch)
        {
            var customers = _customerRepository.GetAll();
            if(!String.IsNullOrEmpty(firstNameSearch))
            {
                // filter by first name
                customers = customers.Where(c => c.FirstName == firstNameSearch);
            }
            if(!String.IsNullOrEmpty(lastNameSearch))
            {
                // filter by last name
                customers = customers.Where(c => c.LastName == lastNameSearch);
            }
            
            return View(customers);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int id)
        {
            var customer = _customerRepository.Get(id);
            var orders = _orderRepository.GetByCustomerId(id);
            // make the view model
            //convert all the orders to a orderViewModel
            var orderVM = Helpers.Helpers.ConvertOrdersToViewModel(orders);
            var customerWithOrderDetail = new CustomerWithOrderViewModel()
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Id = customer.Id,
                Orders = orderVM
            };
            return View(customerWithOrderDetail);
        }

        // GET: Customer/Select/5
        public ActionResult Select(int id)
        {
            // get the customer
            var customer = _customerRepository.Get(id);
            // store the customer name and id in temp data
            TempData["CustomerName"] = customer.FirstName + " " + customer.LastName;
            TempData["CustomerId"] = customer.Id;
            // return to index view
            return RedirectToAction(nameof(Index));
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
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
                _customerRepository.Create(customer);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "There was a problem Creating the Customer");
                return View(viewCustomer);
            }
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int id)
        {
            // add which customer we are currently adding to tempdata to be able to check with after the fact
            TempData["Customer"] = id;
            var customer = _customerRepository.Get(id);
            var viewCustomer = new CustomerViewModel()
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Id = customer.Id
            };
            return View(viewCustomer);
        }

        // POST: Customers/Edit/5
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
                _customerRepository.Update(customer);
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
