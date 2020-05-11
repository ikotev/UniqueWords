using UniqueWords.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Data;

namespace UniqueWords.Infrastructure.Persistence
{
    public abstract class BaseDataContext<TContext> : IDataContext where TContext : DbContext
    {
        protected TContext DbContext { get; }

        protected BaseDataContext(IDbContextFactory<TContext> contextFactory)
        {
            DbContext = contextFactory.Create();                        
        }

        public IDataContextTransaction BeginTransaction() =>
            new DataContextTransaction(DbContext.Database.BeginTransaction());

        public IDataContextTransaction BeginTransaction(IsolationLevel isolationLevel) =>
            new DataContextTransaction(DbContext.Database.BeginTransaction(isolationLevel));

        public async Task<IDataContextTransaction> BeginTransactionAsync()
        {
            var transaction = await DbContext.Database.BeginTransactionAsync().ConfigureAwait(false);
            return new DataContextTransaction(transaction);
        }

        public async Task<IDataContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel)
        {
            var transaction = await DbContext.Database.BeginTransactionAsync(isolationLevel).ConfigureAwait(false);
            return new DataContextTransaction(transaction);
        }

        public void SaveChanges() => DbContext.SaveChanges();

        public async Task SaveChangesAsync() 
        {
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        #region IDispose
        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                DbContext.Dispose();

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion IDispose        
    }
}