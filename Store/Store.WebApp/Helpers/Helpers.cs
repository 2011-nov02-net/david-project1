using Microsoft.AspNetCore.Mvc.Rendering;
using Store.Library;
using Store.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.WebApp.Helpers
{
    public class Helpers
    {
        #nullable enable
        public static List<OrderViewModel>? ConvertOrdersToViewModel(IEnumerable<Order> orders)
        {
            return orders?.Select(o => new OrderViewModel()
            {
                CustomerId = o.CustomerId,
                LocationId = o.LocationId,
                Date = o.Date,
                OrderTotal = o.OrderTotal,
                OrderNumber = o.OrderNumber
            }).ToList() ?? null;
        }
        #nullable disable

        internal static List<InventoryViewModel> ConvertInventoryToViewModel(ICollection<Inventory> locationInventory)
        {
            return locationInventory.Select(i => new InventoryViewModel()
            {
                Name = i.ProductObj.Name,
                Description = i.ProductObj.Description,
                Id = i.ProductObj.Id,
                Price = i.ProductObj.Price,
                OrderLimit = i.ProductObj.OrderLimit,
                Quantity = i.Quantity
            }).ToList();
        }

        internal static LocationWithOrderAndInventoryViewModel ConvertLocationToViewModel(Location location, ICollection<Inventory> inventory, IEnumerable<Order> orders)
        {
            return new LocationWithOrderAndInventoryViewModel()
            {
                Name = location.Name,
                LocationId = location.Id,
                Orders = ConvertOrdersToViewModel(orders),
                Inventory = ConvertInventoryToViewModel(inventory)
            };
        }

        internal static List<SelectListItem> FormTagHelperList(List<InventoryViewModel> inventory)
        {
            return inventory.Select(i => new SelectListItem { Value = i.Name, Text = i.Name }).ToList();
        }

        internal static List<SaleViewModel> ConvertSalesToViewModel(IEnumerable<Sale> sales)
        {
            return sales.Select(s => new SaleViewModel
            { 
                ProductName = s.ProductName,
                PurchasePrice = s.PurchasePrice,
                Quantity = s.SaleQuantity
            }).ToList();
        }

        internal static List<Sale> ConvertSaleViewModelToSale(IEnumerable<SaleViewModel> sales)
        {
            return sales.Select(s => new Sale(s.Id, s.ProductName, s.PurchasePrice, s.Quantity)).ToList();
        }
    }
}
