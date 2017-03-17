using UnityEngine;

public class NetworkSyncMovement : Photon.MonoBehaviour
{
    Vector3 m_PositionSync;
    Quaternion m_RotationSync;
    Animator _Animator;

    bool _IdleBreak = false;
    bool m_FirstUpdate = true;

    void Start ()
    {
        _Animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!photonView.isMine)
        {
            if (Vector3.Distance(transform.position, m_PositionSync) > 0.2f)
            {
                _Animator.SetFloat("Speed", 1);
                IdleBreakReset();
            }
            else if (!_IdleBreak)
            {
                _IdleBreak = true;
                Invoke("IdleBreak", Random.Range(2, 5));
            }
            else
            {
                _Animator.SetFloat("Speed", 0);
            }

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

    void IdleBreak()
    {
        _Animator.SetTrigger("IdleBreak");
        Invoke("IdleBreakReset", Random.Range(10, 15));
    }

    void IdleBreakReset()
    {
        CancelInvoke("IdleBreak");
        _IdleBreak = false;
    }
}