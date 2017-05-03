using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http.Results;
using NUnit.Framework;
using PerfectChannelShoppingCart.Controllers;

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
        private const string _testName = "milk";
        private const int _testId = 1;
        private const int _anotherTestId = 4;
        private const int _outOfStockItemId = 3;
        private const string _outOfStockItemName = "Oranges";
        private Dictionary<string, int> _orderedItems;
        private Invoice _testInvoice;

        [SetUp]
        public void SetUp()
        {
            _cartController = new CartController();
            _orderController = new OrderController();
            _itemController = new ItemController();
            _orderedItems = new Dictionary<string, int>();
            _orderedItems.Add("Bread", 2);
            _orderedItems.Add("Chocolate", 5);
            _testInvoice = new Invoice();
            _testInvoice.OrderedItems = new List<OrderedItem>()
            { new OrderedItem()
                {
                    Name = "Bread",Quantity = 2,TotalPrice = (2 * 1.35m)

                } ,
                new OrderedItem()
                {
                    Name = "Chocolate", Quantity = 5, TotalPrice = 5 * 1.79m

                } };
            _testInvoice.TotalPrice = 2 * 1.35m + 5 * 1.79m;
        }

        [Test]
        public void Get_Item_ReturnAllAvailItems()
        {
            var controller = new ItemController();
            var allItems = _itemRepo.Get();

            var result = controller.Get() as OkNegotiatedContentResult<IEnumerable<Item>>;

            CollectionAssert.AreEqual(allItems, result?.Content);
        }

        [Test]
        public void PostCart_ItemId_ReturnsTheCartWithTheItem()
        {

            _cartRepo.AddByUserName(TestUsername);

            var cart = _cartRepo.GetByUserName(TestUsername);
            var initialItemCount = cart.Items.Count();

            var result = _cartController.Post(_testId, TestUsername) as OkNegotiatedContentResult<Cart>;

            Assert.That(result?.Content.Items.Count() == initialItemCount + 1);
            Assert.That(result?.Content.Items.Last() == _itemRepo.GetbyId(_testId));
        }

        [Test]
        public void PostCart_ItemName_ReturnsTheCartWithTheItem()
        {
            _cartRepo.AddByUserName(TestUsername);

            var cart = _cartRepo.GetByUserName(TestUsername);
            var initialItemCount = cart.Items.Count();

            var result = _cartController.Post(_testName, TestUsername) as OkNegotiatedContentResult<Cart>;

            Assert.That(result?.Content.Items.Count() == initialItemCount + 1);
            Assert.That(result?.Content.Items.Last() == _itemRepo.GetbyName(_testName));
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

            _cartController.Post(_testId, TestUsername);
            var result = _cartController.Post(_anotherTestId, TestUsername) as OkNegotiatedContentResult<Cart>;

            Assert.That(result?.Content.Items.Count() == 2);
            var list = new List<Item> { _itemRepo.GetbyId(_testId), _itemRepo.GetbyId(_anotherTestId) };
            CollectionAssert.AreEqual(list, result.Content.Items);
        }

        [Test]
        public void PostCart_ItemId_DoesntAddIfOutOfStock()
        {
            _cartRepo.AddByUserName(TestUsername);
            var cart = _cartRepo.GetByUserName(TestUsername);
            var initialItemCount = cart.Items.Count();

            _cartController.Post(_outOfStockItemId, TestUsername);

            Assert.That(cart.Items.Count() == initialItemCount);
            Assert.That(cart.Items.Contains(_itemRepo.GetbyId(_outOfStockItemId)) == false);
        }

        [Test]
        public void PostCart_ItemName_DoesntAddIfOutOfStock()
        {
            _cartRepo.AddByUserName(TestUsername);
            var cart = _cartRepo.GetByUserName(TestUsername);
            var initialItemCount = cart.Items.Count();

            _cartController.Post(_outOfStockItemName, TestUsername);

            Assert.That(cart.Items.Count() == initialItemCount);
            Assert.That(cart.Items.Contains(_itemRepo.GetbyId(_outOfStockItemId)) == false);
        }

        [Test]
        public void CheckoutCart_PostOrder_ReturnInvoiceOfItems()
        {
            _cartRepo.AddByUserName(TestUsername);

            var result = _orderController.Post(_orderedItems) as OkNegotiatedContentResult<Invoice>;

            Assert.That(result?.Content.TotalPrice == _testInvoice.TotalPrice);
            //check individual items too (prices)
        }


        [Test]
        public void Get_Invoice_ByCartName_GetInvoiceOfCorrectItems()
        {
        }

        [Test]
        public void CheckoutCart_ThenRequestAllItems_ExpectStockReduced()
        {
            _cartRepo.AddByUserName(TestUsername);

            var result = _orderController.Post(_orderedItems) as OkNegotiatedContentResult<Invoice>;

            var resultItems = _itemController.Get() as OkNegotiatedContentResult<List<Item>>;
            var items = resultItems?.Content;
            Assert.That(items?.FirstOrDefault(item => item.Name == "Chocolate")?.Stock == 15);
        }

        [TearDown]
        public void TearDown()
        {
            CartRepo.Carts.Clear();
        }




    }
}
