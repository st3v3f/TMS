using System;
using System.Data.Entity;
using System.Diagnostics;
using Tms.Core.Models;
using Tms.Core.Services;

namespace Tms.Web.DAL.DataContexts
{
    public class TodosDb : DbContext, ITodosDb
    {
        public TodosDb()
            : base("DefaultConnection")
        {
            Database.SetInitializer<TodosDb>(new TodosDbInitializer());

            Database.Log = sql => Debug.Write(sql);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Todo"); // Set these tables to user their own separate schema.
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Todo> Todos { get; set; }
        public DbSet<Task> Tasks { get; set; }

        public static TodosDb Create()
        {
            return new TodosDb();
        }

        public void Update(Todo todo)
        {
            this.Entry(todo).State = EntityState.Modified;
        }
    }

    //public class TodosDbInitializer : System.Data.Entity.DropCreateDatabaseAlways<TodosDb>
    public class TodosDbInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<TodosDb>
    {
        protected override void Seed(TodosDb dbContext)
        {
            FakeDataInserter.InsertFakeTodosWithAttachedTasks(dbContext, numTodos: 5, tasksPerTodo: 10);
        }
    }
}
