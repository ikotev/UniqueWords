namespace UniqueWords.Application.WorkQueue
{
    public interface IWorkQueuePublisher<T>
    {
        void Publish(T workItem);
    }
}