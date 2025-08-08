using System;
using System.Collections.Generic;


public static class EventManager
{
    private static Dictionary<Type, object> _eventTable = new();

    public static GameEvent<T> GetEvent<T>()
    {
        var type = typeof(T);

        if (!_eventTable.ContainsKey(type))
        {
            _eventTable[type] = new GameEvent<T>();
        }

        return (GameEvent<T>)_eventTable[type];
    }

    public static void Subscribe<T>(Action<T> listener) => GetEvent<T>().Subscribe(listener);
    public static void UnSubscribe<T>(Action<T> listener) => GetEvent<T>().Remove(listener);
    public static void Raise<T>(T eventData) => GetEvent<T>().Raise(eventData);
}