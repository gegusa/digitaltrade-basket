using DbUp;
using Microsoft.Extensions.Configuration;

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
    
var connectionString = configuration.GetConnectionString("DefaultConnection");

EnsureDatabase.For.PostgresqlDatabase(connectionString);

var result =
    DeployChanges.To
        .PostgresqlDatabase(connectionString)
        .WithScriptsFromFileSystem("Scripts/Initial")
        .LogToConsole()
        .Build()
        .PerformUpgrade();  

if (!result.Successful)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(result.Error);
}
else
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Success!");
}

Console.ResetColor();