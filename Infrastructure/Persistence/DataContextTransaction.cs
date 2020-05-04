using System;
using Microsoft.EntityFrameworkCore.Storage;
using UniqueWords.Application.Persistence;

namespace UniqueWords.Infrastructure.Persistence
{
    public class DataContextTransaction : IDataContextTransaction
    {
        private readonly IDbContextTransaction _transaction;

        public DataContextTransaction(IDbContextTransaction transaction)
        {
            _transaction = transaction;                        
        }

        public Guid TransactionId => _transaction.TransactionId;

        public void Commit() => _transaction.Commit();        

        public void Rollback() => _transaction.Rollback();

        // public Task CommitAsync(CancellationToken cancellationToken) => _transaction.CommitAsync(cancellationToken).ConfigureAwait(false);

        // public Task RollbackAsync(CancellationToken cancellationToken) => _transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);

        #region IDispose

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _transaction.Dispose();

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion IDispose
    }
}