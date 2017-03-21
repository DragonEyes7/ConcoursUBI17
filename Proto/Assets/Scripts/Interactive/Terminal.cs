using System;
using UnityEngine;

public class Terminal : Interactive
{
    [SerializeField]Door[] m_Doors;

    IntelligenceAction m_Action;

    AudioSource _AudioSource;

    new void Start()
    {
        base.Start();
        _AudioSource = GetComponent<AudioSource>();
        m_SelectMat = Resources.Load<Material>("MAT_OutlineIntelligence");
    }

    void OnTriggerEnter(Collider other)
    {
        if (!m_IsActivated)
        {
            m_Action = other.GetComponent<IntelligenceAction>();
            if (m_Action && m_Action.enabled)
            {
                RaycastHit hit;
                    
                Vector3 direction = m_Action.GetCenterCam().position - transform.position;

                if (Physics.Raycast(transform.position, direction, out hit, 25f, LayerMask.NameToLayer("Default")))
                {
                    Debug.DrawRay(transform.position, direction, Color.red, 5f);
                    if(hit.transform == m_Action.GetCenterCam().transform)
                    {
                        Select(m_Action);
                    }
                }
            }
        }
    }

    protected override void Select(Action action)
    {
        m_HUD.ShowActionPrompt("Hack Terminal");
        action.SetInteract(true);
        action.SetInteractionObject(this);
        Select();
    }

    protected override void UnSelect(Action action)
    {
        UnSelect();
        m_HUD.HideActionPrompt();
        action.SetInteract(false);
        action.SetInteractionObject(null);
    }

    void OnTriggerExit(Collider other)
    {
        if (!m_IsActivated && m_IsSelected)
        {
            m_Action = other.GetComponent<IntelligenceAction>();
            if (m_Action)
            {
                if (m_Action.enabled)
                {
                    UnSelect(m_Action);
                }
            }
        }
    }

    public override void Interact()
    {
        foreach (Door door in m_Doors)
        {
            door.Unlock();
        }
        GetComponent<PhotonView>().RPC("StopBlinking", PhotonTargets.All);
        m_IsActivated = true;
        _AudioSource.Play();
        if (m_Action)UnSelect(m_Action);
    }

    [PunRPC]
    void StopBlinking()
    {
        var blinker = GetComponentInChildren<Blinker>();
        if(blinker)blinker.Interacted();
    }

    public void SetDoors(Door[] doors)
    {
        m_Doors = doors;
    }

    public override void MoveObject()
    {
        //TODO This here should never be called, as of now at least, probably better to leave it as is, if we ever need it
        throw new NotImplementedException();
    }

    public override void ResetObject()
    {
        //TODO This here should never be called, as of now at least, probably better to leave it as is, if we ever need it
        throw new NotImplementedException();
    }
}