﻿using Domain;

namespace Application.ContractRepo
{
    public interface ICartItemRepository : IBaseRepository<CartItems>
    {
        Task<List<CartItems>> GetCartItemsByCartId(int cartId);
    }
}
