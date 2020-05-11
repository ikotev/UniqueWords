using System;
using System.Threading;
using System.Threading.Tasks;

namespace UniqueWords.Application.Persistence
{
    public interface IDataContextTransaction : IDisposable
    {
        Guid TransactionId { get; }

        void Commit();        

        void Rollback();

         Task CommitAsync(CancellationToken cancellationToken);

         Task RollbackAsync(CancellationToken cancellationToken);
    }
}