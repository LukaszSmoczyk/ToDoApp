using Microsoft.EntityFrameworkCore;
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
        private readonly IFeedRepository _feedRepository;
        public ItemService(IItemRepository itemRepository, IFeedRepository feedRepository)
        {
            _itemRepository = itemRepository;
            _feedRepository = feedRepository;
        }

        public async Task<int> AddItem(CreateItemRequest request)
        {
            var newItem = new Item()
            {
                Quantity = request.Quantity,
                Name = request.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
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

        public async Task<List<CloudEvent>> GetItemsFeedList(Guid? lastEventId, int? timeout)
        {
            var cloudEvents = new List<CloudEvent>();
            var feed = await _feedRepository
                .Find()
                .Where(n => n.Name.Equals("feed-list"))
                .FirstOrDefaultAsync();

            //First iteration
            if (feed == null && lastEventId == null)
            {
                var guid = Guid.NewGuid();
                feed = new Feed()
                {
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Name = "feed-list",
                    EventId = guid,
                };
                await _feedRepository.Add(feed);

                var items = await _itemRepository
                    .Find()
                    .OrderBy(i => i.UpdatedAt)
                    .ToListAsync();

                foreach(var item in items)
                {
                    bool isLastItem = item.Equals(items.Last());
                    var id = Guid.NewGuid();
                    if (isLastItem)
                    {
                        id = guid;
                    }
                    cloudEvents.Add(new CloudEvent()
                    {
                        SpecVersion = "1.0",
                        Type = "org.http-feeds.example.items",
                        Source = "https://example.http-feeds.org/items",
                        Time = DateTime.UtcNow,
                        Data = item,
                        Id = id
                    });
                }

                return cloudEvents;
            }
            else
            {
                if (feed.EventId.Equals(lastEventId))
                {
                    var guid = Guid.NewGuid();
                }
            }


            return null;

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
