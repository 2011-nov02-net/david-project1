using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Store.WebApp.ViewModels
{
    public class ProductViewModel
    {
        [Display(Name = "Product Name")]
        [Required, RegularExpression("[A-Z].*")]
        public string ProductName { get; set; }

        [Display(Name = "Product Description")]
        [Required, RegularExpression("[A-Z].*")]
        public string Description { get; set; }
        [Display(Name = "Product Name")]
        [Required, Range(0.01, 9999999999999999.99)]
        public decimal Price { get; set; }
        [Display(Name = "Order Limit of Item")]
        [Required, Range(1, 9999999999999999)]
        public int OrderLimit { get; set; }
        [Display(Name = "Quantity to Add")]
        [Required, Range(1, 9999999999999999)]
        public int Quantity { get; set; }
    }
}
