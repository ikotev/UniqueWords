using System;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await _transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task RollbackAsync(CancellationToken cancellationToken)
        {
            await _transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
        }

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