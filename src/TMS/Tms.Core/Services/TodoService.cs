using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PagedList;
using Tms.Core.Models;

namespace Tms.Core.Services
{

    public interface IService<T> : IDisposable where T : class
    {       
        IEnumerable<T> GetAll();    // Index (List)
        T GetSingle(Guid entityId); // Details      
        void Add(T entity);         // Create.
        void Update(T entity);      // Edit
        void Remove(Guid entityId); // Delete

        void SaveChanges();         // Commit changes to DB.

        //IQueryable<T> GetQueryable();
        
        //IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        //bool Exists(Guid entityId);
        
        IEnumerable<T> GetPagedList(int pageNumber, int pageSize, string searchString, string sortOrder);
    }

    public interface ITodoService : IService<Todo>
    {
        int CountTodos();
    }

    public class TodoService : ITodoService
    {
        private readonly ITodosDb _todosDb;

        public TodoService(ITodosDb todosDb)
        {
            if (todosDb == null)
            {
                throw new ArgumentNullException("todosDb");
            }
            this._todosDb = todosDb;
        }

        public IEnumerable<Todo> GetAll()
        {
            return _todosDb.Todos.ToList();
        }

        private IQueryable<Todo> GetQueryable()
        {
            return (IQueryable<Todo>)_todosDb.Todos; 
        }

        public IEnumerable<Todo> GetPagedList(int pageNumber, int pageSize, string searchString, string sortOrder)
        {
            // Base Query.
            IQueryable<Todo> todos = GetQueryable();

            // Handle search string selection.
            if (!string.IsNullOrEmpty(searchString))
            {
                todos = ((IQueryable<Todo>)todos).Where(j => j.Title.Contains(searchString) ||
                                            j.Description.Contains(searchString) ||
                                            j.CreatedBy.Contains(searchString));
            }

            IOrderedQueryable<Todo> ordTodos;

            switch (sortOrder)
            {
                case "title_desc":
                    ordTodos = todos.OrderByDescending(j => j.Title);
                    break;
                case "created_at_date_asc":
                    ordTodos = todos.OrderBy(j => j.CreatedAt);
                    break;
                case "created_at_date_desc":
                    ordTodos = todos.OrderByDescending(j => j.CreatedAt);
                    break;
                default:
                    ordTodos = todos.OrderBy(j => j.Title);
                    break;
            }

            var onePageOfJobs = ordTodos.ToPagedList(pageNumber, pageSize);

            return onePageOfJobs;
        }


        public Todo GetSingle(Guid entityId)
        {
            Debug.WriteLine("Retrieving Todo from Id");

            var todo = _todosDb.Todos.SingleOrDefault(t => t.Id == entityId); 

            Debug.WriteLine(todo.ToString());
            return todo;
        }

        public void Remove(Guid entityId)
        {
            Debug.WriteLine("Deleting Todo..");

            var todo = GetSingle(entityId);
            _todosDb.Todos.Remove(todo);
        }

        public int CountTodos()
        {
            return _todosDb.Todos.Count();
        }

        public void SaveChanges()
        {
            _todosDb.SaveChanges();
        }

        public void Add(Todo entity)
        {
            _todosDb.Todos.Add(entity);
        }

        public void Update(Todo entity)
        {
            _todosDb.Update(entity);
        }

        // --- Disposal ---
        // Implement disposal to clean up db context.
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    // Clean up repo (and db context).
                    _todosDb.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
