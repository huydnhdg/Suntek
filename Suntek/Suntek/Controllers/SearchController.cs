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

    [RoutePrefix("tra-cuu")]
    public class SearchController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
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
            return View();
        }
        [HttpPost]
        public ActionResult PostData(PostModel model)
        {
            int status = -1;
            string msg = "";
            var activeresult = new ActiveModel();
            var log = new List<string>();
            var product = db.Product.Where(a => a.Serial == model.Serial).SingleOrDefault();


            if (product == null)
            {
                status = -1;
                msg = "Sản phẩm không tồn tại";
            }
            else if (product.Status == 1)
            {
                var active = db.ProductActive.Where(a => a.ProductId == product.Id).SingleOrDefault();
                var customer = db.Customer.Where(a => a.Id == active.CustomerId).SingleOrDefault();
                var response = new ActiveModel();
                response.Serial = product.Serial;
                response.Productname = product.Name;
                response.Limited = product.Limited;
                response.Model = product.Model;
                response.Username = active.Activeby;
                response.Agent = active.Activeby;
                if (active.Buydate != null)
                {
                    response.Buydate = active.Buydate.Value.ToString("dd/MM/yyyy");
                }
                response.Activedate = active.Activedate.Value.ToString("dd/MM/yyyy");
                response.Enddate = active.Activedate.Value.AddMonths(product.Limited ?? default(int)).ToString("dd/MM/yyyy");

                var warranti = db.ProductWarranti.Where(a => a.ProductId == product.Id);
                List<string> list = new List<string>();
                if (warranti.Count() > 0)
                {
                    foreach (var item in warranti)
                    {
                        list.Add(item.Createdate.Value.ToString("dd/MM/yyyy") + " - " + item.Note);
                    }
                }

                status = 1;
                msg = "Tra cứu thông tin thành công";
                activeresult = response;
                log = list;
            }
            else
            {
                status = -1;
                msg = "Sản phẩm chưa kích hoạt bảo hành";
            }

            var result = new ResponseModel()
            {
                Status = status,
                Message = msg,
                Active = activeresult,
                Log = log
            };
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string json = javaScriptSerializer.Serialize(result);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}
