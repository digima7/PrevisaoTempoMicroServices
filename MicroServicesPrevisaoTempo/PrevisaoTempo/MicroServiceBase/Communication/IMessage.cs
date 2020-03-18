namespace MicroServiceBase.Communication
{
    public interface IMessage
    {
        void Publish<T>(T obj);
        void Register<T>(IMessageConsumer<T> consumer);
    }
}