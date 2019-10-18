using System;

namespace Assets.Scripts.EventHandling
{
    public interface IEventHandler
    {
        void Handle(IEvent @event);
    }

    public interface IEventHandler<T> : IEventHandler where T: IEvent
    {
        Action<T> EventAction { get; set; }
    }

    public class EventHandler<T> : IEventHandler<T> where T:class, IEvent
    {
        public EventHandler()
        {
            EventManager.RegisterHandler(this);
        }

        public Action<T> EventAction { get; set; }
        public void Handle(IEvent @event)
        {
            if (!(@event is T message))
            {
                throw new ArgumentException($"Event must implement {nameof(IEvent)} and must be of type {typeof(T).Name}");
            }
            EventAction?.Invoke(message);
        }
    }
}
