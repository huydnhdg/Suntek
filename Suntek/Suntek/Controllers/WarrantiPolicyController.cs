﻿using Suntek.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Suntek.Controllers
{
    [RoutePrefix("chinh-sach-bao-hanh")]
    public class WarrantiPolicyController : Controller
    {
        private SuntekEntities db = new SuntekEntities();
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

            // Chính sách bảo hành
            var warrantipolicy = db.Article.Where(x => x.Partner.Equals("admin.suntek")).Where(a => a.IDCate == 1).SingleOrDefault();
            // ViewBag.WarrantyPolicy = warrantipolicy;
            return View(warrantipolicy);
        }
    }
}