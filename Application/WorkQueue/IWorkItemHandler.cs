using System.Threading.Tasks;

namespace UniqueWords.Application.WorkQueue
{
    public interface IWorkItemHandler<T>
    {
        Task HandleAsync(T message);
    }
}