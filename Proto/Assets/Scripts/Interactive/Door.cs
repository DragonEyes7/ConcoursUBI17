﻿using UnityEngine;

public class Door : Interactive
{
    protected enum DoorSound
    {
        OPEN,
        CLOSE,
        LOCK
    }

    [SerializeField] AudioClip[] _AudioClip;
    [SerializeField]GameObject[] _GreenLights;
    [SerializeField]GameObject[] _RedLights;
    [SerializeField] Vector3 _OpenPosition = new Vector3(0,90,0);
    [SerializeField] Vector3 _ClosePosition = new Vector3(0,0,0);
    [SerializeField] bool _isOpen;
    [SerializeField] bool _isLock = true;

    InteractiveObjectRecorder _interactiveObjectRecorder;
    AgentActions _action;
    AudioSource _AudioSource;

    protected new void Start()
    {
        base.Start();
        Setup();
    }

    protected void Setup()
    {
        _AudioSource = GetComponent<AudioSource>();
        _interactiveObjectRecorder = GetComponent<InteractiveObjectRecorder>();
        _interactiveObjectRecorder.SetStatus(_isOpen);

        Lock();

        if(_isOpen && transform.localEulerAngles != _OpenPosition)
        {
            Open();
        }
    }

    protected void Open()
    {
        transform.localEulerAngles = _OpenPosition;
        PlaySound(DoorSound.OPEN);
    }

    protected void Close()
    {
        transform.localEulerAngles = _ClosePosition;
        PlaySound(DoorSound.CLOSE);
    }

    protected void OnTriggerExit(Collider other)
    {
        _action = other.GetComponent<AgentActions>();
        if (!_action || !_action.enabled) return;
        m_HUD.HideActionPrompt();
        _action.SetInteract(false);
    }

    protected void OnTriggerStay(Collider other)
    {
        _action = other.GetComponent<AgentActions>();
        if (_action && _action.enabled)
        {
            m_HUD.ShowActionPrompt(_interactiveObjectRecorder.GetStatus() ? "Close door." : "Open door.");
            _action.SetInteract(true);
            _action.SetInteractionObject(this);
        }
    }

    public override void Interact()
    {
        if(!_isLock)
        {
            _interactiveObjectRecorder.ObjectInteraction(!_interactiveObjectRecorder.GetStatus());
        }
        else
        {
            FindObjectOfType<HUD>().ShowMessages("Door is locked", 3f);
            GetComponent<PhotonView>().RPC("RPCLock", PhotonTargets.All);            
        }
    }

    [PunRPC]
    protected void RPCLock()
    {
        PlaySound(DoorSound.LOCK);
    }

    public override void MoveObject()
    {
        Open();
    }

    public override void ResetObject()
    {
        Close();
    }

    public void Unlock()
    {
        GetComponent<PhotonView>().RPC("RPCUnlock", PhotonTargets.All);
    }

    [PunRPC]
    protected void RPCUnlock()
    {
        _isLock = false;

        Lock();
    }

    protected void PlaySound(DoorSound soundID)
    {
        _AudioSource.clip = _AudioClip[(int)soundID];
        _AudioSource.Play();
    }
    
    protected void Lock()
    {
        foreach (GameObject light in _GreenLights)
        {
            light.GetComponentInChildren<Light>().enabled = !_isLock;
        }

        foreach (GameObject light in _RedLights)
        {
            light.GetComponentInChildren<Light>().enabled = _isLock;
        }
    }
}