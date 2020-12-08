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
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Order Total")]
        public decimal OrderTotal { get; set; }
        [Display(Name = "Order Number")]
        public int OrderNumber { get; set; }
        public List<SaleViewModel> Sales { get; set; }
    }
}
