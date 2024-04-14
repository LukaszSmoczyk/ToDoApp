using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Data.DTO;
using ToDoApp.Data.Models;
using ToDoApp.Data.Repositories.Interfaces;
using ToDoApp.Data.Requests;

namespace ToDoApp.Data.Services.Interfaces
{
    public interface IItemService
    {
        public Task<int> AddItem(CreateItemRequest request);
        public Task<List<ItemDTO>> GetItemsList();
        public Task<List<CloudEvent>> GetItemsFeedList(Guid? lastEventId, int? timeout);
        public Task<ItemDTO> GetItemById(int id);
        public Task<Item> UpdateItem(ItemDTO item);
        public Task<bool> DeleteItem(int id);
    }
}
