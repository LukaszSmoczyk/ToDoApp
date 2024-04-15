using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Data.DTO;
using ToDoApp.Data.Exceptions;
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

        public async Task<List<CloudEvent>?> GetItemsFeedList(Guid? lastEventId, int? timeout)
        {
            var cloudEventsList = new List<CloudEvent>();
            var itemsList = new List<Item>();
            var guid = Guid.NewGuid();

            var feed = await _feedRepository
                .Find()
                .Where(n => n.Name.Equals("feed-item-list"))
                .FirstOrDefaultAsync();

            //First iteration
            if (feed == null)
            {
                feed = new Feed()
                {
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Name = "feed-item-list",
                    EventId = guid,
                };
                await _feedRepository.Add(feed);
            }

            if (!lastEventId.HasValue)
            {
                itemsList = await _itemRepository
                    .Find()
                    .OrderBy(i => i.UpdatedAt)
                    .ToListAsync();
            }
            else
            {
                if (!lastEventId.Equals(feed.EventId))
                {
                    throw new EventIdNotFoundException();
                }

                itemsList = await _itemRepository
                      .Find()
                      .Where(i => i.UpdatedAt >= feed.UpdatedAt)
                      .OrderBy(i => i.UpdatedAt)
                      .ToListAsync();
            }

            if (!(itemsList.Count > 0))
            {
                if (timeout.HasValue && timeout > 0)
                {
                    if (timeout > 10000)
                        timeout = 10000; //Safeguard just in case
                    await Task.Delay(timeout.Value);
                    itemsList = await _itemRepository
                        .Find()
                        .OrderBy(i => i.UpdatedAt)
                        .ToListAsync();
                }
                // If still no items, return null
                if (!(itemsList.Count > 0))
                {
                    return null;
                }
            }

            foreach(var item in itemsList)
            {
                bool isLastItem = item.Equals(itemsList.Last());
                var id = Guid.NewGuid();
                if (isLastItem)
                {
                    id = guid;
                }
                cloudEventsList.Add(new CloudEvent()
                {
                    SpecVersion = "1.0",
                    Type = "org.http-feeds.example.items",
                    Source = "https://example.http-feeds.org/items",
                    Time = DateTime.UtcNow,
                    Data = item,
                    Id = id
                });
            }

            feed.UpdatedAt = DateTime.UtcNow;
            feed.EventId = guid;
            await _feedRepository.Update(feed);
            return cloudEventsList;

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

            itemDb.UpdatedAt = DateTime.UtcNow;
            itemDb.Quantity = item.Quantity;
            itemDb.Name = item.Name;
            var updatedItem = await _itemRepository.Update(itemDb);
            return updatedItem;
        }
    }
}
