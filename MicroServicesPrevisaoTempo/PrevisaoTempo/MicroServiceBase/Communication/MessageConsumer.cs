namespace MicroServiceBase.Communication
{
    public interface IMessageConsumer<T>
    {
        void Receive(T message);
    }
}