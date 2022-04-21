using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using PagedList;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        northwndEntities db = new northwndEntities();

        int pageSize = 15;

        #region 列表
        public ActionResult Index(int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;
            var products = db.Products.OrderBy(x => x.ProductID).ToList();
            var result = products.ToPagedList(currentPage, pageSize);
            return View(result);
        }
        #endregion

        #region 新增
        public ActionResult Create()
        {
            var supplierID = db.Suppliers.OrderBy(x => x.SupplierID).Select(x => new SelectListItem
            {
                Text = x.SupplierID.ToString(),
                Value = x.SupplierID.ToString()
            });
            Session["SupplierID"] = supplierID;

            var categoryID = db.Categories.OrderBy(x => x.CategoryID).Select(x => new SelectListItem
            {
                Text = x.CategoryID.ToString(),
                Value = x.CategoryID.ToString(),
            });
            Session["CategoryID"] = categoryID;

            return View();
        }

        [HttpPost]
        public ActionResult Create(Products products)
        {
            ViewBag.Error = "";
            if (ModelState.IsValid)
            {
                var temp = db.Products.Where(x => x.ProductName == products.ProductName).FirstOrDefault();
                if (temp != null)
                {
                    ViewBag.Error = "上架產品重複";
                    return View(products);
                }
                db.Products.Add(products);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Error = "輸入資料未通過驗證";
            return View(products);
        }
        #endregion

        #region 修改
        public ActionResult Edit(int ProductID)
        {
            var productToEdit = db.Products.Where(x=>x.ProductID == ProductID).FirstOrDefault();

            var supplierID = db.Suppliers.OrderBy(x => x.SupplierID).Select(x => new SelectListItem
            {
                Text = x.SupplierID.ToString(),
                Value = x.SupplierID.ToString()
            });
            Session["SupplierID"] = supplierID;

            var categoryID = db.Categories.OrderBy(x => x.CategoryID).Select(x => new SelectListItem
            {
                Text = x.CategoryID.ToString(),
                Value = x.CategoryID.ToString(),
            });
            Session["CategoryID"] = categoryID;

            return View(productToEdit);
        }

        [HttpPost]
        public ActionResult Edit(Products products)
        {
            if (ModelState.IsValid)
            {
                var productToEdit = db.Products.Where(x => x.ProductID == products.ProductID).FirstOrDefault();
                productToEdit.ProductName = products.ProductName;
                productToEdit.SupplierID = products.SupplierID;
                productToEdit.CategoryID = products.CategoryID;
                productToEdit.QuantityPerUnit = products.QuantityPerUnit;
                productToEdit.UnitPrice = products.UnitPrice;
                productToEdit.UnitsInStock = products.UnitsInStock;
                productToEdit.UnitsOnOrder = products.UnitsOnOrder;
                productToEdit.ReorderLevel = products.ReorderLevel;
                productToEdit.Discontinued = products.Discontinued;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(products);
        }
        #endregion

        #region 刪除
        public ActionResult Delete(int ProductID)
        {
            var productToDelete = db.Products.Where(x=>x.ProductID == ProductID).FirstOrDefault();
            db.Products.Remove(productToDelete);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion
    }
}