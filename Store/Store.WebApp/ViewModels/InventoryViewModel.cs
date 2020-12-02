using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Store.WebApp.ViewModels
{
    public class InventoryViewModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int OrderLimit { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
    }
}
