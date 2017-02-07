using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoorRecorder : Recorder
{
    private RecordState _previousState;
    private bool _isOpen;
    private readonly Dictionary<int, DoorRecordState> _states = new Dictionary<int, DoorRecordState>();
    private Renderer _renderer;

    [SerializeField]

    private new void Start()
    {
        _isOpen = false;
        _renderer = GetComponent<Renderer>();
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

    public void DoorInteraction(bool isOpen)
    {
        _isOpen = isOpen;
        OpenDoor();
    }

    public bool DoorStatus()
    {
        return _isOpen;
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
        _photonView.RPC("RPCDoorInteract", PhotonTargets.All, DoorStatus());
    }

    [PunRPC]
    void RPCDoorInteract(bool isOpen)
    {
        _renderer.enabled = !isOpen;
    }
}