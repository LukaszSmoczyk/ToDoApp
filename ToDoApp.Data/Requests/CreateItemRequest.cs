using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Data.Requests
{
    public class CreateItemRequest
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
