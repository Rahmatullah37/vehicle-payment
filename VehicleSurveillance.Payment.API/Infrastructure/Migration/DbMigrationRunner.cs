using DbUp;
using DbUp.Helpers;
using System;
using System.Linq;
using System.Reflection;

namespace VehicleSurveillance.Payment.API.Infrastructure.Migration

{
    public static class DbMigrationRunner
    {
        public static void Run(string DatabaseConnectionString)  //run named method having db con
        {
            EnsureDatabase.For.PostgresqlDatabase(DatabaseConnectionString);

            var upgrader = DeployChanges.To
                .PostgresqlDatabase(DatabaseConnectionString)
                 .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                 .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();                  //Executes any new SQL scripts it finds in the embedded resources that haven’t already been run.

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
                throw new Exception("Database migration failed.");
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Database migration successful.");
            Console.ResetColor();
        }
    }
}

