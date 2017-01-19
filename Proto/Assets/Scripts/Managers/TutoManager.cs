using UnityEngine;

public class TutoManager : MonoBehaviour
{
    [SerializeField]string[] m_ObjectivesMessagesDefuser;
    [SerializeField]string[] m_ObjectivesMessagesDetective;
    float m_TimerInSeconds = 30f;
    int[] m_CurrentObjective = new int[2];

	void Start ()
    {
		
	}

	void Update ()
    {
		
	}

    public float CurrentTimer()
    {
        return m_TimerInSeconds;
    }

    public string CurrentObjectiveDefuser()
    {
        return m_ObjectivesMessagesDefuser[m_CurrentObjective[0]];
    }

    public string CurrentObjectiveDetective()
    {
        return m_ObjectivesMessagesDetective[m_CurrentObjective[1]];
    }
}