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
            throw new NotImplementedException();
        }

        public Library.Location Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Library.Location> GetAll()
        {
            var dbLocations = _context.Locations.ToList();

            return dbLocations.Select(l => new Library.Location(l.Name, l.Id));
        }

        public void Update(Library.Location location)
        {
            throw new NotImplementedException();
        }
    }
}
