namespace UniqueWords.Application.TaskProcessing
{
    public interface IBackgroundTaskQueuePublisher<T>
    {
        void Publish(T workItem);
    }
}