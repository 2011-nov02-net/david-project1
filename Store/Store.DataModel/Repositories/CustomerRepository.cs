using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.Repository_Interfaces;
using Store.Library;

namespace Store.DataModel.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly Project0Context _context;

        public CustomerRepository(Project0Context context)
        {
            _context = context;
        }

        public void Create(Library.Customer customer)
        {
            var cust = new Customer() { FirstName = customer.FirstName, LastName = customer.LastName };

            _context.Add(cust);
            _context.SaveChanges();
        }

        public Library.Customer Get(int id)
        {
            var dbCustomer = _context.Customers.FirstOrDefault(c => c.Id == id);

            return new Library.Customer(dbCustomer.FirstName, dbCustomer.LastName, dbCustomer.Id) ?? null;
        }

        public IEnumerable<Library.Customer> GetAll()
        {
            var dbCustomers = _context.Customers.ToList();

            return dbCustomers.Select(e => new Library.Customer(e.FirstName, e.LastName, e.Id));
        }

        public void Update(Library.Customer customer)
        {
            // get customer from db
            var dbCust = _context.Customers.First(c => c.Id == customer.Id);
            // update names
            dbCust.FirstName = customer.FirstName;
            dbCust.LastName = customer.LastName;
            // save changes
            _context.SaveChanges();
        }
    }
}
