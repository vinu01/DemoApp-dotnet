using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoApp.Models;

namespace DemoApp.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index(string search = "", string SortColumn="productName", string IconClass="fa-sort-asc", int pageNo=1)
        {
            EFDBFirstDatabaseEntities db = new EFDBFirstDatabaseEntities();
            ViewBag.search = search;
            List<Product> products = db.Products.Where(temp => temp.ProductName.Contains(search)).ToList();


            ViewBag.SortColumn = SortColumn;
            ViewBag.IconClass = IconClass;

            if(ViewBag.SortColumn == "ProductID")
            {
                if(ViewBag.IconClass=="fa-sort-asc")
                {
                    products = products.OrderBy(temp => temp.ProductID).ToList();
                }
                else
                {
                    products = products.OrderByDescending(temp => temp.ProductID).ToList();
                }

            }
            else if (ViewBag.SortColumn == "ProductName")
            {
                if (ViewBag.IconClass == "fa-sort-asc")
                {
                    products = products.OrderBy(temp => temp.ProductName).ToList();
                }
                else
                {
                    products = products.OrderByDescending(temp => temp.ProductName).ToList();
                }

            }
           else if (ViewBag.SortColumn == "Price")
            {
                if (ViewBag.IconClass == "fa-sort-asc")
                {
                    products = products.OrderBy(temp => temp.Price).ToList();
                }
                else
                {
                    products = products.OrderByDescending(temp => temp.Price).ToList();
                }

            }

            int noOfRecordsPerPage = 5;
            int noOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(products.Count) / Convert.ToDouble(noOfRecordsPerPage)));
            int noOfRecordsToSkip = (pageNo - 1) * noOfRecordsPerPage;
            ViewBag.pageNo = pageNo;
            ViewBag.noOfPages = noOfPages;
            products = products.Skip(noOfRecordsToSkip).Take(noOfRecordsPerPage).ToList();
            return View(products);
        }

        public ActionResult Details(long id)
        {
            EFDBFirstDatabaseEntities db = new EFDBFirstDatabaseEntities();
            Product p = db.Products.Where(temp => temp.ProductID == id).FirstOrDefault();
            return View(p);
        }
        public ActionResult Create()
        {
            EFDBFirstDatabaseEntities db = new EFDBFirstDatabaseEntities();
            ViewBag.Categories = db.Categories.ToList();

            ViewBag.Brands = db.Brands.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product p)
        {
            EFDBFirstDatabaseEntities db = new EFDBFirstDatabaseEntities();

            if (Request.Files.Count >= 1)
            {
                var file = Request.Files[0];
                var imgBytes = new Byte[file.ContentLength];
                file.InputStream.Read(imgBytes, 0, file.ContentLength);
                var base64String = Convert.ToBase64String(imgBytes, 0, imgBytes.Length);
                p.Photo = base64String;
            }


            db.Products.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(long id)
        {
            EFDBFirstDatabaseEntities db = new EFDBFirstDatabaseEntities();
            Product existingPrd = db.Products.Where(temp => temp.ProductID == id).FirstOrDefault();
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Brands = db.Brands.ToList();
                return View(existingPrd);
        }
        [HttpPost]
        public ActionResult Edit(Product p)
        {
            EFDBFirstDatabaseEntities db = new EFDBFirstDatabaseEntities();
            Product existingPrd = db.Products.Where(temp => temp.ProductID == p.ProductID).FirstOrDefault();
            existingPrd.ProductName = p.ProductName;
            existingPrd.Price = p.Price;
            existingPrd.DateOfPurchase = p.DateOfPurchase;
            existingPrd.AvailabilityStatus = p.AvailabilityStatus;
            existingPrd.CategoryID = p.CategoryID;
            existingPrd.BrandID = p.BrandID;
            existingPrd.Active = p.Active;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(long id)
        {
            EFDBFirstDatabaseEntities db = new EFDBFirstDatabaseEntities();
            Product existingPrd = db.Products.Where(temp => temp.ProductID == id).FirstOrDefault();
            return View(existingPrd);
        }
        [HttpPost]
        public ActionResult Delete(long id, Product p)
        {
            EFDBFirstDatabaseEntities db = new EFDBFirstDatabaseEntities();
            Product existingPrd = db.Products.Where(temp => temp.ProductID == p.ProductID).FirstOrDefault();
            db.Products.Remove(existingPrd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}