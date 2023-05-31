using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Suntek.Models
{
    public class ResponseModel
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public ActiveModel Active { get; set; }
        public List<string> Log { get; set; }
    }
}