using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoorRecorder : Recorder
{
    private RecordState _previousState;
    private bool _isOpen;
    private readonly Dictionary<int, DoorRecordState> _states = new Dictionary<int, DoorRecordState>();

    [SerializeField]

    private new void Start()
    {
        _isOpen = false;
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
        var curState = new DoorRecordState(_isOpen);
        if (curState.Equals(_previousState)) return 0;
        _previousState = curState;
        _states[time] = curState;
        return 0;
    }

    public bool GetDoorStatus()
    {
        return _isOpen;
    }

    public void DoorInteraction(bool isOpen)
    {
        _photonView.RPC("RPCDoorInteraction", PhotonTargets.All, isOpen);
    }
    [PunRPC]
    public void RPCDoorInteraction(bool isOpen)
    {
        SetDoorStatus(isOpen);
        if(!_isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    public void SetDoorStatus(bool isOpen)
    {
        _isOpen = isOpen;
    }

    [PunRPC]
    internal override void DoRewind(int time)
    {
        if (this == null) return;
        if (_states.ContainsKey(time))
        {
            PlayState(_states[time]);
        }
        else
        {
            PlayState(_states.Last().Key < time ? _previousState : FindClosestState(time));
        }
    }

    internal override void PlayState<T>(T recordState)
    {
        if (!(recordState is DoorRecordState)) return;
        var state = recordState as DoorRecordState;
        DoorInteraction(state.IsOpen);
    }

    private void OpenDoor()
    {
        transform.rotation = Quaternion.Euler(0,90,0);
    }

    private void CloseDoor()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}