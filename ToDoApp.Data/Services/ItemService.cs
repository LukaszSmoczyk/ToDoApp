using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Data.DTO;
using ToDoApp.Data.Models;
using ToDoApp.Data.Repositories.Interfaces;
using ToDoApp.Data.Requests;
using ToDoApp.Data.Services.Interfaces;

namespace ToDoApp.Data.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<int> AddItem(CreateItemRequest request)
        {
            var newItem = new Item()
            {
                Quantity = request.Quantity,
                Name = request.Name,
                CreatedAt = DateTime.UtcNow
            };

            var itemDb = await _itemRepository.Add(newItem);
            return itemDb.Id;
        }

        public async Task<bool> DeleteItem(int id)
        {
            var item = await _itemRepository.Get(id);
            if (item == null)
            {
                return false;
            }
            else
            {
                var result = await _itemRepository.Delete(id);
                return true;
            }
        }

        public async Task<ItemDTO> GetItemById(int id)
        {
            var item = await _itemRepository.Get(id);
            
            if (item == null)
            {
                return null;
            }
            else
            {
                var itemDto = new ItemDTO()
                {
                    Quantity = item.Quantity,
                    Name = item.Name,
                    Id = item.Id
                };
                return itemDto;
            }
        }

        public async Task<List<ItemDTO>> GetItemsList()
        {
            var list = await _itemRepository.GetAll();
            if (list != null &&  list.Count > 0)
            {
                var items = new List<ItemDTO>();
                foreach(var item in list)
                {
                    var itemDto = new ItemDTO()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Quantity = item.Quantity
                    };
                    items.Add(itemDto);
                }
                return items;

            }
            else
            {
                return null;
            }
        }

        public async Task<Item> UpdateItem(ItemDTO item)
        {
            var itemDb = await _itemRepository.Get(item.Id);

            if (itemDb == null)
            {
                return null;
            }

            itemDb.UpdatedAt = DateTime.Now;
            itemDb.Quantity = item.Quantity;
            itemDb.Name = item.Name;
            var updatedItem = await _itemRepository.Update(itemDb);
            return updatedItem;
        }
    }
}
