using System;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public MultipleDelegate Tick = new MultipleDelegate();
    public MultipleDelegate End = new MultipleDelegate();

    int m_Time, _maxTime;
    bool isForward = true;
    float timer;
    public int time
    {
        get { return m_Time; }
        set { m_Time = value; }
    }


    public bool isFoward
    {
        get { return isForward; }
        set { isForward = value; }
    }

    void FixedUpdate()
    {
        DoTick();
    }

    void DoTick()
    {
        timer += Time.deltaTime;
        if (timer >= 1f && isForward)
        {
            timer = 0;
            m_Time++;
            Tick.Execute(m_Time);
            if (m_Time == _maxTime)
            {
                //Game has ended stop countdown and show the players they f*cked up
                End.Execute(m_Time);
                End.Empty();
            }
        }
    }

    public void SetMaxTime(int maxTime)
    {
        _maxTime = maxTime;
    }

    public int GetMaxTime(int newTime)
    {
        Debug.Log("What the hell : " + (time - newTime >= 0 ? newTime : time));
        return time - newTime >= 0 ? newTime : time;
    }
}