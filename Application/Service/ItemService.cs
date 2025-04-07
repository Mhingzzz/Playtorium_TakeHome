using Application.ContractRepo;
using Application.Interfaces;
using Domain;

namespace Application.Service
{
    public class ItemService : IItemService
    {

        private readonly IItemRepository _itemRepository;
        public ItemService(IItemRepository ItemRepository)
        {
            _itemRepository = ItemRepository;
        }

        public async Task<Items> GetItemById(int id)
        {
            try
            {
                var Item = await _itemRepository.GetByIdAsync(id);
                return Item;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the Item: {ex.Message}");
            }
        }   
        
     
        public async Task<Items> UpdateItem(Items item)
        {
            try
            {
                var updatedItem = await _itemRepository.UpdateAsync(item);
                return updatedItem;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the Item: {ex.Message}");
            }
        }
        public async Task<bool> DeleteItem(int id)
        {
            try
            {
                var result = await _itemRepository.DeleteById(id);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the Item: {ex.Message}");
            }
        }
        public async Task<List<Items>> GetAllItems()
        {
            try
            {
                var items = await _itemRepository.GetAllAsync();
                return items;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving all Items: {ex.Message}");
            }
        }
        public async Task<Items> CreateItem(Items item)
        {
            try
            {
                var newItem = await _itemRepository.AddAsync(item);
                return newItem;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while creating the Item: {ex.Message}");
            }
        }
    }
}
