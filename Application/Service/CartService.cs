using Application.ContractRepo;
using Application.Interfaces;
using Domain;

namespace Application.Service
{
    public class CartService : ICartService
    {

        private readonly ICartRepository _cartRepository;
        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<Cart> InitCart(int customerId)
        {
            try
            {
                var cartList = await _cartRepository.GetCartByCustomerId(customerId);

                var activeCart = cartList.FirstOrDefault(x => x.Status == CartStatus.Active.ToString());
                if (activeCart != null)
                {
                    return activeCart;
                }

                var newCart = new Cart
                {
                    CustomerId = customerId,
                    Status = CartStatus.Active.ToString(),

                };

                await _cartRepository.CreateCart(newCart);
                return newCart;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while initializing cart", ex);
            }
        }

        public async Task<List<Cart>> GetCartByCustomerId(int customerId)
        {
            try
            {
                var cartList = await _cartRepository.GetCartByCustomerId(customerId);
                // var activeCart = cartList.Where(x => x.Status == CartStatus.Active.ToString()).ToList();
                return cartList;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting cart by customer id", ex);
            }
        }



    }
}
