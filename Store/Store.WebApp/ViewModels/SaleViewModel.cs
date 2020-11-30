using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Store.WebApp.ViewModels
{
    public class SaleViewModel
    {
        [Display(Name = "Product Name")]
        [Required]
        public string ProductName { get; set; }
        [Display(Name = "Purchase Price")]
        [Required]
        public decimal PurchasePrice { get; set; }
        [Display(Name = "Quantity Purchased")]
        public int Quantity { get; set; }
    }
}
