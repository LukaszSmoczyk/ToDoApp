using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Data.Context;
using ToDoApp.Data.Models;
using ToDoApp.Data.Repositories.Interfaces;

namespace ToDoApp.Data.Repositories
{
    public class FeedRepository : BaseRepository<Feed, int>, IFeedRepository
    {
        public FeedRepository(DataContext context) : base(context)
        {

        }
    }
}
