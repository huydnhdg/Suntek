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
    public class ProductAgentController : Controller
    {
        private SuntekEntities db = new SuntekEntities();
        string userId = "";
        private static List<ProductAgentViewModel> listExc = new List<ProductAgentViewModel>();
        public ActionResult Index(string SearchString, int? page, string StartDate, string EndDate)
        {
            //if (User.Identity.Name == "administrator")
            //{
            //    var model1 = from a in db.ProductAgent
            //                 join b in db.Product on a.ProductId equals b.Id
            //                 join c in db.AspNetUsers on a.AgentId equals c.Id
            //                 select new ProductAgentViewModel()
            //                 {
            //                     Id = a.Id,
            //                     ProductName = b.Name,
            //                     Serial = b.Serial,
            //                     AgentName = c.UserName,
            //                     Createdate = a.Createdate,
            //                     Createby = a.Createby,
            //                     Type = a.Type,
            //                     Importdate = a.Importdate,
            //                 };
            //    return View(model1.OrderByDescending(x => x.Id));
            //}
            //string partner = db.AspNetUsers.Find(User.Identity.GetUserId()).Createby;
            //if (User.IsInRole("Mod"))
            //{
            //    userId = partner;
            //}
            //else
            //{
            //    userId = User.Identity.GetUserId();
            //}
            //var model = from a in db.ProductAgent
            //            join b in db.Product on a.ProductId equals b.Id
            //            where b.Createby == userId
            //            join c in db.AspNetUsers on a.AgentId equals c.Id
            //            select new ProductAgentViewModel()
            //            {
            //                Id = a.Id,
            //                ProductName = b.Name,
            //                Serial = b.Serial,
            //                AgentName = c.UserName,
            //                Createdate = a.Createdate,
            //                Createby = a.Createby,
            //                Type = a.Type,
            //                Importdate = a.Importdate,
            //            };
            //return View(model.OrderByDescending(x => x.Id));

            //if (User.Identity.Name == "administrator")
            //{
            //    var model1 = from a in db.ProductAgent
            //                 join b in db.Product on a.ProductId equals b.Id
            //                 join c in db.AspNetUsers on a.AgentId equals c.Id
            //                 select new ProductAgentViewModel()
            //                 {
            //                     Id = a.Id,
            //                     ProductName = b.Name,
            //                     Serial = b.Serial,
            //                     AgentName = c.UserName,
            //                     Createdate = a.Createdate,
            //                     Createby = a.Createby,
            //                     Type = a.Type,
            //                     Importdate = a.Importdate,
            //                 };
            //    return View(model1.OrderByDescending(x => x.Id));
            //}
            string partner = db.AspNetUsers.Find(User.Identity.GetUserId()).Createby;
            if (User.IsInRole("Mod"))
            {
                userId = partner;
            }
            else
            {
                userId = User.Identity.GetUserId();
            }
            var model = from a in db.ProductAgent
                        join b in db.Product on a.ProductId equals b.Id
                        where b.Createby == userId
                        join c in db.AspNetUsers on a.AgentId equals c.Id
                        select new ProductAgentViewModel()
                        {
                            Id = a.Id,
                            ProductName = b.Name,
                            Serial = b.Serial,
                            AgentName = c.UserName,
                            Createdate = a.Createdate,
                            Createby = a.Createby,
                            Type = a.Type,
                            Importdate = a.Importdate,
                        };

            if (!String.IsNullOrEmpty(SearchString))
            {
                model = model.Where(s => s.ProductName.Contains(SearchString)
                                       || s.Serial.Contains(SearchString)
                                         || s.AgentName.Contains(SearchString)
                                       );
                ViewBag.SearchString = SearchString;
            }

            if (!String.IsNullOrEmpty(StartDate))
            {
                DateTime d = DateTime.Parse(StartDate);
                model = model.OrderByDescending(a => a.Importdate).Where(s => s.Importdate >= d);
                ViewBag.startDate = StartDate;
            }
            if (!String.IsNullOrEmpty(EndDate))
            {
                DateTime d = DateTime.Parse(EndDate);
                d = d.AddDays(1);
                model = model.OrderByDescending(a => a.Importdate).Where(s => s.Importdate < d);
                ViewBag.endDate = EndDate;
            }

            model = model.OrderByDescending(a => a.Createdate);
            listExc = model.ToList();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(listExc.ToPagedList(pageNumber, pageSize));
            // return View(model.OrderByDescending(x => x.Id));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "")] ProductWarranti productWarranti)
        {
            if (ModelState.IsValid)
            {
                productWarranti.Createby = User.Identity.GetUserId();
                productWarranti.Createdate = DateTime.Now;
                db.ProductWarranti.Add(productWarranti);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productWarranti);

        }
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductAgent productAgent = db.ProductAgent.Find(id);
            if (productAgent == null)
            {
                return HttpNotFound();
            }
            return View(productAgent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "")] ProductAgent productAgent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productAgent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productAgent);
        }
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductAgent productAgent = db.ProductAgent.Find(id);
            if (productAgent == null)
            {
                return HttpNotFound();
            }
            return View(productAgent);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ProductAgent productAgent = db.ProductAgent.Find(id);
            db.ProductAgent.Remove(productAgent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}