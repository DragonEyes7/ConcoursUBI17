using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class Recorder : MonoBehaviour
{
    private RecordState _previousState;
    internal PhotonView _photonView;
    private MainRecorder _mainRecorder;

    //Animator m_Animator;
    internal Rigidbody _rigidbody;


    internal void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _photonView = GetComponent<PhotonView>();
        _mainRecorder = FindObjectOfType<MainRecorder>();
        Register(DoOnTick, DoOnRewind);
        DoOnTick(0);
    }

    protected virtual void Register(Func<int, int> doOnTick, Func<int, int> doOnRewind)
    {
        if (!_mainRecorder) return;
        _mainRecorder.OnTick.Suscribe(doOnTick);
        _mainRecorder.OnRewind.Suscribe(doOnRewind);
    }

    internal virtual int FindClosestKey(int key, List<int> keys)
    {
        var val = 0;
        for (var i = 0; i < keys.Count-1; ++i)
        {
            val = keys[i];
            if (val < key && keys[i + 1] > key) break;
        }
        return val;
    }

    internal abstract RecordState FindClosestState(int key);

    protected abstract int DoOnTick(int time);

    protected virtual int DoOnRewind(int time)
    {
        _photonView.RPC("DoRewind", PhotonTargets.All, time);
        return 0;
    }

    [PunRPC]
    internal abstract void DoRewind(int time);

    internal virtual Dictionary<int, T> WipeRemainingRecordedStates<T>(int key, Dictionary<int, T> states)
        where T : RecordState
    {
        return states.Where(state => state.Key <= key).ToDictionary(state => state.Key, state => state.Value);
    }

    internal abstract void PlayState<T>(T recordState) where T : RecordState;
}