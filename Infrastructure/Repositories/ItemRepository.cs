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
        public async Task<Items> GetItemById(int id)
        {
            var result = await GetByIdAsync(id);
            return result;
        }

        public async Task<List<Items>> GetAllItems()
        {
            var result = await GetAllAsync();
            return result;
        }

        public async Task<Items> CreateItem(Items item)
        {
            var result = await AddAsync(item);
            return result;
        }

        public async Task<Items> UpdateItem(Items item)
        {
            var result = await UpdateAsync(item);
            return result;
        }

        public async Task<bool> DeleteItem(int id)
        {
            var result = await DeleteById(id);
            return result;
        }

    }
}
