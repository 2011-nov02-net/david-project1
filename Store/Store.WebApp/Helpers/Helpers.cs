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
        public static List<OrderViewModel> ConvertOrdersToViewModel(IEnumerable<Order> orders)
        {
            return orders.Select(o => new OrderViewModel()
            {
                CustomerId = o.CustomerId,
                LocationId = o.LocationId,
                Date = o.Date,
                OrderTotal = o.OrderTotal,
                OrderNumber = o.OrderNumber
            }).ToList();
        }

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
    }
}
