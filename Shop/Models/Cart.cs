﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class CartItem
    {
        public Product _product { get; set; }
        public int _quantity { get; set; }

    }
    public class Cart
    {
        List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items
        {
            get => items;
        }
        public void AddProductCart(Product _pro, int _quan = 1)
        {
            var item = Items.FirstOrDefault(s => s._product.ProductID == _pro.ProductID);
            if (item == null)
                items.Add(new CartItem
                {
                    _product = _pro,
                    _quantity = _quan
                });
            else if(_pro.Quantity > item._quantity)
            {
                item._quantity += _quan;
            }
        }
        public int Total_quantity() { return items.Sum(s => s._quantity); }
        public decimal Total_Price()
        {
            var total = items.Sum(s => s._quantity * s._product.Price);
            return (decimal)total;
        }
        public void Update_quantity(int id, int _newquan)
        {
            var item = items.Find(s => s._product.ProductID == id);
            if (item != null)
            {
                if (items.Find(s => s._product.Quantity >= _newquan) != null)
                    item._quantity = _newquan;
                else item._quantity = 1;
            }
        }
        public void Remove_CartItem(int id)
        {
            items.RemoveAll(s => s._product.ProductID == id);
        }
        public void ClearCart()
        {
            items.Clear();
        }

    }

}