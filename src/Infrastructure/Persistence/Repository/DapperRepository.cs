using de.WebApi.Application.Common.Exceptions;
using de.WebApi.Application.Common.Persistence;
using de.WebApi.Domain.Common.Contracts;
using de.WebApi.Infrastructure.Persistence.Context;
using Dapper;
using System.Data;

namespace de.WebApi.Infrastructure.Persistence.Repository;

public class DapperRepository : IDapperRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DapperRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    where T : class, IEntity =>
        (await _dbContext.Connection.QueryAsync<T>(sql, param, transaction))
            .AsList();

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    where T : class, IEntity
    {
        var entity = await _dbContext.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
        return entity ?? throw new NotFoundException(string.Empty);
    }

    public Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    where T : class, IEntity
    {
        return _dbContext.Connection.QuerySingleAsync<T>(sql, param, transaction);
    }
}