# Migrations

## Migrating for Development

Create a new project of type ASP.Net Core Web Application, Empty.

It will automatically be in ASP.Net Core 1.1, but we want 2.0, so we
right-click on the project, and choose Properties on the context menu (the very bottom
item). Under Target Framework, choose .NET Core 2.0. Save.

Right-click on Dependencies, choose Manage NuGet Packages...
Ensure that the Updates sub-tab is underlined in blue. Select Microsoft.AspNetCore
to update to 2.0.0. Click the Update button.

Still in the NuGet Package Manager, click the Browse sub-tab. Install the
following packages:

* Microsoft.AspNetCore.Identity.EntityFrameworkCore
* Microsoft.EntityFrameworkCore.Design
* Microsoft.EntityFrameworkCore.SqlServer
* Microsoft.EntityFrameworkCore.Tools
* Microsoft.EntityFrameworkCore.Tools.DotNet

Create the appsettings.json file at the project's root. This will be from where
the application will read the connection string. Make sure that this file is not
included in the repository, so we may have to add appsettings.json to our solution's
.gitignore if not already.

We will need to create a DbContextFactory.cs file. This is new in ASP.net Core 2.0.
We will name it typically <SolutionName>DbContextFactory. This file is sought by
ASP.net Core when we run migrations. It answers the question "which database
should we use for migration?". See the file we have created for more details.

Ensure that the Deploy project is setup as startup project. At the Package Manager
Console, ensure that the default project chosen is the Deploy project.

Type `Add-Migration <MigrationName>` where `<MigrationName>` is usually
"InitialMigration" for your first one. A Migrations folder will be created,
and there will be a migration file that ends in underscore `<MigrationName>`.
Open this file and observe. Depending on the project's data, we may need to tweak
some of the foreign key delete constraints since the scaffolder "guessed".
After tweaks are done,

Run `Update-Database`. If you configured everything correctly, which includes,
but is not limited to valid connection string, no cycles in your database schema,
no syntax errors in your code, the database tables and their columns will be created
automatically.

We have not actually seeded yet.

### Seeding
This is optional, but it is typically helpful to do. When it's time to deploy to
production, we would not want to manually fill reference tables.

We typically create a Seeder class in our `<SolutionName>.EntityFramework` project.
The seeder should take in at least our data context. If we want to fill some initial
users and roles, we will need `UserManager<TUser>` and `RoleManager<TRole>` from
the Identity framework. Observe what we have in our `Seeder.cs` file. Note that
a lot of our "add" attempts are really "add or update".

We then need to set up our `Startup.cs` file. Consult the file for more details.
It is in this file where we set up dependency injection and call up the Seeder
class we wrote. When the application is run, our seeder will populate our database
according to what we wrote in the seeder.


## Migrating for Production
TBD