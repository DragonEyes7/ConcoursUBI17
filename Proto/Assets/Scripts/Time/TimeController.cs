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
        timer += Time.deltaTime;
        if (timer >= 1f && isForward) DoTick();
    }

    void DoTick()
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

    public void SetMaxTime(int maxTime)
    {
        _maxTime = maxTime;
    }
}