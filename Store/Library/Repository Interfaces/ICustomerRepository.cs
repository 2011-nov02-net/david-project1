using Store.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Repository_Interfaces
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAll();
        Customer Get(int id);
        void Create(Customer customer);
        void Update(Customer customer);
    }
}
