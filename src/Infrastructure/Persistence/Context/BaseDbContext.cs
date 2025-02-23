using de.WebApi.Application.Common.Interfaces;
using de.WebApi.Domain.Common.Contracts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.WebApi.Infrastructure.Persistence.Context;
public abstract class BaseDbContext : DbContext
{
    private readonly ISerializerService _serializer;
    private readonly DatabaseSettings _dbSettings;

    protected BaseDbContext(DbContextOptions options, ISerializerService serializer, IOptions<DatabaseSettings> dbSettings)
        : base(options)
    {
        _serializer = serializer;
        _dbSettings = dbSettings.Value;
    }

    // Used by Dapper
    public IDbConnection Connection => Database.GetDbConnection();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // QueryFilters need to be applied before base.OnModelCreating
        modelBuilder.AppendGlobalQueryFilter<ISoftDelete>(s => s.DeletedOn == null);

        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // TODO: We want this only for development probably... maybe better make it configurable in logger.json config?
        optionsBuilder.EnableSensitiveDataLogging();

        // If you want to see the sql queries in the console log uncomment:
        // optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        int result = await base.SaveChangesAsync(cancellationToken);

        // TODO: Maybe we need to add audit trails or event domain notifications??

        return result;
    }
}
