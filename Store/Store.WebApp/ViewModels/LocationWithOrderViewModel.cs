using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Store.WebApp.ViewModels
{
    public class LocationWithOrderViewModel
    {
        [Required, RegularExpression("[A-Z].*")]
        public string Name { get; set; }
        public int LocationId { get; set; }
        public List<OrderViewModel> Orders { get; set; }
    }
}
