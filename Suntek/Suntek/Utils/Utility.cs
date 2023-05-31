using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Suntek.Utils
{
    public class Utility
    {        
        public static string getclientIP()
        {
            var HostIP = HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : "";
            return HostIP;
        }
        // public static string IdPatner = "1afe25fc-f6b7-4918-8473-8d1e1e677332";
        public static string IDPartner = "4dfd24f4-4332-4670-bc74-d537e6c4d6ed";

        public static int nonActivationMaxMonthNumber = 6;        
    }
}