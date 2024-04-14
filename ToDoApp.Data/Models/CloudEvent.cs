using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Data.Models
{
    public class CloudEvent
    {
        public string SpecVersion { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public Item Data { get; set; }
    }
}
