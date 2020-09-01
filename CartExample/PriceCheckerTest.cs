using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CartExample
{
    [TestClass]
    public class PriceCheckerTest   
    {
        [TestMethod]
        public void TestCreateNewCartIfNotExists()
        {
            var archiveMock = new Mock<CartArchive>();
            PriceChecker pc = new PriceChecker(archiveMock.Object);
            Cart newCart = new Cart();

            archiveMock.Setup(mc => mc.ById(0815)).Returns((Cart)null);
            archiveMock.Setup(mc => mc.CreateNewCart()).Returns(newCart);

            Cart returnedCart = pc.RecalculateCart(0815);
            Assert.AreEqual(newCart, returnedCart);

        }
    }
}
