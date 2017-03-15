using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SyncPlayersUI : MonoBehaviour
{
    [SerializeField]Text m_ReadyText;
    [SerializeField]Text m_TimerText;
    [SerializeField]int m_WaitTime = 5;
    [SerializeField]AudioSource _BGMAudioSource;
    int m_Timer;

	void Start ()
    {
        m_Timer = m_WaitTime;
        m_TimerText.gameObject.SetActive(false);
	}

    void StartGame()
    {
        _BGMAudioSource.Play();
        var timeController = FindObjectOfType<TimeController>();
        timeController.isPlaying = true;
        timeController.Tick.Execute(0);
        TimeStopper.StartTime();
        CancelInvoke("Timer");
        Destroy(gameObject);
    }

    public void PlayerReady()
    {
        m_TimerText.gameObject.SetActive(true);
        m_ReadyText.gameObject.SetActive(false);
        FindObjectOfType<MainRecorder>().GetComponent<AudioSource>().Play();
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
