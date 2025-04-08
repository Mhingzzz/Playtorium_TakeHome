using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface ICartService
    {
       Task<Cart> InitCart(int customerId);
       Task<Cart?> GetCartByCustomerIdAsync(int customerId);
   }
}
