﻿using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace ACME.DataLayer.Repository.SqlServer.Interceptors;

// TODO 1: Create a class QueryInterceptor that checks if a command takes
// longer than 50ms. 
// If so log a warning with the execution time and the query.
public class QueryInterceptor : DbCommandInterceptor
{
    private readonly ILogger<QueryInterceptor> _logger;

    public QueryInterceptor(ILogger<QueryInterceptor> logger)
    {
        _logger = logger;
    }

    public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
    {
        if (eventData.Duration.Milliseconds > 50)
        {
            _logger.LogWarning($"The following command took longer than 50ms. ({eventData.Duration.Milliseconds}ms)\r\n" +
                $"{command.CommandText}");
        }
        return base.ReaderExecuted(command, eventData, result);
    }
}
