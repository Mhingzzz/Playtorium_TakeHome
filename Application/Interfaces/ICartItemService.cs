using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Requests;
using Domain;

namespace Application.Interfaces
{
    public interface ICartItemService
    {
        Task<CartItems> AddItemToCart(AddItemToCartRequest request);
        Task<CartItems> UpdateCartItem(int cartItemId, int quantity);
        
        Task<List<CartItems>> GetCartItemsByCartId(int cartId);
        
    }
}
