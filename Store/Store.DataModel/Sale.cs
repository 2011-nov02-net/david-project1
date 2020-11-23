﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Store.DataModel
{
    public partial class Sale
    {
        public int OrderNumber { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal PurchasePrice { get; set; }
        public int Quantity { get; set; }

        public virtual Order OrderNumberNavigation { get; set; }
        public virtual Product Product { get; set; }
    }
}
