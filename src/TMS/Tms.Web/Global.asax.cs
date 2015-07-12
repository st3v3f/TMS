using System.Diagnostics;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Newtonsoft.Json;
using NLog;
using Tms.Core.Services;
using Tms.Web.DAL.DataContexts;

namespace Tms.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Debug("Starting app!");

            Debug.WriteLine("Starting app <diagnostics>");

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Setup IoC Dependency Injection (Automapper)
            RegisterIoc();

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }

        private void RegisterIoc()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterApiControllers(typeof(MvcApplication).Assembly);
            AutofacBootstrap.Init(builder);
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }

    public class AutofacBootstrap
    {
        internal static void Init(ContainerBuilder builder)
        {
            // Register individual components.
            //builder.RegisterType<FakeTodosDb>().As<ITodosDb>();
            builder.RegisterType<TodosDb>().As<ITodosDb>();        // DbContext
            builder.RegisterType<TodoService>().As<ITodoService>();

            //EXAMPLES.
            //builder.RegisterInstance(new JobRepository).As<IJobRepository>();
            //builder.RegisterType<TaskController>();
            //builder.Register(c => new LogManager(DateTime.Now)).As<ILogger>();

            // Scan an assembly for components.
            /*var myAssembly = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(myAssembly)
              .Where(t => t.Name.EndsWith("Repository"))
              .AsImplementedInterfaces();
             */
        }
    }
}
