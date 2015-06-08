using Tms.Common.Fakes;
using Tms.Core.Services;

namespace Tms.Web.DAL
{
    public class FakeDataInserter
    {       
        public static void InsertFakeTodosWithAttachedTasks(ITodosDb dbContext, int numTodos = 30, int tasksPerTodo = 100)
        {
            var fakeBuilder = new FakeBuilder();

            var jobs = fakeBuilder.GetFakeTodosWithTasks(numTodos, tasksPerTodo);

            foreach (var job in jobs)
            {
                dbContext.Todos.Add(job); //This adds the whole object graph (i.e. tasks as well)
            }
            dbContext.SaveChanges(); 
        }
    }
}
