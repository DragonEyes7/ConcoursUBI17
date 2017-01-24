using UnityEngine;

public class TimeController : MonoBehaviour
{
    public delegate void Tick(int tick);
    public event Tick EventTick;

    public delegate void End();
    public event End EventEnd;

    int m_Time, _maxTime;
    bool isForward = true;
    float timer = 0;

    delegate void OnThick();
    event OnThick EventOnThick;

    TimeController()
    {
        EventOnThick += DoThick;
    }
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
        if(EventOnThick != null) EventOnThick();
    }

    void DoThick()
    {
        timer += Time.deltaTime;
        if (timer >= 1f && isForward)
        {
            timer = 0;
            m_Time++;
            if(EventTick != null)EventTick(m_Time);
            if (m_Time == _maxTime)
            {
                //Game has ended stop countdown and show the players they f*cked up
                if (EventEnd != null) EventEnd();
                EventOnThick -= DoThick;
            }
        }
    }

    public void SetMaxTime(int maxTime)
    {
        _maxTime = maxTime;
    }

    public int GetMaxTime(int newTime)
    {
        return newTime >= 0 ? newTime : time;
    }
}