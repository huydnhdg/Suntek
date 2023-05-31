using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Suntek.Models;

namespace Suntek.Areas.Admin.Data
{
    public class WarrantiViewModel:ProductWarranti
    {
        public string ProductName { get; set; }
        public string Serial { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerDistrict { get; set; }
        public string CustomerProvince { get; set; }
        public string CustomerWard { get; set; }
    }
}