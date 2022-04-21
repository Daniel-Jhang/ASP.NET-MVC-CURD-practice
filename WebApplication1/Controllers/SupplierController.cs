using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using PagedList;

namespace WebApplication1.Controllers
{
    public class SupplierController : Controller
    {
        northwndEntities db = new northwndEntities();

        int pageSize = 10;

        #region 列表
        // GET: Supplier
        public ActionResult Index(int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;
            var suppliers = db.Suppliers.OrderBy(x => x.SupplierID).ToList();
            var result = suppliers.ToPagedList(currentPage, pageSize);
            return View(result);
        }
        #endregion

        #region 新增
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Suppliers suppliers)
        {
            ViewBag.Error = false;
            if (ModelState.IsValid)
            {
                var temp = db.Suppliers.Where(x => x.CompanyName == suppliers.CompanyName).FirstOrDefault();
                if (temp != null)
                {
                    ViewBag.Error = true;
                    return View(suppliers);
                }
                db.Suppliers.Add(suppliers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }
        #endregion

        #region 修改
        public ActionResult Edit(int SupplierID = 1)
        {
            var supplierToEdit = db.Suppliers.Where(x=>x.SupplierID == SupplierID).FirstOrDefault();
            return View(supplierToEdit);
        }

        [HttpPost]
        public ActionResult Edit(Suppliers suppliers)
        {
            var supplierToEdit = db.Suppliers.Where(x=>x.SupplierID == suppliers.SupplierID).FirstOrDefault();
            if (ModelState.IsValid)
            {
                supplierToEdit.CompanyName = suppliers.CompanyName;
                supplierToEdit.ContactName = suppliers.ContactName;
                supplierToEdit.ContactTitle = suppliers.ContactTitle;
                supplierToEdit.Address = suppliers.Address;
                supplierToEdit.City = suppliers.City;
                supplierToEdit.Region = suppliers.Region;
                supplierToEdit.PostalCode = suppliers.PostalCode;
                supplierToEdit.Country = suppliers.Country;
                supplierToEdit.Phone = suppliers.Phone;
                supplierToEdit.Fax = suppliers.Fax;
                supplierToEdit.HomePage = suppliers.HomePage;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplierToEdit);
        }
        #endregion

        #region 刪除
        public ActionResult Delete(int SupplierID)
        {
            var supplierToDelete = db.Suppliers.Where(x => x.SupplierID == SupplierID).FirstOrDefault();
            db.Suppliers.Remove(supplierToDelete);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion
    }
}