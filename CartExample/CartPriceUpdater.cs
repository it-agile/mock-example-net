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
            var cartToCheck = ArchiveService.ById(CartId);
            var changedPrices = new Dictionary<String, double>();

            if(cartToCheck == null)
            {
                return ArchiveService.CreateNewCart();
            }

            var changedPriceCount = 0;
            foreach (var pair in cartToCheck.Items)
            {
                try
                {
                    var newPrice = PriceService.PriceForProduct(pair.Key);
                    if (newPrice != pair.Value)
                    {
                        changedPrices.Add(pair.Key, newPrice);
                        changedPriceCount++;
                    }
                }
                catch (PriceNotFound ex)
                {
                   changedPrices.Add(pair.Key, 0.0);
                }
            }
            if(changedPriceCount > 0)
            {
                PriceService.PricesChangedStats(changedPrices.Count);
            }
            foreach (var pair in changedPrices)
            {
                cartToCheck.Items.Remove(pair.Key);
                cartToCheck.Items.Add(pair.Key, pair.Value);
            }

            return cartToCheck;
        }
    }
}