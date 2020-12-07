using Store.Library.Repository_Interfaces;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.DataModel.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly Project0Context _context;

        public ProductRepository(Project0Context context)
        {
            _context = context;
        }

        public bool Exists(string name)
        {
            return _context.Products.All(p => p.Name == name);
        }

        public Library.Product Get(string name)
        {
            var dbProduct = _context.Products.FirstOrDefault(p => p.Name == name);

            return new Library.Product(dbProduct.Name, dbProduct.Id, dbProduct.Price, dbProduct.Description, dbProduct.OrderLimit);
        }
    }
}
