using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public struct RecordState
{
    private readonly Vector3 _position;
    private readonly Quaternion _rotation;

    public RecordState(Vector3 position, Quaternion rotation)
    {
        _position = position;
        _rotation = rotation;
    }

    public Vector3 Position
    {
        get { return _position; }
    }

    public Quaternion Rotation
    {
        get { return _rotation; }
    }
}

public class Recorder : MonoBehaviour
{
    private readonly Dictionary<int, RecordState> _states = new Dictionary<int, RecordState>();
    private RecordState _previousState;
    private PhotonView _photonView;

    //Animator m_Animator;
    private Rigidbody _rigidbody;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _photonView = GetComponent<PhotonView>();
        Register();
        DoOnTick(0);
    }

    private void Register()
    {
        var mainRecorder = FindObjectOfType<MainRecorder>();
        if (!mainRecorder) return;
        mainRecorder.OnTick.Suscribe(DoOnTick);
        mainRecorder.OnRewind.Suscribe(DoOnRewind);
    }

    private RecordState FindClosestState(int key)
    {
        var keys = new List<int>(_states.Keys);
        var index = keys.BinarySearch(key);
        //~ = Bitwise NOT
        index = ~index - 1;
        if (index < 0) index = 0;
        if (!_states.ContainsKey(index))
            Debug.Log("Using previous state, Dictionnary did not contain proper key : " + index +
                      " Dictionnary count : " + _states.Count);
        return !_states.ContainsKey(index) ? _previousState : _states[index];
    }

    private int DoOnTick(int time)
    {
        var curState = new RecordState(transform.position, transform.rotation);
        if (curState.Equals(_previousState)) return 0;
        _previousState = curState;
        _states[time] = new RecordState(transform.position, transform.rotation);
        return 0;
    }

    private int DoOnRewind(int time)
    {
        _photonView.RPC("DoRewind", PhotonTargets.All, time);
        return 0;
    }

    [PunRPC]
    private void DoRewind(int time)
    {
        if (_states.ContainsKey(time))
        {
            PlayState(_states[time]);
        }
        else
        {
            PlayState(_states.Last().Key < time ? _previousState : FindClosestState(time));
        }

        if (_rigidbody)
        {
            _rigidbody.isKinematic = false;
        }
    }

    void PlayState(RecordState recordState)
    {
        transform.position = recordState.Position;
        transform.rotation = recordState.Rotation;
    }
}