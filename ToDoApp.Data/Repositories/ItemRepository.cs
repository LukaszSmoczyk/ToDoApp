﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Data.Context;
using ToDoApp.Data.Models;
using ToDoApp.Data.Repositories.Interfaces;

namespace ToDoApp.Data.Repositories
{
    public class ItemRepository : BaseRepository<Item, int>, IItemRepository
    {
        public ItemRepository(DataContext context) : base(context)
        {
            
        }
    }
}
