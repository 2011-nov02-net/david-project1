﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Library.Repository_Interfaces
{
    public interface IProductRepository
    {
        public Product Get(string name);
    }
}
