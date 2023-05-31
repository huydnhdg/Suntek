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
    public class ArticleController : Controller
    {
        private SuntekEntities db = new SuntekEntities();
        public ActionResult Index()
        {
            // Bổ sung Partner chính là UserId, chỉ áp dụng cho Partner
            string userId = User.Identity.GetUserName();
            var model = db.Article.Where(x => x.Partner.Equals(userId)).OrderByDescending(x => x.Createdate);
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "")] Article article)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserName();
                article.Partner = userId;
                article.Createdate = DateTime.Now;
                db.Article.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(article);

        }
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Article.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(article);
        }

        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Article.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Article article = db.Article.Find(id);
            db.Article.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}