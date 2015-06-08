using System;
using System.Data.Entity;
using Tms.Core.Models;

namespace Tms.Core.Services
{
    public interface ITodosDb : IDisposable
    {
        // NB Need reference to Entity Framework to resolve DbSet. 
        // (Ideally we wouldn't have dependency on EF in our Core Lib).
        DbSet<Todo> Todos { get; } 
        DbSet<Task> Tasks { get; }
        int SaveChanges();
        void Update(Todo todo);
    }
}
