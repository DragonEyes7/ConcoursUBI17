using System.Collections.Generic;
using System.Linq;

public class CharacterRecorder : Recorder
{
    private RecordState _previousState;
    private Dictionary<int, CharacterRecordState> _states = new Dictionary<int, CharacterRecordState>();

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