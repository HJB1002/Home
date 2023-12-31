﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Shop.Models;

namespace Shop.Controllers
{
    public class CategoriesController : Controller
    {
        private ShopEntities db = new ShopEntities();

        // GET: Categories
        public ActionResult Index(string _name)
        {
            if (_name == null)
                return View(db.Categories.ToList());
            else
                return View(db.Categories.Where(s => s.NameCate.Contains(_name)).ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Category category = db.Categories.Find(id);
            //if (category == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(category);
            return View(database.Categories.Where(s => s.Id == id).FirstOrDefault());
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IDCate,NameCate")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Category category = db.Categories.Find(id);
            //if (category == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(category);
            return View(database.Categories.Where(s => s.Id == id).FirstOrDefault());
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Category cate)
        {
            database.Entry(cate).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("Index");
        }
        //public ActionResult Edit([Bind(Include = "Id,IDCate,NameCate")] Category category)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(category).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(category);
        //}

        // GET: Categories/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Category category = db.Categories.Find(id);
        //    if (category == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(category);
        //}
        public ActionResult Delete(int id)
        {
            return View(database.Categories.Where(s => s.Id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, Category cate)
        {
            try
            {
                cate = database.Categories.Where(s => s.Id == id).FirstOrDefault();
                database.Categories.Remove(cate);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("This data is using on other table");
            }
        }

        // POST: Categories/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string idCate)
        //{
        //    Category category = db.Categories.Find();
        //    db.Categories.Remove(category);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult WatchAllPro(string cate)
        {
            return RedirectToAction("Index", "Products", new { id = cate });
        }
        ShopEntities database = new ShopEntities();
        public PartialViewResult CategoryPartial()
        {
            var cateList = database.Categories.ToList();
            return PartialView(cateList);
        }
        public PartialViewResult CategoryPartial2()
        {
            List<Category> list = database.Categories.ToList();
            ViewBag.ShowListUp = new SelectList(list, "IDCate", "NameCate", 1);
            return PartialView(list);
        }

    }
}
