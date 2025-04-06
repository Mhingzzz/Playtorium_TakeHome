using Application.ContractRepo;
using Application.Interfaces;
using Domain;

namespace Application.Service
{
    public class CartItemService : ICartItemService
    {

        private readonly ICartItemRepository _cartItemRepository;
        public CartItemService(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

       
       


    }
}
