using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Suntek.Models
{
    public class ActiveModel
    {
        public string Serial { get; set; }
        public string Productname { get; set; }
        public string Model { get; set; }
        public string Activedate { get; set; }
        public string Enddate { get; set; }
        public int? Limited { get; set; }
        public string Username { get; set; }
        public string Agent { get; set; }
        public string Buydate { get; set; }
    }
}