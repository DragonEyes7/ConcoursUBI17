﻿using UnityEngine;

public class TimeController : MonoBehaviour
{
    public MultipleDelegate Tick = new MultipleDelegate();
    public MultipleDelegate End = new MultipleDelegate();

    int _Time, _maxTime = 30;
    float _Timer;
    bool _IsPlaying = false;

    public int time
    {
        get { return _Time; }
        set { _Time = value; }
    }

    public int maxTime
    {
        get { return _maxTime; }
    }

    public bool isPlaying
    {
        get { return _IsPlaying; }
        set { _IsPlaying = value; }
    }

    void FixedUpdate()
    {
        _Timer += Time.deltaTime;
        if (_Timer >= 1f && _IsPlaying) DoTick();
    }

    public void DoTick()
    {
        _Timer = 0;
        _Time++;
        Tick.Execute(_Time);
        if (_Time == _maxTime)
        {
            //Game has ended stop countdown and show the players they f*cked up
            FindObjectOfType<GameManager>().Defeat();

            End.Execute(_Time);
            End.Empty();
        }
    }

    public void SetMaxTime(int maxTime)
    {
        _maxTime = maxTime;
    }
}