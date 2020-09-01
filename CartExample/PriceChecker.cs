using System;

namespace CartExample
{
    public class PriceChecker
    {
        private CartArchive Archive;

        public PriceChecker(CartArchive Archive)
        {
            this.Archive = Archive;
        }

        public Cart RecalculateCart(int CartId)
        {
            var cartToCheck = Archive.ById(CartId);

            if(cartToCheck == null)
            {
                return Archive.CreateNewCart();
            }
            return null;
        }
    }
}