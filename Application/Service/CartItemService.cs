using Application.ContractRepo;
using Application.DTOs.Requests;
using Application.Interfaces;
using Domain;

namespace Application.Service
{
    public class CartItemService : ICartItemService
    {

        private readonly ICartItemRepository _cartItemRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IItemRepository _itemRepository;
        public CartItemService(ICartItemRepository cartItemRepository , ICartRepository cartRepository ,IItemRepository itemRepository)
        {
            _cartItemRepository = cartItemRepository;
            _cartRepository = cartRepository;
            _itemRepository = itemRepository;
        }

        public async Task<CartItems> AddItemToCart(AddItemToCartRequest request)
        {
            try{
                var cart = await _cartRepository.GetByIdAsync(request.CartId);
                
                if (cart == null)
                {
                    throw new Exception("Cart not found");
                }
                // get price for item to map and then add to cart

                var item = await _itemRepository.GetByIdAsync(request.ItemId);
                if (item == null)
                {
                    throw new Exception("Item not found");
                }
                var cartItem = new CartItems
                {
                    CartId = request.CartId,
                    ItemId = request.ItemId,
                    Quantity = request.Quantity,
                };  
                var  totalPrice = item.Price * request.Quantity;
                
                // update cart total price
                cart.TotalPrice += totalPrice;
                await _cartRepository.UpdateAsync(cart);

                // add items to cartItem
                var result = await _cartItemRepository.AddAsync(cartItem);

                return result;


            }catch (Exception ex)
            {
                throw new Exception("Error while adding item to cart", ex);
            }
        }

        public async Task<CartItems> UpdateCartItem(int cartItemId, int quantity)
        {
            try
            {
                var cartItem = await _cartItemRepository.GetByIdAsync(cartItemId);
                if (cartItem == null)
                {
                    throw new Exception("Cart item not found");
                }

                cartItem.Quantity = quantity;
                await _cartItemRepository.UpdateAsync(cartItem);
                return cartItem;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating cart item", ex);
            }
        }

        public async Task<List<CartItems>> GetCartItemsByCartId(int cartId)
        {
            try
            {
                var cartItems = await _cartItemRepository.GetAllAsync();
                var cartItemList = cartItems.Where(x => x.CartId == cartId).ToList();
                return cartItemList;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting cart items by cart id", ex);
            }
        }
    }
}
