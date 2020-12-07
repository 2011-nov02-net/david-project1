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

        public void AddInventory(string productName, int locationId, int quantity)
        {
            // get the location iventories with all the product details
            var dbLocation = _context.Locations
                .Include(l => l.Inventories).ThenInclude(i => i.Product)
                .FirstOrDefault(l => l.Id == locationId).Inventories.FirstOrDefault(i => i.Product.Name == productName);
            if(dbLocation == null)
            {
                // means the store doesn't have this item in its inventory
                var location = _context.Locations.First(l => l.Id == locationId);
                var product = _context.Products.First(p => p.Name == productName);
                location.Inventories.Add(new Inventory() 
                { 
                    ProductId = product.Id,
                    LocationId = locationId,
                    Quantity = 0
                });
                _context.Update(location);
                _context.SaveChanges();
                dbLocation = _context.Locations
                .Include(l => l.Inventories).ThenInclude(i => i.Product)
                .FirstOrDefault(l => l.Id == locationId).Inventories.FirstOrDefault(i => i.Product.Name == productName);
            }
            dbLocation.Quantity += quantity;
            _context.Update(dbLocation);
            _context.SaveChanges();
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
