using System;
using System.Collections.Generic;
using System.Text;
using Store.Library;

namespace Store.Library.Repository_Interfaces
{
    public interface ILocationRepository
    {
        IEnumerable<Location> GetAll();
        Location GetWithInventory(int id);
        Location Get(int id);
        void Create(Location location);
        void Update(Location location);
        void UpdateInventoryAfterOrder(int id, IEnumerable<Sale> sales);
    }
}
