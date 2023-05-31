//using Suntek.Models;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;

//namespace Suntek.Areas.Admin.Controllers
//{
//    [Authorize(Roles = "Partner")]
//    public class KeyController : Controller
//    {
//        SuntekEntities db = new SuntekEntities();
//        public ActionResult Index()
//        {
//            var model = db.Keys;
//            return View(model);
//        }

//        public ActionResult Create()
//        {
//            TempData["province"] = db.Provinces.ToList();
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "")] Key key)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Keys.Add(key);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(key);

//        }
//        public ActionResult Edit(long? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Key key = db.Keys.Find(id);
//            if (key == null)
//            {
//                return HttpNotFound();
//            }
//            return View(key);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "")] Key key)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(key).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(key);
//        }

//        public ActionResult Delete(long? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Key key = db.Keys.Find(id);
//            if (key == null)
//            {
//                return HttpNotFound();
//            }
//            return View(key);
//        }

//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(long id)
//        {
//            Key key = db.Keys.Find(id);
//            db.Keys.Remove(key);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }
//    }
//}