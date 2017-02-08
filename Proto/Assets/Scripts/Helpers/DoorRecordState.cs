public class DoorRecordState : RecordState
{
    private readonly bool _isOpen;
    public DoorRecordState(bool isOpen)
    {
        _isOpen = isOpen;
    }

    public bool IsOpen
    {
        get { return _isOpen; }
    }

    public override bool Equals(object obj)
    {
        if (!(obj is DoorRecordState)) return false;
        return Equals((DoorRecordState) obj);
    }

    private bool Equals(DoorRecordState obj)
    {
        return obj.IsOpen == IsOpen;
    }
}