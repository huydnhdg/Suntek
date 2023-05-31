using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Suntek.Areas.Admin.Data;
using Suntek.Models;
using PagedList;

namespace Suntek.Areas.Admin.Controllers
{
    [Authorize]
    public class ActiveController : Controller
    {
        private SuntekEntities db = new SuntekEntities();
        string userId = "";
        public ActionResult Index(long? id, int? page, int? channel)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            if (User.Identity.Name == "administrator")
            {
                var model1 = from a in db.ProductActive
                             join b in db.Product on a.ProductId equals b.Id
                             join c in db.Customer on a.CustomerId equals c.Id
                             select new ActiveViewModel()
                             {
                                 Id = a.Id,
                                 Activedate = a.Activedate,
                                 Activeby = a.Activeby,
                                 Type = a.Type,
                                 Buydate = a.Buydate,
                                 ProductName = b.Name,
                                 Limited = b.Limited,
                                 Serial = b.Serial,
                                 CustomerName = c.Name,
                                 CustomerPhone = c.Phone,
                                 CustomerId = c.Id
                             };
                if (id != null)
                {
                    model1 = model1.Where(a => a.CustomerId == id);
                }
                if (channel != null)
                {
                    model1 = model1.Where(c => c.Type == channel);
                    ViewBag.Channel = channel;
                }
                return View(model1.OrderByDescending(x => x.Id).ToPagedList(pageNumber, pageSize));
            }
            string partner = db.AspNetUsers.Find(User.Identity.GetUserId()).Createby;
            if (User.IsInRole("Mod"))
            {
                userId = partner;
            }
            else
            {
                userId = User.Identity.GetUserId();
            }
            var model = from a in db.ProductActive
                        join b in db.Product on a.ProductId equals b.Id
                        where b.Createby == userId
                        join c in db.Customer on a.CustomerId equals c.Id
                        select new ActiveViewModel()
                        {
                            Id = a.Id,
                            Activedate = a.Activedate,
                            Activeby = a.Activeby,
                            Type = a.Type,
                            Buydate = a.Buydate,
                            ProductName = b.Name,
                            Limited = b.Limited,
                            Serial = b.Serial,
                            CustomerName = c.Name,
                            CustomerPhone = c.Phone,
                            CustomerId = c.Id
                        };
            if (id != null)
            {
                model = model.Where(a => a.CustomerId == id);
            }
            if (channel != null)
            {
                model = model.Where(c => c.Type == channel);
                ViewBag.Channel = channel;
            }

            return View(model.OrderByDescending(x => x.Id).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            string userId = User.Identity.GetUserId();
            ViewBag.ProductId = new SelectList(db.Product.Where(a => a.Createby == userId).ToList(), "Id", "Name", null);
            ViewBag.CustomerId = new SelectList(db.Customer.Where(a => a.Createby == userId).ToList(), "Id", "Name", null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "")] ProductActive productActive)
        {
            if (ModelState.IsValid)
            {
                db.ProductActive.Add(productActive);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productActive);

        }
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductActive productActive = db.ProductActive.Find(id);
            if (productActive == null)
            {
                return HttpNotFound();
            }
            string userId = User.Identity.GetUserId();
            ViewBag.ProductId = new SelectList(db.Product.Where(a => a.Createby == userId).ToList(), "Id", "Name", null);
            ViewBag.CustomerId = new SelectList(db.Customer.Where(a => a.Createby == userId).ToList(), "Id", "Name", null);
            return View(productActive);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "")] ProductActive productActive)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productActive).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productActive);
        }
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductActive productActive = db.ProductActive.Find(id);
            if (productActive == null)
            {
                return HttpNotFound();
            }
            return View(productActive);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ProductActive productActive = db.ProductActive.Find(id);
            db.ProductActive.Remove(productActive);

            var prod = db.Product.Find(productActive.ProductId);
            prod.Status = null;
            db.Entry(prod).State = EntityState.Modified;

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}