using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Store.WebApp.ViewModels
{
    public class LocationViewModel
    {
        [Required, RegularExpression("[A-Z].*")]
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
