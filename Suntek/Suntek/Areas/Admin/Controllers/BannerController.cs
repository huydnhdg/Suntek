using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Suntek.Models;

namespace Suntek.Areas.Admin.Controllers
{
    public class BannerController : Controller
    {
        private SuntekEntities db = new SuntekEntities();
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserName();
            var model = db.Banner.Where(x => x.Partner.Equals(userId)).OrderBy(x => x.Sort);
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "")] Banner banner)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserName();
                banner.Partner = userId;
                banner.Createdate = DateTime.Now;
                if (userId.Equals("admin.suntek"))
                {
                    banner.Link = "https://kichhoat.baohanhdientu.vip/" + banner.Image;
                }
                db.Banner.Add(banner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(banner);
        }
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banner.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "")] Banner banner)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserName();
                if (userId.Equals("admin.suntek"))
                {
                    banner.Link = "https://kichhoat.baohanhdientu.vip/" + banner.Image;
                }
                db.Entry(banner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(banner);
        }

        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banner.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Banner banner = db.Banner.Find(id);
            db.Banner.Remove(banner);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}