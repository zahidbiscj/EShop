﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Core.Entities.Core
{
    public class Category:BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
