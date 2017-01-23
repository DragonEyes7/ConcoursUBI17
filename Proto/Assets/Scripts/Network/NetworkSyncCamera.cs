using UnityEngine;

public class NetworkSyncCamera : Photon.MonoBehaviour
{
    Quaternion m_RotationSync;

    bool m_FirstUpdate = true;

    void Update()
    {
        if (!photonView.isMine)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, m_RotationSync, Time.deltaTime * 5);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.rotation);
        }
        else
        {
            m_RotationSync = (Quaternion)stream.ReceiveNext();

            if (m_FirstUpdate)
            {
                transform.rotation = m_RotationSync;
                m_FirstUpdate = false;
            }
        }
    }
}