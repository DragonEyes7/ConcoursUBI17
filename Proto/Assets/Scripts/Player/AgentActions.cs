using UnityEngine;

public class AgentActions : Action
{
    [SerializeField]AudioClip _AudioClip;
    MainRecorder m_Recorder;
    MovementPlayer _MovementPlayer;
    PhotonView _PhotonView;
    bool _FromIntecept = false;

    new void Start()
    {
        base.Start();
        _PhotonView = GetComponent<PhotonView>();
        _MovementPlayer = GetComponent<MovementPlayer>();
        m_Recorder = FindObjectOfType<MainRecorder>();
    }

    void Update()
    {
        if (InputMode.isInMenu) return;
        if (m_Interact && Input.GetButtonDown("Action") && m_Recorder.IsRecording)
        {
            InteractStart();
            _FromIntecept = false;
        }

        if (_intercept && Input.GetButtonDown("Uplink") && m_Recorder.IsRecording)
        {
            InteractStart();
            _FromIntecept = true;
        }
    }

    void InteractStart()
    {
        _MovementPlayer.CantMove();
        transform.LookAt(new Vector3(m_Interactive.transform.position.x, transform.position.y, m_Interactive.transform.position.z));
        _PhotonView.RPC("RPCInteract", PhotonTargets.All);
    }

    [PunRPC]
    void RPCInteract()
    {
        GetComponent<Animator>().SetBool("Interact", true);
    }

    public void Interact()
    {
        AudioSource AS = GetComponent<AudioSource>();
        AS.clip = _AudioClip;
        AS.Play();
        if (!m_Interactive) return;
        if (_FromIntecept) m_Interactive.Intercept();
        else m_Interactive.Interact();
        m_Interact = false;
        _intercept = false;
    }
}