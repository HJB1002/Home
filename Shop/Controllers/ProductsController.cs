using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shop.Models;
using Antlr.Runtime.Tree;
using System.IO;
using PagedList;
using PagedList.Mvc;


namespace Shop.Controllers
{
    public class ProductsController : Controller
    { 
        private ShopEntities db = new ShopEntities();
        public ActionResult SearchOption(double min = double.MinValue, double max = double.MaxValue)
        {
            var list = db.Products.Where(s=>(double)s.Price >= min && (double)s.Price <= max).ToList();
            return View(list);
        }
        // GET: Products
        public ActionResult Index(string category)
        {
            ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate");
            if (category == null)
            {
                var productList = db.Products.OrderByDescending(x => x.NamePro);
                return View(productList);
            }
            else
            {
                var productList = db.Products.OrderByDescending(x => x.NamePro).Where(x => x.Category == category);
                return View(productList);
            }
        }
        public ActionResult xemTatCa(string category,int? page)
        {
            int pageSize = 16;
            int pageNum = (page ?? 1);
            if (category == null)
            {
                ViewBag.Category = "Tất cả sản phẩm";
                var productList = db.Products.OrderByDescending(x => x.NamePro).Where(x => x.Quantity > 0);
                return View(productList.ToPagedList(pageNum,pageSize));
            }
            else
            {
                var cate = db.Categories.Find(category);
                ViewBag.Category = cate.NameCate;
                var productList = db.Products.OrderByDescending(x => x.NamePro).Where(x => x.Category == category);
                return View(productList.ToPagedList(pageNum, pageSize));
            }
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,NamePro,DecriptionPro,Category,Price,ImagePro,UploadImage,Quantity,Size")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate", product.Category);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate", product.Category);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,NamePro,DecriptionPro,Category,Price,ImagePro,UploadImage,Quantity,Size")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.UploadImage != null)
                {
                    string path = "~/Content/image/";
                    string filename = Path.GetFileName(product.UploadImage.FileName);
                    product.ImagePro = path + filename;
                    product.UploadImage.SaveAs(Path.Combine(Server.MapPath(path), filename));
                }
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate", product.Category);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult UploadProduct()
        {
            ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadProduct([Bind(Include = "ProductID, NamePro, DecriptionPro, Category, Price, ImagePro, UploadImage, Quantity, Size")] Product product)
        {
            if(ModelState.IsValid)
            {
                if(product.UploadImage != null)
                {
                    string path = "~/Content/image/";
                    string filename = Path.GetFileName(product.UploadImage.FileName);
                    product.ImagePro = path + filename;
                    product.UploadImage.SaveAs(Path.Combine(Server.MapPath(path), filename));
                }
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate", product.Category);
            return View(product);
        }
        public ActionResult ProductDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        public ActionResult searchByName(string name, string cate, int? size)
        {
            var productList = db.Products.ToList();
            if (String.IsNullOrEmpty(name))
            {
                if(size == null)
                {
                    var productList1 = db.Products.OrderByDescending(x => x.NamePro).Where( x => x.Category == cate && x.Quantity > 0).ToList();
                    return View(productList1);
                }
                else
                {
                    var productList2 = db.Products.OrderByDescending(x => x.NamePro).Where(x => x.Category == cate && x.Size == size && x.Quantity > 0).ToList();
                    return View(productList2);
                }
               
            }
            else if(!String.IsNullOrEmpty(name))
            {
                if(size == null)
                {
                    var productList3 = db.Products.OrderByDescending(x => x.NamePro).Where(x => x.Category == cate && x.Quantity > 0 && x.NamePro.ToUpper().Contains(name.ToUpper())).ToList();
                    return View(productList3);
                }
                else
                {
                    var productList4 = db.Products.OrderByDescending(x => x.NamePro).Where(x => x.Category == cate && x.Quantity > 0 && x.NamePro.ToUpper().Contains(name.ToUpper()) && x.Size == size).ToList();
                    return View(productList4);
                }
            }
            return View(productList);
        }
     
    }
}
