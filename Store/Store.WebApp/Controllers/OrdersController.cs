using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
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
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
