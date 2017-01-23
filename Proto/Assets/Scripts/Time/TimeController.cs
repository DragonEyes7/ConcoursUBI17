using UnityEngine;

public class TimeController : MonoBehaviour
{
    public delegate void Tick(int tick);
    public event Tick EventTick;

    int m_Time, _maxTime;
    bool isForward = true;
    float timer = 0;

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
        if (timer >= 1f && isForward)
        {
            timer = 0;
            m_Time++;
            EventTick(m_Time);
        }
    }
}
