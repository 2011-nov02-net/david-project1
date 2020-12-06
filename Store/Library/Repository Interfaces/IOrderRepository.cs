using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Library.Repository_Interfaces
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetByCustomerId(int id);
        void Create(int customerId, int locationId, List<Sale> sales);
        Order GetOrderByOrderNumber(int OrderNumber);
        IEnumerable<Order> GetByLocationId(int id);
    }
}
