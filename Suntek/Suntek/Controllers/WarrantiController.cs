using NLog;
using Suntek.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Suntek.Controllers
{
    [RoutePrefix("bao-hanh")]
    public class WarrantiController : Controller
    {
        Logger logger = LogManager.GetCurrentClassLogger();
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
                && !string.IsNullOrEmpty(model.Address)
                // && !string.IsNullOrEmpty(model.Sub)
                && !string.IsNullOrEmpty(model.Note)
                && !string.IsNullOrEmpty(model.Serial))
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                string objecct = javaScriptSerializer.Serialize(model);
                logger.Info("waranti: " + objecct);

                model.Phone = Utils.FormatString.formatUserId(model.Phone, 2);
                var user = db.AspNetUsers.Where(a => a.UserName == "admin.suntek").SingleOrDefault();
                var customer = db.Customer.Where(a => a.Phone == model.Phone && a.Createby == user.Id).SingleOrDefault();
                var product = db.Product.Where(a => a.Serial == model.Serial && a.Createby == user.Id).SingleOrDefault();
                // var user = db.AspNetUsers.Where(a => a.UserName == "admin.suntek").SingleOrDefault();
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
                if (customer == null)
                {
                    status = -1;
                    msg = "Số điện thoại không phải là khách hàng đã mua sản phẩm";
                    var resulter = new ResponseModel()
                    {
                        Status = status,
                        Message = msg,
                    };
                    return Json(resulter);
                }
                var warranti = new ProductWarranti()
                {
                    ProductId = product.Id,
                    CustomerId = customer.Id,
                    Category = model.Sub,
                    Note = model.Note,
                    Createdate = DateTime.Now,
                    Createby = user.Id,
                    Status = 0,
                };
                db.ProductWarranti.Add(warranti);
                db.SaveChanges();
                ////add log
                //var log = new LogWarranti()
                //{
                //    IdWarranti = warranti.Id,
                //    Createdate = DateTime.Now,
                //    Note = "Tạo mới phiếu bảo hành từ WEB"
                //};
                //db.LogWarrantis.Add(log);
                //db.SaveChanges();

                status = 1;
                msg = "Gửi yêu cầu thành công! Chúng tôi sẽ liên hệ lại trong vòng 24h";
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
        public ActionResult GetProduct(string serial)
        {

            var product = db.Product.Where(a => a.Serial == serial).SingleOrDefault();

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(product);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}