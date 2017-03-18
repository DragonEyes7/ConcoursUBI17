using UnityEngine;

public class NetworkSyncCamera : Photon.MonoBehaviour
{
    Quaternion m_RotationSync;

    bool m_FirstUpdate = true;

    AudioSource AS_Move, AS_ServoStop;
    bool _IsMoving = false;

    void Start()
    {
        AudioSource[] sounds = GetComponents<AudioSource>();
        AS_Move = sounds[0];
        AS_ServoStop = sounds[2];
    }

    void Update()
    {
        if (!photonView.isMine)
        {
            if (Vector3.Distance(transform.rotation.eulerAngles, m_RotationSync.eulerAngles) > 2f && !_IsMoving)
            {
                AS_Move.Play();
                AS_ServoStop.Stop();
                _IsMoving = true;
            }
            else if(Vector3.Distance(transform.rotation.eulerAngles, m_RotationSync.eulerAngles) < 2f && _IsMoving)
            {
                AS_ServoStop.Play();
                AS_Move.Stop();
                _IsMoving = false;
            }

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