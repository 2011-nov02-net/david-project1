using Microsoft.EntityFrameworkCore;
using Store.Library.Repository_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store.DataModel.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly Project0Context _context;

        public OrderRepository(Project0Context context)
        {
            _context = context;
        }

        public void Create(Library.Customer customer, List<Library.Sale> sales)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Library.Order> Get(int id)
        {
            var dbOrders = _context.Orders.Where(o => o.CustomerId == id).ToList();

            return dbOrders.Select(o => new Library.Order(o.CustomerId, o.LocationId, o.Date, o.OrderNumber, o.OrderTotal));
        }

        public Library.Order GetOrderByOrderNumber(int OrderNumber)
        {
            var dbOrder = _context.Orders.Include(s => s.Sales).First(o => o.OrderNumber == OrderNumber);
            // convert the Sales from dbSales to library sales
            var sales = new List<Library.Sale>();
            foreach(var item in dbOrder.Sales)
            {
                sales.Add(new Library.Sale(item.ProductId, item.ProductName, item.PurchasePrice, item.Quantity));
            }
            return new Library.Order(dbOrder.CustomerId, dbOrder.LocationId, sales, dbOrder.Date, dbOrder.OrderNumber, dbOrder.OrderTotal);
        }
    }
}
