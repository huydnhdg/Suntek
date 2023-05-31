using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Suntek.Utils
{
    public static class CapChar
    {
        public static bool IsValidRecaptcha(string recaptcha)
        {
            if (string.IsNullOrEmpty(recaptcha))
            {
                return false;
            }
            string PrivateKey = "6LeVE4YUAAAAAAX8W9DEQtj7TNG6gGuGFNuthxxS";

            var client = new System.Net.WebClient();

            var GoogleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", PrivateKey, recaptcha));

            var serializer = new JavaScriptSerializer();
            dynamic data = serializer.Deserialize(GoogleReply, typeof(object));

            Boolean Status = data["success"];
            string challenge_ts = data["challenge_ts"];
            string hostname = data["hostname"];

            return Status;
        }
    }
}