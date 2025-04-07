using Application.ContractRepo;
using Domain;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ItemRepository : BaseRepository<Items>, IItemRepository
    {

        public ItemRepository(DataContext dataContext) : base(dataContext)
        {

        }
        
    }
}
