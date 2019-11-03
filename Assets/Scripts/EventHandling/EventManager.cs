using System;
using System.Collections.Generic;
using Assets.Scripts.EventHandling;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static readonly Dictionary<Type, IList<IEventHandler>> Handlers;

    static EventManager()
    {
        Handlers = new Dictionary<Type, IList<IEventHandler>>();
    }

    public static void RegisterHandler<T>(IEventHandler<T> handler) where T: IEvent
    {
        var type = typeof(T);
        if (!Handlers.ContainsKey(type))
        {
            Handlers[type] = new List<IEventHandler>();
        }
        Handlers[type].Add(handler);
    }

    public static void DisposeAllHandlers()
    {
        Handlers.Clear();
    }

    public static void PublishEvent(IEvent @event)
    {
        var eventType = @event.GetType();
        if (Handlers.ContainsKey(eventType))
        {
            foreach (var handler in Handlers[eventType])
            {
                handler.Handle(@event);
            }
        }
    }
}
