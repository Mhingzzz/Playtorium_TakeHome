using Application.ContractRepo;
using Infrastructure.Database;

namespace Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {

        private readonly DataContext _dataContext;
        public CartRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


    }
}
