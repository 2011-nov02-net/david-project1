using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Store.Library.Repository_Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Store.DataModel.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly Project0Context _context;

        public LocationRepository(Project0Context context)
        {
            _context = context;
        }
        public void Create(Library.Location location)
        {
            var loc = new Location() { Name = location.Name };

            _context.Add(loc);
            _context.SaveChanges();
        }

        public Library.Location Get(int id)
        {
            var dbLocation = _context.Locations.FirstOrDefault(l => l.Id == id);

            return new Library.Location(dbLocation.Name, dbLocation.Id) ?? null;
        }

        public IEnumerable<Library.Location> GetAll()
        {
            var dbLocations = _context.Locations.ToList();

            return dbLocations.Select(l => new Library.Location(l.Name, l.Id));
        }

        public Library.Location GetWithInventory(int id)
        {
            var dbLocation = _context.Locations.Include(l => l.Inventories).ThenInclude(i => i.Product).FirstOrDefault(l => l.Id == id);
            // convert Inventories to Library model
            var inventory = new List<Library.Inventory>();
            foreach(var item in dbLocation.Inventories)
            {
                var product = new Library.Product(item.Product.Name, item.Product.Id, item.Product.Price, item.Product.Description, item.Product.OrderLimit);
                inventory.Add(new Library.Inventory(product, item.Quantity));
            }

            return new Library.Location(dbLocation.Name, dbLocation.Id, inventory) ?? null;
        }

        public void Update(Library.Location location)
        {
            // get the location from db
            var dbLoc = _context.Locations.First(l => l.Id == location.Id);

            // update name
            dbLoc.Name = location.Name;
            // save changes
            _context.SaveChanges();
        }

        public void UpdateInventoryAfterOrder(int id, IEnumerable<Library.Sale> sales)
        {
            var dbLoc = _context.Locations.Include(l => l.Inventories).ThenInclude(i => i.Product).FirstOrDefault(l => l.Id == id);

            foreach (var sale in sales)
            {
                var item = dbLoc.Inventories.First(i => i.ProductId == sale.ProductId);
                item.Quantity -= sale.SaleQuantity;
                _context.Update(item);
            }
            _context.SaveChanges();
        }
    }
}
