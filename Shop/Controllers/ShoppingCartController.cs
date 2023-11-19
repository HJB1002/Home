using Shop.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    public class ShoppingCartController : Controller
    {
        ShopEntities database = new ShopEntities();
        // GET: ShoppingCart
        public ActionResult Index(string category)
        {
            if(category == null)
            {
                var productList = database.Products.OrderByDescending(x => x.NamePro);
                return View(productList);
            }
            else
            {
                var productList = database.Products.OrderByDescending(x => x.NamePro).Where(x => x.Category == category);
                return View(productList);
            }
        }
        public ActionResult ShowCart()
        {
            if (Session["Cart"] == null)
                return View("EmptyCart");
            Cart _cart = Session["Cart"] as Cart;
            return View(_cart);
        }
        //Action tạo mới giỏ hàng
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if(cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        public ActionResult AddToCart(int id)
        {
            var _pro = database.Products.FirstOrDefault(s => s.ProductID == id);
            if(_pro != null)
            {
                GetCart().AddProductCart(_pro);
            }
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public ActionResult themGioHang(int id)
        {
            var _pro = database.Products.FirstOrDefault(s => s.ProductID == id);
            if (_pro != null)
            {
                GetCart().AddProductCart(_pro);
            }
            return RedirectToAction("ProductDetail","Products", new {id});
        }
        public ActionResult UpdateCartQuantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(form["idPro"]);
            int quantity = int.Parse(form["cartQuantity"]);
            cart.Update_quantity(id_pro, quantity);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_CartItem(id);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public PartialViewResult BagCart()
        {
            int total_quantity_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
                total_quantity_item = cart.Total_quantity();
            ViewBag.QuantityCart = total_quantity_item;
            return PartialView("BagCart");
        }
        public ActionResult CheckOut(FormCollection form)
        {
            try
            {
                Cart cart = Session["Cart"] as Cart;
                OrderPro _order = new OrderPro();
                _order.DateOrder = DateTime.Now;
                _order.AddressDeliverry = form["AddressDelivery"];
                _order.IDCus = int.Parse(form["CodeCustomer"]);
                database.OrderProes.Add(_order);
                foreach (var item in cart.Items)
                {
                    OrderDetail _orderdetail = new OrderDetail();
                    _orderdetail.IDOrder = _order.ID;
                    _orderdetail.IDProduct = item._product.ProductID;
                    _orderdetail.UnitPrice = (double)item._product.Price;
                    _orderdetail.Quantity = item._quantity;
                    database.OrderDetails.Add(_orderdetail);
                    foreach (var p in database.Products.Where(s => s.ProductID == _orderdetail.IDProduct))
                    {
                        var update_quan_pro = p.Quantity - item._quantity;
                        p.Quantity = update_quan_pro;
                    }
                }
                database.SaveChanges();
                cart.ClearCart();
                return RedirectToAction("CheckOut_Success", "ShoppingCart");
        }
            catch
            {
                return Content("Error checkout. Please check information of customer.... Thank for your patient");
    }

}
        public ActionResult CheckOut_Success()
        {
            return View();
        }
    }
}