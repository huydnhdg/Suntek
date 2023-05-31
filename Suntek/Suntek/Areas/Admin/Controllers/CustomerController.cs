using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Suntek.Models;
using Suntek.Utils;
using PagedList;

namespace Suntek.Areas.Admin.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private SuntekEntities db = new SuntekEntities();
        string userId = "";
        public ActionResult Index(int? page, string SearchString, string currentFilter)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;

            }
            if (User.Identity.Name == "administrator")
            {
                var model1 = new List<Customer>();
                if (!String.IsNullOrEmpty(SearchString))
                {
                    model1 = db.Customer.Where(a => a.Phone.Contains(SearchString)).ToList();
                }
                else
                {
                    model1 = db.Customer.OrderByDescending(m => m.Createdate).ToList(); ;
                }
                ViewBag.currentFilter = SearchString;
                return View(model1.ToPagedList(pageNumber, pageSize));
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
            var model = new List<Customer>();
            if (!String.IsNullOrEmpty(SearchString))
                model = db.Customer.Where(a => a.Phone.Contains(SearchString) && a.Createby == userId).ToList();
            else
            {
                model = db.Customer.Where(a => a.Createby == userId).OrderByDescending(m => m.Createdate).ToList();

            }
            ViewBag.currentFilter = SearchString;
            return View(model.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Create()
        {
            ViewBag.City = new SelectList(db.Province.ToList(), "Name", "Name", null);
            return View();
        }

        public ActionResult FillDistricts(string City)
        {
            var id = db.Province.Where(s => s.Name == City).SingleOrDefault();
            var provi = db.District.Where(x => x.ProvinceId == id.Id).ToList();
            var ress = new List<String>();
            foreach (var i in provi)
            {
                ress.Add(i.Name);
            }
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(ress);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.Phone = FormatString.formatUserId(customer.Phone, 0);
                customer.Createby = User.Identity.GetUserId();
                customer.Createdate = DateTime.Now;
                db.Customer.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);

        }
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.City = new SelectList(db.Province.ToList(), "Name", "Name", id);
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.Phone = FormatString.formatUserId(customer.Phone, 0);
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.City = new SelectList(db.Province.ToList(), "Name", "Name", customer.Id);
            return View(customer);
        }
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Customer customer = db.Customer.Find(id);
            db.Customer.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}