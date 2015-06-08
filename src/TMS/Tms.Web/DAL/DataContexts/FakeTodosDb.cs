using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using Microsoft.Ajax.Utilities;
using Tms.Core.Models;
using Tms.Core.Services;


namespace Tms.Web.DAL.DataContexts
{
    public class FakeTodosDb : ITodosDb
    {
        public FakeTodosDb()
        {
            this.Todos = new TestDbSet<Todo>();
            this.Tasks = new TestDbSet<Task>();

            // Add fake data on intialisation.
            Seed();
        }

        public DbSet<Todo> Todos { get;  set;} // Todos collection
        public DbSet<Task> Tasks { get; set; } // Tasks collection

        public int SaveChangesCount { get; private set; }
        public int SaveChanges()
        {
            this.SaveChangesCount++;
            return 1;
        }

        public void Update(Todo todo)
        {
            Debug.WriteLine("NumTodos <{0}>", Todos.Count());

            // Find it
            var existingTodo = Todos.Find(todo.Id);

            // Remove it
            if (null != existingTodo)
            {
                Debug.WriteLine("Removing existing version.");
                Todos.Remove(existingTodo);
            }
            
            // Add new one
            Debug.WriteLine("Adding new version.");
            Todos.Add(todo);

            Debug.WriteLine("NumTodos <{0}>", Todos.Count());
        }

        public void Seed(int numTodos = 3, int tasksPerTodo = 10)
        {
            Debug.WriteLine("Seeding In-memory DbContext  with Fake Todos!");
            FakeDataInserter.InsertFakeTodosWithAttachedTasks(this,
                numTodos: numTodos,
                tasksPerTodo: tasksPerTodo);         
        }

        public void DeleteAll() // Use in FakeDb only.
        {
            Todos = new TestDbSet<Todo>();
            Tasks = new TestDbSet<Task>();
        }

        public void Dispose()
        {
            //Dummy to match interface.
        }

    }
}
