using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Store.Library.Repository_Interfaces;

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

        public void Update(Library.Location location)
        {
            // get the location from db
            var dbLoc = _context.Locations.First(l => l.Id == location.Id);

            // update name
            dbLoc.Name = location.Name;
            // save changes
            _context.SaveChanges();
        }
    }
}
