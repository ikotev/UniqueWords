namespace UniqueWords.Application.WorkQueue
{
    public interface IBackgroundWorkQueuePublisher<T>
    {
        void Publish(T workItem);
    }
}