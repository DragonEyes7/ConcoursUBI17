using UnityEngine;
using UnityEngine.UI;

public class SyncPlayersUI : MonoBehaviour
{
    [SerializeField]Text m_ReadyText;
    [SerializeField]Text m_TimerText;
    [SerializeField]float m_WaitTime = 5f;

    float m_Timer;
    float m_DeltaPaused;
    bool m_IsReady = false;

	void Start ()
    {
        m_Timer = m_WaitTime;
        m_TimerText.gameObject.SetActive(false);
	}

    void StartGame()
    {
        FindObjectOfType<TimeController>().isPlaying = true;
        Time.timeScale = 1f;
        Destroy(gameObject);
    }

    public void PlayerReady()
    {
        m_IsReady = true;
        m_TimerText.gameObject.SetActive(true);
        m_ReadyText.gameObject.SetActive(false);

        InvokeRepeating("Timer", 0, 1f);
    }

    void Timer()
    {
        m_Timer -= Time.realtimeSinceStartup - m_DeltaPaused;
        m_TimerText.text = m_Timer.ToString("F0");
        if (m_Timer <= 0)
        {
            StartGame();
        }
    }
}
