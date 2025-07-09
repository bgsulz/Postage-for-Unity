using UnityEngine;

namespace Extra.Postage.Behaviours
{
    public abstract class MessageBehaviour : MonoBehaviour
    {
        private MessageBus _bus;

        protected virtual void OnEnable()
        {
            _bus = new();
            Subscribe(_bus);
        }

        protected virtual void OnDisable()
        {
            Unsubscribe();
        }

        protected abstract void Subscribe(MessageBus bus);
        protected virtual void Unsubscribe() => _bus.Dispose();
    }
}