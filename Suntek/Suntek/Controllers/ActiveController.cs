using Microsoft.AspNet.Identity;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Suntek.Models;

namespace Suntek.Controllers
{
    [RoutePrefix("kich-hoat")]
    public class ActiveController : Controller

    {
        Logger logger = LogManager.GetCurrentClassLogger();
        SuntekEntities db = new SuntekEntities();

        [Route]
        public ActionResult Index(string serial = "", string code = "")
        {
            if (!string.IsNullOrEmpty(serial) || !string.IsNullOrEmpty(code))
            {
                var user = db.AspNetUsers.Where(a => a.UserName == "admin.suntek").SingleOrDefault();
                var product = db.Product.Where(a => a.Createby == user.Id).Where(a => a.Serial == serial || a.Serial == code).SingleOrDefault();
                if (product != null)
                {
                    ViewBag.serial = product.Serial;
                }
                else
                {
                    ViewBag.serial = serial;
                }
            }
            //else if (!string.IsNullOrEmpty(code))
            //{
            //    var user = db.AspNetUsers.Where(a => a.UserName == "admin.suntek").SingleOrDefault();
            //    var product = db.Product.Where(a => a.Serial == serial && a.Createby == user.Id).SingleOrDefault();
            //    if (product != null)
            //    {
            //        ViewBag.serial = product.Serial;
            //    }
            //    else
            //    {
            //        ViewBag.serial = serial;
            //    }
            //}
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

            TempData["province"] = db.Province.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult PostData(PostModel model)
        {
            string msg = "";
            int status = -1;
            if (!string.IsNullOrEmpty(model.Phone)
                && !string.IsNullOrEmpty(model.Phone)
                && !string.IsNullOrEmpty(model.Province)
                && !string.IsNullOrEmpty(model.District)
                // && !string.IsNullOrEmpty(model.Ward)
                && !string.IsNullOrEmpty(model.Address)
                && !string.IsNullOrEmpty(model.Serial))
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                string objecct = javaScriptSerializer.Serialize(model);
                logger.Info("active: " + objecct);

                model.Phone = Utils.FormatString.formatUserId(model.Phone, 2);
                var user = db.AspNetUsers.Where(a => a.UserName == "admin.suntek").SingleOrDefault();
                var customer = db.Customer.Where(a => a.Phone == model.Phone && a.Createby == user.Id).SingleOrDefault();
                var product = db.Product.Where(a => a.Serial == model.Serial && a.Createby == user.Id).SingleOrDefault();

                if (product == null)
                {
                    status = -1;
                    msg = "Sản phẩm không tồn tại";
                    var resulter = new ResponseModel()
                    {
                        Status = status,
                        Message = msg,
                    };
                    return Json(resulter);
                }
                else if (product.Status == 1)
                {
                    status = -1;
                    msg = "Sản phẩm đã kích hoạt trước đó";
                    var resulter = new ResponseModel()
                    {
                        Status = status,
                        Message = msg,
                    };
                    return Json(resulter);
                }
                if (customer != null)
                {
                    if (!string.IsNullOrEmpty(model.CusName))
                    {
                        customer.Name = model.CusName;
                        customer.City = model.Province;
                        customer.District = model.District;
                        customer.Address = model.Address;
                        customer.Createby = user.Id;
                        db.Entry(customer).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else
                {
                    var newcus = new Customer()
                    {
                        Phone = model.Phone,
                        Name = model.CusName,
                        City = model.Province,
                        District = model.District,
                        Createby = user.Id,
                        Address = model.Address
                    };
                    db.Customer.Add(newcus);
                    db.SaveChanges();
                    customer = new Customer() { Id = newcus.Id };
                }
                //add thong tin kich hoat
                product.Status = 1;
                var active = new ProductActive()
                {
                    ProductId = product.Id,
                    CustomerId = customer.Id,
                    Activedate = DateTime.Now,
                    Activeby = User.Identity.Name,
                    Type = 1,
                    Buydate = DateTime.Now,
                };
                db.ProductActive.Add(active);
                db.SaveChanges();

                status = 1;
                msg = "Kích hoạt thành công. Hạn bảo hành từ ngày " + active.Activedate.Value.ToString("dd/MM/yyyy") + " đến ngày " + active.Activedate.Value.AddMonths(product.Limited ?? 0).ToString("dd/MM/yyyy");
                var result = new ResponseModel()
                {
                    Status = status,
                    Message = msg,
                };
                return Json(result);
            }
            else
            {
                status = -1;
                msg = "Yêu cầu nhập đủ thông tin";
                var resulter = new ResponseModel()
                {
                    Status = status,
                    Message = msg,
                };
                return Json(resulter);
            }
        }
        [HttpPost]
        public ActionResult GetDistrict(string name)
        {
            var province = db.Province.Where(a => a.Name == name).SingleOrDefault();
            var district = db.District.Where(a => a.ProvinceId == province.Id).Select(a => a.Name);
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(district);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetWard(string name)
        {
            var district = db.District.Where(a => a.Name == name).SingleOrDefault();
            var ward = db.Ward.Where(a => a.DistrictID == district.Id).Select(a => a.Name);
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(ward);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetCustomer(string phone)
        {
            var user = db.AspNetUsers.Where(a => a.UserName == "admin.suntek").SingleOrDefault();
            phone = Utils.FormatString.formatUserId(phone, 2);
            var customer = db.Customer.Where(a => a.Phone == phone && a.Createby == user.Id).SingleOrDefault();

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(customer);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetAgent(string name)
        {
            var agent = db.AspNetUsers.Where(a => a.UserName == name).SingleOrDefault();

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(agent);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}