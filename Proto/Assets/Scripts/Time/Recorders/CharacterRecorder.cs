using System;
using System.Collections.Generic;
using System.Linq;

public class CharacterRecorder : Recorder
{
    private RecordState _previousState;
    private readonly Dictionary<int, CharacterRecordState> _states = new Dictionary<int, CharacterRecordState>();

    protected override void Register(Func<int, int> doOnTick, Func<int, int> doOnRewind)
    {
        if (!_mainRecorder) return;
        _mainRecorder.OnTick.Suscribe(doOnTick);
        _mainRecorder.OnRewind.Suscribe(doOnRewind);
    }

    internal override RecordState FindClosestState(int key)
    {
        var index = FindClosestKey(key, new List<int>(_states.Keys));
        return !_states.ContainsKey(index) ? _previousState : _states[index];
    }

    protected override int DoOnTick(int time)
    {
        if (this == null) return 0;
        var curState = new CharacterRecordState(transform.position, transform.rotation);
        if (curState.Equals(_previousState)) return 0;
        _previousState = curState;
        _states[time] = curState;
        return 0;
    }

    protected override int DoOnRewind(int time)
    {
        _photonView.RPC("DoRewind", PhotonTargets.All, time);
        return 0;
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

        if (_rigidbody)
        {
            _rigidbody.isKinematic = false;
        }
    }

    internal override void PlayState<T>(T recordState)
    {
        if (!(recordState is CharacterRecordState)) return;
        var state = recordState as CharacterRecordState;
        transform.position = state.Position;
        transform.rotation = state.Rotation;
    }
}