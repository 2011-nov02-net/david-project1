using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Library.Repository_Interfaces
{
    public interface IOrderRepository
    {
        IEnumerable<Order> Get(int id);
        void Create(Customer customer, List<Sale> sales);

        Order GetOrderByOrderNumber(int OrderNumber);
    }
}
