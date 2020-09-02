using System;
using System.Collections.Generic;
using System.Text;

namespace CartExample
{
    public class Cart
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public String UserName { get; set; }
        public Dictionary<String,double> Items { get; set; }

        public Cart()
        {
            Items = new Dictionary<string, double>();
        }

        public Cart withId(int CartId)
        {
            this.CartId = CartId;
            return this;
        }

        public Cart withUser(int UserId, string Name)
        {
            this.UserId = UserId;
            this.UserName = Name;
            return this;
        }

        public Cart addProduct(string id, double price)
        {
            this.Items.Add(id, price);
            return this;
        }
    }
}
