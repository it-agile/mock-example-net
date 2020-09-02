using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CartExample
{
    [TestClass]
    public class CartPriceUpdaterTest   
    {
        Mock<CartArchive> archiveMock = new Mock<CartArchive>();
        Mock<PriceUpdates> priceMock = new Mock<PriceUpdates>();

        [TestMethod]
        public void TestCreateNewCartIfNotExists()
        {
            CartPriceUpdater pc = new CartPriceUpdater(archiveMock.Object, null);
            Cart newCart = new Cart();

            archiveMock.Setup(mc => mc.ById(0815)).Returns((Cart)null);
            archiveMock.Setup(mc => mc.CreateNewCart()).Returns(newCart);

            Cart returnedCart = pc.RecalculateCart(0815);
            Assert.AreEqual(newCart, returnedCart);
        }

        [TestMethod]
        public void TestEmptyCartNeverCallsPrice()
        {
            CartPriceUpdater pc = new CartPriceUpdater(archiveMock.Object, priceMock.Object);
            Cart newCart = new Cart();

            archiveMock.Setup(mc => mc.ById(0815)).Returns(newCart);
            Cart returnedCart = pc.RecalculateCart(0815);
            Assert.AreEqual(newCart, returnedCart);

            priceMock.Verify(m => m.PriceForProuct(It.IsAny<string>()), Times.Never());
        }
    }
}
