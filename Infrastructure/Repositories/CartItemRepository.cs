using Application.ContractRepo;
using Domain;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CartItemRepository : BaseRepository<CartItems> ,ICartItemRepository
    {

        public CartItemRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public async Task<List<CartItems>> GetCartItemsByCartId(int cartId)
        {
            var cartItems = await _dataContext.CartItems
                .Where(x => x.CartId == cartId)
                .Include(x => x.Item)
                .ToListAsync();

            return cartItems;
        }

        
    }
}
