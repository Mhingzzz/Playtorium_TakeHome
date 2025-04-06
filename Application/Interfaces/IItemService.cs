using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface IItemService
    {
        Task<Items> GetItemById(int id);
        Task<List<Items>> GetAllItems();
        Task<Items> CreateItem(Items item);
        Task<Items> UpdateItem(Items item);
        Task<bool> DeleteItem(int id);
       
    }
}
