using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Store.WebApp.ViewModels
{
    public class CustomerViewModel
    {
        [Display(Name = "First Name")]
        [Required, RegularExpression("[A-Z].*")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required, RegularExpression("[A-Z].*")]
        public string LastName { get; set; }
        public int Id { get; set; }
    }
}
