using System;
using System.Collections.Generic;

public class MultipleDelegate
{
    private readonly List<Func<int, int>> _delegates = new List<Func<int, int>>();
    private Type _typeOf;

    public void Suscribe(Func<int, int> item)
    {
        _delegates.Add(item);
    }

    public void Empty()
    {
        _delegates.Clear();
    }

    public void Execute(int value)
    {
        foreach (var func in _delegates)
        {
            func(value);
        }
    }
}