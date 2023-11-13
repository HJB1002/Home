using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    public class LoginUserController : Controller
    {
        ShopEntities db = new ShopEntities();
        // GET: LoginUser
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoginCustomer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginCustomer(Customer _user)
        {
            var check = db.Customers.Where(s => s.IDCus == _user.IDCus && s.Password == _user.Password).FirstOrDefault();
            if (check == null) // login sai thong tin
            {
                ViewBag.ErrorInfo = "Sai thông tin";
                return View("LoginCustomer");
            }

            else
            {
                var cus = db.Customers.Find(_user.IDCus);
                db.Configuration.ValidateOnSaveEnabled = false;
                Session["ID"] = _user.IDCus;
                Session["NameCus"] = cus.NameCus;
                ViewBag.NameCus = _user.NameCus;
                return RedirectToAction("xemTatCa", "Products");
            }
        }
        public ActionResult LoginAcount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAcount(AdminUser _user)
        {
            var check = db.AdminUsers.Where(s => s.ID == _user.ID && s.PasswordUser == _user.PasswordUser).FirstOrDefault();
            if (check == null) // login sai thong tin
            {
                ViewBag.ErrorInfo = "Sailnfo";
                return View("LoginAcount");
            }

            else
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                Session["ID"] = _user.ID;
                Session["PasswordUser"] = _user.PasswordUser;
                return RedirectToAction("Index", "Product");
            }
        }
        public ActionResult RegisterUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterUser(AdminUser _user)
        {
            if (ModelState.IsValid)
            {
                var check_ID = db.AdminUsers.Where(s => s.ID == _user.ID).FirstOrDefault();
                if (check_ID == null)
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.AdminUsers.Add(_user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorRegister = "This ID is exixst";
                    return View();
                }
            }
            return View();
        }
        public ActionResult LogOutCustomer()
        {
            Session.Abandon();
            return RedirectToAction("LoginCustomer", "LoginUser");
        }
    }
}