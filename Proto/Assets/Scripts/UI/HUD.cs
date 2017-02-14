using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HUD : MonoBehaviour
{
    [SerializeField]RectTransform m_ActionPrompt;
    [SerializeField]Slider m_ActionSlider;
    [SerializeField]Text m_Messages;
    [SerializeField]Text m_Timer;
    [SerializeField]GameObject _clockUI;
    [SerializeField]RectTransform m_CenterCam;
    [SerializeField]RectTransform m_Uplink;
    [SerializeField]RectTransform m_UplinkIncoming;
    Text m_ActionSliderTimer;
    GameObject m_Player;

    Action m_Action;

    TimeController m_TimeController;
    int m_LevelTime;
    float m_CurrentActionDuration;

    #region private
    void Start ()
    {
        m_Messages.gameObject.SetActive(false);
        m_ActionPrompt.gameObject.SetActive(false);
        _clockUI.SetActive(false);

        m_Uplink.gameObject.SetActive(false);
        m_UplinkIncoming.gameObject.SetActive(false);

        m_ActionSliderTimer = m_ActionSlider.GetComponentInChildren<Text>();

        m_ActionSlider.gameObject.SetActive(false);

        m_TimeController = FindObjectOfType<TimeController>();
        m_TimeController.Tick.Suscribe(ShowTimer);
    }

    void Update()
    {
        if (m_Action)
        {
            if (m_Action.isInteracting)
            {
                m_ActionSliderTimer.text = (m_CurrentActionDuration - Time.time).ToString("F2");
                m_ActionSlider.value += Time.deltaTime;

                if ((m_CurrentActionDuration - Time.time) <= 0)
                {
                    m_ActionSlider.gameObject.SetActive(false);
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PhotonNetwork.Disconnect();
        }
        if (Input.GetButtonDown("TimeRewind"))
        {
            _clockUI.SetActive(true);
        }
        else if (Input.GetButtonUp("TimeRewind"))
        {
            _clockUI.SetActive(false);
        }
    }

    void SetupAction()
    {
        m_Action = m_Player.GetComponent<Action>();
        //m_Action.EventActionStartTimer += SetTimer;
    }

    void SetActionDuration(float duration)
    {
        m_CurrentActionDuration = duration + Time.time;
        m_ActionPrompt.gameObject.SetActive(false);

        m_ActionSlider.maxValue = duration;
        m_ActionSlider.value = 0;

        HideActionMeter();
        m_ActionSlider.gameObject.SetActive(true);
    }

    void FadeActionMeter()
    {
        CanvasRenderer[] canvas = m_ActionSlider.GetComponentsInChildren<CanvasRenderer>();
        foreach (CanvasRenderer canva in canvas)
        {
            canva.SetAlpha(canva.GetAlpha()-0.05f);
        }

        if(canvas[0].GetAlpha() <= 0)
        {
            HideActionMeter();
        }
    }

    void HideActionMeter()
    {
        CanvasRenderer[] canvas = m_ActionSlider.GetComponentsInChildren<CanvasRenderer>();
        m_ActionSlider.gameObject.SetActive(false);
        foreach (CanvasRenderer canva in canvas)
        {
            canva.SetAlpha(1f);
        }
        CancelInvoke("FadeActionMeter");
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

        string minSec = string.Format("{0}:{1:00}", time / 60, time % 60);

        m_Timer.text = minSec;
        return 0;
    }

    string GetColorName(int colorID)
    {
        switch (colorID)
        {
            case 0:
                return "Blue";
            case 1:
                return "Green";
            case 2:
                return "Pink";
            case 3:
                return "Red";
            case 4:
                return "Yellow";
        }

        return null;
    }
    #endregion

    #region public
    public void SetPlayer(GameObject player)
    {
        if (player)
        {
            m_CenterCam.gameObject.SetActive(false);
            m_Player = player;

            SetupAction();
        }
    }

    public void SetLevelTimer(int timer)
    {
        m_LevelTime = timer;
        m_TimeController.SetMaxTime(timer);
    }

    public void ShowMessages(string msg, float duration)
    {
        m_Messages.text = msg;
        m_Messages.gameObject.SetActive(true);
        InvokeRepeating("FadeMessage", duration, 0.15f);
    }

    public void ShowActionPrompt(string message)
    {
        m_ActionSlider.gameObject.SetActive(false);
        m_ActionPrompt.GetComponentInChildren<Text>().text = message;
        //m_ActionPrompt.GetComponentInChildren<Image>() = sprite;
        m_ActionPrompt.gameObject.SetActive(true);
    }

    public void HideActionPrompt()
    {
        if (m_ActionPrompt.gameObject.activeSelf)
        {
            m_ActionPrompt.gameObject.SetActive(false);
        }
        else if(m_Action)
        {
            m_ActionSliderTimer.text = "Cancelled";
            m_Action.SetInteract(false);
            InvokeRepeating("FadeActionMeter", 0f, 0.15f);
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
        if(m_Messages.gameObject.activeSelf)
        {
            m_Messages.gameObject.SetActive(false);
        }
        else
        {
            Dictionary<string, int> clues = FindObjectOfType<GameManager>().GetIntelligenceClues();
            string message = "";

            if (clues.ContainsKey("Hair"))
            {
                message += "The target has " + GetColorName(clues["Hair"]) + " hairs";
            }

            if (clues.ContainsKey("Nose"))
            {
                if (message == "")
                {
                    message += "\nThe target has a " + GetColorName(clues["Nose"]) + " Nose";
                }
                else
                {
                    message += ", a " + GetColorName(clues["Nose"]) + " Nose";
                }
            }

            if (clues.ContainsKey("Backpack"))
            {
                if(message == "")
                {
                    message += "\nThe target has a " + GetColorName(clues["Backpack"]) + " backpack";
                }
                else
                {
                    message += " and a " + GetColorName(clues["Backpack"]) + " backpack";
                }
            }

            if(message != "")
            {
                m_Messages.text = message + ".";
                m_Messages.gameObject.SetActive(true);
            }            
        }
    }
    #endregion
}