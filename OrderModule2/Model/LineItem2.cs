﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Order.Model;

namespace OrderModule2.Web.Model
{
    public class LineItem2 : LineItem
    {
        public string OuterId { get; set; }
    }
}