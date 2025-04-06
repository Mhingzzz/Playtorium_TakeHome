using Application.ContractRepo;
using Domain;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CartRepository : BaseRepository<Cart> ,ICartRepository
    {

        public CartRepository(DataContext dataContext) : base(dataContext)
        {
        }
        public async Task<List<Cart>> GetCartByCustomerId(int customerId)
        {
            var result = await _dataContext.Cart.Where(x => x.CustomerId == customerId).AsNoTracking().ToListAsync();
            return result;
        }

        public async Task<Cart> CreateCart(Cart cart)
        {
            await AddAsync(cart);
            return cart;
        }
    }
}
