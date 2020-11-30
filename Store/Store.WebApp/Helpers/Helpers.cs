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
    }
}
