using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using TDDStore.Api.Controllers;
using TDDStore.Core;
using TDDStore.Core.Abstractions;
using Xunit;

namespace TDDStore.Api.UnitTests.Controllers
{
    public class StoreControllerTests
    {
        private readonly IStoreItemRepository _storeItemRepository;
        private readonly IWarehouseService _warehouseService;
        private readonly IShoppingCardRepository _shoppingCardRepository;
        private readonly IUserCreditService _userCreditService;
        private readonly IPaymentService _paymentService;

        public StoreControllerTests()
        {
            _storeItemRepository = A.Fake<IStoreItemRepository>();
            _warehouseService = A.Fake<IWarehouseService>();
            _shoppingCardRepository = A.Fake<IShoppingCardRepository>();
            _userCreditService = A.Fake<IUserCreditService>();
            _paymentService = A.Fake<IPaymentService>();
        }

        [Fact]
        public void GetAllStoreItems_StoreItemList()
        {
            // Arrange
            var storeItems = new List<StoreItem>
            {
                new StoreItem("StoreItem1", "StoreItem1 Description", 10),
                new StoreItem("StoreItem2", "StoreItem2 Description", 20),
                new StoreItem("StoreItem3", "StoreItem3 Description", 30)
            };

            A.CallTo(() => _storeItemRepository.GetAll())
                .Returns(storeItems);
            
            var sut = new StoreController(_storeItemRepository, _warehouseService, _shoppingCardRepository,
                _userCreditService, _paymentService);

            // Act
            var result = sut.GetAllStoreItems();

            // Assert
            result.Should()
                .BeOfType<OkObjectResult>()
                .And.Match<OkObjectResult>(objectResult => ((IEnumerable<StoreItem>)objectResult.Value).Count() == storeItems.Count);
        }

        [Fact]
        public void AddToCart_ValidParameters_OkObjectResult()
        {
            // Arrange
            var userId = "userId";
            var storeItemId = Guid.NewGuid();
            var quantity = 5;
            var sut = new StoreController(_storeItemRepository, _warehouseService, _shoppingCardRepository,
                _userCreditService, _paymentService);

            A.CallTo(() => _warehouseService.GetInventoryCount(A<Guid>.Ignored))
                .Returns(10);

            // Act
            var result = sut.AddToCart(userId, storeItemId, quantity);

            // Assert
            result.Should()
                .BeOfType<OkObjectResult>();
        }

        [Fact]
        public void AddToCart_QuantityZero_BadRequestObjectResult()
        {
            // Arrange
            var userId = "userId";
            var storeItemId = Guid.NewGuid();
            var quantity = 0;
            var sut = new StoreController(_storeItemRepository, _warehouseService, _shoppingCardRepository,
                _userCreditService, _paymentService);

            A.CallTo(() => _warehouseService.GetInventoryCount(A<Guid>.Ignored))
                .Returns(10);

            // Act
            var result = sut.AddToCart(userId, storeItemId, quantity);

            // Assert
            result.Should()
                .BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void AddToCart_InventoryCountLessThanOrderQuantity_BadRequestObjectResult()
        {
            // Arrange
            var userId = "userId";
            var storeItemId = Guid.NewGuid();
            var quantity = 5;
            var sut = new StoreController(_storeItemRepository, _warehouseService, _shoppingCardRepository,
                _userCreditService, _paymentService);

            A.CallTo(() => _warehouseService.GetInventoryCount(A<Guid>.Ignored))
                .Returns(0);

            // Act
            var result = sut.AddToCart(userId, storeItemId, quantity);

            // Assert
            result.Should()
                .BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void Pay_ValidParameters_OkObjectResult()
        {
            // Arrange
            var userId = "userId";
            var shoppingCartId = Guid.NewGuid();
            var sut = new StoreController(_storeItemRepository, _warehouseService, _shoppingCardRepository,
                _userCreditService, _paymentService);

            // Act
            var result = sut.Pay(userId, shoppingCartId);

            // Assert
            result.Should()
                .BeOfType<OkObjectResult>();
        }

        [Fact]
        public void Pay_UserCreditBalanceLessThanTotalCartPrice_BadRequestObjectResult()
        {
            // Arrange
            var userId = "userId";
            var shoppingCartId = Guid.NewGuid();
            var sut = new StoreController(_storeItemRepository, _warehouseService, _shoppingCardRepository,
                _userCreditService, _paymentService);

            var shoppingCart = new ShoppingCart(userId);
            shoppingCart.AddOrder(new Order(Guid.NewGuid(), 10, 1));
            shoppingCart.AddOrder(new Order(Guid.NewGuid(), 20, 2));
            shoppingCart.AddOrder(new Order(Guid.NewGuid(), 30, 3));

            A.CallTo(() => _shoppingCardRepository.GetShoppingCartForUser(A<string>.Ignored))
                .Returns(shoppingCart);
            A.CallTo(() => _userCreditService.GetUserCurrentBalance(A<string>.Ignored))
                .Returns(0);

            // Act
            var result = sut.Pay(userId, shoppingCartId);

            // Assert
            result.Should()
                .BeOfType<BadRequestObjectResult>();
        }
    }
}
