using System.Collections.Generic;
using UnityEngine;

public class InteractiveObjectRecorder : Recorder
{
    private RecordState _previousState;
    private bool _isMoved;
    private Dictionary<int, InteractiveObjectRecordState> _states = new Dictionary<int, InteractiveObjectRecordState>();
    private Interactive _interactiveObject;

    [SerializeField]

    private new void Start()
    {
        _isMoved = false;
        _interactiveObject = GetComponent<Interactive>();
        base.Start();
    }

    internal override RecordState FindClosestState(int key)
    {
        var index = FindClosestKey(key, new List<int>(_states.Keys));
        return !_states.ContainsKey(index) ? _previousState : _states[index];
    }

    protected override int DoOnTick(int time)
    {
        if (this == null) return 0;
        var curState = new InteractiveObjectRecordState(_isMoved);
        if (curState.Equals(_previousState)) return 0;
        _previousState = curState;
        _states[time] = curState;
        return 0;
    }

    public bool GetStatus()
    {
        return _isMoved;
    }

    public void ObjectInteraction(bool isMoved)
    {
        _photonView.RPC("RPCObjectInteraction", PhotonTargets.All, isMoved);
    }

    [PunRPC]
    public void RPCObjectInteraction(bool isOpen)
    {
        SetStatus(isOpen);
        if(!_isMoved)
        {
            ResetObject();
        }
        else
        {
            MoveObject();
        }
    }

    public void SetStatus(bool isOpen)
    {
        _isMoved = isOpen;
    }

    [PunRPC]
    internal override void DoRewind(int time)
    {
        if (this == null) return;
        if (!_states.ContainsKey(time))
        {
            time = FindClosestKey(time, new List<int>(_states.Keys));
        }
        PlayState(_states[time]);
        _states = WipeRemainingRecordedStates(time, _states);
    }

    internal override void PlayState<T>(T recordState)
    {
        if (!(recordState is InteractiveObjectRecordState)) return;
        var state = recordState as InteractiveObjectRecordState;
        ObjectInteraction(state.IsMoved);
    }

    private void MoveObject()
    {
        _interactiveObject.MoveObject();
    }

    private void ResetObject()
    {
        _interactiveObject.ResetObject();
    }
}