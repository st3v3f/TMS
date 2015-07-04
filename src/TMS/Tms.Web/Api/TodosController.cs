using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using Tms.Core.Models;
using Tms.Web.DAL.DataContexts;

namespace Tms.Web.Api
{
    [RoutePrefix("api/reports")]
    public class TodosController : ApiController
    {
        private readonly TodosDb db = new TodosDb();

        
        [Route("AllTodosReport")]
        public IEnumerable<Todo> GetTodos()
        {
            var results = db.Todos.ToList();

            var json = JsonConvert.SerializeObject(results, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            System.Diagnostics.Debug.WriteLine(json);

            return results;
        }

        [Route("SearchTodosReport")]
        public IEnumerable<Todo> GetSearchTodos([FromUri] string searchString)
        {
            var results = db.Todos
                .Where(t => t.Title.Contains(searchString) || t.Description.Contains(searchString))
                .ToList();

            var json = JsonConvert.SerializeObject(results, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            System.Diagnostics.Debug.WriteLine(json);

            return results;
        }

        // GET: api/Todos/5
        [ResponseType(typeof(Todo))]
        public IHttpActionResult GetTodo(Guid id)
        {
            Todo todo = db.Todos.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        // PUT: api/Todos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTodo(Guid id, Todo todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != todo.Id)
            {
                return BadRequest();
            }

            db.Entry(todo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Todos
        [ResponseType(typeof(Todo))]
        public IHttpActionResult PostTodo(Todo todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Todos.Add(todo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = todo.Id }, todo);
        }

        // DELETE: api/Todos/5
        [ResponseType(typeof(Todo))]
        public IHttpActionResult DeleteTodo(Guid id)
        {
            Todo todo = db.Todos.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            db.Todos.Remove(todo);
            db.SaveChanges();

            return Ok(todo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TodoExists(Guid id)
        {
            return db.Todos.Count(e => e.Id == id) > 0;
        }
    }
}