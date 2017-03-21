using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HUD : MonoBehaviour
{
    [SerializeField]AudioSource _BGM;
    [SerializeField]RectTransform m_ActionPrompt;
    [SerializeField]RectTransform _interceptPrompt;
    [SerializeField]Text m_Messages;
    [SerializeField]Text _VictoryMessage;
    [SerializeField]Text m_Timer;
    [SerializeField]ClockUI _clockUI;
    [SerializeField]CameraMenuUI _cameraSelectionUI;
    [SerializeField]RectTransform m_CenterCam;
    [SerializeField]RectTransform m_Uplink;
    [SerializeField]RectTransform m_UplinkIncoming;
    [SerializeField]RectTransform _CluesPrint;
    GameObject m_Player;

    TimeController m_TimeController;
    int m_LevelTime;
    float m_CurrentActionDuration;

    GameManager _GameManager;

    Color _TimerColor;
    int _TimerFontSize;

    #region private
    void Start ()
    {
        m_Messages.gameObject.SetActive(false);
        m_ActionPrompt.gameObject.SetActive(false);
        _interceptPrompt.gameObject.SetActive(false);
        _clockUI.gameObject.SetActive(false);
        _cameraSelectionUI.gameObject.SetActive(false);
        _CluesPrint.GetComponent<PrintClues>().Setup();
        _CluesPrint.gameObject.SetActive(false);
        _VictoryMessage.gameObject.SetActive(false);

        m_Uplink.gameObject.SetActive(false);
        m_UplinkIncoming.gameObject.SetActive(false);

        m_TimeController = FindObjectOfType<TimeController>();
        m_TimeController.Tick.Suscribe(ShowTimer);
        _TimerColor = m_Timer.color;
        _TimerFontSize = m_Timer.fontSize;

        _GameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PhotonNetwork.Disconnect();
        }

        if (PhotonNetwork.isMasterClient && Input.GetButtonDown("TimeRewind"))
        {
            _clockUI.Toggle();
        }

        if (!PhotonNetwork.isMasterClient && Input.GetButtonDown("TimeRewind"))
        {
            _cameraSelectionUI.Toggle();
        }
    }

    void FadeMessage()
    {
        CanvasRenderer canva = m_Messages.GetComponent<CanvasRenderer>();
        canva.SetAlpha(canva.GetAlpha() - 0.05f);

        if (canva.GetAlpha() <= 0)
        {
            HideMessage();
        }
    }

    void HideMessage()
    {
        CanvasRenderer canva = m_Messages.GetComponent<CanvasRenderer>();
        m_Messages.gameObject.SetActive(false);
        canva.SetAlpha(1f);
        CancelInvoke("FadeMessage");
    }

    void FadeInUplink()
    {
        CanvasRenderer canva = m_UplinkIncoming.GetComponent<CanvasRenderer>();
        canva.SetAlpha(canva.GetAlpha() + 0.05f);

        if (canva.GetAlpha() >= 1)
        {
            CancelInvoke("FadeInUplink");
            InvokeRepeating("FadeOutUplink", 0f, 0.01f);            
        }
    }

    void FadeOutUplink()
    {
        CanvasRenderer canva = m_UplinkIncoming.GetComponent<CanvasRenderer>();
        canva.SetAlpha(canva.GetAlpha() - 0.05f);

        if (canva.GetAlpha() <= 0)
        {
            CancelInvoke("FadeOutUplink");
            InvokeRepeating("FadeInUplink", 0f, 0.01f);
        }
    }
    
    void HideUplink()
    {
        CancelInvoke("FadeInUplink");
        CancelInvoke("FadeOutUplink");

        m_UplinkIncoming.gameObject.SetActive(false);
    }

    int ShowTimer(int time)
    {
        time = (m_LevelTime - time);

        if (time < 0)
        {
            time = 0;
        }

        if (time <= 20 && m_Timer.color != Color.red)
        {
            m_Timer.color = Color.red;
            _BGM.pitch = 1.2f;
        }
        else if (time > 20 && m_Timer.color == Color.red)
        {
            m_Timer.color = _TimerColor;
            _BGM.pitch = 1;
        }
        else if(time < 10 && time != 0)
        {
            m_Timer.fontSize = _TimerFontSize +  (11 - time) * 5;
        }
        else if(time > 10 && m_Timer.fontSize != _TimerFontSize)
        {
            m_Timer.fontSize = _TimerFontSize;
        }

        string minSec = string.Format("{0}:{1:00}", time / 60, time % 60);

        m_Timer.text = minSec;
        return 0;
    }
    #endregion

    #region public
    public void SetPlayer(GameObject player)
    {
        if (player)
        {
            m_CenterCam.gameObject.SetActive(false);
            m_Player = player;
        }
    }

    public void SetLevelTimer(int timer)
    {
        m_LevelTime = timer;
        m_TimeController.SetMaxTime(timer);
    }

    public void ShowVictoryMessage(string msg)
    {
        _VictoryMessage.text = msg;
        _VictoryMessage.gameObject.SetActive(true);
    }

    public void ShowMessages(string msg, float duration)
    {
        m_Messages.text = msg;
        m_Messages.gameObject.SetActive(true);
        InvokeRepeating("FadeMessage", duration, 0.15f);
    }

    public void ShowActionPrompt(string message)
    {
        m_ActionPrompt.GetComponentInChildren<Text>().text = message;
        m_ActionPrompt.gameObject.SetActive(true);
    }

    public void ShowIntercepPrompt(string message)
    {
        _interceptPrompt.GetComponentInChildren<Text>().text = message;
        _interceptPrompt.gameObject.SetActive(true);
    }

    public void HideActionPrompt()
    {
        if (m_ActionPrompt.gameObject.activeSelf)
        {
            m_ActionPrompt.gameObject.SetActive(false);
        }
    }

    public void HideInterceptPrompt()
    {
        if (_interceptPrompt.gameObject.activeSelf)
        {
            _interceptPrompt.gameObject.SetActive(false);
        }
    }

    public void ShowUplink(bool value)
    {
        m_Uplink.gameObject.SetActive(value);
    }

    public void BlinkUplink()
    {
        CanvasRenderer canva = m_UplinkIncoming.GetComponent<CanvasRenderer>();
        canva.SetAlpha(0f);
        m_UplinkIncoming.gameObject.SetActive(true);

        if(!m_Uplink.gameObject.activeSelf)
        {
            m_Uplink.GetComponentInChildren<Text>().text = "Analyse clues";
            m_Uplink.gameObject.SetActive(true);
        }

        InvokeRepeating("FadeInUplink", 0f, 0.01f);
        Invoke("HideUplink", 3f);
    }

    public void LookAtClues()
    {
        if(_CluesPrint.gameObject.activeSelf)
        {
            _CluesPrint.gameObject.SetActive(false);
            m_Uplink.GetComponentInChildren<Text>().text = "Analyse clues";
        }
        else
        {
            m_Uplink.GetComponentInChildren<Text>().text = "Hide clues";
            List<Clue> clues = _GameManager.GetIntelligenceClues();
            _CluesPrint.gameObject.SetActive(true);

            List<string> messages = new List<string>();

            foreach(Clue clue in clues)
            {
                messages.Add(clue.ClueString);
            }

            _CluesPrint.GetComponent<PrintClues>().Print(messages.ToArray());
        }
    }

    public void GameEndedSuccessfully()
    {
        m_TimeController.SaveTime();
        m_TimeController.isPlaying = false;
    }

    public void WrongTargetIntercepted()
    {
        m_TimeController.WrongTargetIntercepted();
    }
    #endregion
}