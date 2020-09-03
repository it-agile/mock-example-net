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

        /**
         * Wird kein Cart gefunden (null-Rückgabe) soll ein neuer Cart 
         * am Archiv erzeugt werden.
         */
        [TestMethod]
        public void TestCreateNewCartIfNotExists()
        {
            Cart newCart = new Cart();

            // Hier müssen ein oder mehrere Mocks vorbereitet werden

            Cart returnedCart = objectUnderTest.RecalculateCart(0815);
            Assert.AreEqual(newCart, returnedCart);
        }

        /**
         * Bei einem leeren Cart wird niemals der Preis neu ermittelt
         */
        [TestMethod]
        public void TestEmptyCartNeverCallsPriceForProduct()
        {
            Cart cart = new Cart().withId(1);

            // Hier müssen ein oder mehrere Mocks vorbereitet werden

            Cart returnedCart = objectUnderTest.RecalculateCart(1);
            Assert.AreEqual(cart, returnedCart);

            // Überprüfung?
        }

        [TestMethod]
        public void TestCartWithOneItemIsUpdated()
        {
            Cart cart = new Cart().withId(1).addProduct("Brot", 1.69);

            // Hier müssen ein oder mehrere Mocks vorbereitet werden

            Cart returnedCart = objectUnderTest.RecalculateCart(1);

            Assert.AreEqual(1.79, cart.Items["Brot"]);
        }

        /**
         * Wird der Preis nicht verändert, so wird die Stats-Methode
         * nicht aufgerufen.
         */
        [TestMethod]
        public void TestCartWithoutChangeDoesNotCallStats()
        {
            Cart cart = new Cart().withId(1).addProduct("Brot", 1.69);

            // Hier müssen ein oder mehrere Mocks vorbereitet werden

            Cart returnedCart = objectUnderTest.RecalculateCart(1);

            // Überprüfung?
        }

        /**
         * Wird der Preis verändert, so wird die Stats-Methode aufgerufen.
         */
        [TestMethod]
        public void TestCartWithChangesCallsStats()
        {
            Cart cart = new Cart().withId(1).addProduct("Brot", 1.69);

            // Hier müssen ein oder mehrere Mocks vorbereitet werden

            Cart returnedCart = objectUnderTest.RecalculateCart(1);

            // Überprüfung?
        }

        /**
         * Testet viele Optionen mit einigen Produkten,
         * deren Preise sich ändern. Auch die Statistik-Methode
         * wird aufgerufen
         */
        [TestMethod]
        public void TestCartCanHandleMultipleEntries()
        {
            Cart cart = new Cart().withId(1)
                .addProduct("Brot", 1.69)
                .addProduct("Butter", 1.19)
                .addProduct("Marmelade", 2.69);

            // Hier müssen ein oder mehrere Mocks vorbereitet werden

            Cart returnedCart = objectUnderTest.RecalculateCart(1);

            Assert.AreEqual(1.69, cart.Items["Brot"]);
            Assert.AreEqual(1.29, cart.Items["Butter"]);
            Assert.AreEqual(2.79, cart.Items["Marmelade"]);

            // Überprüfung?
        }

        /**
         * Wird kein Preis gefunden, soll eine Exception fliegen.
         * Der Preis wird zurückgesetzt und die Statistik soll nicht aktualisiert werden.
         */
        [TestMethod]
        public void TestProductWithoutPrice()
        {
            Cart cart = new Cart().withId(1).addProduct("<UNKNOWN>", 9.99);

            // Hier müssen ein oder mehrere Mocks vorbereitet werden

            Cart returnedCart = objectUnderTest.RecalculateCart(1);

            Assert.AreEqual(0.0, cart.Items["<UNKNOWN>"]);

            // Überprüfung?
        }
    }
}
