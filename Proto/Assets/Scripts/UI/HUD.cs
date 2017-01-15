using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]RectTransform m_ActionPrompt;
    [SerializeField]Slider m_ActionSlider;
    [SerializeField]Text m_Messages;
    Text m_ActionSliderTimer;
    GameObject m_Player;

    Action m_Action;

    float m_CurrentActionDuration;

	void Start ()
    {
        m_Messages.gameObject.SetActive(false);
        m_ActionPrompt.gameObject.SetActive(false);

        m_ActionSliderTimer = m_ActionSlider.GetComponentInChildren<Text>();

        m_ActionSlider.gameObject.SetActive(false);
	}

    void Update()
    {
        if(m_Action)
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
    }

    public void SetPlayer(GameObject player)
    {
        m_Player = player;

        SetupAction();
    }

    void SetupAction()
    {
        m_Action = m_Player.GetComponent<Action>();
        //m_Action.EventActionStartTimer += SetTimer;
    }

    void SetTimer(float duration)
    {
        m_CurrentActionDuration = duration + Time.time;
        m_ActionPrompt.gameObject.SetActive(false);

        m_ActionSlider.maxValue = duration;
        m_ActionSlider.value = 0;

        HideActionMeter();
        m_ActionSlider.gameObject.SetActive(true);
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
        if(m_ActionPrompt.gameObject.activeSelf)
        {
            m_ActionPrompt.gameObject.SetActive(false);
        }
        else
        {
            m_ActionSliderTimer.text = "Cancelled";
            m_Action.SetInteract(false);
            InvokeRepeating("FadeActionMeter", 0f, 0.15f);
        }
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

    public void ShowMessages(string msg, float duration)
    {
        m_Messages.text = msg;
        m_Messages.gameObject.SetActive(true);
        InvokeRepeating("FadeMessage", duration, 0.15f);
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
}