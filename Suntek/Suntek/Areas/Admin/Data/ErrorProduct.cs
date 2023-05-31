using Suntek.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Suntek.Areas.Admin.Data
{
    public class ErrorProduct:Product
    {
        public Product Products { get; set; }
    }
}