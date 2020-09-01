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
        public Dictionary<String,float> Items { get; set; }
    }
}
