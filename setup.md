
##Steps to create this from scratch.




* Create top level folder.
* Create /src folder.
* Add read.md to top level.
* Add .gitignore to /src folder (get latest Visual studio .gitignore file from github).
* Create git repo at highest level (git init).

(Initial setup based on EF6 section on ASP.NET MVC5 Fundamentals video by Scott Allen.)

In VS 2013:
* Create empty VS 2013 project (TMS) in /src folder:
   * New Project - Other Project Types - Blank Solution
   * Uncheck "Create new Git repository" ** Do we want a .gitatttributes to normalise line endings as provided by VS? **


* Add projects:
  * Tms.Web - ASP.NET web application project - MVC (Include WebAPI and  Unit test project - Tms.Tests)
  * Tms.Core - For entity classes / models. - simple Class library

[Note that could have had Tms.DAL but we wouldnt be able to keep DbContext for ASP.NET ApplicationUser in same folder as it exists in .Web project but we can't have circular project references, and we dont want to create out own user class.]

* Add references:
  Core -> Web

Build and run to check all ok.
Build and run tests - 3 tests should run and pass.

Remove class1.cs files from class library projects.

Add entity classes to a new /Models folder in Tms.Core.
  - Todo.cs (list of tasks)
  - Task.cs
  - entity.cs (base for entities)

Add System.ComponentModel.DataAnnotatoins as a reference to allow
data annotations on entity properties.


Create \DAL\DataContexts folder in the .Web project.
Create a DbContext for our application entities (TodosDb).
* Add a DbSet for each of the Entities.

Move ApplicationDbContext from IdentityModels.cs in web project into the new \DataContexts folder.
Call it IdentityDb.cs
Fix reference to it in Startup.ConfigureAuth().
and in IdentityConfig.cs

Make sure each dbContext uses the same 'DefaultConnection' string.


Setup Migrations using PM console.

enable-migrations -ContextTypeName IdentityDb -MigrationsDirectory DAL\DataContexts\IdentityMigrations
enable-migrtaions -ContextTypeName TodosDb -MigrationsDirectory DAL\DataContexts\TodosMigrations

Create initial migrations ( first set each db context to use its own schema):

add-migration -ConfigurationTypeName  Tms.Web.DAL.DataContexts.IdentityMigrations.Configuration "InitialCreate"

add-migration -ConfigurationTypeName  Tms.Web.DAL.DataContexts.TodosMigrations.Configuration "InitialCreate"

Note these setup scripts can be re-scaffolded by running 'Add-Migration InitialCreate' again.

run
    update-database to apply the latest migration.

    update-database -ConfigurationTypeName  Tms.Web.DAL.DataContexts.IdentityMigrations.Configuration -Verbose

    update-database -ConfigurationTypeName  Tms.Web.DAL.DataContexts.TodosMigrations.Configuration -Verbose

(To start with a clean empty DB, just change connection string in Web.config ot have new filename TMS_database.mdf initial catalog to 'TMS')


## Scaffolding Controllers and Views based on app models.

Controllers Folder - Add - Scaffolded item - MVC Controller with views using EF - Select entity (Todo), select dbConteext TodoDb.

Add link to NavBar for new Todos - Index.
(Optionally set [Authorise] attributes on controller methods.)

Build and run.


## Setup Logging of SQL
Setup logging on DBContext to see SQL in output . In dbContext constructor:
 - Database.Log = sql => Debug.Write(sql);

## Install Glimpse via Nuget  (Glimpse.MVC and Glimpse.EF6)
(Glimpse wont work with async methods)

Enable at /Glimpse.axd URL.

## Create Fake seed data

Create new .Common project (class library)
To be used to created fake data (will be used in testing and database seeding)

Install these Nuget packages into the new .Common project:
Faker.NET
NBuilder

Create FakeBuilder.cs
Create TodosDbInitialiser in DAL

## Fix details retrieval to include child entities
Change Find() to use Where
Fix View to display child data.

## IoC DI using AutoFac

## Create Service Layer

## Create FakeDbContext

## Create Tests to use FakeDbcontext.


## Add in paged List functionality

PagedList.MVC

## Add Fontawesome
