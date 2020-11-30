using System;
using TDDStore.Core;
using TDDStore.Core.Abstractions;
using TDDStore.Infrastructure.Services;

namespace TDDStore.App
{
    class Program
    {
        private static readonly IWarehouseService _warehouseService;

        static Program()
        {
            _warehouseService = new WarehouseService();
        }

        static void Main(string[] args)
        {
            var userId = args[0];
            var storeItemId = Guid.Parse(args[1]);
            var quantity = int.Parse(args[2]);

            var inventoryCount = _warehouseService.GetInventoryCount(storeItemId);

            if (quantity > inventoryCount)
            {
                throw new Exception("Warehouse doesn't have enough items in inventory - order canceled.");
            }

            var shoppingCart = new ShoppingCart(userId);
            var order = new Order(storeItemId, quantity);
            shoppingCart.AddOrder(order);
        }
    }
}
