using Microsoft.EntityFrameworkCore;
using TodoApp.Data.Models;

namespace TodoApp.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
        }

        public DbSet<ToDo> ToDos { get; set; }
    }
}
