using System.Data.Entity;
using Tms.Core.Models;

namespace Tms.Dal.DataContexts
{
    class TodosDb : DbContext
    {
        public DbSet<Todo> Todos { get; set; }
        public DbSet<Task> Tasks { get; set; }
    }
}
