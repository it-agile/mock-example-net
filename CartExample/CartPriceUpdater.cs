using System;

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
            var cartToCheck = ArchiveService.ById(CartId);

            if(cartToCheck == null)
            {
                return ArchiveService.CreateNewCart();
            }
            return cartToCheck;
        }
    }
}