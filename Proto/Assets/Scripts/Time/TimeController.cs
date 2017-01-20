using UnityEngine;

public class TimeController : MonoBehaviour
{
	int m_Time;
	bool isForward = true;

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
		if (isForward)
		{
			m_Time++;
		}
		else
		{
			m_Time--;
			m_Time = m_Time < 0 ? 0 : m_Time;
		}
	}
}
