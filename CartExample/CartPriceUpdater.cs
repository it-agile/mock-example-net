using System;
using System.Collections.Generic;

namespace CartExample
{
    public class CartPriceUpdater
    {
        private CartArchive ArchiveService;
        private PriceUpdates PriceService;

        public CartPriceUpdater(CartArchive Archive, PriceUpdates Price)
        {
            ArchiveService = Archive;
            PriceService = Price;
        }

        public Cart RecalculateCart(int CartId)
        {
            Cart cartToCheck = null; // Woher laden?

            if(cartToCheck == null)
            {
                // Was passiert dann?
            }

            foreach (var pair in cartToCheck.Items)
            {
                // Irgendwas passiert noch mit den Einträgen
            }
            // Je nachdem, ob Preise verändert werden, muss
            // das am Price Service vermerkt werden.

            return cartToCheck;
        }
    }
}