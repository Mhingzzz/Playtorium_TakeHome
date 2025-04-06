using Application.ContractRepo;
using Domain;
using Infrastructure.Database;

namespace Infrastructure.Repositories
{
    public class CartItemRepository : BaseRepository<CartItems> ,ICartItemRepository
    {

        public CartItemRepository(DataContext dataContext) : base(dataContext)
        {
        }

        
    }
}
