using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterRecorder : Recorder
{
    private struct RecordState
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
    private RecordState _previousState;
    private readonly Dictionary<int, RecordState> _states = new Dictionary<int, RecordState>();

    protected override void Register(Func<int, int> doOnTick, Func<int, int> doOnRewind)
    {
        if (!_mainRecorder) return;
        _mainRecorder.OnTick.Suscribe(doOnTick);
        _mainRecorder.OnRewind.Suscribe(doOnRewind);
    }

    protected override int DoOnTick(int time)
    {
        if (this == null) return 0;
        var curState = new RecordState(transform.position, transform.rotation);
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

    private void PlayState(RecordState state)
    {
        transform.position = state.Position;
        transform.rotation = state.Rotation;
    }
}