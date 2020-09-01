using System;
using System.Collections.Generic;
using System.Text;

namespace CartExample
{
    public interface CartArchive
    {
        Cart ById(int Id);
        List<Cart> ByName(string UserName);
        Cart CreateNewCart();
    }
}
