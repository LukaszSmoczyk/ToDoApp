using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using ToDoApp.Data.DTO;
using ToDoApp.Data.Models;
using ToDoApp.Data.Repositories.Interfaces;
using ToDoApp.Data.Requests;
using ToDoApp.Data.Services.Interfaces;

namespace ToDoApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        /// <summary>
        /// Get list of items
        /// </summary>
        /// <returns>Retrieves a list of items.</returns>
        [HttpGet("list")]
        [ProducesResponseType(typeof(IEnumerable<ItemDTO>), 200)]
        public async Task<IActionResult> GetList()
        {
            var list = await _itemService.GetItemsList();
            return Ok(list);
        }

        /// <summary>
        /// Add new item
        /// </summary>
        /// <returns>Returns id of added item</returns>
        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> AddItem([FromBody]CreateItemRequest item)
        {
            var result = await _itemService.AddItem(item);
            return Ok(result);
        }

        /// <summary>
        /// Update existing item
        /// </summary>
        /// <returns>Returns updated item</returns>
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> UpdateItem([FromBody] ItemDTO requestItem)
        {
            var item = await _itemService.UpdateItem(requestItem);

            if (item == null)
            {
                return NotFound($"Item for id: {requestItem.Id} wasn't found");
            }
            
            return Ok();
        }

        /// <summary>
        /// Get item by Id
        /// </summary>
        /// <returns>Returns item</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ItemDTO), 200)]
        public async Task<IActionResult> GetItem(int id)
        {
            var item = await _itemService.GetItemById(id);

            if (item == null)
            {
                return NotFound($"Item for id: {id} wasn't found");
            }

            return Ok(item);
        }

        /// <summary>
        /// Delete item
        /// </summary>
        /// <returns></returns>
        [HttpDelete(Name = "{id}")]
        [ProducesResponseType(typeof(IEnumerable<Item>), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var result = await _itemService.DeleteItem(id);

            if (result == false)
            {
                return NotFound($"Item for id: {id} wasn't found");
            }

            return Ok();
        }

    }
}
