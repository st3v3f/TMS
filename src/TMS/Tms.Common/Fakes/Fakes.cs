using System.Collections.Generic;
using FizzWare.NBuilder;
using Tms.Core.Models;

namespace Tms.Common.Fakes
{
    public class FakeBuilder
    {
        public IList<Todo> GetFakeTodosWithTasks(int numJobs, int tasksPerJob)
        {
            var fakeJobsWithTasks = Builder<Todo>.CreateListOfSize(numJobs)
                .All()
                .With(j => j.CreatedBy = (Faker.Name.First() + " " + Faker.Name.Last()))
                .With(j => j.Tasks = GetFakeTasks(tasksPerJob))
                .Build();

            return fakeJobsWithTasks;
        }

        private IList<Task> GetFakeTasks(int numTasks)
        {
            // Create fake tasks.
            var fakeTasks = Builder<Task>.CreateListOfSize(numTasks)
                .All()
                    .With(t => t.CreatedBy = (Faker.Name.First() + " " + Faker.Name.Last()))
                    .With(t => t.Description = Faker.Lorem.Sentence(4))
                .Build();

            return fakeTasks;
        }
    }
}