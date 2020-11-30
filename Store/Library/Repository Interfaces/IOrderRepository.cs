using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Library.Repository_Interfaces
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetByCustomerId(int id);
        void Create(Customer customer, List<Sale> sales);
        Order GetOrderByOrderNumber(int OrderNumber);
        IEnumerable<Order> GetByLocationId(int id);
    }
}
