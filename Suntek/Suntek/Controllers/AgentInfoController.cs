using Microsoft.AspNet.Identity;
using Suntek.Models;
using Suntek.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Suntek.Controllers
{
    [RoutePrefix("thong-tin-dai-ly")]
    public class AgentInfoController : Controller
    {
        private SuntekEntities db = new SuntekEntities();
        [Route]
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var model = from a in db.ProductAgent
                        where a.AgentId == userId
                        join b in db.Product on a.ProductId equals b.Id
                        where b.Createby == Utility.IDPartner
                        join c in db.ProductActive on a.ProductId equals c.ProductId
                        into temp
                        from t in temp.DefaultIfEmpty()
                        select new StoreViewModel()
                        {
                            NgayKH = t.Activedate,
                            NgayNK = a.Createdate,
                            Name = b.Name,
                            Serial = b.Serial,
                            Code = b.Code,
                            Limited = b.Limited,
                        };

            // Banner
            var banner = db.Banner.Where(x => x.Partner.Equals("admin.suntek")).ToList();
            ViewBag.Banner = banner;

            // Footer
            var footer = db.Article.Where(x => x.Partner.Equals("admin.suntek")).Where(a => a.IDCate == 2).SingleOrDefault();
            ViewBag.Footer = footer;

            // Logo
            var logo = db.Banner.Where(x => x.Partner.Equals("admin.suntek")).Where(a => a.IDCate == 2).SingleOrDefault();
            ViewBag.Logo = logo;

            // Hotline
            var hotline = db.Article.Where(x => x.Partner.Equals("admin.suntek")).Where(a => a.IDCate == 4).FirstOrDefault();
            ViewBag.HotLine = hotline;

            ViewBag.user = db.AspNetUsers.Find(userId);
            ViewBag.modelactive = model.Where(a => a.NgayKH != null).Count();
            ViewBag.modelnoactive = model.Where(a => a.NgayKH == null).Count();
            return View(model);
        }
    }
}