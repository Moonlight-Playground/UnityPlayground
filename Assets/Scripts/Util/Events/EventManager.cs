using System;
using System.Collections.Generic;

public static class EventExtensions
{
    public static void AddEventListener<T>(this ICollection<EventManager.EventListenerHandle> self, EventManager.EventDelegate<T> e, string id = "")
        where T : EventManager.BaseEvent => self.Add(EventManager.AddListener(e, id));
    public static bool RemoveEventListener<T>(this ICollection<EventManager.EventListenerHandle> self, EventManager.EventDelegate<T> e)
        where T : EventManager.BaseEvent
    {
        var listener = EventManager.FindListener(e);
        return listener != null && self.Remove(listener);
    }
    public static void RemoveEventListeners(this ICollection<EventManager.EventListenerHandle> self)
    {
        foreach (var h in self)
        {
            EventManager.RemoveListener(h);
        }
        self.Clear();
    }
}

public static class EventManager
{
    public class BaseEvent { }

    public delegate void EventDelegate<T>(T e, object target) where T : BaseEvent;
    public delegate void EventDelegate(BaseEvent e, object target);

    private static Dictionary<Type, Dictionary<string, EventListenerHandle>> delegatesByType = new Dictionary<Type, Dictionary<string, EventListenerHandle>>();
    private static Dictionary<Delegate, EventListenerHandle> delegateLookup = new Dictionary<Delegate, EventListenerHandle>();

    public class EventListenerHandle
    {
        public EventDelegate del;
        public Type type;
        public Delegate lookup;
        public string ID;

        public EventListenerHandle(EventDelegate del, Type type, Delegate lookup, string id)
        {
            this.del = del;
            this.type = type;
            this.lookup = lookup;
            this.ID = id;
        }

        public void Remove()
        {
            EventManager.RemoveListener(this);
        }
    }

    public static EventListenerHandle AddListener<T>(EventDelegate<T> del, string id = "") where T : BaseEvent
    {
        // Early return if delegate already exists
        if (delegateLookup.TryGetValue(del, out var internalDelegate))
        {
            return internalDelegate;
        }

        // Create a new non-generic delegate which calls our generic one, this one we actually invoke
        internalDelegate = new EventListenerHandle((e, t) => del((T)e, t), typeof(T), del, id);
        delegateLookup[del] = internalDelegate;

        if (delegatesByType.TryGetValue(typeof(T), out var delegatesByID) && delegatesByID.TryGetValue(id, out var tempDel))
        {
            tempDel.del += internalDelegate.del;
            delegatesByType[typeof(T)][id] = tempDel;
        }
        else
        {
            if (delegatesByID == null)
            {
                delegatesByType[typeof(T)] = new Dictionary<string, EventListenerHandle>();
            }

            delegatesByType[typeof(T)][id] = new EventListenerHandle(internalDelegate.del, internalDelegate.type, del, id);
        }

        return internalDelegate;
    }

    public static void RemoveListener(EventListenerHandle handle)
    {
        var removed = RemoveListener(handle.del, handle.type, handle.ID);
        delegateLookup.Remove(handle.lookup);
    }

    public static bool RemoveListener(EventDelegate internalDelegate, Type type, string id)
    {
        if (delegatesByType.TryGetValue(type, out var delegatesByID) && delegatesByID.TryGetValue(id, out var tempDel))
        {
            tempDel.del -= internalDelegate;
            if (tempDel.del == null)
            {
                delegatesByType[type].Remove(id);

                if (delegatesByType[type].Count == 0)
                {
                    delegatesByType.Remove(type);
                }
            }
            else
            {
                delegatesByType[type][id].del = tempDel.del;
            }

            return true;
        }

        return false;
    }

    public static void RemoveListener<T>(EventDelegate<T> del, string id = "") where T : BaseEvent
    {
        if (delegateLookup.TryGetValue(del, out var internalDelegate))
        {
            var removed = RemoveListener(internalDelegate.del, internalDelegate.type, id);
            delegateLookup.Remove(del);
        }
    }

    public static EventListenerHandle FindListener<T>(EventDelegate<T> del) where T : BaseEvent
    {
        return delegateLookup.TryGetValue(del, out var internalDelegate) ? internalDelegate : null;
    }

    public static void Raise(BaseEvent e, string id = "", object target = null)
    {
        RaiseInner(e, id, target);
    }

    private static void RaiseInner(BaseEvent e, string id = "", object target = null)
    {
        if (delegatesByType.TryGetValue(e.GetType(), out var listenersByID))
        {
            // Invoke the delegate for the specific ID
            if (listenersByID.TryGetValue(id, out var listener))
            {
                listener.del.Invoke(e, target);
            }

            // Invoke the delegate for ID-less subscribers as well
            if (!string.IsNullOrEmpty(id) && listenersByID.TryGetValue(string.Empty, out listener))
            {
                listener.del.Invoke(e, target);
            }
        }
    }
}
