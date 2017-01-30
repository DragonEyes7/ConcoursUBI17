using System;
using System.Collections.Generic;

public class MultipleDelegate
{
    private readonly Dictionary<int, Func<int, int>> _delegates = new Dictionary<int, Func<int, int>>();
    private Type _typeOf;
    private int _pos = 0;

    public int Suscribe(Func<int, int> item)
    {
        _delegates.Add(_pos, item);
        return _pos++;
    }

    public void Unsuscribe(int key)
    {
        _delegates.Remove(key);
    }

    public void Empty()
    {
        _delegates.Clear();
    }

    public void Execute(int value)
    {
        foreach (var item in _delegates)
        {
            var func = item.Value;
            func(value);
        }
    }
}