﻿using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Suntek.Areas.Admin.Data;
using Suntek.Models;

namespace Suntek.Areas.Admin.Controllers
{
    [Authorize]
    public class AgentController : Controller
    {
        private SuntekEntities db = new SuntekEntities();
        public ActionResult Index()
        {
            if (User.Identity.Name == "administrator")
            {
                var model1 = from a in db.AspNetUsers
                             from b in a.AspNetRoles
                             where b.Id == "Agent"
                             select a;
                return View(model1);
            }
            string userId = User.Identity.GetUserId();
            var model = from a in db.AspNetUsers
                        where a.Createby == userId
                        from b in a.AspNetRoles
                        where b.Id == "Agent"
                        select a;
            return View(model);
        }

        /// <summary>
        /// CHG BY TRUNGVD 2021-06-23 Task = 603459
        /// </summary>
        /// <param name="id">Id của Đại lý cần xóa</param>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers user = db.AspNetUsers.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUsers user = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult uploadlist()
        {
            return View();
        }
        [HttpPost]
        public ActionResult uploadlist(FormCollection formCollection)
        {
            ViewBag.mess = "Đã có lỗi xảy ra.";
            if (Request != null)
            {
                try
                {
                    HttpPostedFileBase file = Request.Files["UploadedFile"];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        //int count = 0;
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        var products = new List<ProductAgentModel>();
                        List<ProductAgent> listprodagent = new List<ProductAgent>();
                        int count = 0;
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;
                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                ProductAgentModel prodview = new ProductAgentModel();
                                string serial = workSheet.Cells[rowIterator, 1].Value.ToString();
                                string importdate = workSheet.Cells[rowIterator, 2].Value.ToString();
                                string agent = workSheet.Cells[rowIterator, 3].Value.ToString();
                                string userId = User.Identity.GetUserId();

                                var product = db.Product.Where(a => a.Serial.Equals(serial)).SingleOrDefault();
                                var user = db.AspNetUsers.Where(a => a.UserName.Equals(agent)).SingleOrDefault();
                                if (product != null && user != null)
                                {
                                    //ton tai san pham va dai ly add thong tin vao table productagent
                                    ProductAgent productAgent = new ProductAgent()
                                    {
                                        ProductId = product.Id,
                                        AgentId = user.Id,
                                        Importdate = DateTime.ParseExact(importdate, "dd/MM/yyyy", null),
                                        Createdate = DateTime.Now,
                                        Createby = userId
                                    };
                                    db.ProductAgent.Add(productAgent);
                                    // Dùng Để View
                                    prodview.ProdName = product.Name;
                                    prodview.Agent = user.UserName;
                                    prodview.Importdate = importdate;
                                    count++;
                                }
                                else
                                {
                                    // Có lỗi xảy ra
                                    prodview.Error = "Serial hoặc đại lý không tồn tại";
                                    // prodview.ProdName = product.Name;
                                    prodview.ProdName = serial;
                                    // prodview.Agent = user.UserName;
                                    prodview.Agent = agent;
                                    prodview.Importdate = importdate;
                                }
                                products.Add(prodview);
                            }
                            if (count > 0)
                            {
                                ViewBag.mess = "Đã lưu " + count + "sản phẩm thành công.";
                                db.SaveChanges();
                            }
                        }
                        return View(products);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.mess = ex;
                    return View();
                }
            }
            return View();
        }
        //// private string agentId = "";
        //public ActionResult upload(string id)
        //{
        //    ViewBag.Id = id;
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult upload(FormCollection formCollection)
        //{
        //    ViewBag.mess = "Đã có lỗi xảy ra.";
        //    if (Request != null)
        //    {
        //        try
        //        {
        //            HttpPostedFileBase file = Request.Files["UploadedFile"];
        //            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
        //            {
        //                //int count = 0;
        //                string fileName = file.FileName;
        //                string fileContentType = file.ContentType;
        //                byte[] fileBytes = new byte[file.ContentLength];
        //                var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
        //                var products = new List<ProductAgentModel>();
        //                List<ProductAgent> listprodagent = new List<ProductAgent>();
        //                int count = 0;
        //                using (var package = new ExcelPackage(file.InputStream))
        //                {
        //                    var currentSheet = package.Workbook.Worksheets;
        //                    var workSheet = currentSheet.First();
        //                    var noOfCol = workSheet.Dimension.End.Column;
        //                    var noOfRow = workSheet.Dimension.End.Row;
        //                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
        //                    {
        //                        ProductAgentModel prodview = new ProductAgentModel();
        //                        string serial = workSheet.Cells[rowIterator, 1].Value.ToString();
        //                        string importdate = workSheet.Cells[rowIterator, 2].Value.ToString();
        //                        string userId = User.Identity.GetUserId();

        //                        var product = db.Products.Where(a => a.Serial.Equals(serial)).SingleOrDefault();
        //                        if (product != null)
        //                        {
        //                            //ton tai san pham va dai ly add thong tin vao table productagent
        //                            ProductAgent productAgent = new ProductAgent()
        //                            {
        //                                ProductId = product.Id,
        //                                AgentId = ViewBag.Id,
        //                                Importdate = DateTime.ParseExact(importdate, "dd/MM/yyyy", null),
        //                                Createdate = DateTime.Now,
        //                                Createby = userId
        //                            };
        //                            db.ProductAgents.Add(productAgent);

        //                            prodview.ProdName = product.Name;
        //                            prodview.Importdate = importdate;
        //                            count++;
        //                        }
        //                        else
        //                        {
        //                            //co loi xay ra

        //                            prodview.Error = "Serial không tồn tại";
        //                            prodview.ProdName = product.Name;
        //                            prodview.Importdate = importdate;
        //                        }
        //                        products.Add(prodview);
        //                    }
        //                    if (count > 0)
        //                    {
        //                        ViewBag.mess = "Đã lưu " + count + " sản phẩm thành công.";
        //                        db.SaveChanges();
        //                    }
        //                }
        //                return View(products);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.mess = ex;
        //            return View();
        //        }
        //    }
        //    return View();
        //}
    }
}