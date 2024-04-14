using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Data.Models;

namespace ToDoApp.Data.DataContext
{
    public class ToDoDataContext : DbContext
    {
        public ToDoDataContext(DbContextOptions<ToDoDataContext> options) : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
