using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TDDStore.Core;
using TDDStore.Core.Abstractions;

namespace TDDStore.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreItemRepository _storeItemRepository;
        private readonly IWarehouseService _warehouseService;
        private readonly IShoppingCardRepository _shoppingCardRepository;
        private readonly IUserCreditService _userCreditService;
        private readonly IPaymentService _paymentService;

        public StoreController(IStoreItemRepository storeItemRepository, IWarehouseService warehouseService,
            IShoppingCardRepository shoppingCardRepository, IUserCreditService userCreditService, IPaymentService paymentService)
        {
            _storeItemRepository = storeItemRepository;
            _warehouseService = warehouseService;
            _shoppingCardRepository = shoppingCardRepository;
            _userCreditService = userCreditService;
            _paymentService = paymentService;
        }

        [HttpGet]
        public IActionResult GetAllStoreItems()
        {
            var storeItems = _storeItemRepository.GetAll();

            return Ok(storeItems);
        }

        [HttpGet]
        public IActionResult AddToCart(string userId, Guid storeItemId, int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest("Quantity must be greater than 0.");
            }

            var inventoryCount = _warehouseService.GetInventoryCount(storeItemId);

            if (quantity > inventoryCount)
            {
                return BadRequest("Warehouse doesn't have enough items in inventory - order canceled.");
            }

            var storeItem = _storeItemRepository.GetOne(storeItemId);

            var shoppingCart = new ShoppingCart(userId);
            var order = new Order(storeItemId, storeItem.Price, quantity);
            shoppingCart.AddOrder(order);

            _shoppingCardRepository.SaveShoppingCart(shoppingCart);

            return Ok("Order added to shopping cart!");
        }

        [HttpGet]
        public IActionResult Pay(string userId, Guid shoppingCartId)
        {
            var shoppingCart = _shoppingCardRepository.GetShoppingCartForUser(userId);
            var currentBalance = _userCreditService.GetUserCurrentBalance(userId);

            if (shoppingCart.GetTotalPrice() > currentBalance)
            {
                return BadRequest("Not enough credits.");
            }

            var transactionId = _paymentService.Pay(userId, shoppingCartId);

            return Ok($"Payment was successful! Your transaction id is '{transactionId}'.");
        }
    }
}
