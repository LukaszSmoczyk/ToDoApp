using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ToDoApp.Data.Models;

namespace ToDoApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoController : Controller
    {
        private List<Item> list;

        public ToDoController()
        {
            list = new List<Item>()
            {
                new Item()
                {
                    Id = 1,
                    Name = "Test"
                },
                new Item()
                {
                    Id = 2,
                    Name = "Test2"
                },
                new Item()
                {
                    Id = 3,
                    Name = "Test3"
                }
            };
        }
        /// <summary>
        /// Lorem ipsum
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "list")]
        public IEnumerable<Item> GetList()
        {
            return list;
        }

        /// <summary>
        /// Lorem ipsum
        /// </summary>
        /// <returns></returns>
        [HttpPost(Name = "list")]
        public IActionResult AddItmem([FromBody] Item item)
        {
            list.Add(item);
            return Ok();
        }

        /// <summary>
        /// Lorem ipsum
        /// </summary>
        /// <returns></returns>
        [HttpPut(Name = "list/{id}")]
        public IActionResult UpdateItem(int id, [FromBody] Item item)
        {
            var itemExist = list.Where(Item => Item.Id == id).FirstOrDefault();
            if (itemExist != null)
            {
                item.Name = itemExist.Name;
            }
            return Ok();
        }

        /// <summary>
        /// Lorem ipsum
        /// </summary>
        /// <returns></returns>
        [HttpDelete(Name = "list/{id}")]
        public IActionResult DeleteItem()
        {
            return Ok();
        }

    }
}
