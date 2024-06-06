using System.Data;
using Domic.Domain.Commons.Contracts.Interfaces;
using Domic.Persistence.Contexts.C;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.C;

public class CommandUnitOfWork(SQLContext sqlContext) : ICommandUnitOfWork
{
    private IDbContextTransaction _transaction;

    public void Transaction(IsolationLevel isolationLevel) 
        => _transaction = sqlContext.Database.BeginTransaction(isolationLevel); //Resource

    public async Task TransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        CancellationToken cancellationToken = new CancellationToken())
    {
        _transaction = await sqlContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken); //Resource
    }

    public void Commit()
    {
        sqlContext.SaveChanges();
        _transaction.Commit();
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await sqlContext.SaveChangesAsync(cancellationToken);
        await _transaction.CommitAsync(cancellationToken);
    }

    public void Rollback() => _transaction?.Rollback();

    public Task RollbackAsync(CancellationToken cancellationToken)
    {
        if (_transaction is not null)
            return _transaction.RollbackAsync(cancellationToken);

        return Task.CompletedTask;
    }

    public void Dispose() => _transaction?.Dispose();

    public ValueTask DisposeAsync()
    {
        if (_transaction is not null)
            return _transaction.DisposeAsync();

        return ValueTask.CompletedTask;
    }
}