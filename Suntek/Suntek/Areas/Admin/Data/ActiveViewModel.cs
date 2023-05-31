using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Suntek.Models;

namespace Suntek.Areas.Admin.Data
{
    public class ActiveViewModel : ProductActive
    {
        public string Serial { get; set; }
        public string ProductName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerWard { get; set; }
        public string CustomerDistrict { get; set; }
        public string CustomerCity { get; set; }
        //public string CustomerInstallationAgentAddress { get; set; }
        //public string CustomerCarBrandname { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public int? Limited { get; set; }
        public string Note { get; set; }
    }
}