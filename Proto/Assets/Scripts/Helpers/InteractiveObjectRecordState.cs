public class InteractiveObjectRecordState : RecordState
{
    private readonly bool _isMoved;
    public InteractiveObjectRecordState(bool isMoved)
    {
        _isMoved = isMoved;
    }

    public bool IsMoved
    {
        get { return _isMoved; }
    }

    public override bool Equals(object obj)
    {
        if (!(obj is InteractiveObjectRecordState)) return false;
        return Equals((InteractiveObjectRecordState) obj);
    }

    private bool Equals(InteractiveObjectRecordState obj)
    {
        return obj.IsMoved == IsMoved;
    }
}