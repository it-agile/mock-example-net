namespace CartExample
{
    public interface PriceUpdates
    {
        double PriceForProduct(string productId);
        void PricesChangedStats(int numberOfChanges);
    }


    [System.Serializable]
    public class PriceNotFound : System.Exception
    {
        public PriceNotFound() { }
        public PriceNotFound(string message) : base(message) { }
        public PriceNotFound(string message, System.Exception inner) : base(message, inner) { }
        protected PriceNotFound(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}