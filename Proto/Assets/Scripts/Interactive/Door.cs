public class Door : Interactive
{
    bool m_IsOpen = false;

    new void Start()
    {
        base.Start();

        m_IsActivated = true;
    }

    public override void Interact()
    {
        if(!m_IsOpen)
        {
            m_IsOpen = true;
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        GetComponent<PhotonView>().RPC("RPCOpenDoor", PhotonTargets.All);
    }

    [PunRPC]
    void RPCOpenDoor()
    {
        if(PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }        
    }
}
