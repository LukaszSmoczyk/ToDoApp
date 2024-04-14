using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ToDoApp.Data.Models;
using ToDoApp.Data.Repositories.Interfaces;

namespace ToDoApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : Controller
    {
        private readonly IItemRepository _itemRepository;

        public ItemController(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }
        /// <summary>
        /// Lorem ipsum
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "list")]
        public async Task<IActionResult> GetList()
        {
            var list = await _itemRepository.GetAll();
            return Ok(list);
        }

        /// <summary>
        /// Lorem ipsum
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] string name)
        {
            var item = new Item()
            {
                Name = name
            };
            var result = await _itemRepository.Add(item);
            return Ok(result);
        }

        ///// <summary>
        ///// Lorem ipsum
        ///// </summary>
        ///// <returns></returns>
        //[HttpPut(Name = "list/{id}")]
        //public IActionResult UpdateItem(int id, [FromBody] Item item)
        //{
        //    var itemExist = list.Where(Item => Item.Id == id).FirstOrDefault();
        //    if (itemExist != null)
        //    {
        //        item.Name = itemExist.Name;
        //    }
        //    return Ok();
        //}

        ///// <summary>
        ///// Lorem ipsum
        ///// </summary>
        ///// <returns></returns>
        //[HttpDelete(Name = "list/{id}")]
        //public IActionResult DeleteItem()
        //{
        //    return Ok();
        //}

    }
}
