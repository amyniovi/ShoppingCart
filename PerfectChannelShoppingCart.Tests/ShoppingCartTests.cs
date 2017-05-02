using System;
using System.Collections.Generic;
using System.Web.Http.Results;
using NUnit.Framework;
using PerfectChannelShoppingCart.Controllers;

namespace PerfectChannelShoppingCart.Tests
{
    [TestFixture]
    public class ShoppingCartTests
    {
        /// <summary>
        /// Test Api Get
        /// </summary>
        [Test]
        public void Get_Item_ReturnAllAvailItems()
        {
            var controller = new ItemController();
            var allItems = new ItemRepo().Get();
            var result = controller.Get() as OkNegotiatedContentResult<IEnumerable<Item>>;
            CollectionAssert.AreEqual(allItems,result?.Content);
        }


        [Test]
        public void Patch_Cart_ItemId_ReturnsTheCartWithTheItem()
        {
        }

        [Test]
        public void Patch_Cart_ItemName_ReturnsTheCartWithTheItem()
        {
        }

        [Test]
        public void Get_Cart_UserName_ReturnsCart()
        {
        }

        [Test]
        public void Get_Cart_UserName_ReturnsCartWithExpectedItems()
        {
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

    }
}
