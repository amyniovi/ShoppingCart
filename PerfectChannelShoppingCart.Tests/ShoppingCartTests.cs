using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using NUnit.Framework;
using PerfectChannelShoppingCart.Controllers;
using PerfectChannelShoppingCart.Models;
using PerfectChannelShoppingCart.PChannel.Interfaces;
using PerfectChannelShoppingCart.PChannel.Repositories;

namespace PerfectChannelShoppingCart.Tests
{
    [TestFixture]
    public class ShoppingCartTests
    {
        private readonly IItemRepo _itemRepo = new ItemRepo();
        private readonly ICartRepo _cartRepo = new CartRepo();
        private const string TestUsername = "amy";
        private ItemController _itemController;
        private CartController _cartController;
        private OrderController _orderController;
        private const string TestName = "milk";
        private const int TestId = 1;
        private const int AnotherTestId = 4;
        private const int OutOfStockItemId = 3;
        private const string OutOfStockItemName = "Oranges";
        private readonly List<CartItem> _orderedItems = new List<CartItem>();
        private Invoice _testInvoice;

        [SetUp]
        public void SetUp()
        {
            _cartController = new CartController();
            _orderController = new OrderController();
            _itemController = new ItemController();
            _orderedItems.Add(new CartItem() { Name = "Bread", Qty = 2 });
            _orderedItems.Add(new CartItem() { Name = "chocolate", Qty = 5 });
            _testInvoice = new Invoice
            {
                OrderedItems = _orderedItems,
                TotalPrice = 2*1.35m + 5*1.79m
            };
        }

        [Test]
        public void Get_Item_ReturnAllAvailItems()
        {
            var controller = new ItemController();
            var allItems = _itemRepo.Get();
           // controller.Request = new HttpRequestMessage();
           // controller.Request.RequestUri = new Uri("http://api/item");
            var result = controller.Get() as OkNegotiatedContentResult<List<Item>>;

            CollectionAssert.AreEqual(allItems, result?.Content);
        }

        [Test]
        public void PostCart_ItemId_ReturnsTheCartWithTheItem()
        {

            _cartRepo.AddByUserName(TestUsername);

            var cart = _cartRepo.GetByUserName(TestUsername);
            var initialItemCount = cart.Items.Count();

            var result = _cartController.Post(TestId, 1, TestUsername) as OkNegotiatedContentResult<Cart>;

            Assert.That(result?.Content.Items.Count() == initialItemCount + 1);
            Assert.That(result?.Content.Items.Last().Id == TestId);
        }

        [Test]
        public void PostCart_ItemName_ReturnsTheCartWithTheItem()
        {
            _cartRepo.AddByUserName(TestUsername);

            var cart = _cartRepo.GetByUserName(TestUsername);
            var initialItemCount = cart.Items.Count();

            var result = _cartController.Post(TestName, 1, TestUsername) as OkNegotiatedContentResult<Cart>;

            Assert.That(result?.Content.Items.Count() == initialItemCount + 1);
            Assert.That(String.Equals(result?.Content.Items.Last().Name , TestName,StringComparison.CurrentCultureIgnoreCase));
        }

        [Test]
        public void Get_Cart_UserName_ReturnsCartForThatUser()
        {
            _cartRepo.AddByUserName(TestUsername);

            var result = _cartController.Get(TestUsername) as OkNegotiatedContentResult<Cart>;

            Assert.AreEqual(_cartRepo.GetByUserName(TestUsername), result?.Content);
        }

        [Test]
        public void AddDifferentItems_GetCart_ReturnsCartWithExpectedItems()
        {
            _cartRepo.AddByUserName(TestUsername);
            _cartController.Get(TestUsername);

            _cartController.Post(TestId, 1, TestUsername);
            var result = _cartController.Post(AnotherTestId, 1, TestUsername) as OkNegotiatedContentResult<Cart>;

            Assert.That(result?.Content.Items.Count() == 2);
           Assert.That(result.Content.Items.All(x=>x.Id ==TestId || x.Id == AnotherTestId));
        }

        [Test]
        public void AddSameItems_GetCart_ReturnsCartWithSingleItemAndCorrectQty()
        {
            _cartRepo.AddByUserName(TestUsername);
            _cartController.Get(TestUsername);

            _cartController.Post(TestId, 1, TestUsername);
            var result = _cartController.Post(TestId, 4, TestUsername) as OkNegotiatedContentResult<Cart>;

            Assert.That(result?.Content.Items.Count() == 1);
            Assert.That(result.Content.Items.All(x => x.Id == TestId));
        }


        [Test]
        public void PostCart_ItemId_DoesntAddIfOutOfStock()
        {
            _cartRepo.AddByUserName(TestUsername);
            var cart = _cartRepo.GetByUserName(TestUsername);
            var initialItemCount = cart.Items.Count();

            _cartController.Post(OutOfStockItemId, 1, TestUsername);

            Assert.That(cart.Items.Count() == initialItemCount);
            Assert.That(cart.Items.All(item => item.Id != OutOfStockItemId));
        }

        [Test]
        public void PostCart_ItemName_DoesntAddIfOutOfStock()
        {
            _cartRepo.AddByUserName(TestUsername);
            var cart = _cartRepo.GetByUserName(TestUsername);
            var initialItemCount = cart.Items.Count();

            _cartController.Post(OutOfStockItemName, 1, TestUsername);

            Assert.That(cart.Items.Count() == initialItemCount);
            Assert.That(cart.Items.All(item => item.Name != OutOfStockItemName));
        }

        [Test]
        public void CheckoutCart_PostOrder_ReturnInvoiceOfItems()
        {
            _cartRepo.AddByUserName(TestUsername);
            _cartController.Post("Bread", 2, TestUsername);
            _cartController.Post("Chocolate", 5, TestUsername);
            var result = _orderController.Get(TestUsername) as OkNegotiatedContentResult<Invoice>;

            Assert.That(result?.Content.TotalPrice == _testInvoice.TotalPrice);
        }

        [Test]
        public void CheckoutCart_ThenRequestAllItems_ExpectStockReduced()
        {
            _cartRepo.AddByUserName(TestUsername);
            _cartController.Post("Milk", 5, TestUsername);

            //checkout order
            _orderController.Get(TestUsername);

            var resultItems = _itemController.Get() as OkNegotiatedContentResult<List<Item>>;
            var items = resultItems?.Content;
            Assert.That(items?.FirstOrDefault(item => item.Name == "Milk")?.Stock == 5);
        }

        [TearDown]
        public void TearDown()
        {
            CartRepo.Carts.Clear();
        }
    }
}
