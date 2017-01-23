using UnityEngine;

public class NetworkSyncMovement : Photon.MonoBehaviour
{
    Vector3 m_PositionSync;
    Quaternion m_RotationSync;

    bool m_FirstUpdate = true;

    void Start ()
    {
		
	}

    void FixedUpdate()
    {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, m_PositionSync, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, m_RotationSync, Time.deltaTime * 5);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            m_PositionSync = (Vector3)stream.ReceiveNext();
            m_RotationSync = (Quaternion)stream.ReceiveNext();

            if (m_FirstUpdate)
            {
                transform.position = m_PositionSync;
                transform.rotation = m_RotationSync;
                m_FirstUpdate = false;
            }
        }
    }
}