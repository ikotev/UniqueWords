using System;
using System.Data;
using System.Threading.Tasks;

namespace UniqueWords.Application.Persistence
{
    public interface IDataContext : IDisposable
    {
        void SaveChanges();

        Task SaveChangesAsync();

        IDataContextTransaction BeginTransaction();

        IDataContextTransaction BeginTransaction(IsolationLevel isolationLevel);

        Task<IDataContextTransaction> BeginTransactionAsync();

        Task<IDataContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel);
    }
}