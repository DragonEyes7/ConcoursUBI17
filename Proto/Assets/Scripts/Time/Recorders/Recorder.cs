using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class Recorder : MonoBehaviour
{
    internal struct RecordState{}

    private readonly Dictionary<int, RecordState> _states = new Dictionary<int, RecordState>();
    private RecordState _previousState;
    internal PhotonView _photonView;
    internal MainRecorder _mainRecorder;

    //Animator m_Animator;
    private Rigidbody _rigidbody;


    internal void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _photonView = GetComponent<PhotonView>();
        _mainRecorder = FindObjectOfType<MainRecorder>();
        Register(DoOnTick, DoOnRewind);
        DoOnTick(0);
    }

    protected abstract void Register(Func<int, int> doOnTick, Func<int, int> doOnRewind);

    internal virtual RecordState FindClosestState(int key)
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

    protected virtual int DoOnTick(int time)
    {
        if (this == null) return 0;
        var curState = new RecordState();
        if (curState.Equals(_previousState)) return 0;
        _previousState = curState;
        _states[time] = curState;
        return 0;
    }

    protected virtual int DoOnRewind(int time)
    {
        _photonView.RPC("DoRewind", PhotonTargets.All, time);
        return 0;
    }

    [PunRPC]
    internal abstract void DoRewind(int time);

    internal abstract void PlayState(RecordState recordState);
}