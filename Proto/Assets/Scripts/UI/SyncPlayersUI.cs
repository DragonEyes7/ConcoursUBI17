using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SyncPlayersUI : MonoBehaviour
{
    [SerializeField]Text m_ReadyText;
    [SerializeField]Image _ReadyButton;
    [SerializeField]AudioSource _BGMAudioSource;
    int m_Timer;
    bool _Ready = false, _RainbowMode;

	void Start ()
    {
        _ReadyButton.enabled = false;
	}

    [PunRPC]
    void StartGame()
    {
        _BGMAudioSource.Play();
        var timeController = FindObjectOfType<TimeController>();
        timeController.IsPlaying = true;
        timeController.Tick.Execute(0);
        TimeStopper.StartTime();
        Destroy(gameObject);
    }

    public void PlayerReady()
    {
        _ReadyButton.enabled = true;
        m_ReadyText.gameObject.SetActive(false);
        FindObjectOfType<MainRecorder>().GetComponent<AudioSource>().Play();
        _Ready = true;
    }

    void Update()
    {
        if (Input.GetButton("Action") && _Ready)
        {
            GetComponent<PhotonView>().RPC("StartGame", PhotonTargets.All);
        }

        if(Input.GetButton("Uplink") && _Ready && !_RainbowMode)
        {
            _RainbowMode = true;
            GetComponent<PhotonView>().RPC("RPCRainbowMode", PhotonTargets.All);
        }
    }

    [PunRPC]
    void RPCRainbowMode()
    {
        StartCoroutine(RainbowMode());
    }

    IEnumerator RainbowMode()
    {
        _ReadyButton.color = Random.ColorHSV();
        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine(RainbowMode());
    }
}
