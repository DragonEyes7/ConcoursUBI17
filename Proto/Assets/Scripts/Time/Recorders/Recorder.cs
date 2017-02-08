using System;
using UnityEngine;
using System.Collections.Generic;

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
        var index = keys.BinarySearch(key);
        //~ = Bitwise NOT
        index = ~index - 1;
        if (index < 0) index = 0;
        if (!keys.Contains(index))
            Debug.Log("Using previous state, Dictionnary did not contain proper key : " + index + " Dictionnary count : " + keys.Count);
        return index;
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

    internal abstract void PlayState<T>(T recordState) where T : RecordState;
}