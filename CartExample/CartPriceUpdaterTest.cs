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
            Cart newCart = new Cart().withId(1);

            archiveMock.Setup(mc => mc.ById(1)).Returns(newCart);
            Cart returnedCart = pc.RecalculateCart(1);
            Assert.AreEqual(newCart, returnedCart);

            priceMock.Verify(m => m.PriceForProduct(It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public void TestCartWithOneItemIsUpdated()
        {
            CartPriceUpdater pc = new CartPriceUpdater(archiveMock.Object, priceMock.Object);
            Cart newCart = new Cart().withId(1).addProduct("Brot", 1.69);

            archiveMock.Setup(mc => mc.ById(1)).Returns(newCart);
            priceMock.Setup(mc => mc.PriceForProduct("Brot")).Returns(1.79);
            Cart returnedCart = pc.RecalculateCart(1);

            Assert.AreEqual(1.79, newCart.Items["Brot"]);
        }

        [TestMethod]
        public void TestCartWithoutChangeDoesNotCallStats()
        {
            CartPriceUpdater pc = new CartPriceUpdater(archiveMock.Object, priceMock.Object);
            Cart newCart = new Cart().withId(1).addProduct("Brot", 1.69);

            archiveMock.Setup(mc => mc.ById(1)).Returns(newCart);
            priceMock.Setup(mc => mc.PriceForProduct("Brot")).Returns(1.69);
            Cart returnedCart = pc.RecalculateCart(1);

            priceMock.Verify(m => m.PricesChanged(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void TestCartWillCallStatistics()
        {
            CartPriceUpdater pc = new CartPriceUpdater(archiveMock.Object, priceMock.Object);
            Cart newCart = new Cart().withId(1).addProduct("Brot", 1.69);

            archiveMock.Setup(mc => mc.ById(1)).Returns(newCart);
            priceMock.Setup(mc => mc.PriceForProduct("Brot")).Returns(1.79);
            Cart returnedCart = pc.RecalculateCart(1);

            priceMock.Verify(m => m.PricesChanged(1), Times.Once());
        }

        [TestMethod]
        public void TestCartCanHandleMultipleEntries()
        {
            CartPriceUpdater pc = new CartPriceUpdater(archiveMock.Object, priceMock.Object);
            Cart newCart = new Cart().withId(1)
                .addProduct("Brot", 1.69)
                .addProduct("Butter", 1.19)
                .addProduct("Marmelade", 2.69);

            archiveMock.Setup(mc => mc.ById(1)).Returns(newCart);
            priceMock.Setup(mc => mc.PriceForProduct("Brot")).Returns(1.69);
            priceMock.Setup(mc => mc.PriceForProduct("Butter")).Returns(1.29);
            priceMock.Setup(mc => mc.PriceForProduct("Marmelade")).Returns(2.79);
            Cart returnedCart = pc.RecalculateCart(1);

            priceMock.Verify(m => m.PricesChanged(2), Times.Once());
            Assert.AreEqual(1.69, newCart.Items["Brot"]);
            Assert.AreEqual(1.29, newCart.Items["Butter"]);
            Assert.AreEqual(2.79, newCart.Items["Marmelade"]);
        }
    }
}
