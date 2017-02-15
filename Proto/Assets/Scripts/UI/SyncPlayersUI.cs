using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SyncPlayersUI : MonoBehaviour
{
    [SerializeField]Text m_ReadyText;
    [SerializeField]Text m_TimerText;
    [SerializeField]int m_WaitTime = 5;

    int m_Timer;
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
        CancelInvoke("Timer");
        Destroy(gameObject);
    }

    public void PlayerReady()
    {
        m_IsReady = true;
        m_TimerText.gameObject.SetActive(true);
        m_ReadyText.gameObject.SetActive(false);

        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        --m_Timer;
        m_TimerText.text = m_Timer.ToString();
        if (m_Timer <= 0)
        {
            StartGame();
            yield break;
        }
        yield return new WaitForSecondsRealtime(1f);
        StartCoroutine(Timer());
    }
}
