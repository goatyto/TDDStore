using System;
using System.Collections.Generic;
using System.Text;

namespace TDDStore.Core
{
    public class Order
    {
        public Guid Id { get; }
        public Guid StoreItemId { get; }
        public decimal PricePerItem { get; }
        public int Quantity { get; }
        public decimal TotalPrice => PricePerItem * Quantity;

        public Order(Guid storeItemId, decimal pricePerItem, int quantity)
        {
            Id = Guid.NewGuid();
            StoreItemId = storeItemId;
            PricePerItem = pricePerItem;
            Quantity = quantity;
        }
    }
}
