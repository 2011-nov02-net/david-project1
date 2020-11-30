using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Store.WebApp.ViewModels
{
    public class OrderViewModel
    {
        [Display(Name = "Location")]
        public int LocationId { get; set; }
        public DateTime Date { get; set; }
        public decimal OrderTotal { get; set; }
        public int OrderNumber { get; set; }
    }
}
