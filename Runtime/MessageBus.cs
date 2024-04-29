using System;

namespace Extra.Postman.Behaviours
{
    public class MessageBus : IDisposable
    {
        private Action _unsubscribe;

        public void Subscribe(Address address, Action action)
        {
            Message.Subscribe(address, action);
            _unsubscribe += () => Message.Unsubscribe(address, action);
        }

        public void Subscribe<R>(Address address, Action<R> action)
        {
            Message.Subscribe(address, action);
            _unsubscribe += () => Message.Unsubscribe(address, action);
        }

        public void Dispose()
        {
            _unsubscribe?.Invoke();
            _unsubscribe = null;
        }
    }
}