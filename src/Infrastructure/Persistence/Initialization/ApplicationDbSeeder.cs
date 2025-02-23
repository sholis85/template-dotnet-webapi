using de.WebApi.Infrastructure.Persistence.Context;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.WebApi.Infrastructure.Persistence.Initialization;

/// <summary>
/// Use this in case we need to prepopulate the DB with initial data.
/// </summary>
internal class ApplicationDbSeeder
{
    private readonly ILogger<ApplicationDbSeeder> _logger;

    public ApplicationDbSeeder(ILogger<ApplicationDbSeeder> logger)
    {
        _logger = logger;
    }

    public async Task SeedDatabaseAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        // Put your code here
    }

}