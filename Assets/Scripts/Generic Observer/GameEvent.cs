using System;
using System.Collections.Generic;

public class GameEvent<T>
{
    private List<Action<T>> _subs = new();

    public void Subscribe(Action<T> subscriber)
    {
        if (!_subs.Contains(subscriber))
            _subs.Add(subscriber);
    }
    public void Remove(Action<T> subscriber)
    {
        if (_subs.Contains(subscriber))
            _subs.Remove(subscriber);
    }
    public void Raise(T eventData)
    {
        _subs?.ForEach((subscriber) => subscriber.Invoke(eventData));
    }
}