using System;
using System.Collections.Generic;
using UnityEngine;

public class MultipleDelegate
{
    private readonly List<Func<int, int>> _delegates = new List<Func<int, int>>();
    private Type _typeOf;
    private int _pos = 0;

    public int Suscribe(Func<int, int> item)
    {
        _delegates.Add(item);
        return 0;
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