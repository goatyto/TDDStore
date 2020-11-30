using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TDDStore.Core
{
    public class ShoppingCart
    {
        public Guid Id { get; }
        public string UserId { get; }
        public List<Order> Orders { get; }

        public ShoppingCart(string userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Orders = new List<Order>();
        }

        public void AddOrder(Order order)
        {
            Orders.Add(order);
        }

        public decimal GetTotalPrice()
        {
            return Orders.Sum(order => order.TotalPrice);
        }
    }
}