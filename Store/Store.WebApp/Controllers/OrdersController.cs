using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using Store.Library;
using Store.Library.Repository_Interfaces;
using Store.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.WebApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IProductRepository _productRepository;

        public OrdersController(IOrderRepository orderRepository, ILocationRepository locationRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _locationRepository = locationRepository;
            _productRepository = productRepository;
        }

        // GET: Orders/Details/5
        public ActionResult Details(int id)
        {
            var orders = _orderRepository.GetOrderByOrderNumber(id);
            // convert the Library Sales to sales view model
            var salesVM = orders.SalesList.Select(s => new SaleViewModel() 
            { 
                ProductName = s.ProductName,
                PurchasePrice = s.PurchasePrice,
                Quantity = s.SaleQuantity
            }).ToList();

            // create view model
            var orderVM = new OrderViewModel() 
            {
                CustomerId = orders.CustomerId,
                LocationId = orders.LocationId,
                Date = orders.Date,
                OrderNumber = orders.OrderNumber,
                OrderTotal = orders.OrderTotal,
                Sales = salesVM
            };

            return View(orderVM);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            // ensure that we actually have both location and customer created
            if(TempData.Peek("LocationId") == null && TempData.Peek("CustomerId") == null)
            {
                ModelState.AddModelError("Invalid Location or Customer", "Need both a location and customer to be selected");
                return RedirectToAction(nameof(Index), "Home");
            }
            // get the location with inventory
            var location = _locationRepository.GetWithInventory((int)TempData.Peek("LocationId"));
            var locationVM = Helpers.Helpers.ConvertLocationToViewModel(location, location.LocationInventory, null);
            // check for current sales list
            if(TempData.Peek("OrderList") != null)
            {
                // deserialize the json
                var sales = JsonSerializer.Deserialize<List<SaleViewModel>>(TempData.Peek("OrderList") as string);
                // convert and add to the locationVM
                locationVM.Sales = sales;
            }
            // get the list for the drop down menu
            ViewData["Inventory"] = Helpers.Helpers.FormTagHelperList(locationVM.Inventory); 
            return View(locationVM);
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                if(TempData["OrderList"] == null)
                {
                    throw new ArgumentNullException("Order List", "The order list can not be empty");
                }
                var orderList = JsonSerializer.Deserialize<List<SaleViewModel>>(TempData["OrderList"] as string);
                //convert from SaleViewModel to Library model
                var sales = Helpers.Helpers.ConvertSaleViewModelToSale(orderList);
                _orderRepository.Create((int)TempData.Peek("CustomerId"), (int)TempData.Peek("LocationId"), sales);
                // update inventory for location
                _locationRepository.UpdateInventoryAfterOrder((int)TempData.Peek("LocationId"), sales);
                // clear out the orderlist
                TempData.Remove("OrderList");
                return RedirectToAction(nameof(Index), "Home");
            }
            catch (ArgumentNullException ex)
            {
                // log
                return RedirectToAction(nameof(Create));
            }
        }

        // POST: Orders/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(IFormCollection collection)
        {
            // check model state
            if(!ModelState.IsValid)
            {
                return View();
            }
            // get the product details
            var prod = _productRepository.Get(collection["Inventory"]);
            // get the quantity
            int quantity = 0;
            try
            {
                quantity = int.Parse(collection["Quantity"]);
            }
            catch (ArgumentException ex)
            {
                // log
                throw;
            }
            // get the amount in the store inventory
            var inventoryAmount = _locationRepository.GetWithInventory((int)TempData.Peek("LocationId"))
                .LocationInventory.First(i => i.ProductObj.Id == prod.Id).Quantity;
            // check to make sure that the quantity requested is both available and within the order limit

            if (quantity <= inventoryAmount && quantity <= prod.OrderLimit)
            {
                SaleViewModel sale = new SaleViewModel()
                {
                    ProductName = prod.Name,
                    PurchasePrice = prod.Price,
                    Quantity = quantity,
                    Id = prod.Id
                };
                // check if the orderlist has been started
                if (TempData.Peek("OrderList") == null)
                {
                    TempData["OrderList"] = JsonSerializer.Serialize(new List<SaleViewModel>());
                }
                // add sale to sales list in temp data
                var orderList = JsonSerializer.Deserialize<List<SaleViewModel>>(TempData["OrderList"] as string);
                orderList.Add(sale);
                TempData["OrderList"] = JsonSerializer.Serialize(orderList);
            }

            return RedirectToAction(nameof(Create));
        }

        // GET: Orders/Cancel
        public ActionResult Cancel()
        {
            // The client wants to cancel the order so we need
            // to clear out the temp data of an order that we 
            // started to store there.
            TempData.Remove("OrderList");
            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
