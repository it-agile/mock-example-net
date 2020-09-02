namespace CartExample
{
    public interface PriceUpdates
    {
        double PriceForProduct(string productId);
        void PricesChanged(int v);
    }
}