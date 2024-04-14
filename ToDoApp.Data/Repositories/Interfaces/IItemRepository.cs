using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Data.Models;

namespace ToDoApp.Data.Repositories.Interfaces
{
    public interface IItemRepository : IBaseRepository<Item, int>
    {
    }
}
