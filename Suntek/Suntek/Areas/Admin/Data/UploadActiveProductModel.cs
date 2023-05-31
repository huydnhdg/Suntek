using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Suntek.Models;

namespace Suntek.Areas.Admin.Data
{
    public class UploadActiveProductModel
    {
        public string Error { get; set; }
        public int Index { get; set; }
        public Product Product { get; set; }
        public Customer Customer { get; set; }
    }
}