using Suntek.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Suntek.Controllers
{
    [RoutePrefix("bai-viet")]
    public class PolicyController : Controller
    {
        //CMSBHDTCHUNGEntities db = new CMSBHDTCHUNGEntities();
        [Route]
        public ActionResult Index(string url)
        {
            //var model = db.Articles.Where(a => a.Link == url).SingleOrDefault();
            return View();
        }
    }
}