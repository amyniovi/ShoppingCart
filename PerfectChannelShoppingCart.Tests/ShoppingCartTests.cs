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
        private CartController _cartController;
        private string _testName = "milk";
        private int _testId = 1;
        private int _anotherTestId = 4;
        [SetUp]
        public void SetUp()
        {
            _cartController = new CartController();

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
        public void Patch_Cart_ItemId_DoesntAddIfOutOfStock()
        {
        }

        [Test]
        public void Patch_Cart_ItemName_DoesntAddIfOutOfStock()
        {
        }

        [Test]
        public void Patch_Cart_ItemsAndQty_ReturnsCartWithExpectedItemsandQty()
        {
        }

        [Test]

        public void Get_Cart_Checkout_ReturnInvoiceOfItems()
        {
        }

        [Test]
        public void Get_Invoice_ByCartName_GetInvoiceOfCorrectItems()
        {
        }

        [Test]
        public void Get_Invoice_ByCartName_ThenRequestAllItems_ExpectStockReduced()
        {
        }

        [TearDown]
        public void TearDown()
        {
            CartRepo.Carts.Clear();
        }

    }
}
