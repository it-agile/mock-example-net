using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CartExample
{
    [TestClass]
    public class CartPriceUpdaterTest   
    {
        Mock<CartArchive> archiveMock = new Mock<CartArchive>();
        Mock<PriceUpdates> priceMock = new Mock<PriceUpdates>();
        CartPriceUpdater objectUnderTest;

        [TestInitialize]
        public void setUp()
        {
            objectUnderTest = new CartPriceUpdater(archiveMock.Object, priceMock.Object);
        }

        [TestMethod]
        public void TestCreateNewCartIfNotExists()
        {
            objectUnderTest = new CartPriceUpdater(archiveMock.Object, null);
            Cart newCart = new Cart();

            archiveMock.Setup(mc => mc.ById(0815)).Returns((Cart)null);
            archiveMock.Setup(mc => mc.CreateNewCart()).Returns(newCart);

            Cart returnedCart = objectUnderTest.RecalculateCart(0815);
            Assert.AreEqual(newCart, returnedCart);
        }

        [TestMethod]
        public void TestEmptyCartNeverCallsPriceForProduct()
        {
            Cart cart = new Cart().withId(1);

            archiveMock.Setup(mc => mc.ById(1)).Returns(cart);
            Cart returnedCart = objectUnderTest.RecalculateCart(1);
            Assert.AreEqual(cart, returnedCart);

            priceMock.Verify(m => m.PriceForProduct(It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public void TestCartWithOneItemIsUpdated()
        {
            Cart cart = new Cart().withId(1).addProduct("Brot", 1.69);

            archiveMock.Setup(mc => mc.ById(1)).Returns(cart);
            priceMock.Setup(mc => mc.PriceForProduct("Brot")).Returns(1.79);
            Cart returnedCart = objectUnderTest.RecalculateCart(1);

            Assert.AreEqual(1.79, cart.Items["Brot"]);
        }

        [TestMethod]
        public void TestCartWithoutChangeDoesNotCallStats()
        {
            Cart cart = new Cart().withId(1).addProduct("Brot", 1.69);

            archiveMock.Setup(mc => mc.ById(1)).Returns(cart);
            priceMock.Setup(mc => mc.PriceForProduct("Brot")).Returns(1.69);
            Cart returnedCart = objectUnderTest.RecalculateCart(1);

            priceMock.Verify(m => m.PricesChangedStats(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void TestCartWithChangesCallsStats()
        {
            Cart cart = new Cart().withId(1).addProduct("Brot", 1.69);

            archiveMock.Setup(mc => mc.ById(1)).Returns(cart);
            priceMock.Setup(mc => mc.PriceForProduct("Brot")).Returns(1.79);
            Cart returnedCart = objectUnderTest.RecalculateCart(1);

            priceMock.Verify(m => m.PricesChangedStats(1), Times.Once());
        }

        [TestMethod]
        public void TestCartCanHandleMultipleEntries()
        {
            Cart cart = new Cart().withId(1)
                .addProduct("Brot", 1.69)
                .addProduct("Butter", 1.19)
                .addProduct("Marmelade", 2.69);

            archiveMock.Setup(mc => mc.ById(1)).Returns(cart);
            priceMock.Setup(mc => mc.PriceForProduct("Brot")).Returns(1.69);
            priceMock.Setup(mc => mc.PriceForProduct("Butter")).Returns(1.29);
            priceMock.Setup(mc => mc.PriceForProduct("Marmelade")).Returns(2.79);
            Cart returnedCart = objectUnderTest.RecalculateCart(1);

            priceMock.Verify(m => m.PricesChangedStats(2), Times.Once());
            Assert.AreEqual(1.69, cart.Items["Brot"]);
            Assert.AreEqual(1.29, cart.Items["Butter"]);
            Assert.AreEqual(2.79, cart.Items["Marmelade"]);
        }

        [TestMethod]
        public void TestProductWithoutPrice()
        {
            Cart cart = new Cart().withId(1).addProduct("<UNKNOWN>", 9.99);

            archiveMock.Setup(mc => mc.ById(1)).Returns(cart);
            priceMock.Setup(mc => mc.PriceForProduct("<UNKNOWN>")).Throws(new PriceNotFound());
            Cart returnedCart = objectUnderTest.RecalculateCart(1);

            Assert.AreEqual(0.0, cart.Items["<UNKNOWN>"]);
            priceMock.Verify(m => m.PricesChangedStats(It.IsAny<int>()), Times.Never);
        }
    }
}
