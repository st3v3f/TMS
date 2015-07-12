using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Tms.Web.Models;

namespace Tms.Web.DAL.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Tms.Web.DAL.DataContexts.IdentityDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DAL\DataContexts\IdentityMigrations";
        }

        protected override void Seed(Tms.Web.DAL.DataContexts.IdentityDb context)
        {

            if (!(context.Users.Any(u => u.UserName == "admin@tms.com")))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser { UserName = "admin@tms.com", PhoneNumber = "01212342345" };
                userManager.Create(userToInsert, "Password@456");
            }

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
