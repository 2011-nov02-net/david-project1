using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Store.Library.Repository_Interfaces;
using Store.WebApp.ViewModels;
using Store.Library;

namespace Store.WebApp.Controllers
{
    public class LocationsController : Controller
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public LocationsController(ILocationRepository locationRepository, IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _locationRepository = locationRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        // GET: Locations
        public ActionResult Index()
        {
            var locations = _locationRepository.GetAll();
            return View(locations);
        }

        // GET: Locations/Details/5
        public ActionResult Details(int id)
        {
            var location = _locationRepository.GetWithInventory(id);
            var orders = _orderRepository.GetByLocationId(id);
            // make the view model
            var locationWithOrderAndInventoryDetail = Helpers.Helpers.ConvertLocationToViewModel(location, location.LocationInventory, orders);
            return View(locationWithOrderAndInventoryDetail);
        }

        // GET: Locations/Select/5
        public ActionResult Select(int id)
        {
            // get location
            var location = _locationRepository.Get(id);
            // store the location name and id in Temp data
            TempData["LocationName"] = location.Name;
            TempData["LocationId"] = location.Id;
            // return to index view
            return RedirectToAction(nameof(Index));
        }

        // GET: Locations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Locations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Name")] LocationViewModel viewLocation, IFormCollection collection)
        {
            if(!ModelState.IsValid)
            {
                return View(viewLocation);
            }

            try
            {
                // give the id as 1 just to get it into the system
                // the create repo does not use that id to form a new DB entry
                var location = new Library.Location(viewLocation.Name, 1);
                _locationRepository.Create(location);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "There was a problem Creating the Location");
                return View(viewLocation);
            }
        }

        // GET: Locations/Edit/5
        public ActionResult Edit(int id)
        {
            // add the id to temp data for security
            TempData["Location"] = id;
            // get the location needed from the db
            var location = _locationRepository.Get(id);
            // convert to view model
            var viewLocation = new LocationViewModel() { Name = location.Name, Id = location.Id };
            return View(viewLocation);
        }

        // POST: Locations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind ("Name, Id")] LocationViewModel viewLocation, IFormCollection collection)
        {
            // get the current id from tempdata to ensure the user hasn't modified the id that the page put out
            int tdId = (int)TempData.Peek("Location");
            if (tdId != id && tdId != viewLocation.Id)
            {
                // *ER add error message
                return RedirectToAction(nameof(Index));
            }

            // check modelstate
            if(!ModelState.IsValid)
            {
                return View(viewLocation);
            }

            try
            {
                var location = new Library.Location(viewLocation.Name, viewLocation.Id);
                _locationRepository.Update(location);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Locations/Add/5
        public ActionResult Add(int id)
        {
            // add the id to temp data for security
            TempData["Location"] = id;
            return View();
        }

        // POST: Locations/Add/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(int id, [Bind("ProductName,Description,Price,OrderLimit,Quantity")] ProductViewModel productVM, IFormCollection collection)
        {
            // get the current id from tempdata to ensure the user hasn't modified the id that the page put out
            int tdId = (int)TempData.Peek("Location");
            if (tdId != id)
            {
                // *ER add error message
                return RedirectToAction(nameof(Index));
            }

            // check modelstate
            if (!ModelState.IsValid)
            {
                return View(productVM);
            }

            try
            {
                // check to see if product with name exists
                if (_productRepository.Exists(productVM.ProductName))
                {
                    // is an existing product in the database, update it (should i do this here? This is going to update for all stores)
                    Product product = new Product(productVM.ProductName, 1, productVM.Price, productVM.Description, productVM.OrderLimit);
                    _productRepository.UpdateProduct(product);
                    _locationRepository.AddInventory(product.Name, id, productVM.Quantity);
                }
                else
                {
                    // is a new product
                    Product product = new Product(productVM.ProductName, 1, productVM.Price, productVM.Description, productVM.OrderLimit);
                    _productRepository.Add(product);
                    _locationRepository.AddInventory(product.Name, id, productVM.Quantity);
                }
                return RedirectToAction(nameof(Details), new { id });
            }
            catch
            {
                return View(productVM);
            }
        }
    }
}
