using Suntek.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Suntek.Controllers
{
    [RoutePrefix("tram-bao-hanh")]
    public class WarrantiStationController : Controller
    {
        private SuntekEntities db = new SuntekEntities();
        // GET: WarrantiStation
        [Route]
        public ActionResult Index()
        {
            // Banner
            var banner = db.Banner.Where(x => x.Partner.Equals("admin.suntek")).Where(a => a.IDCate == 1).ToList();
            ViewBag.Banner = banner;

            // Footer
            var footer = db.Article.Where(x => x.Partner.Equals("admin.suntek")).Where(a => a.IDCate == 2).SingleOrDefault();
            ViewBag.Footer = footer;

            // Logo
            var logo = db.Banner.Where(x => x.Partner.Equals("admin.suntek")).Where(a => a.IDCate == 2).SingleOrDefault();
            ViewBag.Logo = logo;

            // Hotline
            var hotline = db.Article.Where(x => x.Partner.Equals("admin.suntek")).Where(a => a.IDCate == 4).SingleOrDefault();
            ViewBag.HotLine = hotline;

            // Trạm bảo hành
            var warrantistation = db.Article.Where(x => x.Partner.Equals("admin.suntek")).Where(a => a.IDCate == 3).SingleOrDefault();
            // ViewBag.WarrantyPolicy = warrantipolicy;
            return View(warrantistation);
        }
    }
}