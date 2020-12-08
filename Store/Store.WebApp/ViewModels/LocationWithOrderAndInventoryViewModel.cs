using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Store.WebApp.ViewModels
{
    public class LocationWithOrderAndInventoryViewModel
    {
        [Required, RegularExpression("[A-Z].*")]
        [Display(Name = "Location Name")]
        public string Name { get; set; }
        [Display(Name = "Location Id")]
        public int LocationId { get; set; }
        public List<OrderViewModel> Orders { get; set; }
        public List<InventoryViewModel> Inventory { get; set; }
        public List<SaleViewModel> Sales { get; set; }
    }
}
