using UnityEngine;

public class AgentActions : Action
{
    MainRecorder m_Recorder;
    MovementPlayer _MovementPlayer;
    PhotonView _PhotonView;

    new void Start()
    {
        base.Start();
        _PhotonView = GetComponent<PhotonView>();
        _MovementPlayer = GetComponent<MovementPlayer>();
        m_Recorder = FindObjectOfType<MainRecorder>();
    }

    void Update()
    {
        if (m_Interact && Input.GetButtonDown("Action") && m_Recorder.IsRecording)
        {
            _MovementPlayer.CantMove();
            transform.LookAt(new Vector3(m_Interactive.transform.position.x, transform.position.y, m_Interactive.transform.position.z));
            _PhotonView.RPC("RPCInteract", PhotonTargets.All);
        }
        if (_intercept && Input.GetButtonDown("Uplink") && m_Recorder.IsRecording)
        {
            m_Interactive.Intercept();
            _intercept = false;
        }
    }

    [PunRPC]
    void RPCInteract()
    {
        GetComponent<Animator>().SetBool("Interact", true);
    }

    public void Interact()
    {
        if (!m_Interactive) return;
        m_Interactive.Interact();
        m_Interact = false;
    }
}