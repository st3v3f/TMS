using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Tms.Web.DAL.DataContexts;
using Tms.Core.Services;

namespace Tms.Tests.ServiceTests
{
    [TestClass]
    public class TodoServiceTests
    {
        //Name like this : MethodName_Scenario_Expectation
        [TestMethod]
        public void GetAll_WhenTodosExist_ReturnsAllStoredTodos()
        {
            // Arrange
            var todosDb = new FakeTodosDb(); // In-memory DbContext.
            todosDb.DeleteAll(); // Remove auto-seeded data.
            int numTodos = 6;
            int tasksPerTodo = 5;
            todosDb.Seed(numTodos: numTodos, tasksPerTodo: tasksPerTodo);
            TodoService todoService = new TodoService(todosDb);

            // Act
            var todos = todoService.GetAll();

            // Assert
            Assert.IsNotNull(todos);
            Assert.AreEqual(numTodos, todos.Count());

            // Debug
            var json = JsonConvert.SerializeObject(todos, Formatting.Indented);
            Debug.WriteLine(json);
            Debug.Flush();
        }
    }
}
