public class Door : Interactive
{
    private DoorRecorder _doorRecorder;
    private bool _isUnlocked;

    private new void Start()
    {
        base.Start();
        _doorRecorder = GetComponent<DoorRecorder>();
        m_IsActivated = true;
    }

    public void Unlock()
    {
        _isUnlocked = true;
    }

    public override void Interact()
    {
        if (!_isUnlocked || !_doorRecorder) return;
        if (_doorRecorder.DoorStatus()) return;
        _doorRecorder.DoorInteraction(true);
    }


}
