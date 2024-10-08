﻿using ACME.DataLayer.Entities;
using ACME.DataLayer.Repository.SqlServer;
using ACME.DataLayer.Repository.SqlServer.Interceptors;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data;
using IsolationLevel = System.Data.IsolationLevel;

namespace ACME.Frontend.ConsoleClient;

internal class Program
{
    const string databaseName = "ProductCatalog";
    const string connectionString = @$"Server=.\SQLEXPRESS;Database={databaseName};Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true;";

    static void Main()
    {
        Host.CreateDefaultBuilder()
           .ConfigureLogging(logBld => {
               // TODO 6: Filter out the logging for "Microsoft.EntityFrameworkCore.Database"
               // Run the application.
               logBld.AddFilter("Microsoft.EntityFrameworkCore.Database", LogLevel.None);
           })
           .ConfigureServices(svcs =>
           {
               // TODO 5: Register the interceptor for the Dependency Injector.
               svcs.AddTransient<DbCommandInterceptor, QueryInterceptor>();
               svcs.AddHostedService<ConsoleClient>();
               svcs.AddDbContext<ShopDatabaseContext>(opts =>
               {
                   opts.UseSqlServer(connectionString);

               });
           })
           .Build()
           .Start();
    }
}