﻿using ACME.DataLayer.Entities;
using ACME.DataLayer.Repository.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace ACME.Frontend.ConsoleClient;

internal class Program
{
    const string databaseName = "Shop2";
    const string connectionString = @$"Server=.\SQLEXPRESS;Database={databaseName};Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true;";
    
    // TODO 8: Run the application and check the generated database (Shop2)
    static void Main()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ShopDatabaseContext>();
        optionsBuilder.UseSqlServer(connectionString);
        var ctx = new ShopDatabaseContext(optionsBuilder.Options);

        ctx.Database.EnsureCreated();
        
        Console.WriteLine("Done!");      
    }
}