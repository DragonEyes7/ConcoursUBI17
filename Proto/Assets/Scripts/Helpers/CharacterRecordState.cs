 using UnityEngine;

public class CharacterRecordState : RecordState
{
    private readonly Vector3 _position;
    private readonly Quaternion _rotation;

    public CharacterRecordState(Vector3 position, Quaternion rotation)
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

    public override bool Equals(object obj)
    {
        if (!(obj is CharacterRecordState)) return false;
        return Equals((CharacterRecordState) obj);
    }

    private bool Equals(CharacterRecordState obj)
    {
        return obj.Position.Equals(Position) && obj.Rotation.Equals(Rotation);
    }
}