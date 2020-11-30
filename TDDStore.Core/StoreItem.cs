using System;
using System.Collections.Generic;
using System.Text;

namespace TDDStore.Core
{
    public class StoreItem
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public decimal Price { get; private set; }

        public StoreItem(string name, string description, decimal price)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
        }

        public void SetPrice(decimal price)
        {
            Price = price;
        }
    }
}
